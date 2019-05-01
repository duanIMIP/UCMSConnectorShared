using IMIP.UniversalScan.Data;
using IMIP.UniversalScan.Def;
using IMIP.UniversalScan.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using UCMS.ImportController.Data;
using UCMS.RestClient;

namespace UCMS.ImportController
{
    public partial class frmMain : Form
    {
        public UCMSApiClient oUCMSApiClient = null;
        public String _ReName { get; set; }
        public String _MoveTo { get; set; }
        public String _Type { get; set; }
        public BatchNamingProfile oBatchNamingProfile { get; set; }
        public String grdName { get; set; }
        public frmMain()
        {
            InitializeComponent();
            oUCMSApiClient = new UCMSApiClient(Common.Username, Common.Password, Common.UCMSWebAPIEndPoint, Common.UCMSAuthorizationServer);
            _ReName = "";
            _MoveTo = "";
            _Type = "";
            grdContentField.ContextMenuStrip = new ContextMenuStrip();
        }

        private void WatchFolder_Load(object sender, EventArgs e)
        {
            if (!oUCMSApiClient.Login())
            {
                this.Close();
            }
            cboBrank.DataSource = GetData.GetBranch(oUCMSApiClient);
            cboBrank.DisplayMember = "Name";
            cboBrank.ValueMember = "Name";
            cboLibrary.DataSource = GetData.GetFolder(oUCMSApiClient);
            cboLibrary.DisplayMember = "Name";
            cboLibrary.ValueMember = "Id";
        }
        
        private void cboLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLibrary.SelectedItem != null)
            {
                cboWorkflow.Text = "";
                var obj = cboLibrary.SelectedItem as Model.Folder;
                cboWorkflow.DataSource = GetData.GetWorkflow(oUCMSApiClient, obj.Id);
                cboWorkflow.DisplayMember = "Name";
                cboWorkflow.ValueMember = "Id";
            }
        }

        private void cboWorkflow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWorkflow.SelectedItem != null)
            {
                cboWorkflowStep.Text = "";
                var obj = (sender as ComboBox).SelectedItem as Model.Workflow;
                cboWorkflowStep.DataSource = obj.Steps;
                cboWorkflowStep.DisplayMember = "Name";
                cboWorkflowStep.ValueMember = "Id";
            }
        }

        private void cboWorkflowStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWorkflowStep.SelectedItem != null)
            {
                Model.WorkflowStep obj = (sender as ComboBox).SelectedItem as Model.WorkflowStep;
                ActivityConfiguration oActivityConfiguration = GetData.GetContentType(oUCMSApiClient, obj.Id);
                if (oActivityConfiguration.DocumentTypeProfile != null)
                {
                    if (oActivityConfiguration.DocumentTypeProfile.UniFormtypeList != null && oActivityConfiguration.DocumentTypeProfile.UniFormtypeList.Count > 0)
                    {
                        cboContentType.Text = "";
                        cboContentType.DataSource = oActivityConfiguration.DocumentTypeProfile.UniFormtypeList;
                        cboContentType.DisplayMember = "Name";
                        cboContentType.ValueMember = "ExternalID";
                    }
                    if (oActivityConfiguration.BatchNamingProfile != null)
                    {
                        oBatchNamingProfile = oActivityConfiguration.BatchNamingProfile;
                    }
                }
                else
                {
                    cboContentType.DataSource = new List<UniFormType>();
                    cboContentType.Text = "";
                }
            }
        }

        private void cboContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboContentType.SelectedItem != null)
            {
                UniFormType oUniFormType = cboContentType.SelectedItem as UniFormType;
                if (grdContentField.Rows.Count > 0) grdContentField.Rows.Clear();
                if (grdLibraryField.Rows.Count > 0) grdLibraryField.Rows.Clear();                
                var UniFieldList = GetData.GetListContentField(oUCMSApiClient, oUniFormType);
                for (int i = 0; i < UniFieldList.Count; i++)
                {
                    string[] row = new string[] { (i + 1).ToString(), UniFieldList[i].DisplayName, "", UniFieldList[i].Name };
                    grdContentField.Rows.Add(row);
                }
                UniFieldList = GetData.GetListLibraryField(oUCMSApiClient, oUniFormType, cboLibrary.SelectedValue.ToString());
                for (int i = 0; i < UniFieldList.Count; i++)
                {
                    string[] row = new string[] { (i + 1).ToString(), UniFieldList[i].DisplayName, "", UniFieldList[i].Name };
                    grdLibraryField.Rows.Add(row);
                }

                if(!oUniFormType.Root)
                {
                    cboParentContentType.Enabled = true;
                    List<UniFormType> dbParent = cboContentType.DataSource as List<UniFormType>;
                    cboParentContentType.DataSource = dbParent.FindAll(x=>x.Root);
                    cboParentContentType.DisplayMember = "Name";
                    cboParentContentType.ValueMember = "ExternalID";
                    tabContent.TabPages[2].Show();
                    tabContent.TabPages[3].Show();
                }
                else
                {
                    cboParentContentType.DataSource = new List<UniFormType>();
                    cboParentContentType.Text = "";
                    cboParentContentType.Enabled = false;
                    if (grdContentParent.Rows.Count > 0) grdContentParent.Rows.Clear();
                    if (grdLibraryParent.Rows.Count > 0) grdLibraryParent.Rows.Clear();
                    tabContent.TabPages[2].Hide();
                    tabContent.TabPages[3].Hide();
                }
            }
        }

        private void cboParentContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboParentContentType.SelectedItem != null)
            {
                UniFormType oUniFormType = cboParentContentType.SelectedItem as UniFormType;
                if (grdContentParent.Rows.Count > 0) grdContentParent.Rows.Clear();
                if (grdLibraryParent.Rows.Count > 0) grdLibraryParent.Rows.Clear();
                var UniFieldList = GetData.GetListContentField(oUCMSApiClient, oUniFormType);
                for (int i = 0; i < UniFieldList.Count; i++)
                {
                    string[] row = new string[] { (i + 1).ToString(), UniFieldList[i].DisplayName, "", UniFieldList[i].Name };
                    grdContentParent.Rows.Add(row);
                }
                UniFieldList = GetData.GetListLibraryField(oUCMSApiClient, oUniFormType, cboLibrary.SelectedValue.ToString());
                for (int i = 0; i < UniFieldList.Count; i++)
                {
                    string[] row = new string[] { (i + 1).ToString(), UniFieldList[i].DisplayName, "", UniFieldList[i].Name };
                    grdLibraryParent.Rows.Add(row);
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            AddControllProfile();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = fbd.SelectedPath;
                //UploadRanDomRepeat();
            }
            else
            {
                txtFolder.Text = "";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtFolder.Text = "";
            cboBrank.Text = "";
            cboLibrary.Text = "";
            cboWorkflow.Text = "";
            cboWorkflowStep.Text = "";
            cboContentType.Text = "";
            cboParentContentType.Text = "";
            _ReName = "";
            _MoveTo = "";
            _Type = "";
            cboWorkflow.DataSource = new List<Model.Workflow>();
            cboWorkflowStep.DataSource = new List<Model.WorkflowStep>();
            cboContentType.DataSource = new List<UniFormType>();
            cboParentContentType.DataSource = new List<UniFormType>();
            if (grdContentField.Rows.Count > 0) grdContentField.Rows.Clear();
            if (grdLibraryField.Rows.Count > 0) grdLibraryField.Rows.Clear();
            if (grdContentParent.Rows.Count > 0) grdContentParent.Rows.Clear();
            if (grdLibraryParent.Rows.Count > 0) grdLibraryParent.Rows.Clear();
            tabContent.TabPages[2].Hide();
            tabContent.TabPages[3].Hide();
            cboBrank.DataSource = GetData.GetBranch(oUCMSApiClient);
            cboBrank.DisplayMember = "Name";
            cboBrank.ValueMember = "Name";
            cboLibrary.DataSource = GetData.GetFolder(oUCMSApiClient);
            cboLibrary.DisplayMember = "Name";
            cboLibrary.ValueMember = "Id";
        }

        #region AddProfile

        private Boolean Profile(UCMSApiClient oUCMSApiClient, DataValue oBranch,  Model.Folder oFolder, DataValue oWorkflow, DataValue oWorkflowStep, DataValue oContenType, DataValue oContenTypeParent, Dictionary<string, object> oContentField, Dictionary<string, object> oLibraryField, Dictionary<string, object> oContentParent, Dictionary<string, object> oLibraryParent, FileInfo[] arrayFileInfor, String RenameFile, String RemoveFile, String keyPrivateData, String pathClient)
        {
            try
            {
                Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
                oWorkflowItem.Content = new Model.Content();
                oWorkflowItem.Content.Folder = oFolder;
                oWorkflowItem.Content.Tags = new List<string>();
                oWorkflowItem.Workflow = new Model.Workflow(){Id = oWorkflow.Key };
                oWorkflowItem.WorkflowStep = new Model.WorkflowStep() { Id = oWorkflowStep.Key };
                oWorkflowItem.State = Model.Enum.WorkflowItemState.Ready;
                oWorkflowItem.Priority = Model.Enum.WorkflowItemPriority.Normal;                
                if(!String.IsNullOrEmpty(oContenTypeParent.Key))//Branch or Document
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenTypeParent.Value, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenTypeParent.Key };
                    var temp = oContentParent;
                    temp.Add("BranchId", cboBrank.Text);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryParent;
                }
                else
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenType.Value, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenType.Key };
                    var temp = oContentField;
                    temp.Add("BranchId", cboBrank.Text);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryField;
                }
                oWorkflowItem.Content = oUCMSApiClient.Content.Create(oWorkflowItem.Content);

                oWorkflowItem.Content.Attachments = new List<Model.Attachment>();
                oUCMSApiClient.Content.Checkout(oWorkflowItem.Content.Id);

                foreach (var oFileInfor in arrayFileInfor)
                {
                    var attachment = new Model.Attachment
                    {
                        ContentId = oWorkflowItem.Content.Id,
                        Data = File.ReadAllBytes(oFileInfor.FullName),
                        MIME = "image/universalscan",
                        Type = UCMS.Model.Enum.AttachmentType.Public,
                        Name = string.IsNullOrEmpty(RenameFile) ? oFileInfor.Name : (oFileInfor.Name.Replace(oFileInfor.Extension, "") + RenameFile)
                    };

                    oUCMSApiClient.Attachment.Upload(attachment);

                    if (!String.IsNullOrEmpty(RemoveFile))
                    {
                        oFileInfor.MoveTo(RemoveFile + @"\\" + oFileInfor.Name);
                    }
                    if (!string.IsNullOrEmpty(RenameFile))
                    {
                        oFileInfor.CopyTo((String.IsNullOrEmpty(RemoveFile) ? oFileInfor.DirectoryName : RemoveFile) + @"\\" + attachment.Name);
                        oFileInfor.Delete();
                    }
                }

                oUCMSApiClient.Content.Checkin(oWorkflowItem.Content.Id);
                Model.ContentPrivateData oContentPrivateData = new Model.ContentPrivateData();
                oContentPrivateData.Key = keyPrivateData;
                //--------------Set value ContentPrivateData-----------------------------
                UniBatch oBatch = new UniBatch();
                oBatch.BranchID = oBranch.Key;
                oBatch.Name = oWorkflowItem.Content.Name;
                oBatch.ClientName = oFolder.Name;
                oBatch.ProcessName = oWorkflow.Value;
                oBatch.ProcessStepName = oWorkflow.Value;
                oBatch.FormTypeName = oWorkflowItem.Content.ContentType.Name;
                oBatch.Fields = new List<UniField>();
                oBatch.Pages = new List<UniPage>();
                if (String.IsNullOrEmpty(oContenTypeParent.Key))
                {
                    foreach (var oFileInfor in arrayFileInfor)
                    {
                        oBatch.Pages.Add(new UniPage()
                        {
                            ID = Path.GetFileNameWithoutExtension(oFileInfor.Name),
                            FullFileName = string.IsNullOrEmpty(RenameFile) ? Path.GetFullPath(oFileInfor.Name) : (oFileInfor.Name.Replace(oFileInfor.Extension, "") + RenameFile),
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
                    UniDocument oUniDocument = new UniDocument();
                    oUniDocument.Fields = new List<UniField>();
                    oUniDocument.Pages = new List<UniPage>();
                    oUniDocument.FormTypeName = oContenType.Value;

                    foreach (var oFileInfor in arrayFileInfor)
                    {
                        oUniDocument.Pages.Add(new UniPage()
                        {
                            ID = Path.GetFileNameWithoutExtension(oFileInfor.Name),
                            FullFileName = string.IsNullOrEmpty(RenameFile) ? Path.GetFullPath(oFileInfor.Name) : (oFileInfor.Name.Replace(oFileInfor.Extension, "") + RenameFile),
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
                oUCMSApiClient.WorkflowItem.Insert(oWorkflowItem, true);
                return true;
            }
            catch(Exception ex)
            {
                Common.LogToFile(ex.Message);
                return false;
            }
        }

        private bool AddControllProfile()
        {
            try
            {
                if (!CheckControllProfile())
                {
                    return false;
                }
                DataValue oBranch = new DataValue() { Key = cboBrank.SelectedValue.ToString(), Value = cboBrank.Text };
                Model.Folder oFolder = cboLibrary.SelectedItem as Model.Folder;
                DataValue oWorkflow = new DataValue() { Key = cboWorkflow.SelectedValue.ToString(), Value = cboWorkflow.Text };
                DataValue oWorkflowStep = new DataValue() { Key = cboWorkflowStep.SelectedValue.ToString(), Value = cboWorkflowStep.Text };
                DataValue oContenType = new DataValue() { Key = cboContentType.SelectedValue.ToString(), Value = cboContentType.Text };
                DataValue oContenTypeParent = new DataValue();
                if(!(cboContentType.SelectedItem as UniFormType).Root)
                {
                    oContenTypeParent.Key = cboParentContentType.SelectedValue.ToString();
                    oContenTypeParent.Value = cboParentContentType.Text;
                }
                Dictionary<string, object> oContentField = new Dictionary<string, object>();
                Dictionary<string, object> oLibraryField = new Dictionary<string, object>();
                Dictionary<string, object> oContentParent = new Dictionary<string, object>();
                Dictionary<string, object> oLibraryParent = new Dictionary<string, object>();
                foreach (DataGridViewRow item in grdContentField.Rows)
                {
                    if (!item.IsNewRow)
                    {
                        oContentField.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString()));
                    }
                }                
                foreach (DataGridViewRow item in grdLibraryField.Rows)
                {
                    if (!item.IsNewRow)
                    {
                        oLibraryField.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString()));
                    }
                }
                if(!String.IsNullOrEmpty(oContenTypeParent.Key))
                {
                    foreach (DataGridViewRow item in grdContentParent.Rows)
                    {
                        if (!item.IsNewRow)
                        {
                            oContentParent.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString()));
                        }
                    }
                    foreach (DataGridViewRow item in grdLibraryParent.Rows)
                    {
                        if (!item.IsNewRow)
                        {
                            oLibraryParent.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString()));
                        }
                    }
                }
                FileInfo[] arrayFileInfor = new FileInfo[0];                
                var folderPath = txtFolder.Text.Trim();
                if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
                {
                    DirectoryInfo directInfo = new DirectoryInfo(folderPath);
                    arrayFileInfor = directInfo.GetFiles();
                    if (!string.IsNullOrEmpty(_MoveTo))
                    {
                        Path.Combine(_MoveTo);
                        Directory.CreateDirectory(_MoveTo);
                        folderPath = _MoveTo;
                    }
                }

                if (Profile(oUCMSApiClient, oBranch, oFolder, oWorkflow, oWorkflowStep, oContenType, oContenTypeParent, oContentField, oLibraryField, oContentParent, oLibraryParent, arrayFileInfor, _ReName, _MoveTo, "USCBatch", folderPath))
                {
                    MessageBox.Show("Cập nhật thành công");
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Có lỗi trong quá trình cập nhật");
                Common.LogToFile(ex.Message);
                return false;
            }
        }
        private bool CheckControllProfile()
        {
            if (cboBrank.Text == "")
            {
                MessageBox.Show("Branch không được để trống");
                return false;
            }

            if (cboLibrary.SelectedItem == null || (cboLibrary.SelectedItem as Model.Folder).Id == "")
            {
                MessageBox.Show("Library không được để trống");
                return false;
            }

            if (cboWorkflow.SelectedItem == null || (cboWorkflow.SelectedItem as Model.Workflow).Id == "")
            {
                MessageBox.Show("Workflow không được để trống");
                return false;
            }

            if (cboWorkflowStep.SelectedItem == null || (cboWorkflowStep.SelectedItem as Model.WorkflowStep).Id == "")
            {
                MessageBox.Show("WorkflowStep không được để trống");
                return false;
            }
            if (cboContentType.SelectedItem == null || (cboContentType.SelectedItem as UniFormType).ExternalID == "")
            {
                MessageBox.Show("ContentType không được để trống");
                return false;
            }

            if (!(cboContentType.SelectedItem as UniFormType).Root && (cboContentType.SelectedItem == null || (cboContentType.SelectedItem as UniFormType).ExternalID == ""))
            {
                MessageBox.Show("Parent ContentType không được để trống");
                return false;
            }
            return true;
        }

        #endregion AddProfile

        //private void UploadRanDomRepeat()
        //{
        //    var folderPath = txtFolder.Text.Trim();
        //    if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
        //    {
        //        if (!string.IsNullOrEmpty(_MoveTo))
        //        {
        //            Path.Combine(_MoveTo);
        //            Directory.CreateDirectory(_MoveTo);
        //            folderPath = _MoveTo;
        //        }
        //        DirectoryInfo directInfo = new DirectoryInfo(folderPath);
        //        foreach (var item in directInfo.GetFiles())
        //        {
        //            while(!ProfileRandom(item, folderPath))
        //            {
        //                //Tamj thowi
        //            }
        //        }
        //        MessageBox.Show("Cập nhật thành công");
        //    }

        //}

        //private Boolean ProfileRandom(FileInfo fileInfor, String folderPath)
        //{
        //    Random rdUpload = new Random();
        //    try
        //    {
        //        Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
        //        oWorkflowItem.Content = new Model.Content();
        //        List<Model.Folder> folderList = GetLibrary("");
        //        if (folderList == null || folderList.Count == 0)
        //        {
        //            //MessageBox.Show("Library không có giá trị");
        //            return false;
        //        }
        //        oWorkflowItem.Content.Folder = folderList[rdUpload.Next(0, folderList.Count - 1)];

        //        List<IMIP.UniversalScan.Data.Branch> listBranch = GetBranch();
        //        if (listBranch == null || listBranch.Count == 0)
        //        {
        //            //MessageBox.Show("Branch không có giá trị");
        //            return false;
        //        }
        //        IMIP.UniversalScan.Data.Branch oBranch = listBranch[rdUpload.Next(0, listBranch.Count - 1)];

        //        List<Model.Workflow> workflowList = oUCMSApiClient.Workflow.GetAll(oWorkflowItem.Content.Folder.Id);
        //        if (workflowList == null || workflowList.Count == 0)
        //        {
        //            //MessageBox.Show("Workflow không có giá trị");
        //            return false;
        //        }
        //        oWorkflowItem.Workflow = workflowList[rdUpload.Next(0, workflowList.Count - 1)];
        //        if (oWorkflowItem.Workflow.Steps == null || oWorkflowItem.Workflow.Steps.Count == 0)
        //        {
        //            //MessageBox.Show("WorkflowStep không có giá trị");
        //            return false;
        //        }
        //        oWorkflowItem.WorkflowStep = oWorkflowItem.Workflow.Steps[rdUpload.Next(0, oWorkflowItem.Workflow.Steps.Count - 1)];
        //        List<UniFormType> listUniFormType = GetContentType(oWorkflowItem.WorkflowStep.Id);
        //        if (listUniFormType == null || listUniFormType.Count == 0)
        //        {
        //            //MessageBox.Show("ContentType không có giá trị");
        //            return false;
        //        }
        //        UniFormType oUniFormType = listUniFormType[rdUpload.Next(0, listUniFormType.Count - 1)];
        //        oWorkflowItem.Content.Values = new Dictionary<string, object>();
        //        foreach (UniFieldDefinition item in oUniFormType.UniFieldDefinitions)
        //        {
        //            oWorkflowItem.Content.Values.Add(item.Name, item.DefaultValue);
        //        }

        //        oWorkflowItem.Content.LibraryFieldValues = new Dictionary<string, object>();
        //        return Profile(oUCMSApiClient, oBranch.Name, oWorkflowItem.Content.Folder, oWorkflowItem.Workflow.Id, oWorkflowItem.WorkflowStep.Id, oUniFormType.ExternalID, oWorkflowItem.Content.Values, oWorkflowItem.Content.LibraryFieldValues, new FileInfo[] { fileInfor }, _ReName, _MoveTo, "USCBatch", oWorkflowItem.Workflow.Name, oWorkflowItem.WorkflowStep.Name, folderPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogToFile(ex.Message);
        //        return false;
        //    }
        //}

        //private Boolean UploadWatchRandom(string contentId, string folderPath)
        //{
        //    try
        //    {
        //            DirectoryInfo directInfo = new DirectoryInfo(folderPath);
        //            oUCMSApiClient.Content.Checkout(contentId);//Checkout content
        //            foreach (var item in directInfo.GetFiles())
        //            {
        //                if (String.IsNullOrEmpty(_Type) || _Type.Contains(item.Extension + ";"))
        //                {
        //                    var attachment = new Model.Attachment
        //                    {
        //                        ContentId = contentId,
        //                        Data = File.ReadAllBytes(item.FullName),
        //                        MIME = "image/universalscan",
        //                        Type = UCMS.Model.Enum.AttachmentType.Public,
        //                        Name = string.IsNullOrEmpty(_ReName) ? item.Name : (item.Name.Replace(item.Extension, "") + _ReName)
        //                    };

        //                    oUCMSApiClient.Attachment.Upload(attachment);

        //                    if (!String.IsNullOrEmpty(_MoveTo))
        //                    {
        //                        item.MoveTo(_MoveTo + @"\\" + item.Name);
        //                    }
        //                    if (!string.IsNullOrEmpty(_ReName))
        //                    {
        //                        item.CopyTo((String.IsNullOrEmpty(_MoveTo) ? item.DirectoryName : _MoveTo) + @"\\" + attachment.Name);
        //                        item.Delete();
        //                    }
        //                }
        //            }
        //            oUCMSApiClient.Content.Checkin(contentId);// Checkint content
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        return false;
        //    }
        //    return true;
        //}

        //private String PrivateDataRandom(String contentId, string pathClient)
        //{
        //    var content = oUCMSApiClient.Content.GetById(contentId);
        //    UniBatch oBatch = new UniBatch();
        //    oBatch.BranchID = cboBrank.Text;
        //    oBatch.Name = content.Name;
        //    oBatch.ClientName = cboLibrary.Text;
        //    oBatch.ProcessName = cboWorkflow.Text;
        //    oBatch.ProcessStepName = cboWorkflowStep.Text;
        //    oBatch.FormTypeName = content.ContentType.Name;
        //    oBatch.Fields = new List<UniField>();

        //    foreach (DataGridViewRow item in grdContentField.Rows)
        //    {
        //        if (!item.IsNewRow)
        //        {
        //            oBatch.Fields.Add(new UniField() { Name = item.Cells["NameId"].Value.ToString(), Value = convertValueField(item.Cells["txtValue"].Value.ToString()) });
        //        }
        //    }

        //    oBatch.Pages = new List<UniPage>();

        //    for (int i = 0; i < content.Attachments.Count; i++)
        //    {
        //        oBatch.Pages.Add(new UniPage()
        //        {
        //            ID = Path.GetFileNameWithoutExtension(content.Attachments[i].Name),
        //            FullFileName = (string.IsNullOrEmpty(pathClient) ? txtFolder.Text : pathClient) + @"\" + content.Attachments[i].Name,
        //            Rejected = false,
        //            IsRescan = false,
        //            IsNew = false,
        //            SheetID = "",
        //            RejectedNote = ""
        //        });
        //    }

        //    return Common.SerializeObjectToString(typeof(UniBatch), oBatch);
        //}

        #region UploadFolder
        private void btnUploadFolder_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            ctmWatchFolder.Show(ptLowerLeft);
        }

        private void PathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = fbd.SelectedPath;
            }
            else
            {
                txtFolder.Text = "";
            }
        }

        private void ConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWatchFolder frm = new frmWatchFolder();
            frm._ReName = _ReName;
            frm._MoveTo = _MoveTo;
            frm._Type = _Type;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _ReName = frm._ReName;
                _MoveTo = frm._MoveTo;
                _Type = frm._Type;
            }
        }

        private Boolean UploadSave(string contentId)
        {
            try
            {
                if (!string.IsNullOrEmpty(_MoveTo))
                {
                    Path.Combine(_MoveTo);
                    Directory.CreateDirectory(_MoveTo);
                }                
                
                var folderPath = txtFolder.Text.Trim();

                if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
                {
                    DirectoryInfo directInfo = new DirectoryInfo(folderPath);                    

                    oUCMSApiClient.Content.Checkout(contentId);//Checkout content
                    foreach (var item in directInfo.GetFiles())
                    {
                        if (String.IsNullOrEmpty(_Type) || _Type.Contains(item.Extension + ";"))
                        {
                            var attachment = new Model.Attachment
                            {
                                ContentId = contentId,
                                Data = File.ReadAllBytes(item.FullName),
                                MIME = "image/universalscan",
                                Type = UCMS.Model.Enum.AttachmentType.Public,
                                Name = string.IsNullOrEmpty(_ReName) ? item.Name: (item.Name.Replace(item.Extension, "") + _ReName)
                            };

                            oUCMSApiClient.Attachment.Upload(attachment);

                            if (!String.IsNullOrEmpty(_MoveTo))
                            {
                                item.MoveTo(_MoveTo + @"\\" + item.Name);
                            }
                            if (!string.IsNullOrEmpty(_ReName))
                            {
                                item.CopyTo((String.IsNullOrEmpty(_MoveTo)?item.DirectoryName: _MoveTo) + @"\\" + attachment.Name);
                                item.Delete();
                            }
                        }
                    }
                    oUCMSApiClient.Content.Checkin(contentId);// Checkint content
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
            return true;
        }

        #endregion UploadFolder

        #region ConfigField
        private void ConfigContentField_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btnConfig = sender as ToolStripMenuItem;
            DataGridView grdView = grdContentField;
            switch (grdName)
            {
                case "grdLibraryField":
                    grdView = grdLibraryField;
                    break;
                case "grdContentParent":
                    grdView = grdContentParent;
                    break;
                case "grdLibraryParent":
                    grdView = grdLibraryParent;
                    break;
            }
            switch (btnConfig.Name)
            {
                case "ctmiDocumentType":
                    grdView.CurrentCell.Value += "{$Type}";
                    break;
                case "ctmiDate":
                    grdView.CurrentCell.Value += "{$Date}";
                    break;
                case "timeToolStripMenuItem":
                    grdView.CurrentCell.Value += "{$Time}";
                    break;
                case "machineToolStripMenuItem":
                    grdView.CurrentCell.Value += "{$MachineName}";
                    break;
                case "userNameToolStripMenuItem":
                    grdView.CurrentCell.Value += "{$UserName}";
                    break;
            }
        }
        private void grdContentField_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grdView = sender as DataGridView;
            grdName = grdView.Name;
            if (e.Button == MouseButtons.Right)
            {
                if (grdView.CurrentCell != null && grdView.CurrentCell.Value != null && e.RowIndex != -1)
                {
                    if (grdView.CurrentCell is DataGridViewTextBoxCell && grdView.CurrentCell.ColumnIndex.Equals(2))
                    {
                        DataGridViewTextBoxCell btnSender = (DataGridViewTextBoxCell)grdView.CurrentCell;
                        Point ptLowerLeft = new Point(btnSender.Size.Width, btnSender.Size.Height);
                        ptLowerLeft = grdView.PointToScreen(ptLowerLeft);
                        ctmFieldRight.Show(ptLowerLeft);
                    }
                }
            }
        }
        private void grdContentField_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grdView = sender as DataGridView;
            grdName = grdView.Name;
            if (e.Button == MouseButtons.Left)
            {
                if (grdView.CurrentCell != null && grdView.CurrentCell.Value != null && e.RowIndex != -1)
                {
                    if (grdView.CurrentCell is DataGridViewTextBoxCell && grdView.CurrentCell.ColumnIndex.Equals(2))
                    {
                        DataGridViewTextBoxCell btnSender = (DataGridViewTextBoxCell)grdView.CurrentCell;
                        Point ptLowerLeft = new Point(btnSender.Size.Width, btnSender.Size.Height);
                        ptLowerLeft = grdView.PointToScreen(ptLowerLeft);
                        ctmDetails.Show(ptLowerLeft);
                    }
                }
            }
        }
        private string convertValueField(string config)
        {
            string documentType = cboContentType.Text;
            string cfDate = DateTime.Now.ToString("ddMMyyyy");
            string cfTime = DateTime.Now.ToString("HHmmss");
            string cfMachineName = Environment.MachineName.ToString();
            string cfUsername = Common.Username;

            config = config.Replace("{$Type}", documentType);
            config = config.Replace("{$Date}", cfDate);
            config = config.Replace("{$Time}", cfTime);
            config = config.Replace("{$MachineName}", cfMachineName);
            config = config.Replace("{$UserName}", cfUsername);
            return config;
        }
        private void clearStripMenuItem_Click(object sender, EventArgs e)
        {
            switch(grdName)
            {
                case "grdContentField":
                    grdContentField.CurrentCell.Value = "";
                    break;
                case "grdLibraryField":
                    grdLibraryField.CurrentCell.Value = "";
                    break;
                case "grdContentParent":
                    grdContentParent.CurrentCell.Value = "";
                    break;
                case "grdLibraryParent":
                    grdLibraryParent.CurrentCell.Value = "";
                    break;
            }            
        }
        #endregion ConfigField

       
    }
}
