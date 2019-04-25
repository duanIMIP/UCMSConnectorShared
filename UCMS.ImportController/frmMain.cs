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
        public Dictionary<string, object> LibraryField = new Dictionary<string, object>();
        private String oControlName;
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
                var oUniFormType = cboContentType.SelectedItem as UniFormType;
                tabContentField.Controls.Clear();
                for (int i = 0; i < oUniFormType.UniFieldDefinitions.Count; i++)
                {
                    Label lblnew = new System.Windows.Forms.Label();
                    lblnew.Location = new Point(10, 19 + i * 26);
                    lblnew.Text = oUniFormType.UniFieldDefinitions[i].DisplayName;
                    lblnew.AutoSize = true;
                    lblnew.BackColor = System.Drawing.Color.LightGray;
                    lblnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    tabContentField.Controls.Add(lblnew);

                    TextBox txtnew = new System.Windows.Forms.TextBox();
                    txtnew.Location = new Point(134, 12 + i * 26);
                    txtnew.ReadOnly = true;
                    txtnew.Text = "";
                    txtnew.Name = "txtConfig" + oUniFormType.UniFieldDefinitions[i].Name;
                    txtnew.Size = new Size(374, 20);
                    txtnew.BackColor = System.Drawing.Color.LightGray;
                    txtnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    tabContentField.Controls.Add(txtnew);

                    Button btnSubmit = new System.Windows.Forms.Button();
                    btnSubmit.Location = new System.Drawing.Point(509, 12 + i * 26);
                    btnSubmit.Name = "btnConfig" + oUniFormType.UniFieldDefinitions[i].Name;
                    btnSubmit.Size = new System.Drawing.Size(32, 20);
                    btnSubmit.TabIndex = 0;
                    btnSubmit.Text = "...";
                    btnSubmit.UseVisualStyleBackColor = true;
                    btnSubmit.Click += new System.EventHandler(this.btnConfig_Click);
                    if (oUniFormType.UniFieldDefinitions[i].ReadOnly)
                    {
                        btnSubmit.Enabled = false;
                    }
                    tabContentField.Controls.Add(btnSubmit);
                }
                groupBox1.Visible = true;
            }
            else
            {
                groupBox1.Visible = false;
                tabContentField.Controls.Clear();
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
                foreach (Control item in tabContentField.Controls)
                {
                    if (item.GetType().Name.Equals("TextBox") && (item as TextBox).Name.IndexOf("txtConfig") >= 0)
                    {
                        oContent.Values.Add((item as TextBox).Name.Replace("txtConfig", ""), convertValueField((item as TextBox).Text));
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
            foreach (Control item in tabContentField.Controls)
            {
                if (item.GetType().Name.Equals("TextBox") && (item as TextBox).Name.Contains("txtConfig"))
                {
                    oBatch.Fields.Add(new IMIP.UniversalScan.Data.UniField() { Name = (item as TextBox).Name.Replace("txtConfig", ""), Value = convertValueField((item as TextBox).Text) });
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            var tempNameFolders = "";
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.CheckFileExists = true;
            opnfd.AddExtension = true;
            opnfd.Multiselect = true;
            opnfd.Filter = "Image Files (*.jfif;*.jpg;*.jpeg;.*.gif;*.png;*.doc;*.docx;*.xls;*.xlsx;*.pdf;)|*.jfif;*.jpg;*.jpeg;.*.gif;*.png;*.doc;*.docx;*.xls;*.xlsx;*.pdf;";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in opnfd.FileNames)
                {
                    if (fileName.Replace(@"\" + Path.GetFileName(fileName), "") == txtFolder.Text)
                    {
                        MessageBox.Show("File có đường dẫn trùng với Upload Folder");
                        return;
                    }
                    tempNameFolders = tempNameFolders.Replace(fileName + ";" + Environment.NewLine, "") + fileName + ";" + Environment.NewLine;
                }
            }
        }

        private void btlLibraryField_Click(object sender, EventArgs e)
        {
            frmChildren frm = new frmChildren();
            LibraryField = new Dictionary<string, object>();
            frm.oLibrary = oUCMSApiClient.Folder.GetLibrary(cboLibrary.SelectedValue.ToString());
            //var listContent = oUCMSApiClient.Content. GetContents(cboLibrary.SelectedValue.ToString());
            //foreach (Model.Content item in listContent.Items)
            //{
            //    if(item.ContentType.Id == cboContentType.SelectedValue.ToString())
            //    {
            //        frm.oContent = item;
            //        break;
            //    }
            //}
            frm.oFromChild = "btlLibraryField";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < frm.Controls.Count; i++)
                {
                    if (frm.Controls[i].GetType().Name == "TextBox")
                    {
                        LibraryField.Add(frm.Controls[i].Name, frm.Controls[i].Text);
                    }
                    else if (frm.Controls[i].GetType().Name == "ComboBox")
                    {
                        LibraryField.Add(frm.Controls[i].Name, frm.Controls[i].Text);
                    }
                }
            }
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
        private void btnConfig_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            oControlName = btnSender.Name.Replace("btnConfig", "txtConfig");
            ctmDetails.Show(ptLowerLeft);
        }

        private void ConfigContentField_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btnConfig = sender as ToolStripMenuItem;
            foreach (Control item in tabContentField.Controls)
            {
                if (item.GetType().Name.Equals("TextBox") && (item as TextBox).Name == oControlName)
                {
                    switch (btnConfig.Name)
                    {
                        case "ctmiDocumentType":
                            (item as TextBox).Text += "{$Type}";
                            break;
                        case "ctmiDate":
                            (item as TextBox).Text += "{$Date}";
                            break;
                        case "timeToolStripMenuItem":
                            (item as TextBox).Text += "{$Time}";
                            break;
                        case "machineToolStripMenuItem":
                            (item as TextBox).Text += "{$MachineName}";
                            break;
                        case "userNameToolStripMenuItem":
                            (item as TextBox).Text += "{$UserName}";
                            break;
                    }
                    break;
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
