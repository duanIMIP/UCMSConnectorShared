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
using UCMS.RestClient;

namespace UCMS.ImportController
{
    public partial class WatchFolder : Form
    {
        public UCMSApiClient oUCMSApiClient = null;
        public Dictionary<string, object> LibraryField = new Dictionary<string, object>();
        public Dictionary<string, object> ContentTypes = new Dictionary<string, object>();
        public WatchFolder()
        {
            InitializeComponent();
            oUCMSApiClient = new UCMSApiClient(Common.Username, Common.Password, Common.UCMSWebAPIEndPoint, Common.UCMSAuthorizationServer);
        }

        private void WatchFolder_Load(object sender, EventArgs e)
        {
            if (!oUCMSApiClient.Login())
            {
                this.Close();
            }

            //LoadBranch
            LoadBranch();
            LoadLibrary("");
        }

        private void LoadBranch()
        {
            Boolean checkAdd = false;
            if(cboBrank.Items!= null && cboBrank.Items.Count>0)
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
            if(cboBrank.Items.Count>0)
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
                //Workflow;
                var obj = cboLibrary.SelectedItem as Model.Folder;
                cboWorkflow.DataSource = oUCMSApiClient.Workflow.GetAll(obj.Id);
                cboWorkflow.DisplayMember = "Name";
                cboWorkflow.ValueMember = "Id";

                var content = oUCMSApiClient.Folder.GetLibrary(obj.Id);
                cboContentType.DataSource = content.ContentTypes;
                cboContentType.DisplayMember = "Name";
                cboContentType.ValueMember = "Id";
            }
        }

        private void cboWorkflow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWorkflow.SelectedItem != null)
            {
                //Workflow;
                var obj = (sender as ComboBox).SelectedItem as Model.Workflow;
                cboWorkflowStep.DataSource = obj.Steps;
                cboWorkflowStep.DisplayMember = "Name";
                cboWorkflowStep.ValueMember = "Id";
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
                oContent.Name = cboBrank.Text + cboContentType.Text + DateTime.Now.ToString("yyMMddHHmmssff");
                //oContent.Tags.Add(oContent.Name.Replace(" ", "_"));
                //-----------------------------------------------------------

                //Update Field Library content values
                if(LibraryField.Count == 0 || ContentTypes.Count == 0)
                {
                    var library = oUCMSApiClient.Folder.GetLibrary(oContent.Folder.Id);
                    if (LibraryField.Count == 0)
                    {
                        LibraryField = new Dictionary<string, object>();
                        foreach (var item in library.Fields)
                        {
                            LibraryField.Add(item.Id, item.DefaultValue);
                        }
                    }

                    if (ContentTypes.Count == 0 && library.ContentTypes.Count > 0)
                    {
                        var contentType = oUCMSApiClient.ContentType.GetById(cboContentType.SelectedValue.ToString());
                        ContentTypes = new Dictionary<string, object>();
                        foreach (var item in contentType.Fields) //Chon gia tri cho contentType
                        {
                            ContentTypes.Add(item.Name, item.DefaultValue);
                        }
                    }
                }

                oContent.ContentType = new Model.ContentType() { Id = cboContentType.SelectedValue.ToString() };
                oContent.LibraryFieldValues = LibraryField;
                ContentTypes.Add("BranchId", cboBrank.Text);
                oContent.Values = ContentTypes;

                var oContentMd = oUCMSApiClient.Content.Create(oContent);               

                // ---------Attachment-----------------------------------------------------------------
                oUCMSApiClient.Content.Checkout(oContentMd.Id);//Checkout content
                foreach (String item in txtFiles.Text.Split(';'))
                {
                    if (item.Trim().Length > 0)
                    {
                        var attachment = new Model.Attachment();
                        attachment.Name = Path.GetFileName(item.Trim());
                        attachment.ContentId = oContentMd.Id;
                        attachment.MIME = Path.GetExtension(item.Trim()).Replace(".", "image/");
                        attachment.Data = File.ReadAllBytes(item.Trim());
                        attachment.Type = Model.Enum.AttachmentType.Public;
                        attachment.Path = item.Trim();
                        oUCMSApiClient.Attachment.Upload(attachment);
                    }
                }
                if (txtFolder.Text.Trim() != "")
                {
                    if (Directory.Exists(txtFolder.Text.Trim()))
                    {
                        String[] fileEntries = Directory.GetFiles(txtFolder.Text.Trim());
                        foreach (string item in fileEntries)
                        {
                            var attachment = new Model.Attachment();
                            attachment.Name = Path.GetFileName(item.Trim());
                            attachment.ContentId = oContentMd.Id;
                            attachment.MIME = Path.GetExtension(item.Trim()).Replace(".", "image/");
                            attachment.Data = File.ReadAllBytes(item.Trim());
                            attachment.Type = Model.Enum.AttachmentType.Public;
                            attachment.Path = item.Trim();
                            oUCMSApiClient.Attachment.Upload(attachment);
                        }
                    }
                }

                oUCMSApiClient.Content.Checkin(oContentMd.Id);// Checkint content
                //-----------------------------------------------------------------------------------------

                oUCMSApiClient.Content.SetPrivateData(oContentMd.Id, new Model.ContentPrivateData()
                {
                    Key = "USCBatch",
                    Value = PrivateData(oContentMd.Id)
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
                txtFiles.Text = "";
                txtFolder.Text = "";
                ContentTypes = new Dictionary<string, object>();
                LibraryField = new Dictionary<string, object>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi trong quá trình cập nhật");
                Common.LogToFile(ex.Message);
            }
        }

        private String PrivateData(String contentId)
        {
            IMIP.UniversalScan.Data.UniBatch oBatch = new IMIP.UniversalScan.Data.UniBatch();
            oBatch.ClientName = cboLibrary.SelectedText;
            oBatch.ProcessName = cboWorkflow.SelectedText;
            oBatch.ProcessStepName = cboWorkflowStep.SelectedText;
            oBatch.FormTypeName = cboContentType.SelectedText;
            oBatch.Fields = new List<IMIP.UniversalScan.Data.UniField>();
            foreach (var item in LibraryField)
            {
                oBatch.Fields.Add(new IMIP.UniversalScan.Data.UniField() { Name = item.Key, Value = item.Value.ToString(),  });
            }
            oBatch.Pages = new List<IMIP.UniversalScan.Data.UniPage>();
            var content = oUCMSApiClient.Content.GetById(contentId);
            for (int i = 0; i < content.Attachments.Count; i++)
            {
                oBatch.Pages.Add(new IMIP.UniversalScan.Data.UniPage()
                {
                    ID = content.Attachments[i].Id,
                    FullFileName = content.Attachments[i].Name,
                    Rejected = false,
                    IsRescan = false,
                    IsNew = false,
                    SheetID = "",
                    RejectedNote = ""
                });
            }
            XmlSerializer xsSubmit = new XmlSerializer(typeof(IMIP.UniversalScan.Data.UniBatch));
            XmlDocument doc = new XmlDocument();
            StringWriter sww = new StringWriter();
            XmlWriter writer = XmlWriter.Create(sww);
            xsSubmit.Serialize(writer, oBatch);
            var xml = sww.ToString(); // Your xml
            return xml;
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
            if (cboContentType.SelectedItem == null || (cboContentType.SelectedItem as Model.ContentType).Id == "")
            {
                MessageBox.Show("ContentType không được để trống");
                return false;
            }
            return true;
        }
        
        private void btnUpload_Click(object sender, EventArgs e)
        {
            var tempNameFolders = txtFiles.Text;
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.CheckFileExists = true;
            opnfd.AddExtension = true;
            opnfd.Multiselect = true;
            opnfd.Filter = "Image Files (*.jfif;*.jpg;*.jpeg;.*.gif;*.png;*.doc;*.docx;*.xls;*.xlsx;*.pdf;)|*.jfif;*.jpg;*.jpeg;.*.gif;*.png;*.doc;*.docx;*.xls;*.xlsx;*.pdf;";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {                
                foreach (string fileName in opnfd.FileNames)
                {
                    if(fileName.Replace(@"\" + Path.GetFileName(fileName), "") == txtFolder.Text)
                    {
                        MessageBox.Show("File có đường dẫn trùng với Upload Folder");
                        return;
                    }
                    tempNameFolders = tempNameFolders.Replace(fileName + ";" + Environment.NewLine, "") + fileName + ";" + Environment.NewLine;
                }
                txtFiles.Text = tempNameFolders;
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

        private void btlContentType_Click(object sender, EventArgs e)
        {
            frmChildren frm = new frmChildren();
            ContentTypes = new Dictionary<string, object>();
            frm.oContentType = oUCMSApiClient.ContentType.GetById(cboContentType.SelectedValue.ToString());
            frm.oFromChild = "btlContentType";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < frm.Controls.Count; i++)
                {
                    if (frm.Controls[i].GetType().Name == "TextBox")
                    {
                        ContentTypes.Add(frm.Controls[i].Name, frm.Controls[i].Text);
                    }
                }
            }
        }

        private void btnUploadFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog()== DialogResult.OK)
            {
                foreach (string fileName in txtFiles.Text.Split(';'))
                {
                    if (fileName.Trim().Replace(@"\" + Path.GetFileName(fileName.Trim()), "") == fbd.SelectedPath)
                    {
                        MessageBox.Show("Folder Upload có đường dẫn trùng với Upload File");
                        return;
                    }
                }
                txtFolder.Text = fbd.SelectedPath;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtFiles.Text = "";
            txtFolder.Text = "";
            ContentTypes = new Dictionary<string, object>();
            LibraryField = new Dictionary<string, object>();
        }
    }
}
