using IMIP.SharedComponent.GdPictureWrapper;
using IMIP.UniversalScan.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UCMS.RestClient;

namespace UCMS.ImportController.Data
{
    public class ContentProfile
    {
        public UCMSApiClient oUCMSApiClient { get; set; }

        public ContentProfile(UCMSApiClient oUCMSApiClient)
        {
            this.oUCMSApiClient = oUCMSApiClient;
        }

        public void Profile(DataValue oBranch, Model.Folder oFolder, DataValue oWorkflow, DataValue oWorkflowStep, DataValue oContenType, DataValue oContenTypeParent, Dictionary<string, object> oContentField, Dictionary<string, object> oLibraryField, Dictionary<string, object> oContentParent, Dictionary<string, object> oLibraryParent, FileInfo oFileInfo, String RenameFile, String RemoveFile, String keyPrivateData, String Namming)
        {
            Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
            oWorkflowItem.Content = new Model.Content();
            oWorkflowItem.Content.Folder = oFolder;
            oWorkflowItem.Content.Tags = new List<string>();
            oWorkflowItem.Content.Attachments = new List<Model.Attachment>();
            oWorkflowItem.Workflow = new Model.Workflow() { Id = oWorkflow.Key };
            oWorkflowItem.WorkflowStep = new Model.WorkflowStep() { Id = oWorkflowStep.Key };
            oWorkflowItem.State = Model.Enum.WorkflowItemState.Ready;
            oWorkflowItem.Priority = Model.Enum.WorkflowItemPriority.Normal;

            Dictionary<string, object> temp = null;
            Model.ContentPrivateData oContentPrivateData = new Model.ContentPrivateData();
            UniBatch oBatch = new UniBatch();
            UniDocument oUniDocument = new UniDocument();
            DateTime DateValue = DateTime.MinValue;
            try
            {
                if (!String.IsNullOrEmpty(oContenTypeParent.Key))//Branch or Document
                {
                    oWorkflowItem.Content.Name = Namming;
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenTypeParent.Key };
                    temp = oContentParent;
                    temp.Add("BranchID", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryParent;
                }
                else
                {
                    oWorkflowItem.Content.Name = Namming;
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenType.Key };
                    temp = oContentField;
                    temp.Add("BranchID", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryField;
                }

                DateValue = DateTime.Now;
                oWorkflowItem.Content = oUCMSApiClient.Content.Create(oWorkflowItem.Content);
                SetThreadSleep(DateValue, DateTime.Now);

                oWorkflowItem.Content.Attachments = UploadAttachment(oWorkflowItem.Content.Id, oFileInfo, RenameFile, RemoveFile);
                
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
            }
            catch (Exception ex)
            {
                Common.LogToFile("Profile_" + ex.Message);
            }

            oWorkflowItem.Priority = 0;
            oWorkflowItem.State = 0;
            oWorkflowItem.WorkflowStep = null;
            oBatch = null;
            oWorkflowItem.Workflow = null;
            if (oWorkflowItem.Content.Attachments.Count > 0) oWorkflowItem.Content.Attachments.Clear();
            oWorkflowItem.Content.Attachments = null;
            oWorkflowItem.Content.Tags = null;
            oWorkflowItem.Content.Folder = null;
            oWorkflowItem.Content = null;
            oWorkflowItem = null;
            temp = null;
            oContentPrivateData = null;
            oBatch = null;
            oUniDocument = null;
        }

        /// <summary>
        /// Upload Attachmen in database and return list Attachment
        /// </summary>
        /// <param name="ContentID"></param>
        /// <param name="oFileInfo"></param>
        /// <param name="RenameFile"></param>
        /// <param name="RemoveFile"></param>
        /// <returns></returns>
        private List<Model.Attachment> UploadAttachment(String ContentID, FileInfo oFileInfo, String RenameFile, String RemoveFile)
        {
            List<Model.Attachment> AttachmentList = new List<Model.Attachment>();
            try
            {
                String FilePathName = oFileInfo.FullName;

                oUCMSApiClient.Content.Checkout(ContentID);

                if (oFileInfo.Extension == ".pdf")
                {
                    var tempFileSplit = Guid.NewGuid().ToString();
                    Directory.CreateDirectory(tempFileSplit);

                    ImageProcessing.SplitPDF2Tiff(oFileInfo.FullName, tempFileSplit, 300);
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
                        Data = File.ReadAllBytes(oFileInfo.FullName),
                        MIME = "image/universalscan",
                        Type = UCMS.Model.Enum.AttachmentType.Public,
                        Name = Guid.NewGuid() + oFileInfo.Extension
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
                    File.Copy(FilePathName, FilePathName.Replace(oFileInfo.Extension, "") + RenameFile, true);
                }

                oUCMSApiClient.Content.Checkin(ContentID);
                FilePathName = "";
            }
            catch(Exception ex)
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
        private void DeleteFileInDirectory(String folderRoot)
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
    }
}
