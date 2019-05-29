using IMIP.SharedComponent.CustomMapping;
using IMIP.SharedComponent.GdPictureWrapper;
using IMIP.UniversalScan.Common;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UCMS.RestClient;

namespace UCMS.ImportController.Data
{
    public class ContentProfile
    {
        [XmlElement("BranchId")]
        public String BranchId { get; set; }
        [XmlElement("FolderId")]
        public String FolderId { get; set; }
        [XmlElement("oWorkflow")]
        public DataValue oWorkflow { get; set; }
        [XmlElement("oWorkflowStep")]
        public DataValue oWorkflowStep { get; set; }
        [XmlElement("oContenType")]
        public DataValue oContenType { get; set; }
        [XmlElement("oContenTypeParent")]
        public DataValue oContenTypeParent { get; set; }
        [XmlElement("oContentField")]
        public List<DataValue> oContentField { get; set; }
        [XmlElement("oLibraryField")]
        public List<DataValue> oLibraryField { get; set; }
        [XmlElement("oContentParent")]
        public List<DataValue> oContentParent { get; set; }
        [XmlElement("oLibraryParent")]
        public List<DataValue> oLibraryParent { get; set; }
        [XmlElement("RenameFile")]
        public String RenameFile { get; set; }
        [XmlElement("RemoveFile")]
        public String RemoveFile { get; set; }
        [XmlElement("Namming")]
        public String Namming { get; set; }
        [XmlElement("ProfileCreated")]
        public int ProfileCreated { get; set; }
        [XmlElement("TypeTime")]
        public String TypeTime { get; set; }
        [XmlElement("TypeDate")]
        public String TypeDate { get; set; }

        public ContentProfile()
        {
            BranchId = "";
            FolderId = "";
            oWorkflow = new DataValue();
            oWorkflowStep = new DataValue();
            oContenType = new DataValue();
            oContenTypeParent = new DataValue();
            oContentField = new List<DataValue>();
            oLibraryField = new List<DataValue>();
            oContentParent = new List<DataValue>();
            oLibraryParent = new List<DataValue>();
            RenameFile = "";
            RemoveFile = "";
            Namming = "";
            TypeTime = "";
            TypeDate = "";
            ProfileCreated = 0;
        }

        private DataValue getBranch(String Key, List<Branch> BranchList)
        {
            foreach (Branch item in BranchList)
            {
                if (item.Name.Equals(Key))
                {
                    return new DataValue() { Key = item.Name, Value = item.Name };
                }
            }
            return new DataValue();
        }

        private Model.Folder getFolder(String Key, List<Model.Folder> FolderList)
        {
            foreach (Model.Folder item in FolderList)
            {
                if (item.Id.Equals(Key))
                {
                    return item;
                }
            }
            return new Model.Folder();
        }

        public void Profile(UCMSApiClient oUCMSApiClient, List<Branch> BranchList, List<Model.Folder> FolderList, FileInfo oFileInfo, String keyPrivateData)
        {
            DataValue oBranch = getBranch(BranchId, BranchList);
            Model.Folder oFolder = getFolder(FolderId, FolderList);
            Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
            oWorkflowItem.Content = new Model.Content();
            oWorkflowItem.Content.Folder = oFolder;
            oWorkflowItem.Content.Tags = new List<string>();
            oWorkflowItem.Content.Attachments = new List<Model.Attachment>();
            oWorkflowItem.Content.Values = new Dictionary<string, object>();
            oWorkflowItem.Content.LibraryFieldValues = new Dictionary<string, object>();
            oWorkflowItem.Workflow = new Model.Workflow() { Id = oWorkflow.Key };
            oWorkflowItem.WorkflowStep = new Model.WorkflowStep() { Id = oWorkflowStep.Key };
            oWorkflowItem.State = Model.Enum.WorkflowItemState.Ready;
            oWorkflowItem.Priority = Model.Enum.WorkflowItemPriority.Normal;

            Model.ContentPrivateData oContentPrivateData = new Model.ContentPrivateData();
            UniBatch oBatch = new UniBatch();
            UniDocument oUniDocument = new UniDocument();
            DateTime DateValue = DateTime.MinValue;
            try
            {
                Namming = Namming.Replace("{TypeDate}", DateTime.Now.ToString(TypeDate));
                Namming = Namming.Replace("{TypeTime}", DateTime.Now.ToString(TypeTime));

                if (!String.IsNullOrEmpty(oContenTypeParent.Key))//Branch or Document
                {
                    oWorkflowItem.Content.Name = Namming;
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenTypeParent.Key };
                    foreach (var item in oContentParent)
                    {
                        oWorkflowItem.Content.Values.Add(item.Key, item.Value);
                    }
                    oWorkflowItem.Content.Values.Add("BranchID", oBranch.Value);
                    foreach (var item in oLibraryParent)
                    {
                        oWorkflowItem.Content.LibraryFieldValues.Add(item.Key, item.Value);
                    }
                }
                else
                {
                    oWorkflowItem.Content.Name = Namming;
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenType.Key };
                    foreach (var item in oContentField)
                    {
                        oWorkflowItem.Content.Values.Add(item.Key, item.Value);
                    }
                    oWorkflowItem.Content.Values.Add("BranchID", oBranch.Value);
                    foreach (var item in oLibraryField)
                    {
                        oWorkflowItem.Content.LibraryFieldValues.Add(item.Key, item.Value);
                    }
                }

                DateValue = DateTime.Now;
                oWorkflowItem.Content = oUCMSApiClient.Content.Create(oWorkflowItem.Content);
                SetThreadSleep(DateValue, DateTime.Now);

                oWorkflowItem.Content.Attachments = UploadAttachment(oUCMSApiClient, oWorkflowItem.Content.Id, oFileInfo);

                oContentPrivateData.Key = keyPrivateData;

                //--------------Set value ContentPrivateData-----------------------------                
                oBatch.BranchID = oBranch.Key;
                oBatch.Name = oWorkflowItem.Content.Name;
                oBatch.ClientName = oFolder.Name;
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
                            FullFileName = (string.IsNullOrEmpty(RemoveFile) ? oFileInfo.DirectoryName : RemoveFile) + @"\" + oWorkflowItem.Content.Attachments[i].Name,
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
                            FullFileName = (string.IsNullOrEmpty(RemoveFile) ? oFileInfo.DirectoryName : RemoveFile) + @"\" + oWorkflowItem.Content.Attachments[i].Name,
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

                DateValue = DateTime.Now;
                oUCMSApiClient.Content.SetPrivateData(oWorkflowItem.Content.Id, oContentPrivateData);
                SetThreadSleep(DateValue, DateTime.Now);

                Boolean autoProcess = false;
                foreach (var item in Common.WFStepProcessAuto)
                {
                    if (item.Equals(oFolder.Name + "_" + oWorkflowStep.Value))
                    {
                        autoProcess = true; break;
                    }
                }

                DateValue = DateTime.Now;
                oUCMSApiClient.WorkflowItem.Insert(oWorkflowItem, autoProcess);
                SetThreadSleep(DateValue, DateTime.Now);
                ProfileCreated = 1;
            }
            catch (Exception ex)
            {
                Common.LogToFile("Profile_" + ex.Message);
            }

            oWorkflowItem.Priority = 0;
            oWorkflowItem.State = 0;
            oWorkflowItem.WorkflowStep = null;
            oWorkflowItem.Workflow = null;
            if (oWorkflowItem.Content.Attachments.Count > 0) oWorkflowItem.Content.Attachments.Clear();
            oWorkflowItem.Content.Attachments = null;
            oWorkflowItem.Content.Tags = null;
            oWorkflowItem.Content.Folder = null;
            oWorkflowItem.Content = null;
            oWorkflowItem = null;
            oContentPrivateData = null;
            oBatch = null;
            oUniDocument = null;
        }

        /// <summary>
        /// Upload Attachmen in database and return list Attachment
        /// </summary>
        /// <param name="oUCMSApiClient"></param>
        /// <param name="ContentID"></param>        
        /// <returns></returns>
        /// 
        private List<Model.Attachment> UploadAttachment(UCMSApiClient oUCMSApiClient, String ContentID, FileInfo oFileInfo)
        {
            List<Model.Attachment> AttachmentList = new List<Model.Attachment>();
            try
            {
                String FilePathName = oFileInfo.FullName;
                String FileExtension = oFileInfo.Extension;

                oUCMSApiClient.Content.Checkout(ContentID);

                if (FileExtension.ToUpper().Equals(".pdf".ToUpper()))
                {
                    var tempFileSplit = Guid.NewGuid().ToString();
                    Directory.CreateDirectory(tempFileSplit);

                    ImageProcessing.SplitPDF2Tiff(FilePathName, tempFileSplit, 300);
                    foreach (var itemSplitFile in Directory.GetFiles(tempFileSplit))
                    {
                        var attachment = new Model.Attachment()
                        {
                            ContentId = ContentID,
                            Data = File.ReadAllBytes(itemSplitFile),
                            MIME = "image/universalscan",
                            Type = UCMS.Model.Enum.AttachmentType.Public,
                            Name = Path.GetFileName(itemSplitFile)
                        };
                        oUCMSApiClient.Attachment.Upload(attachment);
                        AttachmentList.Add(attachment);
                        attachment = null;
                    }
                    DeleteFileInDirectory(tempFileSplit);
                }
                else
                {
                    var attachment = new Model.Attachment()
                    {
                        ContentId = ContentID,
                        Data = File.ReadAllBytes(FilePathName),
                        MIME = "image/universalscan",
                        Type = UCMS.Model.Enum.AttachmentType.Public,
                        Name = Guid.NewGuid() + FileExtension
                    };
                    oUCMSApiClient.Attachment.Upload(attachment);
                    AttachmentList.Add(attachment);
                    attachment = null;
                }

                if (!String.IsNullOrEmpty(RemoveFile))
                {
                    if (File.Exists(Path.Combine(RemoveFile, oFileInfo.Name)))
                    {
                        File.Replace(FilePathName, Path.Combine(RemoveFile, oFileInfo.Name), null);
                    }
                    else
                    {
                        File.Move(FilePathName, Path.Combine(RemoveFile, oFileInfo.Name));
                    }
                    FilePathName = Path.Combine(RemoveFile, oFileInfo.Name);
                }

                if (!string.IsNullOrEmpty(RenameFile))
                {
                    File.Copy(FilePathName, FilePathName.Replace(FileExtension, "") + RenameFile, true);
                    File.Delete(FilePathName);
                }

                oUCMSApiClient.Content.Checkin(ContentID);
                FilePathName = "";
                FileExtension = "";
            }
            catch (Exception ex)
            {
                Common.LogToFile("UploadAttachment_ContentId:" + ContentID + ":" + ex.Message);
            }
            return AttachmentList;
        }

        /// <summary>
        /// If Time of process is slow then Thread will sleep in Common.MaxTimeUpdate
        /// </summary>
        /// <param name="DateValue"></param>
        /// <param name="DateNow"></param>
        private void SetThreadSleep(DateTime DateValue, DateTime DateNow)
        {
            if (DateValue.AddMilliseconds(Common.MaxTimeUpdate) < DateNow)
            {
                Thread.Sleep(Common.MaxTimeUpdate);
            }
        }

        /// <summary>
        /// Delete all file in folder and folder
        /// </summary>
        /// <param name="folderRoot"></param>
        public void DeleteFileInDirectory(String folderRoot)
        {
            if (!String.IsNullOrEmpty(folderRoot))
            {
                foreach (String itemPath in Directory.GetDirectories(folderRoot))
                {
                    DeleteFileInDirectory(itemPath);
                }
                foreach (var TiffFile in Directory.GetFiles(folderRoot))
                {
                    File.Delete(TiffFile);
                }
                Directory.Delete(folderRoot);
            }
        }

        public String getContentName(string ContentTypeName, string BranchName, string LibraryName, BatchNamingProfile oBatchNamingProfile)
        {
            String tempName = "";
            int indexBatch = 0;
            if (oBatchNamingProfile != null && oBatchNamingProfile.Enabled && oBatchNamingProfile.BatchNamingSettings != null)
            {
                BatchNamingSetting oNaming = oBatchNamingProfile.BatchNamingSettings.SingleOrDefault(x => x.DocumentTypeName.Equals(ContentTypeName));
                if (oNaming != null)
                {
                    foreach (SourceField oSourceField in oNaming.BatchNamingTemplate)
                    {
                        indexBatch++;
                        switch (oSourceField.Type)
                        {
                            case SourceFieldType.DocVariable:
                                if (oSourceField.StaticName == ScanCommon.ConstantString.DocType)
                                {
                                    tempName += oSourceField.Type;
                                }
                                else if (oSourceField.StaticName == ScanCommon.ConstantString.DocName)
                                {
                                    tempName += ContentTypeName;
                                }
                                else if (oSourceField.StaticName == ScanCommon.ConstantString.DocSequence)
                                {
                                    tempName += indexBatch.ToString();
                                }
                                break;
                            case SourceFieldType.System:
                                if (oSourceField.StaticName == ScanCommon.ConstantString.SystemMachineName)
                                {
                                    tempName += System.Environment.MachineName;
                                }
                                else if (oSourceField.StaticName == ScanCommon.ConstantString.SystemUserName)
                                {
                                    tempName += Common.Username; //System.Environment.UserName
                                }
                                else if (oSourceField.StaticName == ScanCommon.ConstantString.SystemDate)
                                {
                                    tempName += "{TypeDate}";//DateTime.Now.ToString(oNaming.DateFormat);
                                    TypeDate = oNaming.DateFormat;
                                }
                                else if (oSourceField.StaticName == ScanCommon.ConstantString.SystemTime)
                                {
                                    tempName += "TypeTime"; // DateTime.Now.ToString(oNaming.TimeFormat);
                                    TypeTime = oNaming.TimeFormat;
                                }
                                else if (oSourceField.StaticName.Equals("BranchID"))
                                {
                                    tempName += BranchName;
                                }
                                else
                                {
                                    tempName += oSourceField.DisplayName;
                                }
                                break;
                            case SourceFieldType.TextConstant:
                                tempName += oSourceField.DisplayName;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return tempName;
        }
    }
}
