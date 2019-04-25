using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            LoadBranch();
            LoadLibrary("");
        }

        private void LoadBranch()
        {
            Boolean checkAdd = false;
            if (cboBrank.Items != null && cboBrank.Items.Count > 0)
            {
                cboBrank.Items.Clear();
            }
            var customsetting = oUCMSApiClient.Setting.GetCustomSetting("USCBranches");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(customsetting.Value);
            XmlNode xmlNodeBranch = xmlDocument.DocumentElement.SelectSingleNode("/ActivityConfiguration/Branches");
            foreach (XmlNode item in xmlNodeBranch.ChildNodes)
            {
                checkAdd = false;
                foreach (XmlNode oAccount in item.SelectNodes("Users/Account"))
                {
                    var Name = oAccount.SelectSingleNode("Name").InnerText;
                    if (Name == Common.Username || Name == "Everyone")
                    {
                        checkAdd = true;
                    }
                }
                if (checkAdd)
                {
                    cboBrank.Items.Add(item.SelectSingleNode("Name").InnerText);
                }
            }
            if (cboBrank.Items.Count > 0)
            {
                cboBrank.SelectedIndex = 0;
            }
        }

        private void LoadLibrary(string BranchId)
        {
            var ListFolder = oUCMSApiClient.Folder.GetFolders(BranchId);
            cboLibrary.DataSource = ListFolder.Items;
            cboLibrary.DisplayMember = "Name";
            cboLibrary.ValueMember = "Id";
        }

        private void cboLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLibrary.SelectedItem != null)
            {
                cboWorkflow.Text = "";
                var obj = cboLibrary.SelectedItem as Model.Folder;
                cboWorkflow.DataSource = oUCMSApiClient.Workflow.GetAll(obj.Id);
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
                cboWorkflowStep.ValueMember = "ID";
            }
        }

        private void cboWorkflowStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWorkflowStep.SelectedValue.GetType().Name == "String")
            {
                Model.WorkflowStepSetting workFlowStep = oUCMSApiClient.Workflow.GetStepSetting(cboWorkflowStep.SelectedValue.ToString());
                List<UniFormType> listUniFormType = new List<UniFormType>();

                if (workFlowStep != null && !string.IsNullOrEmpty(workFlowStep.Setting))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(workFlowStep.Setting);
                    XmlNode xmlNodeDocumentTypeProfile = xmlDocument.DocumentElement.SelectSingleNode("/ActivityConfiguration/DocumentTypeProfileSetting");

                    foreach (XmlNode item in xmlNodeDocumentTypeProfile.SelectNodes("UniFormtypeList/UniFormType"))
                    {
                        listUniFormType.Add(new UniFormType()
                        {
                            Name = item.Attributes["Name"].Value,
                            Visible = Convert.ToBoolean(item.Attributes["Visible"].Value),
                            ExternalID = item.Attributes["ExternalID"].Value,
                            Root = Convert.ToBoolean(item.Attributes["Root"].Value),
                            UniFieldDefinitions = getUniFieldDefinition(item)
                        });

                    }
                }

                if (listUniFormType.Count > 0)
                {
                    groupBox1.Visible = true;
                    cboContentType.Text = "";
                    cboContentType.DataSource = listUniFormType;
                    cboContentType.DisplayMember = "Name";
                    cboContentType.ValueMember = "ExternalID";
                }
                else
                {
                    groupBox1.Visible = false;
                }
            }
        }

        private void cboContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboContentType.SelectedItem != null)
            {
                grdContentField.Rows.Clear();
                var oUniFormType = cboContentType.SelectedItem as UniFormType;
                for (int i = 0; i < oUniFormType.UniFieldDefinitions.Count; i++)
                {
                    string[] row = new string[] { (i + 1).ToString(), oUniFormType.UniFieldDefinitions[i].DisplayName, "", oUniFormType.UniFieldDefinitions[i].Name };
                    grdContentField.Rows.Add(row);
                }
                groupBox1.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
                grdContentField.Rows.Clear();
                tabLibraryField.Controls.Clear();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Model.Content oContent = new Model.Content();
                if (!CheckSubmit())
                {
                    return;
                }
                oContent.Folder = cboLibrary.SelectedItem as Model.Folder;

                //---------------------------------------------------------
                oContent.Tags = new List<string>();
                oContent.Name = Naming();
                if (oContent.Name == "")
                {
                    MessageBox.Show("Setting naming cho content");
                    return;
                }
                //oContent.Tags.Add(oContent.Name.Replace(" ", "_"));
                //-----------------------------------------------------------
                oContent.Values = new Dictionary<string, object>();
                foreach (DataGridViewRow item in grdContentField.Rows)
                {
                    if (!item.IsNewRow)
                    {
                        oContent.Values.Add(item.Cells["NameId"].Value.ToString(), convertValueField(item.Cells["txtValue"].Value.ToString()));
                    }
                }
                oContent.Values.Add("BranchId", cboBrank.Text);

                oContent.LibraryFieldValues = new Dictionary<string, object>();
                foreach (Control item in tabLibraryField.Controls)
                {
                    if (item.GetType().Name.Equals("TextBox"))
                    {
                        oContent.LibraryFieldValues.Add((item as TextBox).Name, (item as TextBox).Text);
                    }
                }

                oContent.ContentType = new Model.ContentType() { Id = cboContentType.SelectedValue.ToString() };
                var oContentMd = oUCMSApiClient.Content.Create(oContent);

                if(!UploadSave(oContentMd.Id))
                {
                    return;
                }

                oUCMSApiClient.Content.SetPrivateData(oContentMd.Id, new Model.ContentPrivateData()
                {
                    Key = "USCBatch",
                    Value = PrivateData(oContentMd.Id, _MoveTo)
                });

                //---Insert content in workflow------------------------------------------------------------
                Model.WorkflowItem oWorkflowItem = new Model.WorkflowItem();
                oWorkflowItem.Content = oContentMd;
                oWorkflowItem.WorkflowStep = new Model.WorkflowStep() { Id = (cboWorkflowStep.SelectedItem as Model.WorkflowStep).Id };
                oWorkflowItem.Workflow = new Model.Workflow() { Id = (cboWorkflow.SelectedItem as Model.Workflow).Id };
                oWorkflowItem.State = Model.Enum.WorkflowItemState.Ready;
                oWorkflowItem.Priority = Model.Enum.WorkflowItemPriority.Normal;
                oUCMSApiClient.WorkflowItem.Insert(oWorkflowItem, true);
                //-----------------------------------------------------------------------------------------

                MessageBox.Show("Cập nhật thành công");
                txtFolder.Text = "";
                cboContentType.SelectedValue = "";
                cboContentType.Text = "";
                cboContentType_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi trong quá trình cập nhật");
                Common.LogToFile(ex.Message);
            }
        }

        private String PrivateData(String contentId, string pathClient)
        {
            var content = oUCMSApiClient.Content.GetById(contentId);
            IMIP.UniversalScan.Data.UniBatch oBatch = new IMIP.UniversalScan.Data.UniBatch();
            oBatch.BranchID = cboBrank.Text;
            oBatch.Name = content.Name;
            oBatch.ClientName = cboLibrary.Text;
            oBatch.ProcessName = cboWorkflow.Text;
            oBatch.ProcessStepName = cboWorkflowStep.Text;
            oBatch.FormTypeName = content.ContentType.Name;
            oBatch.Fields = new List<IMIP.UniversalScan.Data.UniField>();

            foreach (DataGridViewRow item in grdContentField.Rows)
            {
                if (!item.IsNewRow)
                {
                    oBatch.Fields.Add(new IMIP.UniversalScan.Data.UniField() { Name = item.Cells["NameId"].Value.ToString(), Value = convertValueField(item.Cells["txtValue"].Value.ToString()) });
                }
            }

            oBatch.Pages = new List<IMIP.UniversalScan.Data.UniPage>();

            for (int i = 0; i < content.Attachments.Count; i++)
            {
                oBatch.Pages.Add(new IMIP.UniversalScan.Data.UniPage()
                {
                    ID = Path.GetFileNameWithoutExtension(content.Attachments[i].Name),
                    FullFileName = (string.IsNullOrEmpty(pathClient)? txtFolder.Text: pathClient) + @"\" + content.Attachments[i].Name,
                    Rejected = false,
                    IsRescan = false,
                    IsNew = false,
                    SheetID = "",
                    RejectedNote = ""
                });
            }

            return Common.SerializeObjectToString(typeof(IMIP.UniversalScan.Data.UniBatch), oBatch);
        }

        private String Naming()
        {
            String tempName = "";
            Model.WorkflowStepSetting workFlowStep = oUCMSApiClient.Workflow.GetStepSetting(cboWorkflowStep.SelectedValue.ToString());
            XmlDocument xmlDocument = new XmlDocument();
            if (string.IsNullOrEmpty(workFlowStep.Setting))
            {
                return "";
            }
            xmlDocument.LoadXml(workFlowStep.Setting);
            XmlNode xmlNodeBatchNaming = xmlDocument.DocumentElement.SelectSingleNode("/ActivityConfiguration/BatchNamingProfileSetting");

            if (xmlNodeBatchNaming == null)
            {
                return "";
            }

            BatchNamingProfileSetting oBatchName = Common.DeserializeXML<BatchNamingProfileSetting>(xmlNodeBatchNaming.OuterXml);

            if (oBatchName == null)
            {
                return "";
            }

            if (!oBatchName.Enabled)
            {
                return cboBrank.Text + cboContentType.Text + DateTime.Now.ToString("yyMMddHHmmssff");
            }

            foreach (var item in oBatchName.BatchNamingSettings.Items)
            {
                if (item.DocumentTypeName.Equals(cboContentType.Text))
                {
                    foreach (var itemChild in item.BatchNamingTemplate.Items)
                    {
                        switch (itemChild.Type)
                        {
                            case Common.SourceFieldType.DocVariable:
                                tempName += cboContentType.Text;
                                break;
                            case Common.SourceFieldType.System:
                                if (itemChild.StaticName.Equals("Date"))
                                {
                                    tempName += DateTime.Now.ToString(item.DateFormat);
                                }
                                else if (itemChild.StaticName.Equals("Time"))
                                {
                                    tempName += DateTime.Now.ToString(item.TimeFormat);
                                }
                                else
                                {
                                    tempName += itemChild.DisplayName;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return tempName;
        }

        private bool CheckSubmit()
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
            return true;
        }        

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtFolder.Text = "";
            cboContentType.SelectedValue = "";
            cboContentType.Text = "";
            groupBox1.Visible = false;
            tabContentField.Controls.Clear();
            tabLibraryField.Controls.Clear();
        }

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
            switch (btnConfig.Name)
            {
                case "ctmiDocumentType":
                    grdContentField.CurrentCell.Value += "{$Type}";
                    break;
                case "ctmiDate":
                    grdContentField.CurrentCell.Value += "{$Date}";
                    break;
                case "timeToolStripMenuItem":
                    grdContentField.CurrentCell.Value += "{$Time}";
                    break;
                case "machineToolStripMenuItem":
                    grdContentField.CurrentCell.Value += "{$MachineName}";
                    break;
                case "userNameToolStripMenuItem":
                    grdContentField.CurrentCell.Value += "{$UserName}";
                    break;
            }
        }
        private void grdContentField_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (grdContentField.CurrentCell != null && grdContentField.CurrentCell.Value != null && e.RowIndex != -1)
                {
                    if (grdContentField.CurrentCell is DataGridViewTextBoxCell && grdContentField.CurrentCell.ColumnIndex.Equals(2))
                    {
                        DataGridViewTextBoxCell btnSender = (DataGridViewTextBoxCell)grdContentField.CurrentCell;
                        Point ptLowerLeft = new Point(btnSender.Size.Width, btnSender.Size.Height);
                        ptLowerLeft = grdContentField.PointToScreen(ptLowerLeft);
                        ctmFieldRight.Show(ptLowerLeft);
                    }
                }
            }
        }
        private void grdContentField_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (grdContentField.CurrentCell != null && grdContentField.CurrentCell.Value != null && e.RowIndex != -1)
                {
                    if (grdContentField.CurrentCell is DataGridViewTextBoxCell && grdContentField.CurrentCell.ColumnIndex.Equals(2))
                    {
                        DataGridViewTextBoxCell btnSender = (DataGridViewTextBoxCell)grdContentField.CurrentCell;
                        Point ptLowerLeft = new Point(btnSender.Size.Width, btnSender.Size.Height);
                        ptLowerLeft = grdContentField.PointToScreen(ptLowerLeft);
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
            grdContentField.CurrentCell.Value = "";
        }

        #endregion ConfigField


        private List<UniFieldDefinition> getUniFieldDefinition(XmlNode node)
        {
            List<UniFieldDefinition> list = new List<UniFieldDefinition>();
            foreach (XmlNode item in node.SelectNodes("FieldDefs/UniFieldDefinition"))
            {
                list.Add(new UniFieldDefinition()
                {
                    DisplayName = item.Attributes["DisplayName"].Value,
                    ReadOnly = Convert.ToBoolean(item.Attributes["ReadOnly"].Value),
                    MaxLength = Convert.ToInt32(item.Attributes["MaxLength"].Value),
                    DefaultValue = item.Attributes["DefaultValue"].Value,
                    ExternalID = item.Attributes["ExternalID"].Value,
                    Name = item.Attributes["Name"].Value
                });
            }

            return list;
        }

        
    }
}
