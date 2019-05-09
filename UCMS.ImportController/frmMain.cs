using IMIP.SharedComponent.GdPictureWrapper;
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
using System.Runtime.InteropServices;
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
        private List<Thread> listThread { get; set; }
        private String nameThread { get; set; }
        private int StopThread = 0;
        private int PrgBarBatchImporterValue = 0;
        private List<MultipleProfile> MultipleProfileList { get; set; }
        
        public frmMain()
        {
            InitializeComponent();
            oUCMSApiClient = new UCMSApiClient(Common.Username, Common.Password, Common.UCMSWebAPIEndPoint, Common.UCMSAuthorizationServer);
            _ReName = "";
            _MoveTo = "";
            _Type = ".tif;.tiff;.pdf;";
            grdContentField.ContextMenuStrip = new ContextMenuStrip();
            MultipleProfileList = new List<MultipleProfile>();
            grdMultipleProfile.ContextMenuStrip = new ContextMenuStrip();
            listThread = new List<Thread>();
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
            grdLibrary.DataSource = GetData.GetFolder(oUCMSApiClient);
            grdMultipleProfile.DataSource = MultipleProfileList;
            grdMultipleProfile.AutoGenerateColumns = true;
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
                if(obj.Steps != null)
                {
                    cboWorkflowStep.DataSource = obj.Steps.FindAll(x=>String.IsNullOrEmpty(x.Setting));
                cboWorkflowStep.DisplayMember = "Name";
                cboWorkflowStep.ValueMember = "Id";
                }
            }
        }

        private void cboWorkflowStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWorkflowStep.SelectedItem != null)
            {
                Model.WorkflowStep obj = (sender as ComboBox).SelectedItem as Model.WorkflowStep;
                ActivityConfiguration oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, obj.Id);

                if (oActivityConfiguration.SettingReference != null && !oActivityConfiguration.SettingReference.Trim().Equals(Common.SettingReferenceDefault))
                {
                    Model.WorkflowStep oWorkflowStepRoot = ((sender as ComboBox).DataSource as List<Model.WorkflowStep>).SingleOrDefault(x => x.Name.Equals(oActivityConfiguration.SettingReference));
                    if (oWorkflowStepRoot == null || string.IsNullOrEmpty(oWorkflowStepRoot.Id))
                    {
                        cboContentType.DataSource = new List<UniFormType>();
                        cboContentType.Text = "";
                        return;
                    }
                    else
                    {
                        oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, oWorkflowStepRoot.Id);
                    }
                }

                if (oActivityConfiguration.DocumentTypeProfile != null)
                {
                    if (oActivityConfiguration.DocumentTypeProfile.UniFormtypeList != null && oActivityConfiguration.DocumentTypeProfile.UniFormtypeList.Count > 0)
                    {
                        cboContentType.Text = "";
                        cboContentType.DataSource = GetData.GetContentType(oUCMSApiClient, (cboLibrary.SelectedItem as Model.Folder).Id, oActivityConfiguration.DocumentTypeProfile.UniFormtypeList);
                        cboContentType.DisplayMember = "Name";
                        cboContentType.ValueMember = "ExternalID";
                    }
                    else
                    {
                        cboContentType.DataSource = new List<UniFormType>();
                        cboContentType.Text = "";
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
                UniFieldList = GetData.GetListLibraryField(oUCMSApiClient, oUniFormType, (cboLibrary.SelectedItem as Model.Folder).Id);
                for (int i = 0; i < UniFieldList.Count; i++)
                {
                    string[] row = new string[] { (i + 1).ToString(), UniFieldList[i].DisplayName, "", UniFieldList[i].Name };
                    grdLibraryField.Rows.Add(row);
                }

                if (!oUniFormType.Root)
                {
                    cboParentContentType.Enabled = true;
                    List<UniFormType> dbParent = cboContentType.DataSource as List<UniFormType>;
                    cboParentContentType.DataSource = dbParent.FindAll(x => x.Root);
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
            if (!string.IsNullOrEmpty(txtFolder.Text) && Directory.Exists(txtFolder.Text))
            {
                btnRandom.Enabled = false;
                btnSubmit.Enabled = false;
                AddEachAttachProfile();
                MemoryManagement.FlushMemory();
                btnSubmit.Enabled = true;
                btnRandom.Enabled = true;

            }
            else
            {
                MessageBox.Show("Not contains attachment");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
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
            btnRefresh.Enabled = true;
            prgBarAddControl.Value = 0;
        }

        #region AddProfile

        private String Profile(UCMSApiClient oUCMSApiClient, DataValue oBranch, Model.Folder oFolder, DataValue oWorkflow, DataValue oWorkflowStep, DataValue oContenType, DataValue oContenTypeParent, Dictionary<string, object> oContentField, Dictionary<string, object> oLibraryField, Dictionary<string, object> oContentParent, Dictionary<string, object> oLibraryParent, List<FileInfo> arrayFileInfor, String RenameFile, String RemoveFile, String keyPrivateData)
        {
            String pathClient = "";
            Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
            oWorkflowItem.Content = new Model.Content();
            oWorkflowItem.Content.Folder = oFolder;
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
            try
            {
                if (!String.IsNullOrEmpty(oContenTypeParent.Key))//Branch or Document
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenTypeParent.Value, oBranch.Value, oFolder.Name, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenTypeParent.Key };
                    temp = oContentParent;
                    temp.Add("BranchID", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryParent;
                }
                else
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenType.Value, oBranch.Value, oFolder.Name, oBatchNamingProfile);
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
                        var tempFileSplit = Guid.NewGuid().ToString();
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
                    if (item.Equals(oFolder.Name + "_" + oWorkflowStep.Value))
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
                ContentName = "";
            }

            oWorkflowItem = null;
            temp = null;
            oContentPrivateData = null;
            oBatch = null;
            oUniDocument = null;
            return ContentName;

        }

        private bool AddControllProfile(Boolean LoadRootFalse = false)// LoadRootFalse = false: không apload với content không là gốc
        {
            try
            {
                if (!CheckControllProfile(LoadRootFalse))
                {
                    return false;
                }
                DataValue oBranch = new DataValue() { Key = cboBrank.SelectedValue.ToString(), Value = cboBrank.Text };
                Model.Folder oFolder = cboLibrary.SelectedItem as Model.Folder;
                DataValue oWorkflow = new DataValue() { Key = cboWorkflow.SelectedValue.ToString(), Value = cboWorkflow.Text };
                DataValue oWorkflowStep = new DataValue() { Key = cboWorkflowStep.SelectedValue.ToString(), Value = cboWorkflowStep.Text };
                DataValue oContenType = new DataValue() { Key = cboContentType.SelectedValue.ToString(), Value = cboContentType.Text };
                DataValue oContenTypeParent = new DataValue();
                if (LoadRootFalse && !(cboContentType.SelectedItem as UniFormType).Root)
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
                        oContentField.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                    }
                }
                foreach (DataGridViewRow item in grdLibraryField.Rows)
                {
                    if (!item.IsNewRow)
                    {
                        oLibraryField.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                    }
                }
                if (!String.IsNullOrEmpty(oContenTypeParent.Key))
                {
                    foreach (DataGridViewRow item in grdContentParent.Rows)
                    {
                        if (!item.IsNewRow)
                        {
                            oContentParent.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                        }
                    }
                    foreach (DataGridViewRow item in grdLibraryParent.Rows)
                    {
                        if (!item.IsNewRow)
                        {
                            oLibraryParent.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                        }
                    }
                }
                List<FileInfo> arrayFileInfor = new List<FileInfo>();
                var folderPath = txtFolder.Text.Trim();
                if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
                {
                    DirectoryInfo directInfo = new DirectoryInfo(folderPath);
                    foreach (var item in directInfo.GetFiles())
                    {
                        if ((String.IsNullOrEmpty(_Type) || _Type.Contains(item.Extension + ";")) && !item.Extension.Equals(_ReName))
                        {
                            arrayFileInfor.Add(item);
                        }
                    }
                    if (!string.IsNullOrEmpty(_MoveTo))
                    {
                        Path.Combine(_MoveTo);
                        Directory.CreateDirectory(_MoveTo);
                        folderPath = _MoveTo;
                    }
                }

                if (Profile(oUCMSApiClient, oBranch, oFolder, oWorkflow, oWorkflowStep, oContenType, oContenTypeParent, oContentField, oLibraryField, oContentParent, oLibraryParent, arrayFileInfor, _ReName, _MoveTo, "USCBatch") != "")
                {
                    MessageBox.Show("Cập nhật thành công");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi trong quá trình cập nhật");
                Common.LogToFile("AddControllProfile_" + ex.Message);
            }
            return false;
        }

        private bool AddEachAttachProfile(Boolean LoadRootFalse = false)// LoadRootFalse = false: không apload với content không là gốc
        {
            try
            {
                if (!CheckControllProfile(LoadRootFalse))
                {
                    return false;
                }
                List<FileInfo> fileInfoList = new List<FileInfo>();
                int CoutNumber = 0;
                DataValue oBranch = new DataValue() { Key = cboBrank.SelectedValue.ToString(), Value = cboBrank.Text };
                Model.Folder oFolder = cboLibrary.SelectedItem as Model.Folder;
                DataValue oWorkflow = new DataValue() { Key = cboWorkflow.SelectedValue.ToString(), Value = cboWorkflow.Text };
                DataValue oWorkflowStep = new DataValue() { Key = cboWorkflowStep.SelectedValue.ToString(), Value = cboWorkflowStep.Text };
                DataValue oContenType = new DataValue() { Key = cboContentType.SelectedValue.ToString(), Value = cboContentType.Text };
                DataValue oContenTypeParent = new DataValue();
                if (LoadRootFalse && !(cboContentType.SelectedItem as UniFormType).Root)
                {
                    oContenTypeParent.Key = cboParentContentType.SelectedValue.ToString();
                    oContenTypeParent.Value = cboParentContentType.Text;
                }

                var folderPath = txtFolder.Text.Trim();
                if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
                {
                    DirectoryInfo directInfo = new DirectoryInfo(folderPath);
                    if (!string.IsNullOrEmpty(_MoveTo))
                    {
                        Path.Combine(_MoveTo);
                        Directory.CreateDirectory(_MoveTo);
                        folderPath = _MoveTo;
                    }

                    FileInfo[] fileInfo = directInfo.GetFiles();
                    foreach (var itemInfo in directInfo.GetFiles())
                    {
                        if ((String.IsNullOrEmpty(_Type) || _Type.Contains(itemInfo.Extension + ";")) && !itemInfo.Extension.Equals(_ReName))
                        {
                            fileInfoList.Add(itemInfo);
                        }
                    }

                    if (fileInfoList.Count == 0)
                    {
                        MessageBox.Show("Not contains attachment");
                        btnUploadFolder.Focus();
                        return false;
                    }

                    prgBarAddControl.Minimum = 0;
                    prgBarAddControl.Maximum = fileInfoList.Count;

                    foreach (var itemInfo in fileInfoList)
                    {
                        Dictionary<string, object> oContentField = new Dictionary<string, object>();
                        Dictionary<string, object> oLibraryField = new Dictionary<string, object>();
                        Dictionary<string, object> oContentParent = new Dictionary<string, object>();
                        Dictionary<string, object> oLibraryParent = new Dictionary<string, object>();
                        foreach (DataGridViewRow item in grdContentField.Rows)
                        {
                            if (!item.IsNewRow)
                            {
                                oContentField.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                            }
                        }
                        foreach (DataGridViewRow item in grdLibraryField.Rows)
                        {
                            if (!item.IsNewRow)
                            {
                                oLibraryField.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                            }
                        }
                        if (!String.IsNullOrEmpty(oContenTypeParent.Key))
                        {
                            foreach (DataGridViewRow item in grdContentParent.Rows)
                            {
                                if (!item.IsNewRow)
                                {
                                    oContentParent.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                                }
                            }
                            foreach (DataGridViewRow item in grdLibraryParent.Rows)
                            {
                                if (!item.IsNewRow)
                                {
                                    oLibraryParent.Add(item.Cells[3].Value.ToString(), convertValueField(item.Cells[2].Value.ToString(), item.Cells[3].Value.ToString()));
                                }
                            }
                        }
                        Profile(oUCMSApiClient, oBranch, oFolder, oWorkflow, oWorkflowStep, oContenType, oContenTypeParent, oContentField, oLibraryField, oContentParent, oLibraryParent, new List<FileInfo>() { itemInfo }, _ReName, _MoveTo, "USCBatch");
                        CoutNumber++;
                        prgBarAddControl.Value = CoutNumber;
                        lblNumberTotalContents.Text = CoutNumber + "/" + fileInfoList.Count;
                    }

                }
                MessageBox.Show("Update successfully");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update error");
                Common.LogToFile("AddControllProfile_" + ex.Message);
                return false;
            }
        }

        private bool CheckControllProfile(Boolean LoadRootFalse)// LoadRootFalse = false: không apload với content không là gốc
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

            if (LoadRootFalse)
            {
                if (!(cboContentType.SelectedItem as UniFormType).Root && (cboContentType.SelectedItem == null || (cboContentType.SelectedItem as UniFormType).ExternalID == ""))
                {
                    MessageBox.Show("Parent ContentType không được để trống");
                    return false;
                }
            }
            else
            {
                if (!(cboContentType.SelectedItem as UniFormType).Root)
                {
                    MessageBox.Show("ContentType không phải là gốc");
                    return false;
                }
            }
            return true;
        }

        #endregion AddProfile

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
                txtRandomFolder.Text = fbd.SelectedPath;
            }
            else
            {
                txtFolder.Text = "";
                txtRandomFolder.Text = "";
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
                                Name = string.IsNullOrEmpty(_ReName) ? item.Name : (item.Name.Replace(item.Extension, "") + _ReName)
                            };

                            oUCMSApiClient.Attachment.Upload(attachment);

                            if (!String.IsNullOrEmpty(_MoveTo))
                            {
                                item.MoveTo(_MoveTo + @"\\" + item.Name);
                            }
                            if (!string.IsNullOrEmpty(_ReName))
                            {
                                item.CopyTo((String.IsNullOrEmpty(_MoveTo) ? item.DirectoryName : _MoveTo) + @"\\" + attachment.Name);
                                item.Delete();
                            }
                        }
                    }
                    oUCMSApiClient.Content.Checkin(contentId);// Checkint content
                }
            }
            catch (Exception ex)
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
        private string convertValueField(string config, string documentType)
        {
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
            switch (grdName)
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
                case "grdMultipleProfile":
                    MultipleProfileList.Remove(MultipleProfileList.SingleOrDefault(x=>x.Name.Equals(grdMultipleProfile.CurrentCell.Value)));
                    var source = new BindingSource();
                    source.DataSource = MultipleProfileList;
                    grdMultipleProfile.DataSource = source;
                    break;
            }
        }

        #endregion ConfigField

        #region RandomUpload
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (StopThread == 1)
            {
                StopThread = 2;
            }
            btnStop.Enabled = false;
        }

        private void btnConfigRandom_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            ctmWatchFolder.Show(ptLowerLeft);
        }

        private void AddRanDomProfile(Thread newThread, List<Model.Folder> FolderList, List<Branch> BranchList, string folderPath, Boolean LoadRootFalse = false, String TypeUp = "", string Extension = "", string MoveTo = "")
        {
            Random rdUpload = null;
            int iUpload = 0;
            int TotalWhile = 0;
            Boolean checkUpload = false;
            DirectoryInfo directInfo = null;
            try
            {
                rdUpload = new Random();
                iUpload = 0;
                checkUpload = false;
                

                if (BranchList.Count == 0 || FolderList.Count == 0)
                {
                    StopThread = 0;
                    GC.Collect();
                    newThread.Abort();
                    return;
                }

                directInfo = new DirectoryInfo(folderPath);
                foreach (var item in directInfo.GetFiles())
                {
                    checkUpload = false;                    

                    if ((String.IsNullOrEmpty(TypeUp) || TypeUp.Contains(item.Extension + ";")) && !item.Extension.Equals(Extension))
                    {
                        Branch oBranch = null;
                        Model.Folder oFolder = null;
                        List<Model.Workflow> WorkflowList = null;
                        Model.Workflow oWorkflow = null;
                        Model.WorkflowStep oWorkflowStep = null;
                        Model.WorkflowStep oWorkflowStepRoot = null;
                        ActivityConfiguration oActivityConfiguration = null;
                        List<UniFormType> UniFormTypeList = null;
                        oBatchNamingProfile = null;
                        UniFormType oUniFormType = null;
                        UniFormType oUniFormTypeParent = null;
                        List<UniFormType> UniFormTypeParentList = null;
                        Dictionary<string, object> oContentField = null;
                        Dictionary<string, object> oLibraryField = null;
                        Dictionary<string, object> oContentParent = null;
                        Dictionary<string, object> oLibraryParent = null;
                        List<UniFieldDefinition> UFDList = null;
                        List<FileInfo> arrayFileInfor = null;

                        while (!checkUpload)
                        {
                            TotalWhile++;
                            if (TotalWhile > 500)// Lặp 500 lần không thực hiện upload thì dừng
                            {                             
                                GC.Collect();
                                StopThread = 0;
                                newThread.Abort();
                                return;
                            }

                            oBranch = null;
                            oFolder = null;
                            WorkflowList = null;
                            oWorkflow = null;
                            oWorkflowStep = null;
                            oWorkflowStepRoot = null;
                            oActivityConfiguration = null;
                            UniFormTypeList = null;
                            oBatchNamingProfile = null;
                            oUniFormType = new UniFormType();
                            oUniFormTypeParent = new UniFormType();
                            UniFormTypeParentList = null;
                            oContentField = new Dictionary<string, object>();
                            oLibraryField = new Dictionary<string, object>();
                            oContentParent = new Dictionary<string, object>();
                            oLibraryParent = new Dictionary<string, object>();
                            UFDList = null;
                            arrayFileInfor = null;

                            iUpload = rdUpload.Next(0, BranchList.Count);
                            if (iUpload > BranchList.Count - 1) iUpload = (BranchList.Count - 1);
                            oBranch = BranchList[iUpload];

                            iUpload = rdUpload.Next(0, FolderList.Count);
                            if (iUpload > FolderList.Count - 1) iUpload = (FolderList.Count - 1);
                            oFolder = FolderList[iUpload];

                            WorkflowList = GetData.GetWorkflow(oUCMSApiClient, oFolder.Id);
                            if (WorkflowList.Count == 0) continue;
                            iUpload = rdUpload.Next(0, WorkflowList.Count);
                            if (iUpload > WorkflowList.Count - 1) iUpload = (WorkflowList.Count - 1);
                            oWorkflow = WorkflowList[iUpload];

                            if (oWorkflow.Steps == null || oWorkflow.Steps.Count == 0) continue;
                            oWorkflow.Steps = oWorkflow.Steps.FindAll(x=>String.IsNullOrEmpty(x.Setting));
                            if (oWorkflow.Steps == null || oWorkflow.Steps.Count == 0) continue;
                            iUpload = rdUpload.Next(0, oWorkflow.Steps.Count);
                            if (iUpload > oWorkflow.Steps.Count - 1) iUpload = (oWorkflow.Steps.Count - 1);
                            oWorkflowStep = oWorkflow.Steps[iUpload];

                            oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, oWorkflowStep.Id);

                            if (oActivityConfiguration.SettingReference != null && !oActivityConfiguration.SettingReference.Trim().Equals(Common.SettingReferenceDefault))
                            {
                                oWorkflowStepRoot = oWorkflow.Steps.SingleOrDefault(x => x.Name.Equals(oActivityConfiguration.SettingReference));
                                if (oWorkflowStepRoot == null || string.IsNullOrEmpty(oWorkflowStepRoot.Id)) continue;
                                oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, oWorkflowStepRoot.Id);
                            }


                            if (oActivityConfiguration.DocumentTypeProfile != null)
                            {
                                if (oActivityConfiguration.DocumentTypeProfile.UniFormtypeList != null && oActivityConfiguration.DocumentTypeProfile.UniFormtypeList.Count > 0)
                                {
                                    if (LoadRootFalse)
                                    {
                                        UniFormTypeList = GetData.GetContentType(oUCMSApiClient, oFolder.Id, oActivityConfiguration.DocumentTypeProfile.UniFormtypeList);
                                    }
                                    else
                                    {
                                        UniFormTypeList = GetData.GetContentType(oUCMSApiClient, oFolder.Id, oActivityConfiguration.DocumentTypeProfile.UniFormtypeList).FindAll(x => x.Root);
                                    }
                                }
                                if (oActivityConfiguration.BatchNamingProfile != null)
                                {
                                    oBatchNamingProfile = oActivityConfiguration.BatchNamingProfile;
                                }
                            }

                            if (UniFormTypeList == null) continue;
                            if (oBatchNamingProfile == null) continue;
                            oUniFormType = UniFormTypeList[rdUpload.Next(0, UniFormTypeList.Count - 1)];
                            if (!oUniFormType.Root)
                            {
                                UniFormTypeParentList = UniFormTypeList.FindAll(x => x.Root);
                                if (UniFormTypeParentList == null) continue;
                                oUniFormTypeParent = UniFormTypeParentList[rdUpload.Next(0, UniFormTypeParentList.Count - 1)];
                            }

                            UFDList = GetData.GetListContentField(oUCMSApiClient, oUniFormType);
                            for (int i = 0; i < UFDList.Count; i++)
                            {
                                oContentField.Add(UFDList[i].Name, convertValueField(UFDList[i].DefaultValue, UFDList[i].Name));
                            }

                            UFDList = GetData.GetListLibraryField(oUCMSApiClient, oUniFormType, oFolder.Id);
                            for (int i = 0; i < UFDList.Count; i++)
                            {
                                oLibraryField.Add(UFDList[i].Name, convertValueField(UFDList[i].DefaultValue, UFDList[i].Name));
                            }

                            if (!oUniFormType.Root && !String.IsNullOrEmpty(oUniFormTypeParent.ExternalID))
                            {
                                UFDList = GetData.GetListContentField(oUCMSApiClient, oUniFormTypeParent);
                                for (int i = 0; i < UFDList.Count; i++)
                                {
                                    oContentParent.Add(UFDList[i].Name, convertValueField(UFDList[i].DefaultValue, UFDList[i].Name));
                                }

                                UFDList = GetData.GetListLibraryField(oUCMSApiClient, oUniFormTypeParent, oFolder.Id);
                                for (int i = 0; i < UFDList.Count; i++)
                                {
                                    oLibraryParent.Add(UFDList[i].Name, convertValueField(UFDList[i].DefaultValue, UFDList[i].Name));
                                }
                            }
                            arrayFileInfor = new List<FileInfo>() { item };
                            String ContentNew = Profile(oUCMSApiClient, new DataValue() { Key = oBranch.Name, Value = oBranch.Name }, oFolder, new DataValue() { Key = oWorkflow.Id, Value = oWorkflow.Name }, new DataValue() { Key = oWorkflowStep.Id, Value = oWorkflowStep.Name }, new DataValue() { Key = oUniFormType.ExternalID, Value = oUniFormType.Name }, new DataValue() { Key = oUniFormTypeParent.ExternalID, Value = oUniFormTypeParent.Name }, oContentField, oLibraryField, oContentParent, oLibraryParent, arrayFileInfor, Extension, MoveTo, "USCBatch");
                            checkUpload = true;
                            PrgBarBatchImporterValue++;
                            WriteTextSafe(ContentNew, PrgBarBatchImporterValue);
                            if (StopThread == 2 || StopThread == 0)
                            {
                                StopThread = 0;
                                oBranch = null;
                                oFolder = null;
                                WorkflowList = null;
                                oWorkflow = null;
                                oWorkflowStep = null;
                                oWorkflowStepRoot = null;
                                oActivityConfiguration = null;
                                UniFormTypeList = null;
                                oBatchNamingProfile = null;
                                oUniFormType = null;
                                oUniFormTypeParent = null;
                                UniFormTypeParentList = null;
                                oContentField = null;
                                oLibraryField = null;
                                oContentParent = null;
                                oLibraryParent = null;
                                UFDList = null;
                                arrayFileInfor = null;
                                GC.Collect();
                                newThread.Abort();
                                return;
                            }
                            
                        }
                        oBranch = null;
                        oFolder = null;
                        WorkflowList = null;
                        oWorkflow = null;
                        oWorkflowStep = null;
                        oWorkflowStepRoot = null;
                        oActivityConfiguration = null;
                        UniFormTypeList = null;
                        oBatchNamingProfile = null;
                        oUniFormType = null;
                        oUniFormTypeParent = null;
                        UniFormTypeParentList = null;
                        oContentField = null;
                        oLibraryField = null;
                        oContentParent = null;
                        oLibraryParent = null;
                        UFDList = null;
                        arrayFileInfor = null;
                    }
                    if(PrgBarBatchImporterValue % MultipleProfileList.Count == 0)
                    {
                        MemoryManagement.FlushMemory();
                    }
                    TotalWhile = 0;
                }
            }
            catch (Exception ex)
            {
                Common.LogToFile("AddRanDomProfile_" + ex.Message);
            }

            if (StopThread == 2 || StopThread == 0)
            {
                StopThread = 0;
                GC.Collect();
                newThread.Abort();
            }
        }

        private void AddRanDomProfileList(Thread newThread, List<Model.Folder> FolderList, List<Branch> BranchList, List<String> ListPath, Boolean LoadRootFalse = false, String TypeUp = "", string Extension = "", string MoveTo = "")
        {
            foreach (var folderPath in ListPath)
            {
                AddRanDomProfile(newThread, FolderList, BranchList, folderPath, LoadRootFalse, TypeUp, Extension, MoveTo);
                MemoryManagement.FlushMemory();
            }            
        }

        public delegate void SafeCallDelegate(String ImporterContentLastestName, int ImporterValue);
        private void WriteTextSafe(String ImporterContentLastestName, int ImporterValue)
        {
            if (lblPrgBarTotalAdd.InvokeRequired || lblContentLastest.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                Invoke(d, new object[] { ImporterContentLastestName, ImporterValue });
            }
            else
            {
                //lblPrgBarBatchImporter.Text = ImporterValue + "/" + ImporterCount;
                lblContentLastest.Text = ImporterContentLastestName;
                lblPrgBarTotalAdd.Text = ImporterValue + "";
                if (StopThread == 2)
                {
                    btnSubmit.Enabled = true;
                    btnRandom.Enabled = true;
                }
            }
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            StopThread = 1;
            btnRandom.Enabled = false;
            btnSubmit.Enabled = false;
            btnStop.Enabled = true;            
            PrgBarBatchImporterValue = 0;

            foreach (MultipleProfile item in MultipleProfileList)
            {
                Thread newThread = new Thread(() =>
                {
                    while (true)
                    {
                        AddRanDomProfileList(Thread.CurrentThread, item.FolderList, item.BranchList, item.PathList, false, item.FileUploadType, item.FileUploadReName, item.FileUploadMoveTo);
                        Thread.Sleep(Common.PoolTime);
                    }
                });
                listThread.Add(newThread);
                newThread.Start();                
            }
        }
               
         private void MessageThread()
        {
            switch (StopThread)
            {
                case 1: MessageBox.Show("Please click button Stop before close app"); break;
                case 2: MessageBox.Show("Please waitting app finished"); break;
            }

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

        #endregion RandomUpload

        private void btlAddProfile_Click(object sender, EventArgs e)
        {
            MultipleProfile oMultipleProfile = new MultipleProfile();
            List<Model.Folder> FolderTemp = GetData.GetFolder(oUCMSApiClient);

            if (string.IsNullOrEmpty(txtRandomFolder.Text) || !Directory.Exists(txtRandomFolder.Text))
            {
                MessageBox.Show("The path of upload file doesn't correct");
                oMultipleProfile.Dispose();
                if (FolderTemp.Count > 0) FolderTemp.Clear();
                FolderTemp = null;
                return;
            }

            if (!chkUploadFile.Checked && !chkUploadFolder.Checked)
            {
                MessageBox.Show("Upload File or Upload Folder must checked");
                oMultipleProfile.Dispose();
                if (FolderTemp.Count > 0) FolderTemp.Clear();
                FolderTemp = null;
                return;
            }            

            foreach (DataGridViewRow item in grdLibrary.Rows)
            {
                if (Convert.ToBoolean(item.Cells["grdChkLibraryName"].Value))
                {
                    oMultipleProfile.FolderList.Add(FolderTemp.SingleOrDefault(x => x.Id.Equals(item.Cells["grdLibraryId"].Value)));
                }
            }

            if (oMultipleProfile.FolderList.Count == 0)
            {
                MessageBox.Show("The Library empty");
                oMultipleProfile.Dispose();
                if (FolderTemp.Count > 0) FolderTemp.Clear();
                FolderTemp = null;
                return;
            }

            oMultipleProfile.BranchList = GetData.GetBranch(oUCMSApiClient);

            if (chkUploadFolder.Checked)
            {
                foreach (var item in Directory.GetDirectories(txtRandomFolder.Text))
                {
                    oMultipleProfile.PathList.Add(item);
                    if (chkUploadFolder.Checked)
                    {
                        foreach (var itemChildren in Directory.GetDirectories(item))
                        {
                            oMultipleProfile.PathList.Add(itemChildren);
                        }
                    }
                }
            }

            if (chkUploadFile.Checked)
            {
                oMultipleProfile.PathList.Add(txtRandomFolder.Text);
            }
            
            oMultipleProfile.FileUploadType = _Type;
            oMultipleProfile.FileUploadReName = _ReName;
            oMultipleProfile.FileUploadMoveTo = _MoveTo;
            oMultipleProfile.PathValue = txtRandomFolder.Text;
            oMultipleProfile.CheckFile = chkUploadFile.Checked;
            oMultipleProfile.CheckFolder = chkUploadFolder.Checked;
            if(String.IsNullOrEmpty(nameThread))
            {
                oMultipleProfile.Name = DateTime.Now.ToString("yyMMdd_hhmmssff");
                MultipleProfileList.Add(oMultipleProfile);
                var source = new BindingSource();
                source.DataSource = MultipleProfileList;
                grdMultipleProfile.DataSource = source;
            }
            else
            {
                for (int i = 0; i < MultipleProfileList.Count; i++)
                {
                    if (MultipleProfileList[i].Name.Equals(nameThread))
                    {
                        oMultipleProfile.Name = nameThread;
                        MultipleProfileList[i] = oMultipleProfile;
                        var source = new BindingSource();
                        source.DataSource = MultipleProfileList;
                        grdMultipleProfile.DataSource = source;                        
                    }
                }                
            }
            LoadMultipleProfile((grdMultipleProfile.CurrentRow.DataBoundItem) as MultipleProfile);
            if (FolderTemp.Count > 0) FolderTemp.Clear();
            FolderTemp = null;
            nameThread = null;            
            
        }

        private void grdMultipleProfile_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void grdMultipleProfile_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grdView = sender as DataGridView;
            grdName = grdView.Name;
            if (e.Button == MouseButtons.Right)
            {
                if (grdView.CurrentCell != null && grdView.CurrentCell.Value != null && e.RowIndex != -1)
                {
                    if (grdView.CurrentCell is DataGridViewTextBoxCell)
                    {
                        DataGridViewTextBoxCell btnSender = (DataGridViewTextBoxCell)grdView.CurrentCell;
                        Point ptLowerLeft = new Point(btnSender.Size.Width, btnSender.Size.Height);
                        ptLowerLeft = grdView.PointToScreen(ptLowerLeft);
                        ctmFieldRight.Show(ptLowerLeft);
                    }
                }
            }
        }

        private void grdMultipleProfile_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grdView = sender as DataGridView;
            grdName = grdView.Name;
            if (e.Button == MouseButtons.Left)
            {
                if (grdView.CurrentCell != null && grdView.CurrentCell.Value != null && e.RowIndex != -1)
                {
                    MultipleProfile oMultipleProfile = (grdView.CurrentRow.DataBoundItem as MultipleProfile);
                    LoadMultipleProfile(oMultipleProfile);
                }
            }
        }

        private void LoadMultipleProfile(MultipleProfile oMultipleProfile)
        {
            txtRandomFolder.Text = oMultipleProfile.PathValue;
            chkUploadFile.Checked = oMultipleProfile.CheckFile;
            chkUploadFolder.Checked = oMultipleProfile.CheckFolder;
            _MoveTo = oMultipleProfile.FileUploadMoveTo;
            _ReName = oMultipleProfile.FileUploadReName;
            _Type = oMultipleProfile.FileUploadType;
            nameThread = oMultipleProfile.Name;
            foreach (DataGridViewRow item in grdLibrary.Rows)
            {
                if (oMultipleProfile.FolderList.Exists(x => x.Id.Equals(item.Cells["grdLibraryId"].Value)))
                {
                    item.Cells["grdChkLibraryName"].Value = true.ToString();
                }
                else
                {
                    item.Cells["grdChkLibraryName"].Value = false.ToString();
                }
            }
        }
    }
}
