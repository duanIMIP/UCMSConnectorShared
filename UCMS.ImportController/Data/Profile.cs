using IMIP.SharedComponent.GdPictureWrapper;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Def;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCMS.RestClient;

namespace UCMS.ImportController.Data
{
    public class ProfileContent
    {
        public UCMSApiClient oUCMSApiClient { get; set; }
        public DataValue oBranch { get; set; }
        public DataValue oFolder { get; set; }
        public DataValue oWorkflow { get; set; }
        public DataValue oWorkflowStep { get; set; }
        public DataValue oContenType { get; set; }
        public DataValue oContenTypeParent { get; set; }
        public Dictionary<string, object> oContentField { get; set; }
        public Dictionary<string, object> oLibraryField { get; set; }
        public Dictionary<string, object> oContentParent { get; set; }
        public Dictionary<string, object> oLibraryParent { get; set; }
        public List<FileInfo> arrayFileInfor { get; set; }
        public String RenameFile { get; set; }
        public String RemoveFile { get; set; }
        public String keyPrivateData { get; set; }
        public IMIP.UniversalScan.Profile.BatchNamingProfile oBatchNamingProfile { get; set; }

        public ProfileContent(UCMSApiClient oUCMSApiClient, String keyPrivateData)
        {
            this.oUCMSApiClient = oUCMSApiClient;
            this.keyPrivateData = keyPrivateData;
            oBranch = new DataValue();
            oFolder = new DataValue();
            oWorkflow = new DataValue();
            oWorkflowStep = new DataValue();
            oContenType = new DataValue();
            oContenTypeParent = new DataValue();
            oContentField = new Dictionary<string, object>();
            oLibraryField = new Dictionary<string, object>();
            oContentParent = new Dictionary<string, object>();
            oLibraryParent = new Dictionary<string, object>();
            arrayFileInfor = new List<FileInfo>();
            RenameFile = "";
            RemoveFile = "";
            oBatchNamingProfile = new IMIP.UniversalScan.Profile.BatchNamingProfile();
        }

        private String UploadProfile()
        {
            String pathClient = "";
            Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
            oWorkflowItem.Content = new Model.Content();
            oWorkflowItem.Content.Folder = oUCMSApiClient.Folder.GetById(oFolder.Key);
            oWorkflowItem.Content.Tags = new List<string>();
            oWorkflowItem.Workflow = new Model.Workflow() { Id = oWorkflow.Key };
            oWorkflowItem.WorkflowStep = new Model.WorkflowStep() { Id = oWorkflowStep.Key };
            oWorkflowItem.State = Model.Enum.WorkflowItemState.Ready;
            oWorkflowItem.Priority = Model.Enum.WorkflowItemPriority.Normal;
            Dictionary<string, object> temp = null;
            Model.ContentPrivateData oContentPrivateData = new Model.ContentPrivateData();
            UniBatch oBatch = new UniBatch();
            UniDocument oUniDocument = new UniDocument();
            String ContentName = "";
            String FilePathName = "";
            String tempFileSplit = "";
            try
            {
                if (!String.IsNullOrEmpty(oContenTypeParent.Key))//Branch or Document
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenTypeParent.Value, oBranch.Value, oWorkflowItem.Content.Folder.Name, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenTypeParent.Key };
                    temp = oContentParent;
                    temp.Add("BranchID", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryParent;
                }
                else
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenType.Value, oBranch.Value, oWorkflowItem.Content.Folder.Name, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenType.Key };
                    temp = oContentField;
                    temp.Add("BranchID", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryField;
                }
                oWorkflowItem.Content = oUCMSApiClient.Content.Create(oWorkflowItem.Content);

                oWorkflowItem.Content.Attachments = new List<Model.Attachment>();
                oUCMSApiClient.Content.Checkout(oWorkflowItem.Content.Id);
                for (int i = 0; i < arrayFileInfor.Count(); i++)
                {
                    pathClient = arrayFileInfor[i].DirectoryName;
                    if (arrayFileInfor[i].Extension == ".pdf")
                    {
                        tempFileSplit = Guid.NewGuid().ToString();
                        Directory.CreateDirectory(tempFileSplit);
                        ImageProcessing.SplitPDF2Tiff(arrayFileInfor[i].FullName, tempFileSplit, 300);

                        foreach (var itemSplitFile in Directory.GetFiles(tempFileSplit))
                        {
                            var attachment = new Model.Attachment()
                            {
                                ContentId = oWorkflowItem.Content.Id,
                                Data = File.ReadAllBytes(itemSplitFile),
                                MIME = "image/universalscan",
                                Type = UCMS.Model.Enum.AttachmentType.Public,
                                Name = Path.GetFileName(itemSplitFile)
                            };
                            oUCMSApiClient.Attachment.Upload(attachment);
                            attachment = null;
                        }
                        DeleteFileInDirectory(tempFileSplit);
                    }
                    else
                    {
                        var attachment = new Model.Attachment()
                        {
                            ContentId = oWorkflowItem.Content.Id,
                            Data = File.ReadAllBytes(arrayFileInfor[i].FullName),
                            MIME = "image/universalscan",
                            Type = UCMS.Model.Enum.AttachmentType.Public,
                            Name = Guid.NewGuid() + arrayFileInfor[i].Extension
                        };
                        oUCMSApiClient.Attachment.Upload(attachment);
                        attachment = null;
                    }

                    FilePathName = arrayFileInfor[i].FullName;
                    if (!String.IsNullOrEmpty(RemoveFile))
                    {
                        if (File.Exists(Path.Combine(RemoveFile, arrayFileInfor[i].Name)))
                        {
                            File.Replace(FilePathName, Path.Combine(RemoveFile, arrayFileInfor[i].Name), null);
                        }
                        else
                        {
                            File.Move(FilePathName, Path.Combine(RemoveFile, arrayFileInfor[i].Name));
                        }
                        FilePathName = Path.Combine(RemoveFile, arrayFileInfor[i].Name);
                    }

                    if (!string.IsNullOrEmpty(RenameFile))
                    {
                        File.Copy(FilePathName, FilePathName.Replace(arrayFileInfor[i].Extension, "") + RenameFile, true);
                    }
                    FilePathName = "";
                }

                oUCMSApiClient.Content.Checkin(oWorkflowItem.Content.Id);

                oContentPrivateData.Key = keyPrivateData;

                //--------------Set value ContentPrivateData-----------------------------                
                oBatch.BranchID = oBranch.Key;
                oBatch.Name = oWorkflowItem.Content.Name;
                oBatch.ClientName = oWorkflowItem.Content.Folder.Name;
                oBatch.ProcessName = oWorkflow.Value;
                oBatch.ProcessStepName = oWorkflowStep.Value;
                oBatch.FormTypeName = oWorkflowItem.Content.ContentType.Name;
                oBatch.Fields = new List<UniField>();
                oBatch.Pages = new List<UniPage>();
                if (String.IsNullOrEmpty(oContenTypeParent.Key))
                {
                    oWorkflowItem.Content = oUCMSApiClient.Content.GetById(oWorkflowItem.Content.Id);
                    for (int i = 0; i < oWorkflowItem.Content.Attachments.Count; i++)
                    {
                        oBatch.Pages.Add(new UniPage()
                        {
                            ID = Path.GetFileNameWithoutExtension(oWorkflowItem.Content.Attachments[i].Name),
                            FullFileName = (string.IsNullOrEmpty(RemoveFile) ? pathClient : RemoveFile) + @"\" + oWorkflowItem.Content.Attachments[i].Name,
                            Rejected = false,
                            IsRescan = false,
                            IsNew = false,
                            SheetID = "",
                            RejectedNote = ""
                        });
                    }

                    foreach (var item in oContentField)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                    foreach (var item in oLibraryField)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                }
                else
                {
                    foreach (var item in oContentParent)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                    foreach (var item in oLibraryParent)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }

                    oBatch.Docs = new List<UniDocument>();
                    oUniDocument = new UniDocument();
                    oUniDocument.Fields = new List<UniField>();
                    oUniDocument.Pages = new List<UniPage>();
                    oUniDocument.FormTypeName = oContenType.Value;

                    for (int i = 0; i < oWorkflowItem.Content.Attachments.Count; i++)
                    {
                        oUniDocument.Pages.Add(new UniPage()
                        {
                            ID = Path.GetFileNameWithoutExtension(oWorkflowItem.Content.Attachments[i].Name),
                            FullFileName = (string.IsNullOrEmpty(RemoveFile) ? pathClient : RemoveFile) + @"\" + oWorkflowItem.Content.Attachments[i].Name,
                            Rejected = false,
                            IsRescan = false,
                            IsNew = false,
                            SheetID = "",
                            RejectedNote = ""
                        });
                    }
                    foreach (var item in oContentField)
                    {
                        oUniDocument.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                    foreach (var item in oLibraryField)
                    {
                        oUniDocument.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }

                    oBatch.Docs.Add(oUniDocument);
                }
                oContentPrivateData.Value = Common.SerializeToString(typeof(UniBatch), oBatch);
                //--------------Set value ContentPrivateData-----------------------------

                oUCMSApiClient.Content.SetPrivateData(oWorkflowItem.Content.Id, oContentPrivateData);
                Boolean autoProcess = false;
                foreach (var item in Common.WFStepProcessAuto)
                {
                    if (item.Equals(oWorkflowItem.Content.Folder.Name + "_" + oWorkflowStep.Value))
                    {
                        autoProcess = true; break;
                    }
                }
                oUCMSApiClient.WorkflowItem.Insert(oWorkflowItem, autoProcess);
                ContentName = oWorkflowItem.Content.Name;
            }
            catch (Exception ex)
            {
                Common.LogToFile("Profile_" + ex.Message);
                DeleteFileInDirectory(tempFileSplit);
                ContentName = "";
            }
            oWorkflowItem = null;
            temp = null;
            oContentPrivateData = null;
            oBatch = null;
            oUniDocument = null;
            return ContentName;
        }

        private void DeleteFileInDirectory(String TiffList)
        {
            if (!String.IsNullOrEmpty(TiffList))
            {
                foreach (String itemPath in Directory.GetDirectories(TiffList))
                {
                    foreach (var itemFile in Directory.GetFiles(itemPath))
                    {
                        File.Delete(itemFile);
                    }
                    Directory.Delete(itemPath);
                }
                foreach (var TiffFile in Directory.GetFiles(TiffList))
                {
                    File.Delete(TiffFile);
                }
                Directory.Delete(TiffList);
                TiffList = "";
            }
        }
    }

    public class ProfileContentRanDom
    {
        public UCMSApiClient oUCMSApiClient { get; set; }
        public List<DataValue> oBranchList { get; set; }
        public DataValue oFolder { get; set; }
        public DataValue oWorkflow { get; set; }
        public DataValue oWorkflowStep { get; set; }
        public DataValue oContenType { get; set; }
        public DataValue oContenTypeParent { get; set; }
        public Dictionary<string, object> oContentField { get; set; }
        public Dictionary<string, object> oLibraryField { get; set; }
        public Dictionary<string, object> oContentParent { get; set; }
        public Dictionary<string, object> oLibraryParent { get; set; }
        public List<FileInfo> arrayFileInfor { get; set; }
        public String RenameFile { get; set; }
        public String RemoveFile { get; set; }
        public String keyPrivateData { get; set; }
        public IMIP.UniversalScan.Profile.BatchNamingProfile oBatchNamingProfile { get; set; }

        public ProfileContentRanDom(UCMSApiClient oUCMSApiClient, String keyPrivateData)
        {
            this.oUCMSApiClient = oUCMSApiClient;
            this.keyPrivateData = keyPrivateData;
            oBranchList = new List<DataValue>();
            oFolder = new DataValue();
            oWorkflow = new DataValue();
            oWorkflowStep = new DataValue();
            oContenType = new DataValue();
            oContenTypeParent = new DataValue();
            oContentField = new Dictionary<string, object>();
            oLibraryField = new Dictionary<string, object>();
            oContentParent = new Dictionary<string, object>();
            oLibraryParent = new Dictionary<string, object>();
            arrayFileInfor = new List<FileInfo>();
            RenameFile = "";
            RemoveFile = "";
            oBatchNamingProfile = new IMIP.UniversalScan.Profile.BatchNamingProfile();
        }

        private String UploadProfile()
        {
            String pathClient = "";
            if(oBranchList == null || oBranchList.Count==0)
            {
                return "";
            }
            Random rd = new Random();
            int ird = rd.Next(0, oBranchList.Count);
            DataValue oBranch = oBranchList[(ird> oBranchList.Count-1)?(oBranchList.Count): ird];

            Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
            oWorkflowItem.Content = new Model.Content();
            oWorkflowItem.Content.Folder = oUCMSApiClient.Folder.GetById(oFolder.Key);
            oWorkflowItem.Content.Tags = new List<string>();
            oWorkflowItem.Workflow = new Model.Workflow() { Id = oWorkflow.Key };
            oWorkflowItem.WorkflowStep = new Model.WorkflowStep() { Id = oWorkflowStep.Key };
            oWorkflowItem.State = Model.Enum.WorkflowItemState.Ready;
            oWorkflowItem.Priority = Model.Enum.WorkflowItemPriority.Normal;
            Dictionary<string, object> temp = null;
            Model.ContentPrivateData oContentPrivateData = new Model.ContentPrivateData();
            UniBatch oBatch = new UniBatch();
            UniDocument oUniDocument = new UniDocument();
            String ContentName = "";
            String FilePathName = "";
            String tempFileSplit = "";
            try
            {
                if (!String.IsNullOrEmpty(oContenTypeParent.Key))//Branch or Document
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenTypeParent.Value, oBranch.Value, oWorkflowItem.Content.Folder.Name, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenTypeParent.Key };
                    temp = oContentParent;
                    temp.Add("BranchID", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryParent;
                }
                else
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenType.Value, oBranch.Value, oWorkflowItem.Content.Folder.Name, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenType.Key };
                    temp = oContentField;
                    temp.Add("BranchID", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryField;
                }
                oWorkflowItem.Content = oUCMSApiClient.Content.Create(oWorkflowItem.Content);

                oWorkflowItem.Content.Attachments = new List<Model.Attachment>();
                oUCMSApiClient.Content.Checkout(oWorkflowItem.Content.Id);
                for (int i = 0; i < arrayFileInfor.Count(); i++)
                {
                    pathClient = arrayFileInfor[i].DirectoryName;
                    if (arrayFileInfor[i].Extension == ".pdf")
                    {
                        tempFileSplit = Guid.NewGuid().ToString();
                        Directory.CreateDirectory(tempFileSplit);
                        ImageProcessing.SplitPDF2Tiff(arrayFileInfor[i].FullName, tempFileSplit, 300);

                        foreach (var itemSplitFile in Directory.GetFiles(tempFileSplit))
                        {
                            var attachment = new Model.Attachment()
                            {
                                ContentId = oWorkflowItem.Content.Id,
                                Data = File.ReadAllBytes(itemSplitFile),
                                MIME = "image/universalscan",
                                Type = UCMS.Model.Enum.AttachmentType.Public,
                                Name = Path.GetFileName(itemSplitFile)
                            };
                            oUCMSApiClient.Attachment.Upload(attachment);
                            attachment = null;
                        }
                        DeleteFileInDirectory(tempFileSplit);
                    }
                    else
                    {
                        var attachment = new Model.Attachment()
                        {
                            ContentId = oWorkflowItem.Content.Id,
                            Data = File.ReadAllBytes(arrayFileInfor[i].FullName),
                            MIME = "image/universalscan",
                            Type = UCMS.Model.Enum.AttachmentType.Public,
                            Name = Guid.NewGuid() + arrayFileInfor[i].Extension
                        };
                        oUCMSApiClient.Attachment.Upload(attachment);
                        attachment = null;
                    }

                    FilePathName = arrayFileInfor[i].FullName;
                    if (!String.IsNullOrEmpty(RemoveFile))
                    {
                        if (File.Exists(Path.Combine(RemoveFile, arrayFileInfor[i].Name)))
                        {
                            File.Replace(FilePathName, Path.Combine(RemoveFile, arrayFileInfor[i].Name), null);
                        }
                        else
                        {
                            File.Move(FilePathName, Path.Combine(RemoveFile, arrayFileInfor[i].Name));
                        }
                        FilePathName = Path.Combine(RemoveFile, arrayFileInfor[i].Name);
                    }

                    if (!string.IsNullOrEmpty(RenameFile))
                    {
                        File.Copy(FilePathName, FilePathName.Replace(arrayFileInfor[i].Extension, "") + RenameFile, true);
                    }
                    FilePathName = "";
                }

                oUCMSApiClient.Content.Checkin(oWorkflowItem.Content.Id);

                oContentPrivateData.Key = keyPrivateData;

                //--------------Set value ContentPrivateData-----------------------------                
                oBatch.BranchID = oBranch.Key;
                oBatch.Name = oWorkflowItem.Content.Name;
                oBatch.ClientName = oWorkflowItem.Content.Folder.Name;
                oBatch.ProcessName = oWorkflow.Value;
                oBatch.ProcessStepName = oWorkflowStep.Value;
                oBatch.FormTypeName = oWorkflowItem.Content.ContentType.Name;
                oBatch.Fields = new List<UniField>();
                oBatch.Pages = new List<UniPage>();
                if (String.IsNullOrEmpty(oContenTypeParent.Key))
                {
                    oWorkflowItem.Content = oUCMSApiClient.Content.GetById(oWorkflowItem.Content.Id);
                    for (int i = 0; i < oWorkflowItem.Content.Attachments.Count; i++)
                    {
                        oBatch.Pages.Add(new UniPage()
                        {
                            ID = Path.GetFileNameWithoutExtension(oWorkflowItem.Content.Attachments[i].Name),
                            FullFileName = (string.IsNullOrEmpty(RemoveFile) ? pathClient : RemoveFile) + @"\" + oWorkflowItem.Content.Attachments[i].Name,
                            Rejected = false,
                            IsRescan = false,
                            IsNew = false,
                            SheetID = "",
                            RejectedNote = ""
                        });
                    }

                    foreach (var item in oContentField)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                    foreach (var item in oLibraryField)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                }
                else
                {
                    foreach (var item in oContentParent)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                    foreach (var item in oLibraryParent)
                    {
                        oBatch.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }

                    oBatch.Docs = new List<UniDocument>();
                    oUniDocument = new UniDocument();
                    oUniDocument.Fields = new List<UniField>();
                    oUniDocument.Pages = new List<UniPage>();
                    oUniDocument.FormTypeName = oContenType.Value;

                    for (int i = 0; i < oWorkflowItem.Content.Attachments.Count; i++)
                    {
                        oUniDocument.Pages.Add(new UniPage()
                        {
                            ID = Path.GetFileNameWithoutExtension(oWorkflowItem.Content.Attachments[i].Name),
                            FullFileName = (string.IsNullOrEmpty(RemoveFile) ? pathClient : RemoveFile) + @"\" + oWorkflowItem.Content.Attachments[i].Name,
                            Rejected = false,
                            IsRescan = false,
                            IsNew = false,
                            SheetID = "",
                            RejectedNote = ""
                        });
                    }
                    foreach (var item in oContentField)
                    {
                        oUniDocument.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }
                    foreach (var item in oLibraryField)
                    {
                        oUniDocument.Fields.Add(new UniField() { Name = item.Key, Value = item.Value.ToString() });
                    }

                    oBatch.Docs.Add(oUniDocument);
                }
                oContentPrivateData.Value = Common.SerializeToString(typeof(UniBatch), oBatch);
                //--------------Set value ContentPrivateData-----------------------------

                oUCMSApiClient.Content.SetPrivateData(oWorkflowItem.Content.Id, oContentPrivateData);
                Boolean autoProcess = false;
                foreach (var item in Common.WFStepProcessAuto)
                {
                    if (item.Equals(oWorkflowItem.Content.Folder.Name + "_" + oWorkflowStep.Value))
                    {
                        autoProcess = true; break;
                    }
                }
                oUCMSApiClient.WorkflowItem.Insert(oWorkflowItem, autoProcess);
                ContentName = oWorkflowItem.Content.Name;
            }
            catch (Exception ex)
            {
                Common.LogToFile("Profile_" + ex.Message);
                DeleteFileInDirectory(tempFileSplit);
                ContentName = "";
            }
            oWorkflowItem = null;
            temp = null;
            oContentPrivateData = null;
            oBatch = null;
            oUniDocument = null;
            return ContentName;
        }

        private void DeleteFileInDirectory(String TiffList)
        {
            if (!String.IsNullOrEmpty(TiffList))
            {
                foreach (String itemPath in Directory.GetDirectories(TiffList))
                {
                    foreach (var itemFile in Directory.GetFiles(itemPath))
                    {
                        File.Delete(itemFile);
                    }
                    Directory.Delete(itemPath);
                }
                foreach (var TiffFile in Directory.GetFiles(TiffList))
                {
                    File.Delete(TiffFile);
                }
                Directory.Delete(TiffList);
                TiffList = "";
            }
        }
    }

    public class ProfileFolderRanDom
    {
        bool disposed = false;
        private ActivityConfiguration oActivityConfiguration;

        public String folderPath { get; set; }
        public List<DataValue> BranchList { get; set; }
        public List<DataValue> FolderList { get; set; }
        public String FileUploadType { get; set; }
        public String FileUploadReName { get; set; }
        public String FileUploadMoveTo { get; set; }
        public String Name { get; set; }
        public Boolean LoadRootFalse { get; set; }
        public Boolean CheckFolder { get; set; }
        public Boolean CheckFile { get; set; }
        public String PathValue { get; set; }
        public Boolean CheckItem { get; set; }
        private String folderXmlName { get; }
        public ProfileFolderRanDom()
        {
            folderPath = "";
            BranchList = new List<DataValue>();
            FolderList = new List<DataValue>();
            FileUploadType = "";
            FileUploadReName = "";
            FileUploadMoveTo = "";
            Name = "";
            CheckFolder = false;
            CheckFile = false;
            PathValue = "";
            CheckItem = false;
        }

        public ProfileFolderRanDom(String folderPath, List<DataValue> BranchList, List<DataValue> FolderList, String FileUploadType, String FileUploadReName, String FileUploadMoveTo, String Name, Boolean CheckFolder, Boolean CheckFile, String PathValue, Boolean CheckItem)
        {
            this.folderPath = folderPath;
            this.BranchList = BranchList;
            this.FolderList = FolderList;
            this.FileUploadType = FileUploadType;
            this.FileUploadReName = FileUploadReName;
            this.FileUploadMoveTo = FileUploadMoveTo;
            this.Name = Name;
            this.CheckFolder = CheckFolder;
            this.CheckFile = CheckFile;
            this.PathValue = PathValue;
            this.CheckItem = CheckItem;
        }

        ~ProfileFolderRanDom()
        {
            folderPath = "";
            if (BranchList != null && BranchList.Count > 0) BranchList.Clear();
            BranchList = null;
            if (FolderList != null && FolderList.Count > 0) FolderList.Clear();
            FolderList = null;
            FileUploadType = null;
            FileUploadReName = null;
            FileUploadMoveTo = null;
            Name = null;
            CheckFolder = false;
            CheckFile = false;
            PathValue = null;
            CheckItem = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    folderPath = "";
                    if (BranchList != null && BranchList.Count > 0) BranchList.Clear();
                    BranchList = null;
                    if (FolderList != null && FolderList.Count > 0) FolderList.Clear();
                    FolderList = null;
                    FileUploadType = null;
                    FileUploadReName = null;
                    FileUploadMoveTo = null;
                    Name = null;
                    CheckFolder = false;
                    CheckFile = false;
                    PathValue = null;
                    CheckItem = false;
                }
                disposed = true;
            }
        }

        public void WriteXmlFromProfileContent(UCMSApiClient oUCMSApiClient)
        {
            List<UniFormType> UniFormTypeList = null;

            foreach (DataValue itemFolder in FolderList)
            {
                foreach (Model.Workflow itemWorkflow in GetData.GetWorkflow(oUCMSApiClient, itemFolder.Key))
                {
                    Model.WorkflowStep oWorkflowStepRoot = null;
                    foreach (var itemWorkFlowStep in itemWorkflow.Steps)
                    {
                        if(!String.IsNullOrEmpty(itemWorkFlowStep.Setting))
                        {
                            ProfileContentRanDom oContentRandom = new ProfileContentRanDom(oUCMSApiClient, "USCBatch");

                            oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, itemWorkFlowStep.Id);

                            if (oActivityConfiguration.SettingReference != null && !oActivityConfiguration.SettingReference.Trim().Equals(Common.SettingReferenceDefault))
                            {
                                oWorkflowStepRoot = itemWorkflow.Steps.SingleOrDefault(x => x.Name.Equals(oActivityConfiguration.SettingReference));
                                if (oWorkflowStepRoot == null || string.IsNullOrEmpty(oWorkflowStepRoot.Id)) continue;
                                oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, oWorkflowStepRoot.Id);
                            }

                            if (oActivityConfiguration.DocumentTypeProfile != null)
                            {
                                if (oActivityConfiguration.DocumentTypeProfile.UniFormtypeList != null && oActivityConfiguration.DocumentTypeProfile.UniFormtypeList.Count > 0)
                                {
                                    if (LoadRootFalse)
                                    {
                                        UniFormTypeList = GetData.GetContentType(oUCMSApiClient, itemFolder.Key, oActivityConfiguration.DocumentTypeProfile.UniFormtypeList);
                                    }
                                    else
                                    {
                                        UniFormTypeList = GetData.GetContentType(oUCMSApiClient, itemFolder.Key, oActivityConfiguration.DocumentTypeProfile.UniFormtypeList).FindAll(x => x.Root);
                                    }
                                }
                                if (oActivityConfiguration.BatchNamingProfile != null)
                                {
                                    oContentRandom.oBatchNamingProfile = oActivityConfiguration.BatchNamingProfile;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
