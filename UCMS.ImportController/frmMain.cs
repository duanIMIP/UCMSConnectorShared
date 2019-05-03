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
        private Thread newThread;
        public frmMain()
        {
            InitializeComponent();
            oUCMSApiClient = new UCMSApiClient(Common.Username, Common.Password, Common.UCMSWebAPIEndPoint, Common.UCMSAuthorizationServer);
            _ReName = "";
            _MoveTo = "";
            _Type = ".tif;.tiff;.pdf";
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
            List<Model.Folder> floderList = GetData.GetFolder(oUCMSApiClient);
            cboLibrary.DataSource = floderList;
            cboLibrary.DisplayMember = "Name";
            cboLibrary.ValueMember = "Id";

            grdLibrary.DataSource = floderList;
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
                ActivityConfiguration oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, obj.Id);
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
            btnSubmit.Enabled = false;
            AddEachAttachProfile();
            btnSubmit.Enabled = true;
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
        }

        #region AddProfile

        private Boolean Profile(UCMSApiClient oUCMSApiClient, DataValue oBranch, Model.Folder oFolder, DataValue oWorkflow, DataValue oWorkflowStep, DataValue oContenType, DataValue oContenTypeParent, Dictionary<string, object> oContentField, Dictionary<string, object> oLibraryField, Dictionary<string, object> oContentParent, Dictionary<string, object> oLibraryParent, IEnumerable<FileInfo> arrayFileInfor, String RenameFile, String RemoveFile, String keyPrivateData)
        {
            try
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
                if (!String.IsNullOrEmpty(oContenTypeParent.Key))//Branch or Document
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenTypeParent.Value, oBranch.Value, oFolder.Name, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenTypeParent.Key };
                    var temp = oContentParent;
                    temp.Add("BranchId", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryParent;
                }
                else
                {
                    oWorkflowItem.Content.Name = GetData.Naming(oContenType.Value, oBranch.Value, oFolder.Name, oBatchNamingProfile);
                    oWorkflowItem.Content.ContentType = new Model.ContentType() { Id = oContenType.Key };
                    var temp = oContentField;
                    temp.Add("BranchId", oBranch.Value);
                    oWorkflowItem.Content.Values = temp;
                    oWorkflowItem.Content.LibraryFieldValues = oLibraryField;
                }
                oWorkflowItem.Content = oUCMSApiClient.Content.Create(oWorkflowItem.Content);

                oWorkflowItem.Content.Attachments = new List<Model.Attachment>();
                oUCMSApiClient.Content.Checkout(oWorkflowItem.Content.Id);

                foreach (var oFileInfor in arrayFileInfor)
                {
                    pathClient = oFileInfor.DirectoryName;
                    var attachment = new Model.Attachment()
                    {
                        ContentId = oWorkflowItem.Content.Id,
                        Data = File.ReadAllBytes(oFileInfor.FullName),
                        MIME = "image/universalscan",
                        Type = UCMS.Model.Enum.AttachmentType.Public,
                        Name = Guid.NewGuid() + oFileInfor.Extension
                    };

                    oUCMSApiClient.Attachment.Upload(attachment);
                    if (!String.IsNullOrEmpty(RemoveFile) && !File.Exists(RemoveFile + @"\\" + oFileInfor.Name))
                    {
                        oFileInfor.MoveTo(RemoveFile + @"\\" + oFileInfor.Name);
                    }
                    if (!string.IsNullOrEmpty(RenameFile) && !File.Exists(oFileInfor.FullName.Replace(oFileInfor.Extension, "") + RenameFile))
                    {
                        oFileInfor.CopyTo(oFileInfor.FullName.Replace(oFileInfor.Extension, "") + RenameFile);
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
                    UniDocument oUniDocument = new UniDocument();
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
                oUCMSApiClient.WorkflowItem.Insert(oWorkflowItem, true);
                return true;
            }
            catch (Exception ex)
            {
                Common.LogToFile("Profile_" + ex.Message);
                return false;
            }
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
                        if((String.IsNullOrEmpty(_Type) || _Type.Contains(item.Extension + ";")) && !item.Extension.Equals(_ReName))
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

                if (Profile(oUCMSApiClient, oBranch, oFolder, oWorkflow, oWorkflowStep, oContenType, oContenTypeParent, oContentField, oLibraryField, oContentParent, oLibraryParent, arrayFileInfor, _ReName, _MoveTo, "USCBatch"))
                {
                    MessageBox.Show("Cập nhật thành công");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi trong quá trình cập nhật");
                Common.LogToFile("AddControllProfile_" + ex.Message);
                return false;
            }
        }

        private bool AddEachAttachProfile(Boolean LoadRootFalse = false)// LoadRootFalse = false: không apload với content không là gốc
        {
            try
            {
                if (!CheckControllProfile(LoadRootFalse))
                {
                    return false;
                }
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

                            Profile(oUCMSApiClient, oBranch, oFolder, oWorkflow, oWorkflowStep, oContenType, oContenTypeParent, oContentField, oLibraryField, oContentParent, oLibraryParent, new FileInfo[] { itemInfo }, _ReName, _MoveTo, "USCBatch");
                            CoutNumber++;
                        }
                    }
                    if (CoutNumber == 0)
                    {
                        MessageBox.Show("Not contains attachment");
                        btnUploadFolder.Focus();
                        return false;
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
            }
        }

        #endregion ConfigField

        #region RandomUpload
        private void btnStop_Click(object sender, EventArgs e)
        {
            newThread.Abort();
            btnRandom.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnConfigRandom_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            ctmWatchFolder.Show(ptLowerLeft);
        }

        private void AddRanDomProfile(List<Model.Folder> FolderList, string folderPath, Boolean LoadRootFalse = false, String TypeUp = "", string Extension = "", string MoveTo = "")// LoadRootFalse = false: không apload với content không là gốc
        {
            try
            {
                Random rdUpload = new Random();
                Boolean checkUpload = false;
                DirectoryInfo directInfo = new DirectoryInfo(folderPath);
                List<Branch> BranchList = GetData.GetBranch(oUCMSApiClient);
                if (BranchList.Count == 0 || FolderList.Count == 0) return;
                foreach (var item in directInfo.GetFiles())
                {
                    checkUpload = false;
                    if ((String.IsNullOrEmpty(TypeUp) || TypeUp.Contains(item.Extension + ";")) && !item.Extension.Equals(Extension))
                    {
                        while (!checkUpload)
                        {
                            Branch oBranch = null;
                            Model.Folder oFolder = null;
                            List<Model.Workflow> WorkflowList = null;
                            Model.Workflow oWorkflow = null;
                            Model.WorkflowStep oWorkflowStep = null;
                            ActivityConfiguration oActivityConfiguration = null;
                            List<UniFormType> UniFormTypeList = null;
                            oBatchNamingProfile = null;
                            UniFormType oUniFormType = new UniFormType();
                            UniFormType oUniFormTypeParent = new UniFormType();
                            List<UniFormType> UniFormTypeParentList = null;
                            Dictionary<string, object> oContentField = new Dictionary<string, object>();
                            Dictionary<string, object> oLibraryField = new Dictionary<string, object>();
                            Dictionary<string, object> oContentParent = new Dictionary<string, object>();
                            Dictionary<string, object> oLibraryParent = new Dictionary<string, object>();

                            oBranch = BranchList[rdUpload.Next(0, BranchList.Count - 1)];
                            oFolder = FolderList[rdUpload.Next(0, FolderList.Count - 1)];

                            WorkflowList = GetData.GetWorkflow(oUCMSApiClient, oFolder.Id);
                            if (WorkflowList.Count == 0) continue;
                            oWorkflow = WorkflowList[rdUpload.Next(0, WorkflowList.Count - 1)];

                            if (oWorkflow.Steps.Count == 0) continue;
                            oWorkflowStep = oWorkflow.Steps[rdUpload.Next(0, oWorkflow.Steps.Count - 1)];

                            oActivityConfiguration = GetData.GetActivityConfiguration(oUCMSApiClient, oWorkflowStep.Id);

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

                            foreach (UniFieldDefinition uniFieldChild in GetData.GetListContentField(oUCMSApiClient, oUniFormType))
                            {
                                oContentField.Add(uniFieldChild.Name, convertValueField(uniFieldChild.DefaultValue, uniFieldChild.Name));
                            }
                            foreach (UniFieldDefinition uniFieldChild in GetData.GetListLibraryField(oUCMSApiClient, oUniFormType, oFolder.Id))
                            {
                                oLibraryField.Add(uniFieldChild.Name, convertValueField(uniFieldChild.DefaultValue, uniFieldChild.Name));
                            }
                            if (!oUniFormType.Root && !String.IsNullOrEmpty(oUniFormTypeParent.ExternalID))
                            {
                                foreach (UniFieldDefinition uniFieldChild in GetData.GetListContentField(oUCMSApiClient, oUniFormTypeParent))
                                {
                                    oContentParent.Add(uniFieldChild.Name, convertValueField(uniFieldChild.DefaultValue, uniFieldChild.Name));
                                }
                                foreach (UniFieldDefinition uniFieldChild in GetData.GetListLibraryField(oUCMSApiClient, oUniFormTypeParent, oFolder.Id))
                                {
                                    oLibraryParent.Add(uniFieldChild.Name, convertValueField(uniFieldChild.DefaultValue, uniFieldChild.Name));
                                }
                            }
                            FileInfo[] arrayFileInfor = new FileInfo[] { item };
                            Profile(oUCMSApiClient, new DataValue() { Key = oBranch.Name, Value = oBranch.Name }, oFolder, new DataValue() { Key = oWorkflow.Id, Value = oWorkflow.Name }, new DataValue() { Key = oWorkflowStep.Id, Value = oWorkflowStep.Name }, new DataValue() { Key = oUniFormType.ExternalID, Value = oUniFormType.Name }, new DataValue() { Key = oUniFormTypeParent.ExternalID, Value = oUniFormTypeParent.Name }, oContentField, oLibraryField, oContentParent, oLibraryParent, arrayFileInfor, Extension, MoveTo, "USCBatch");
                            checkUpload = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogToFile("AddRanDomProfile_" + ex.Message);
            }
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            btnRandom.Enabled = false;
            btnStop.Enabled = true;
            var folderPath = txtRandomFolder.Text;
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Đường dẫn upload file không chính xác");
                btnRandom.Enabled = true;
                btnStop.Enabled = false;
                return;
            }
            List<Model.Folder> FolderTemp = GetData.GetFolder(oUCMSApiClient);
            List<Model.Folder> FolderList = new List<Model.Folder>();
            foreach (DataGridViewRow item in grdLibrary.Rows)
            {
                if (Convert.ToBoolean (item.Cells["grdChkLibraryName"].Value))
                {
                    FolderList.Add(FolderTemp.SingleOrDefault(x => x.Id.Equals(item.Cells["grdLibraryId"].Value)));
                }
            }

            newThread = new Thread(() =>
            {
                while (true)
                {
                    AddRanDomProfile(FolderList, folderPath, false, _Type, _ReName, _MoveTo);
                    Thread.Sleep(Common.PoolTime);
                }
            });
            newThread.Start();
        }

        #endregion RandomUpload
    }
}
