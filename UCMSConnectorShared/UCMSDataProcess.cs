#define  DEBUG

/****************************************************************************
'*   (c) Copyright IMIP Technology And Solution Consultancy JSC., 2010. All rights reserved.
'*   Unauthorized use, duplication or distribution is strictly prohibited.
'*****************************************************************************
'*
'*   File:       PrimusBatchProcess.cs
'*
'*   Purpose:    Work with UCMS server
*********************************************************************************/
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Def;
using NLog;
using System.Linq;
using UCMS.Model;
//using GdPicture12;
using UCMS.RestClient;
using IMIP.UniversalScan.Common;
using System.Globalization;
using UCMS.Model.Enum;
using Newtonsoft.Json;
using System.Threading;
using System.IO;

namespace IMIP.UniversalScan.Connector.UCMSConnectorShared
{
    public class SortByExtension : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return Path.GetExtension(x as string).CompareTo(Path.GetExtension(y as string));
        }
    }
    public class SortBatchByDate : IComparer<UniBatch>
    {
        int IComparer<UniBatch>.Compare(UniBatch x, UniBatch y)
        {
            int returnValue = 1;
            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime();
            if (x is UniBatch)
            {
                d1 = ((UniBatch)x).CreationDate;
                d2 = ((UniBatch)y).CreationDate;
            }

            if (x != null && y == null)
            {
                returnValue = 0;
            }
            else if (x == null && y != null)
            {
                returnValue = 0;
            }
            else if (x != null && y != null)
            {
                returnValue = d1.CompareTo(d2);
            }
            return returnValue;
        }
    }

    public class SortByName : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            string name1 = "";
            string name2 = "";

            if (x is UniBatch)
            {
                name1 = ((UniBatch)x).Name;
                name2 = ((UniBatch)y).Name;
            }
            else if (x is UniFormType)
            {
                name1 = ((UniFormType)x).Name;
                name2 = ((UniFormType)y).Name;
            }
            else if (x is BPProcessStep)
            {
                name1 = ((BPProcessStep)x).Name;
                name2 = ((BPProcessStep)y).Name;
            }
            else if (x is BPProcess)
            {
                name1 = ((BPProcess)x).Name;
                name2 = ((BPProcess)y).Name;
            }
            else if (x is BPClient)
            {
                name1 = ((BPClient)x).Name;
                name2 = ((BPClient)y).Name;
            }
            else if (x is String)
            {
                name1 = ((String)x);
                name2 = ((String)y);
            }
            else if (x is Branch)
            {
                name1 = ((Branch)x).Name;
                name2 = ((Branch)y).Name;
            }

            return name1.CompareTo(name2);
        }
    }

    internal class DownloadHelper
    {
        WorkflowItem _workflowItem;
        UniBatch _oBatch;
        string _strFolderStorage;
        UCMSApiClient _UCMSApiClient;

        public DownloadHelper(UCMSApiClient UCMSApiClient, WorkflowItem workflowItem, UniBatch oBatch, string strFolderStorage)
        {
            _workflowItem = workflowItem;
            _oBatch = oBatch;
            _strFolderStorage = strFolderStorage;
            _UCMSApiClient = UCMSApiClient;
        }

        public void Download()
        {
            try
            {
                foreach (var oDoc in _oBatch.Docs)
                {
                    GetFile(oDoc);
                }

                //Get attachment file
                GetFile(_oBatch);
            }
            catch (Exception oexc)
            {
                LogManager.GetCurrentClassLogger().Error("Download attachment failed. " + oexc.Message);
                LogManager.GetCurrentClassLogger().Error(oexc.StackTrace);
            }
        }

        private void GetFile(UniDocument oDoc)
        {
            foreach (var page in oDoc.Pages)
            {
                string fileName = Path.GetFileName(page.FullFileName);
                var attachment = _workflowItem.Content.Attachments.Where(a => a.Name == fileName).FirstOrDefault();

                if (attachment != null)
                {
                    byte[] byteData = _UCMSApiClient.Attachment.Download(attachment.Id);

                    //Save to file
                    string sFileName = Path.Combine(_strFolderStorage, attachment.Name);
                    File.WriteAllBytes(sFileName, byteData);
                }
            }
        }
    }

    public class UCMSBatchProcess
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string sLicenseModuleID;
        private UCMSApiClient _UCMSApiClient;
        const string AllBranch = "all";

        string _UserName, _Password, _APIEndPoint, _AuthorizationServerEndpoint, _Domain;
        Token _token = null;

        public void SetCredential(string userName, string password, string sDomain)
        {
            _UserName = userName;
            _Password = password;
            _Domain = sDomain;
        }

        public void SetCredential(string tokenstring)
        {
            _token = new Token();
            _token.access_token = tokenstring;
        }

        public string UserName
        {
            get { return _UserName; }
        }

        public bool Impersonate
        {
            get { return (string.IsNullOrEmpty(_Password)); }
        }

        public string Domain
        {
            get { return _Domain; }
        }

        public string Password
        {
            get { return _Password; }
        }


        public UCMSApiClient RestClient
        {
            get { return _UCMSApiClient; }
            set { _UCMSApiClient = value; }
        }

        public UCMSBatchProcess(string licenseModuleID)
        {
            sLicenseModuleID = licenseModuleID;
        }

        public UCMSBatchProcess Clone()
        {
            UCMSBatchProcess oProcess = new UCMSBatchProcess(sLicenseModuleID);
            oProcess.SetCredential(_UserName, _Password, _Domain);
            return oProcess;
        }

        public UserInfo InitializeSession(string apiEndpoint, string authorizationServerEndpoint)
        {
            UserInfo _userInfo = null;
            _APIEndPoint = apiEndpoint;
            _AuthorizationServerEndpoint = authorizationServerEndpoint;

            if (_token != null)
            {
                _UCMSApiClient = new UCMSApiClient(_token, apiEndpoint, authorizationServerEndpoint);
                _userInfo = _UCMSApiClient.User.GetMyProfile();
                _UserName = _userInfo.UserName;
                _Domain = "";
            }
            else
            {
                string ucmsUserName = _UserName;
                if (!string.IsNullOrWhiteSpace(_Domain))
                {
                    ucmsUserName = _Domain + "\\" + _UserName;
                }
                _UCMSApiClient = new UCMSApiClient(ucmsUserName, _Password, apiEndpoint, authorizationServerEndpoint);

                if (!_UCMSApiClient.Login())
                {
                    throw new Exception("Cannot login to UCMS server");
                }
                _userInfo = _UCMSApiClient.User.GetMyProfile();
            }

            return _userInfo;
        }

        private int CountPagesAndMedia(UniBatch uniBatch)
        {
            int Count = 0;

            Count = uniBatch.Pages.Count;

            foreach (UniDocument oDoc in uniBatch.Docs)
            {
                Count = Count + oDoc.Pages.Count;
            }

            return Count;
        }

        private void GetFieldValue(UniBatch oBatch, List<LibraryField> libraryFieldDefs, ref Dictionary<string, object> libraryFieldValues, ContentType contentType, ref Dictionary<string, object> fieldValues)
        {
            List<UniField> oList = new List<UniField>();
            foreach (UniField oField in oBatch.Fields)
                oList.Add(oField);

            //process libary fields first
            foreach (var updateField in oBatch.Fields)
            {
                //Get datatype
                var fiedDef = libraryFieldDefs.Where(f => f.Name == updateField.Name).FirstOrDefault();
                if (fiedDef != null)
                {
                    if (fiedDef.DataType == DataType.Date)
                    {
                        DateTime oDateTime;
                        if (DateTime.TryParse(updateField.Value.Replace(" ", ""), CultureInfo.InvariantCulture, DateTimeStyles.None, out oDateTime))
                        {
                            libraryFieldValues.Add(updateField.Name, oDateTime);
                        }
                        else
                        {
                            libraryFieldValues.Add(updateField.Name, updateField.Value);
                        }
                    }
                    else if (fiedDef.DataType == DataType.Checkbox)
                    {
                        bool bRet = false;
                        if (bool.TryParse(updateField.Value, out bRet))
                            libraryFieldValues.Add(updateField.Name, bRet);
                        else
                            libraryFieldValues.Add(updateField.Name, updateField.Value);
                    }
                    else if (fiedDef.DataType == DataType.Number)
                    {
                        float fRet = 0;
                        if (float.TryParse(updateField.Value, out fRet))
                            libraryFieldValues.Add(updateField.Name, fRet);
                        else
                            libraryFieldValues.Add(updateField.Name, updateField.Value);
                    }
                    else
                    {
                        libraryFieldValues.Add(updateField.Name, updateField.Value);
                    }

                    oList.Remove(updateField);
                }
            }


            foreach (var updateField in oList)
            {
                //Get datatype
                var fiedDef = contentType.Fields.Where(f => f.Name == updateField.Name).FirstOrDefault();
                if (fiedDef != null)
                {
                    if (fiedDef.DataType == DataType.Date)
                    {
                        DateTime oDateTime;
                        if (DateTime.TryParse(updateField.Value.Replace(" ", ""), CultureInfo.InvariantCulture, DateTimeStyles.None, out oDateTime))
                        {
                            fieldValues.Add(updateField.Name, oDateTime);
                        }
                        else
                        {
                            fieldValues.Add(updateField.Name, updateField.Value);
                        }
                    }
                    else if (fiedDef.DataType == DataType.Checkbox)
                    {
                        bool bRet = false;
                        if (bool.TryParse(updateField.Value, out bRet))
                            fieldValues.Add(updateField.Name, bRet);
                        else
                            fieldValues.Add(updateField.Name, updateField.Value);
                    }
                    else if (fiedDef.DataType == DataType.Number)
                    {
                        float fRet = 0;
                        if (float.TryParse(updateField.Value, out fRet))
                            fieldValues.Add(updateField.Name, fRet);
                        else
                            fieldValues.Add(updateField.Name, updateField.Value);
                    }
                    else
                    {
                        fieldValues.Add(updateField.Name, updateField.Value);
                    }
                }
            }
        }

        public bool CreateBatch(UniBatch oBatch, Library oLibrary)
        {
            logger.Debug("Start Create Batch " + oBatch.Name);
            string sTempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            string contentID = "";
            string workflowItemID = "";
            ContentType contentType = null;
            WorkflowStep workflowStep = null;
            List<LibraryField> libraryFieldDefs = new List<LibraryField>();

            try
            {
                DateTime oStart = DateTime.Now;

                CheckAvailableLicense(oBatch);

                if (oLibrary == null)
                {
                    //Get folder
                    var folders = _UCMSApiClient.Folder.GetFolders("", oBatch.ClientName);//.GetFolders();
                    Folder folder = folders.Items.FirstOrDefault(lib => lib.Name.Equals(oBatch.ClientName, StringComparison.CurrentCultureIgnoreCase));
                    if (folder == null)
                    {
                        throw new Exception($"Cannot find the '{oBatch.ClientName}' library");
                    }

                    Library library = _UCMSApiClient.Folder.GetLibrary(folder.Id);

                    libraryFieldDefs = ((Library)library).Fields;

                    //Get workflow
                    var workflows = library.Workflows; // _UCMSApiClient.Workflow.GetAll(library.Id);
                    
                    var workflow = workflows.FirstOrDefault(w => w.Name.Equals(oBatch.ProcessName, StringComparison.CurrentCultureIgnoreCase));
                    if (workflow == null)
                    {
                        throw new Exception($"Cannot find the '{oBatch.ProcessName}' workflow");
                    }

                    //get worklow step
                    workflowStep = workflow.Steps.FirstOrDefault(s => s.Name.Equals(oBatch.ProcessStepName, StringComparison.CurrentCultureIgnoreCase));
                    if (workflowStep == null)
                    {
                        throw new Exception($"Cannot find the '{oBatch.ProcessStepName}' workflow step");
                    }


                    //Get content type
                    List<ContentType> contentTypes = //_UCMSApiClient.ContentType.GetByFolderId(library.Id);
                        library.ContentTypes;

                    contentType = contentTypes.FirstOrDefault(t => t.Name.Equals(oBatch.FormTypeName, StringComparison.CurrentCultureIgnoreCase));
                    if (contentType == null)
                    {
                        throw new Exception($"Cannot find the '{oBatch.FormTypeName}' content type");
                    }
                }
                else
                {
                    contentType = oLibrary.ContentTypes.FirstOrDefault(t => t.Name.Equals(oBatch.FormTypeName, StringComparison.CurrentCultureIgnoreCase));
                    Workflow workflow = oLibrary.Workflows.FirstOrDefault(w => w.Name.Equals(oBatch.ProcessName, StringComparison.CurrentCultureIgnoreCase));
                    workflowStep = workflow.Steps.FirstOrDefault(t => t.Name.Equals(oBatch.ProcessStepName));
                    libraryFieldDefs = oLibrary.Fields;
                }
               
                logger.Debug(string.Format("Finish get library metadata in {0} sec", DateTime.Now.Subtract(oStart).TotalSeconds));
                oStart = DateTime.Now; 

                //Get fields values
                Dictionary<string, object> fieldValues = new Dictionary<string, object>();
                Dictionary<string, object> libraryFieldValues = new Dictionary<string, object>();

                GetFieldValue(oBatch, libraryFieldDefs, ref libraryFieldValues, contentType, ref fieldValues);

                //add the branch id and other fields for web scan
                fieldValues.Add(Global.FiledName_BranchId, oBatch.BranchID);

                //Get attachment file
                Directory.CreateDirectory(sTempFolder);

                var content = new Content
                {
                    Name = oBatch.Name,
                    ContentSource = "Web scan",
                    Folder = contentType.Folder,
                    ContentType = contentType,
                    Tags = new List<string>(),
                    Values = fieldValues,
                    LibraryFieldValues = libraryFieldValues,
                    Attributes = new Dictionary<string, object>()
                };

                logger.Debug("Create content " + oBatch.Name);
                content = _UCMSApiClient.Content.Create(content, true);
                contentID = content.Id;

                logger.Debug(string.Format("Finish create empty content in {0} sec", DateTime.Now.Subtract(oStart).TotalSeconds));
                oStart = DateTime.Now;

                //Upload attachment
                UploadDocPages(content.Id, oBatch);
                foreach (var oDoc in oBatch.Docs)
                {
                    UploadDocPages(contentID, oDoc);
                }

                logger.Debug(string.Format("Finish upload pages in {0} sec", DateTime.Now.Subtract(oStart).TotalSeconds));
                oStart = DateTime.Now;

                logger.Debug("Setprivatedata " + oBatch.Name);
                ContentPrivateData uscPrivateData = new ContentPrivateData
                {
                    Key = Global.USC_BATCH_PRIVATEDATA,
                    Value = Helper.SerializeObjectToString(typeof(UniBatch), oBatch)
                };
                _UCMSApiClient.Content.SetPrivateData(content.Id, uscPrivateData);

                logger.Debug(string.Format("Finish set private data in {0} sec", DateTime.Now.Subtract(oStart).TotalSeconds));
                oStart = DateTime.Now;

                var workflowItem = new WorkflowItem
                {
                    Content = content,
                    WorkflowStep = workflowStep
                };

                workflowItemID = workflowItem.Id;
                workflowItem = _UCMSApiClient.WorkflowItem.Insert(workflowItem, true);
                logger.Debug(string.Format("Finish insert content and workflow item in {0} sec", DateTime.Now.Subtract(oStart).TotalSeconds));

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                //Try to delete workitem and content
                if (workflowItemID != "")
                {
                    try
                    {
                        _UCMSApiClient.WorkflowItem.Delete(workflowItemID);
                    }
                    catch (Exception deleteWorkItemEx)
                    {
                        logger.Error(deleteWorkItemEx);
                    }

                }

                if (contentID != "")
                {
                    try
                    {
                        _UCMSApiClient.Content.Delete(contentID);
                    }
                    catch (Exception deleteContentEx)
                    {
                        logger.Error(deleteContentEx);
                    }
                }

                throw ex;
            }
            finally
            {
                if (Directory.Exists(sTempFolder))
                {
                    Directory.Delete(sTempFolder, true);
                }
            }
        }

        public void UpdateBatch(UniBatch processingBatch, Library oLibrary)
        {
            UniBatch xmlBatch = new UniBatch();
            WorkflowItem workflowItem = null;
            try
            {
                workflowItem = _UCMSApiClient.WorkflowItem.GetById(processingBatch.ExternalID);

                //Get fields values
                Dictionary<string, object> fieldValues = new Dictionary<string, object>();
                Dictionary<string, object> libraryFieldValues = new Dictionary<string, object>();
                List<LibraryField> libraryFieldDefs = new List<LibraryField>();

                if (oLibrary != null)
                    libraryFieldDefs = oLibrary.Fields;
                else
                    libraryFieldDefs = _UCMSApiClient.Folder.GetLibrary(workflowItem.Content.ContentType.Folder.Id).Fields;

                GetFieldValue(processingBatch, libraryFieldDefs, ref libraryFieldValues, workflowItem.Content.ContentType, ref fieldValues);

                //add the branch id and other fields for web scan
                fieldValues.Add(Global.FiledName_BranchId, processingBatch.BranchID);

                workflowItem.Content.LibraryFieldValues = libraryFieldValues;
                workflowItem.Content.Values = fieldValues;

                //get original xmlBatch
                //update attachment
                string xmlString = _UCMSApiClient.Content.GetPrivateData(workflowItem.Content.Id, Global.USC_BATCH_PRIVATEDATA);
                UniBatch oBatch = Helper.DeSerializeObjectFromString(xmlString, typeof(UniBatch)) as UniBatch;
                List<UniPage> lstOriginalPages = new List<UniPage>();
                lstOriginalPages.AddRange(oBatch.Pages);
                foreach (var subDoc in oBatch.Docs)
                {
                    lstOriginalPages.AddRange(subDoc.Pages);
                }

                List<UniPage> lstNewPages = new List<UniPage>();
                lstNewPages.AddRange(processingBatch.Pages);
                foreach (var subDoc in processingBatch.Docs)
                {
                    lstNewPages.AddRange(subDoc.Pages);
                }

                //check available license
                UCMS.LicenseSvc.Models.LicenseConsumeInfo oLicenseConsumeInfo = new UCMS.LicenseSvc.Models.LicenseConsumeInfo();
                oLicenseConsumeInfo.ModuleId = sLicenseModuleID;
                oLicenseConsumeInfo.Count = lstNewPages.Count;
                if (!_UCMSApiClient.License.IsAvailableToConsume(oLicenseConsumeInfo))
                    throw new ApplicationException("not enough license available to complete the operation");

                //Check to insert/update
                foreach (var page in lstNewPages)
                {
                    var originalPage = lstOriginalPages.FirstOrDefault(p => p.ID == page.ID);
                    if (originalPage == null)
                    {
                        //insert new page
                        UploadDocPage(workflowItem.Content.Id, page);
                    }
                    else
                    {
                        if (page.IsRescan)
                        {
                            //update content
                            var attchment = workflowItem.Content.Attachments.FirstOrDefault(a => a.Name.Equals(Path.GetFileName(page.FullFileName), StringComparison.CurrentCultureIgnoreCase));
                            if (attchment != null)
                            {
                                _UCMSApiClient.Attachment.Delete(attchment.Id);
                                UploadDocPage(workflowItem.Content.Id, page);
                            }
                        }
                    }
                }

                //consume license
                _UCMSApiClient.License.Consume(oLicenseConsumeInfo);

                //clear rejection infor
                processingBatch.Rejected = false;
                processingBatch.RejectedNote = "";

                ContentPrivateData uscPrivateData = new ContentPrivateData
                {
                    Key = Global.USC_BATCH_PRIVATEDATA,
                    Value = Helper.SerializeObjectToString(typeof(UniBatch), processingBatch)
                };
                _UCMSApiClient.Content.SetPrivateData(workflowItem.Content.Id, uscPrivateData);

                //Check to delete
                foreach (var page in lstOriginalPages)
                {
                    var newPage = lstNewPages.FirstOrDefault(p => p.ID == page.ID);
                    if (newPage == null)
                    {
                        //update content
                        var attchment = workflowItem.Content.Attachments.FirstOrDefault(a => a.Name.Equals(Path.GetFileName(page.FullFileName), StringComparison.CurrentCultureIgnoreCase));
                        if (attchment != null)
                        {
                            _UCMSApiClient.Attachment.Delete(attchment.Id);
                        }
                    }
                }        

                _UCMSApiClient.Content.Update(workflowItem.Content);
                workflowItem.ProcessedValue = "";
                _UCMSApiClient.WorkflowItem.CheckIn(workflowItem);
            }
            catch
            {
                throw;
            }
        }

        private void CheckAvailableLicense(UniBatch oBatch)
        {
            int totalPages = oBatch.Pages.Count;
            
            foreach (var oDoc in oBatch.Docs)
            {
                totalPages += oDoc.Pages.Count;
            }

            //check available license
            UCMS.LicenseSvc.Models.LicenseConsumeInfo oLicenseConsumeInfo = new UCMS.LicenseSvc.Models.LicenseConsumeInfo();
            oLicenseConsumeInfo.ModuleId = sLicenseModuleID;
            oLicenseConsumeInfo.Count = totalPages;
            if (!_UCMSApiClient.License.IsAvailableToConsume(oLicenseConsumeInfo))
                throw new ApplicationException("not enough license available to complete the operation");
        }

        private void UploadDocPages(string contentID, UniDocument oDoc)
        {
            //check available license
            UCMS.LicenseSvc.Models.LicenseConsumeInfo oLicenseConsumeInfo = new UCMS.LicenseSvc.Models.LicenseConsumeInfo();
            oLicenseConsumeInfo.ModuleId = sLicenseModuleID;
            oLicenseConsumeInfo.Count = oDoc.Pages.Count;
            if (! _UCMSApiClient.License.IsAvailableToConsume(oLicenseConsumeInfo))
                throw new ApplicationException("not enough license available to complete the operation");

            foreach (var page in oDoc.Pages)
            {
                UploadDocPage(contentID, page);
            }

            //consume license
            _UCMSApiClient.License.Consume(oLicenseConsumeInfo);
        }

        private void UploadDocPage(string contentID, UniPage page, bool countLicense = false)
        {
            var uploadFile = page.FullFileName;
            logger.Debug("Attach file: " + Path.GetFileName(uploadFile));

            //var bytes = File.ReadAllBytes(uploadFile);
            _UCMSApiClient.Attachment.Upload(new Attachment
            {
                ContentId = contentID,
                Data = File.ReadAllBytes(uploadFile),
                MIME = "image/universalscan",
                Type = UCMS.Model.Enum.AttachmentType.Public,
                Name = Path.GetFileName(uploadFile)
            });

            if (countLicense)
            {
                UCMS.LicenseSvc.Models.LicenseConsumeInfo oLicenseConsumeInfo = new UCMS.LicenseSvc.Models.LicenseConsumeInfo();
                oLicenseConsumeInfo.ModuleId = sLicenseModuleID;
                oLicenseConsumeInfo.Count = 1;
                _UCMSApiClient.License.Consume(oLicenseConsumeInfo);
            }
        }

        

        public List<UniBatch> GetBatchesWaittingRescan(string strBranchID, string strProcessStepName, string strProcessName, string strClientName, string strContentName, string sScanUser, int nPageIndex = 0, string sStepID = null)
        {
            DateTime dtStart = DateTime.Now;
            try
            {
                if (sStepID == null)
                {
                    //Get folder
                    var rootfolders = _UCMSApiClient.Folder.GetFolders("", strClientName);
                    var oFolder = rootfolders.Items.FirstOrDefault(lib => lib.Name.Equals(strClientName, StringComparison.CurrentCultureIgnoreCase));
                    if (oFolder == null)
                    {
                        throw new Exception($"Cannot find the '{strClientName}' library");
                    }

                    Library oLibrary = _UCMSApiClient.Folder.GetLibrary(oFolder.Id);

                    //Get workflow
                    var workflows = oLibrary.Workflows;
                    var workflow = workflows.FirstOrDefault(w => w.Name.Equals(strProcessName, StringComparison.CurrentCultureIgnoreCase));
                    if (workflow == null)
                    {
                        throw new Exception($"Cannot find the '{strProcessName}' workflow");
                    }

                    var step = workflow.Steps.FirstOrDefault(s => s.Name.Equals(strProcessStepName, StringComparison.CurrentCultureIgnoreCase));
                    if (step == null)
                    {
                        throw new Exception($"Cannot find the '{strProcessStepName}' workflow step");
                    }
                    sStepID = step.Id;
                }


                WorkflowItemFilter oFilter = new WorkflowItemFilter();
                oFilter.StepId = sStepID;
                oFilter.State = "Ready";
                oFilter.Keyword = strContentName;
                oFilter.DetailLevel = GetDetailLevel.Base;
                if (nPageIndex >= 0)
                {
                    oFilter.PageSize = 25;
                    oFilter.ItemIndex = nPageIndex*25;
                }


                if (!string.IsNullOrEmpty(strBranchID) && (strBranchID.ToLower() != AllBranch))
                {
                    oFilter.ContentFields = new Dictionary<string, object>();
                    oFilter.ContentFields.Add(Global.FiledName_BranchId, strBranchID);
                }

                var result = _UCMSApiClient.WorkflowItem.GetItems(oFilter);

                //logger.Debug(string.Format("time to get {0} items total {1}", result.Items.Count, DateTime.Now.Subtract(dtStart).TotalSeconds));
                
                List<UniBatch> lstReturn = new List<UniBatch>();
                foreach (var item in result.Items)
                {
                    UniBatch oSimplebatch = new UniBatch();
                    
                    try
                    {
                        oSimplebatch.Name = item.Content.Name;
                        oSimplebatch.ExternalID = item.Id;
                        //special note here. The ID is the workflowitem id
                        oSimplebatch.ID = item.Content.Id;
                        oSimplebatch.ProcessName = strProcessName;
                        oSimplebatch.ProcessStepName = strProcessStepName;
                        oSimplebatch.ClientName = strClientName;
                        oSimplebatch.RejectedNote = "";
                        oSimplebatch.FormTypeName = item.Content.ContentType.Name;
                        oSimplebatch.CreationDate = item.Content.CreatedDate;
                        oSimplebatch.CreationUser = item.Content.Owner.UserName;
                    }
                    catch
                    {
                        // do watch? or throw exception
                        //throw;
                    }

                    lstReturn.Add(oSimplebatch);
                }

                lstReturn.Sort(new SortBatchByDate());
                return lstReturn;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UndoCheckout(string contentID)
        {
            if (contentID != "")
            {
                WorkflowItem workflowItem = _UCMSApiClient.WorkflowItem.GetByContentId(contentID);
                if (workflowItem != null)
                    _UCMSApiClient.WorkflowItem.UndoCheckout(workflowItem.Id);
                else throw new ApplicationException(string.Format("Cannot find workflow item for content id {0}", contentID));
            }
        }

        public void DeleteBatch(string contentID, string extra = null)
        {
            WorkflowItem workflowItem = _UCMSApiClient.WorkflowItem.GetByContentId(contentID);
            if (workflowItem != null)
            {
                _UCMSApiClient.Content.Delete(contentID);
                _UCMSApiClient.WorkflowItem.CheckIn(workflowItem);
            }
            else throw new ApplicationException(string.Format("Cannot find workflow item for content id {0}", contentID));
        }

        //public void RejectBatch(UniBatch rejectedBatch)
        //{
        //    string workflowItemID = rejectedBatch.ID;
        //    if (workflowItemID == "")
        //    {
        //        return;
        //    }

        //    UniBatch xmlBatch = new UniBatch();
        //    WorkflowItem workflowItem = null;
        //    try
        //    {
        //        workflowItem = _UCMSApiClient.WorkflowItem.GetById(workflowItemID);

        //        //get xmlBatch infor
        //        string xmlString = _UCMSApiClient.Content.GetPrivateData(workflowItem.Content.Id, Global.USC_BATCH_PRIVATEDATA);
        //        UniBatch oBatch = Helper.DeSerializeObjectFromString(xmlString, typeof(UniBatch)) as UniBatch;

        //        //Get fields values
        //        Dictionary<string, object> fieldValues = new Dictionary<string, object>();
        //        Dictionary<string, object> libraryFieldValues = new Dictionary<string, object>();

        //        List<LibraryField> libraryFieldDefs = _UCMSApiClient.Folder.GetLibrary(workflowItem.Content.ContentType.Folder.Id).Fields;
        //        GetFieldValue(rejectedBatch, libraryFieldDefs, ref libraryFieldValues, workflowItem.Content.ContentType, ref fieldValues);

        //        workflowItem.Content.LibraryFieldValues = libraryFieldValues;
        //        workflowItem.Content.Values = fieldValues;

        //        //Update data (reject note, document, fields)
        //        oBatch.Rejected = true;
        //        oBatch.RejectedNote = rejectedBatch.RejectedNote;
        //        oBatch.Fields = rejectedBatch.Fields;
        //        oBatch.Docs = rejectedBatch.Docs;

        //        //Update private data
        //        ContentPrivateData uscPrivateData = new ContentPrivateData
        //        {
        //            Key = Global.USC_BATCH_PRIVATEDATA,
        //            Value = Helper.SerializeObjectToString(typeof(UniBatch), oBatch)
        //        };
        //        _UCMSApiClient.Content.SetPrivateData(workflowItem.Content.Id, uscPrivateData);

        //        workflowItem.ProcessedValue = "reject";
        //        _UCMSApiClient.WorkflowItem.CheckIn(workflowItem);

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public void ProcessBatch(UniBatch processingBatch, Library oLibrary, bool bApprove)
        {
            UniBatch xmlBatch = new UniBatch();
            WorkflowItem workflowItem = null;
            try
            {
                workflowItem = _UCMSApiClient.WorkflowItem.GetById(processingBatch.ExternalID);

                //Get fields values
                Dictionary<string, object> fieldValues = new Dictionary<string, object>();
                Dictionary<string, object> libraryFieldValues = new Dictionary<string, object>();
                List<LibraryField> libraryFieldDefs = new List<LibraryField>();

                if (oLibrary != null)
                    libraryFieldDefs = oLibrary.Fields;
                else
                    libraryFieldDefs = _UCMSApiClient.Folder.GetLibrary(workflowItem.Content.ContentType.Folder.Id).Fields;

                GetFieldValue(processingBatch, libraryFieldDefs, ref libraryFieldValues, workflowItem.Content.ContentType, ref fieldValues);

                //add the branch id and other fields for web scan
                fieldValues.Add(Global.FiledName_BranchId, processingBatch.BranchID);

                workflowItem.Content.LibraryFieldValues = libraryFieldValues;
                workflowItem.Content.Values = fieldValues;

                if (bApprove)
                {
                    processingBatch.Rejected = false;
                    processingBatch.RejectedNote = "";
                }

                ContentPrivateData uscPrivateData = new ContentPrivateData
                {
                    Key = Global.USC_BATCH_PRIVATEDATA,
                    Value = Helper.SerializeObjectToString(typeof(UniBatch), processingBatch )
                };
                _UCMSApiClient.Content.SetPrivateData(workflowItem.Content.Id, uscPrivateData);


                workflowItem.ProcessedValue = bApprove ? "approve" : "reject";
                _UCMSApiClient.Content.Update(workflowItem.Content);
                _UCMSApiClient.WorkflowItem.CheckIn(workflowItem);
            }
            catch
            {
                throw;
            }
        }

        private string GetMimeType(string strFilePath)
        {
            string strExtension = Path.GetExtension(strFilePath);
            string strFormatName = "";

            switch (strExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    strFormatName = "image/jpeg";
                    break;

                case ".tif":
                case ".tiff":
                    strFormatName = "image/tiff";
                    break;

                case ".txt":
                    strFormatName = "text/plain";
                    break;

                case ".pdf":
                    strFormatName = "application/pdf";
                    break;

                case ".bmp":
                    strFormatName = "image/bmp";
                    break;

                default:
                    strFormatName = "application/octet-stream";
                    break;

            }

            return strFormatName;
        }

        private UniFormType GetFormType(List<UniFormType> formTypes, string formTypeName)
        {
            foreach (UniFormType formType in formTypes)
            {
                if (formType.Name == formTypeName)
                    return formType;
            }

            return new UniFormType();
        }

        public string[] GetContentAttachmentByInternalId(string batchID, string strFolderStorage)
        {
            ArrayList attachmentList = new ArrayList();

                Content oContent = _UCMSApiClient.Content.GetById(batchID);

                // Create storage folder
                if (!Directory.Exists(strFolderStorage))
                    Directory.CreateDirectory(strFolderStorage);

            foreach (Attachment attachment in oContent.Attachments)
            {
                if (attachment.Type == AttachmentType.Public)
                {
                    byte[] byteData = _UCMSApiClient.Attachment.Download(attachment.Id);
                    //Save to file
                    string sFileName = Path.Combine(strFolderStorage, attachment.Name);
                    File.WriteAllBytes(sFileName, byteData);
                    attachmentList.Add(attachment.Name);
                }
            }

            attachmentList.Sort(new SortByExtension());
            return attachmentList.ToList<string>().ToArray();
        }

        public UniBatch CheckoutBatchContent(string contentID, string strFolderStorage, List<BPClient> Clients = null)
        {
            UniBatch oBatch = new UniBatch();
            bool bCheckedOut = false;
            WorkflowItem workflowItem = null;
            DateTime dtStart = DateTime.Now;
            UniParameterSet ParameterSet = null;
            try
            {

                workflowItem = _UCMSApiClient.WorkflowItem.GetByContentId(contentID);
                if (workflowItem == null)
                    throw new ApplicationException(string.Format("Cannot find workflow item for content id {0}", contentID));

                if (Clients != null)
                {
                    foreach (BPClient oClient in Clients)
                        foreach (BPProcess oProcess in oClient.bpProcesses)
                            foreach (BPProcessStep oStep in oProcess.bpProcessSteps)
                                if (oStep.ID == workflowItem.WorkflowStep.Id)
                                {
                                    ParameterSet = oStep.uniParameterSet;
                                    break;
                                }
                }

                if (ParameterSet == null)
                {
                    //Get settep setting
                    var stepSetting = _UCMSApiClient.Workflow.GetStepSetting(workflowItem.WorkflowStep.Id);
                    //Get setting
                    ParameterSet = (UniParameterSet)Helper.DeSerializeObjectFromString(stepSetting.Setting, typeof(UniParameterSet)) as UniParameterSet;

                    if (ParameterSet.SettingReference != Global.Own_Setting)
                    {
                        var workFlow = _UCMSApiClient.Workflow.GetById(workflowItem.WorkflowStep.WorkflowId);
                        foreach (var step in workFlow.Steps)
                        {
                            if (step.Name.Equals(ParameterSet.SettingReference))
                            {
                                //Get settep setting
                                stepSetting = _UCMSApiClient.Workflow.GetStepSetting(step.Id);

                                ParameterSet = (UniParameterSet)Helper.DeSerializeObjectFromString(stepSetting.Setting, typeof(UniParameterSet)) as UniParameterSet;
                                break;
                            }
                        }
                    }
                }

                //checkout
                _UCMSApiClient.WorkflowItem.CheckOut(workflowItem);
                bCheckedOut = true;

                // Create storage folder
                if (!Directory.Exists(strFolderStorage))
                    Directory.CreateDirectory(strFolderStorage);

                //get xmlBatch infor
                string xmlString = _UCMSApiClient.Content.GetPrivateData(workflowItem.Content.Id, Global.USC_BATCH_PRIVATEDATA);
                oBatch = Helper.DeSerializeObjectFromString(xmlString, typeof(UniBatch)) as UniBatch;

                //Get attachment file
                GetFile(workflowItem.Content, oBatch, strFolderStorage);
                foreach (var oDoc in oBatch.Docs)
                {
                    GetFile(workflowItem.Content, oDoc, strFolderStorage);
                }
                oBatch.Name = workflowItem.Content.Name;
                oBatch.ExternalID = workflowItem.Id;
                oBatch.ID = workflowItem.Content.Id;
                oBatch.CreationDate = workflowItem.Content.CreatedDate;
                oBatch.UniParameterSet = ParameterSet;
                oBatch.FormTypes = ParameterSet.DocumentTypeProfileSetting.UniFormtypeList;
                oBatch.UniFormType = oBatch.FormTypes.Where(f => f.Name == oBatch.FormTypeName).FirstOrDefault();
                return oBatch;
            }
            catch
            {
                if (bCheckedOut)
                {
                    try
                    {
                        _UCMSApiClient.WorkflowItem.UndoCheckout(workflowItem.Id);
                    }
                    catch (Exception)
                    {
                        //throw or not
                        //throw;
                    }
                }

                throw;
            }
            finally
            {
                logger.Debug(string.Format("Total time to checkout the content id {0} in {1} ms", oBatch.Name, DateTime.Now.Subtract(dtStart).TotalMilliseconds));
            }
        }



        public UniBatch CheckoutBatchContentAsync(string contentID, string strFolderStorage, List<BPClient> Clients = null)
        {
            UniBatch oBatch = new UniBatch();
            bool bCheckedOut = false;
            WorkflowItem workflowItem = null;
            DateTime dtStart = DateTime.Now;
            UniParameterSet ParameterSet = null;
            try
            {

                workflowItem = _UCMSApiClient.WorkflowItem.GetByContentId(contentID);
                if (workflowItem == null)
                    throw new ApplicationException(string.Format("Cannot find workflow item for content id {0}", contentID));

                if (Clients != null)
                {
                    foreach (BPClient oClient in Clients)
                        foreach (BPProcess oProcess in oClient.bpProcesses)
                            foreach (BPProcessStep oStep in oProcess.bpProcessSteps)
                                if (oStep.ID == workflowItem.WorkflowStep.Id)
                                {
                                    ParameterSet = oStep.uniParameterSet;
                                    break;
                                }
                }

                if (ParameterSet == null)
                {
                    //Get settep setting
                    var stepSetting = _UCMSApiClient.Workflow.GetStepSetting(workflowItem.WorkflowStep.Id);
                    //Get setting
                    ParameterSet = (UniParameterSet)Helper.DeSerializeObjectFromString(stepSetting.Setting, typeof(UniParameterSet)) as UniParameterSet;

                    if (ParameterSet.SettingReference != Global.Own_Setting)
                    {
                        var workFlow = _UCMSApiClient.Workflow.GetById(workflowItem.WorkflowStep.WorkflowId);
                        foreach (var step in workFlow.Steps)
                        {
                            if (step.Name.Equals(ParameterSet.SettingReference))
                            {
                                //Get settep setting
                                stepSetting = _UCMSApiClient.Workflow.GetStepSetting(step.Id);

                                ParameterSet = (UniParameterSet)Helper.DeSerializeObjectFromString(stepSetting.Setting, typeof(UniParameterSet)) as UniParameterSet;
                                break;
                            }
                        }
                    }
                }

                //checkout
                _UCMSApiClient.WorkflowItem.CheckOut(workflowItem);
                bCheckedOut = true;

                // Create storage folder
                if (!Directory.Exists(strFolderStorage))
                    Directory.CreateDirectory(strFolderStorage);

                //get xmlBatch infor
                string xmlString = _UCMSApiClient.Content.GetPrivateData(workflowItem.Content.Id, Global.USC_BATCH_PRIVATEDATA);
                oBatch = Helper.DeSerializeObjectFromString(xmlString, typeof(UniBatch)) as UniBatch;

                oBatch.Name = workflowItem.Content.Name;
                oBatch.ExternalID = workflowItem.Id;
                oBatch.ID = workflowItem.Content.Id;
                oBatch.CreationDate = workflowItem.Content.CreatedDate;
                oBatch.UniParameterSet = ParameterSet;
                oBatch.FormTypes = ParameterSet.DocumentTypeProfileSetting.UniFormtypeList;
                oBatch.UniFormType = oBatch.FormTypes.Where(f => f.Name == oBatch.FormTypeName).FirstOrDefault();

                DownloadHelper oHelper = new DownloadHelper(_UCMSApiClient, workflowItem, oBatch, strFolderStorage);
                Thread downloadThread = new Thread(new ThreadStart(oHelper.Download));
                downloadThread.Start();

                return oBatch;
            }
            catch
            {
                if (bCheckedOut)
                {
                    try
                    {
                        _UCMSApiClient.WorkflowItem.UndoCheckout(workflowItem.Id);
                    }
                    catch (Exception)
                    {
                        //throw or not
                        //throw;
                    }
                }

                throw;
            }
            finally
            {
                logger.Debug(string.Format("Total time to checkout the content id {0} in {1} ms", oBatch.Name, DateTime.Now.Subtract(dtStart).TotalMilliseconds));
            }
        }
        public List<Report> GetReports()
        {
            List<UCMS.Model.Report.Report> reportList = _UCMSApiClient.Report.GetAll();
            List<Report> uniReportList = new List<Report>();
            foreach (UCMS.Model.Report.Report oReport in reportList)
            {
                Report uniReport = new Report();
                uniReport.ID = oReport.Id;
                uniReport.Name = oReport.Name;
                foreach (UCMS.Model.Report.ReportParam item in oReport.Params.ToList())
                {
                    ReportParam oParam = new ReportParam();
                    oParam.ParamName = item.Name;
                    oParam.ParamLabel = item.Label;
                    switch (item.Type) {
                        case DataType.Date:
                            oParam.ParamType = "DATE";
                            break;
                        default: oParam.ParamType = "TEXT";
                            break;
                    }
                    uniReport.Params.Add(oParam);
                }
                uniReportList.Add(uniReport);
            }

            return uniReportList;
        }

        public string RunReport(string reportName, Dictionary<string, string> Params)
        {
            var reportDetail = _UCMSApiClient.Report.Run(reportName, 1, 2000, Params);

            return Newtonsoft.Json.JsonConvert.SerializeObject(reportDetail);
        }

        private void GetFile(Content content, UniDocument oDoc, string strFolderStorage)
        {
            foreach (var page in oDoc.Pages)
            {
                string fileName = Path.GetFileName(page.FullFileName);
                var attachment = content.Attachments.Where(a => a.Name == fileName).FirstOrDefault();

                if (attachment != null)
                {
                    byte[] byteData = _UCMSApiClient.Attachment.Download(attachment.Id);

                    //Save to file
                    string sFileName = Path.Combine(strFolderStorage, attachment.Name);
                    File.WriteAllBytes(sFileName, byteData);
                }
            }
        }
    }
}
