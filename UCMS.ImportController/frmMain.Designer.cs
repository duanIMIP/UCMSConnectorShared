namespace UCMS.ImportController
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (newThread!= null && newThread.IsAlive)
            {
                newThread.Abort();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.cboBrank = new System.Windows.Forms.ComboBox();
            this.cboLibrary = new System.Windows.Forms.ComboBox();
            this.lbLibrary = new System.Windows.Forms.Label();
            this.cboWorkflow = new System.Windows.Forms.ComboBox();
            this.cboWorkflowStep = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnUploadFolder = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.cboContentType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabContent = new System.Windows.Forms.TabControl();
            this.tabContentField = new System.Windows.Forms.TabPage();
            this.grdContentField = new System.Windows.Forms.DataGridView();
            this.txtNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtNameId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabLibraryField = new System.Windows.Forms.TabPage();
            this.grdLibraryField = new System.Windows.Forms.DataGridView();
            this.txtlcNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtlcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtlcValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtlcNameId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabContentParent = new System.Windows.Forms.TabPage();
            this.grdContentParent = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabLibraryParent = new System.Windows.Forms.TabPage();
            this.grdLibraryParent = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grUpdate = new System.Windows.Forms.GroupBox();
            this.lblParentContent = new System.Windows.Forms.Label();
            this.cboParentContentType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ctmWatchFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmDetails = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctmiDocumentType = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmiDate = new System.Windows.Forms.ToolStripMenuItem();
            this.timeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmFieldRight = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grAction = new System.Windows.Forms.GroupBox();
            this.btnRandom = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtRandomFolder = new System.Windows.Forms.TextBox();
            this.btnConfigRandom = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabContent.SuspendLayout();
            this.tabContentField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdContentField)).BeginInit();
            this.tabLibraryField.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLibraryField)).BeginInit();
            this.tabContentParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdContentParent)).BeginInit();
            this.tabLibraryParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLibraryParent)).BeginInit();
            this.grUpdate.SuspendLayout();
            this.ctmWatchFolder.SuspendLayout();
            this.ctmDetails.SuspendLayout();
            this.ctmFieldRight.SuspendLayout();
            this.grAction.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Branch";
            // 
            // cboBrank
            // 
            this.cboBrank.FormattingEnabled = true;
            this.cboBrank.Location = new System.Drawing.Point(103, 19);
            this.cboBrank.Name = "cboBrank";
            this.cboBrank.Size = new System.Drawing.Size(510, 21);
            this.cboBrank.TabIndex = 1;
            // 
            // cboLibrary
            // 
            this.cboLibrary.FormattingEnabled = true;
            this.cboLibrary.Location = new System.Drawing.Point(103, 46);
            this.cboLibrary.Name = "cboLibrary";
            this.cboLibrary.Size = new System.Drawing.Size(510, 21);
            this.cboLibrary.TabIndex = 3;
            this.cboLibrary.SelectedIndexChanged += new System.EventHandler(this.cboLibrary_SelectedIndexChanged);
            // 
            // lbLibrary
            // 
            this.lbLibrary.AutoSize = true;
            this.lbLibrary.Location = new System.Drawing.Point(50, 54);
            this.lbLibrary.Name = "lbLibrary";
            this.lbLibrary.Size = new System.Drawing.Size(38, 13);
            this.lbLibrary.TabIndex = 2;
            this.lbLibrary.Text = "Library";
            // 
            // cboWorkflow
            // 
            this.cboWorkflow.FormattingEnabled = true;
            this.cboWorkflow.Location = new System.Drawing.Point(103, 73);
            this.cboWorkflow.Name = "cboWorkflow";
            this.cboWorkflow.Size = new System.Drawing.Size(161, 21);
            this.cboWorkflow.TabIndex = 5;
            this.cboWorkflow.SelectedIndexChanged += new System.EventHandler(this.cboWorkflow_SelectedIndexChanged);
            // 
            // cboWorkflowStep
            // 
            this.cboWorkflowStep.FormattingEnabled = true;
            this.cboWorkflowStep.Location = new System.Drawing.Point(445, 73);
            this.cboWorkflowStep.Name = "cboWorkflowStep";
            this.cboWorkflowStep.Size = new System.Drawing.Size(168, 21);
            this.cboWorkflowStep.TabIndex = 7;
            this.cboWorkflowStep.SelectedIndexChanged += new System.EventHandler(this.cboWorkflowStep_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Watch Folder";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(6, 17);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(76, 29);
            this.btnSubmit.TabIndex = 15;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnUploadFolder
            // 
            this.btnUploadFolder.Location = new System.Drawing.Point(582, 128);
            this.btnUploadFolder.Name = "btnUploadFolder";
            this.btnUploadFolder.Size = new System.Drawing.Size(31, 21);
            this.btnUploadFolder.TabIndex = 16;
            this.btnUploadFolder.Text = "...";
            this.btnUploadFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadFolder.UseVisualStyleBackColor = true;
            this.btnUploadFolder.Click += new System.EventHandler(this.btnUploadFolder_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtFolder.Location = new System.Drawing.Point(103, 128);
            this.txtFolder.Multiline = true;
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(479, 21);
            this.txtFolder.TabIndex = 17;
            // 
            // cboContentType
            // 
            this.cboContentType.FormattingEnabled = true;
            this.cboContentType.Location = new System.Drawing.Point(103, 100);
            this.cboContentType.Name = "cboContentType";
            this.cboContentType.Size = new System.Drawing.Size(161, 21);
            this.cboContentType.TabIndex = 18;
            this.cboContentType.SelectedIndexChanged += new System.EventHandler(this.cboContentType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "WorkFlow";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(6, 54);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(76, 29);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabContent);
            this.groupBox1.Location = new System.Drawing.Point(6, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(713, 265);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // tabContent
            // 
            this.tabContent.Controls.Add(this.tabContentField);
            this.tabContent.Controls.Add(this.tabLibraryField);
            this.tabContent.Controls.Add(this.tabContentParent);
            this.tabContent.Controls.Add(this.tabLibraryParent);
            this.tabContent.Location = new System.Drawing.Point(6, 20);
            this.tabContent.Name = "tabContent";
            this.tabContent.SelectedIndex = 0;
            this.tabContent.Size = new System.Drawing.Size(687, 240);
            this.tabContent.TabIndex = 1;
            // 
            // tabContentField
            // 
            this.tabContentField.BackColor = System.Drawing.SystemColors.Control;
            this.tabContentField.Controls.Add(this.grdContentField);
            this.tabContentField.Location = new System.Drawing.Point(4, 22);
            this.tabContentField.Name = "tabContentField";
            this.tabContentField.Padding = new System.Windows.Forms.Padding(3);
            this.tabContentField.Size = new System.Drawing.Size(679, 214);
            this.tabContentField.TabIndex = 0;
            this.tabContentField.Text = "Content Field";
            // 
            // grdContentField
            // 
            this.grdContentField.BackgroundColor = System.Drawing.Color.White;
            this.grdContentField.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdContentField.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtNo,
            this.txtName,
            this.txtValue,
            this.txtNameId});
            this.grdContentField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdContentField.Location = new System.Drawing.Point(3, 3);
            this.grdContentField.MultiSelect = false;
            this.grdContentField.Name = "grdContentField";
            this.grdContentField.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdContentField.Size = new System.Drawing.Size(673, 208);
            this.grdContentField.TabIndex = 0;
            this.grdContentField.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseClick);
            this.grdContentField.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseDoubleClick);
            // 
            // txtNo
            // 
            this.txtNo.HeaderText = "No";
            this.txtNo.Name = "txtNo";
            this.txtNo.ReadOnly = true;
            // 
            // txtName
            // 
            this.txtName.FillWeight = 300F;
            this.txtName.HeaderText = "Name";
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Width = 200;
            // 
            // txtValue
            // 
            this.txtValue.FillWeight = 400F;
            this.txtValue.HeaderText = "Value";
            this.txtValue.Name = "txtValue";
            this.txtValue.ReadOnly = true;
            this.txtValue.Width = 300;
            // 
            // txtNameId
            // 
            this.txtNameId.HeaderText = "NameId";
            this.txtNameId.Name = "txtNameId";
            this.txtNameId.Visible = false;
            // 
            // tabLibraryField
            // 
            this.tabLibraryField.Controls.Add(this.grdLibraryField);
            this.tabLibraryField.Location = new System.Drawing.Point(4, 22);
            this.tabLibraryField.Name = "tabLibraryField";
            this.tabLibraryField.Padding = new System.Windows.Forms.Padding(3);
            this.tabLibraryField.Size = new System.Drawing.Size(679, 214);
            this.tabLibraryField.TabIndex = 1;
            this.tabLibraryField.Text = "Library Field";
            this.tabLibraryField.UseVisualStyleBackColor = true;
            // 
            // grdLibraryField
            // 
            this.grdLibraryField.BackgroundColor = System.Drawing.Color.White;
            this.grdLibraryField.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLibraryField.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtlcNo,
            this.txtlcName,
            this.txtlcValue,
            this.txtlcNameId});
            this.grdLibraryField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdLibraryField.Location = new System.Drawing.Point(3, 3);
            this.grdLibraryField.MultiSelect = false;
            this.grdLibraryField.Name = "grdLibraryField";
            this.grdLibraryField.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdLibraryField.Size = new System.Drawing.Size(673, 208);
            this.grdLibraryField.TabIndex = 1;
            this.grdLibraryField.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseClick);
            this.grdLibraryField.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseDoubleClick);
            // 
            // txtlcNo
            // 
            this.txtlcNo.HeaderText = "No";
            this.txtlcNo.Name = "txtlcNo";
            this.txtlcNo.ReadOnly = true;
            // 
            // txtlcName
            // 
            this.txtlcName.FillWeight = 300F;
            this.txtlcName.HeaderText = "Name";
            this.txtlcName.Name = "txtlcName";
            this.txtlcName.ReadOnly = true;
            this.txtlcName.Width = 200;
            // 
            // txtlcValue
            // 
            this.txtlcValue.FillWeight = 400F;
            this.txtlcValue.HeaderText = "Value";
            this.txtlcValue.Name = "txtlcValue";
            this.txtlcValue.ReadOnly = true;
            this.txtlcValue.Width = 300;
            // 
            // txtlcNameId
            // 
            this.txtlcNameId.HeaderText = "NameId";
            this.txtlcNameId.Name = "txtlcNameId";
            this.txtlcNameId.Visible = false;
            // 
            // tabContentParent
            // 
            this.tabContentParent.Controls.Add(this.grdContentParent);
            this.tabContentParent.Location = new System.Drawing.Point(4, 22);
            this.tabContentParent.Name = "tabContentParent";
            this.tabContentParent.Size = new System.Drawing.Size(679, 214);
            this.tabContentParent.TabIndex = 2;
            this.tabContentParent.Text = "Content Field Parent";
            this.tabContentParent.UseVisualStyleBackColor = true;
            // 
            // grdContentParent
            // 
            this.grdContentParent.BackgroundColor = System.Drawing.Color.White;
            this.grdContentParent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdContentParent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.grdContentParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdContentParent.Location = new System.Drawing.Point(0, 0);
            this.grdContentParent.MultiSelect = false;
            this.grdContentParent.Name = "grdContentParent";
            this.grdContentParent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdContentParent.Size = new System.Drawing.Size(679, 214);
            this.grdContentParent.TabIndex = 1;
            this.grdContentParent.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseClick);
            this.grdContentParent.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "No";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 300F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 200;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.FillWeight = 400F;
            this.dataGridViewTextBoxColumn7.HeaderText = "Value";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 300;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "NameId";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // tabLibraryParent
            // 
            this.tabLibraryParent.Controls.Add(this.grdLibraryParent);
            this.tabLibraryParent.Location = new System.Drawing.Point(4, 22);
            this.tabLibraryParent.Name = "tabLibraryParent";
            this.tabLibraryParent.Size = new System.Drawing.Size(679, 214);
            this.tabLibraryParent.TabIndex = 3;
            this.tabLibraryParent.Text = "Library Field Parent";
            this.tabLibraryParent.UseVisualStyleBackColor = true;
            // 
            // grdLibraryParent
            // 
            this.grdLibraryParent.BackgroundColor = System.Drawing.Color.White;
            this.grdLibraryParent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLibraryParent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12});
            this.grdLibraryParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdLibraryParent.Location = new System.Drawing.Point(0, 0);
            this.grdLibraryParent.MultiSelect = false;
            this.grdLibraryParent.Name = "grdLibraryParent";
            this.grdLibraryParent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdLibraryParent.Size = new System.Drawing.Size(679, 214);
            this.grdLibraryParent.TabIndex = 1;
            this.grdLibraryParent.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseClick);
            this.grdLibraryParent.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdContentField_CellMouseDoubleClick);
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "No";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.FillWeight = 300F;
            this.dataGridViewTextBoxColumn10.HeaderText = "Name";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 200;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.FillWeight = 400F;
            this.dataGridViewTextBoxColumn11.HeaderText = "Value";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 300;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "NameId";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // grUpdate
            // 
            this.grUpdate.Controls.Add(this.lblParentContent);
            this.grUpdate.Controls.Add(this.cboParentContentType);
            this.grUpdate.Controls.Add(this.label3);
            this.grUpdate.Controls.Add(this.label2);
            this.grUpdate.Controls.Add(this.label1);
            this.grUpdate.Controls.Add(this.txtFolder);
            this.grUpdate.Controls.Add(this.cboBrank);
            this.grUpdate.Controls.Add(this.btnUploadFolder);
            this.grUpdate.Controls.Add(this.cboLibrary);
            this.grUpdate.Controls.Add(this.label4);
            this.grUpdate.Controls.Add(this.label5);
            this.grUpdate.Controls.Add(this.lbLibrary);
            this.grUpdate.Controls.Add(this.cboContentType);
            this.grUpdate.Controls.Add(this.cboWorkflow);
            this.grUpdate.Controls.Add(this.cboWorkflowStep);
            this.grUpdate.Location = new System.Drawing.Point(6, 3);
            this.grUpdate.Name = "grUpdate";
            this.grUpdate.Size = new System.Drawing.Size(619, 172);
            this.grUpdate.TabIndex = 22;
            this.grUpdate.TabStop = false;
            this.grUpdate.Text = "Update attributes of content";
            // 
            // lblParentContent
            // 
            this.lblParentContent.AutoSize = true;
            this.lblParentContent.Location = new System.Drawing.Point(336, 108);
            this.lblParentContent.Name = "lblParentContent";
            this.lblParentContent.Size = new System.Drawing.Size(105, 13);
            this.lblParentContent.TabIndex = 24;
            this.lblParentContent.Text = "Parent Content Type";
            // 
            // cboParentContentType
            // 
            this.cboParentContentType.FormattingEnabled = true;
            this.cboParentContentType.Location = new System.Drawing.Point(445, 100);
            this.cboParentContentType.Name = "cboParentContentType";
            this.cboParentContentType.Size = new System.Drawing.Size(168, 21);
            this.cboParentContentType.TabIndex = 23;
            this.cboParentContentType.SelectedIndexChanged += new System.EventHandler(this.cboParentContentType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(364, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "WorkFlowStep";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Content Type";
            // 
            // ctmWatchFolder
            // 
            this.ctmWatchFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PathToolStripMenuItem,
            this.ConfigToolStripMenuItem});
            this.ctmWatchFolder.Name = "ctmWatchFolder";
            this.ctmWatchFolder.Size = new System.Drawing.Size(120, 48);
            // 
            // PathToolStripMenuItem
            // 
            this.PathToolStripMenuItem.Name = "PathToolStripMenuItem";
            this.PathToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.PathToolStripMenuItem.Text = "Path...";
            this.PathToolStripMenuItem.Click += new System.EventHandler(this.PathToolStripMenuItem_Click);
            // 
            // ConfigToolStripMenuItem
            // 
            this.ConfigToolStripMenuItem.Name = "ConfigToolStripMenuItem";
            this.ConfigToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.ConfigToolStripMenuItem.Text = "Config...";
            this.ConfigToolStripMenuItem.Click += new System.EventHandler(this.ConfigToolStripMenuItem_Click);
            // 
            // ctmDetails
            // 
            this.ctmDetails.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmiDocumentType,
            this.ctmiDate,
            this.timeToolStripMenuItem,
            this.machineToolStripMenuItem,
            this.userNameToolStripMenuItem});
            this.ctmDetails.Name = "ctmDetails";
            this.ctmDetails.Size = new System.Drawing.Size(159, 114);
            // 
            // ctmiDocumentType
            // 
            this.ctmiDocumentType.Name = "ctmiDocumentType";
            this.ctmiDocumentType.Size = new System.Drawing.Size(158, 22);
            this.ctmiDocumentType.Text = "Document Type";
            this.ctmiDocumentType.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // ctmiDate
            // 
            this.ctmiDate.Name = "ctmiDate";
            this.ctmiDate.Size = new System.Drawing.Size(158, 22);
            this.ctmiDate.Text = "Date";
            this.ctmiDate.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // timeToolStripMenuItem
            // 
            this.timeToolStripMenuItem.Name = "timeToolStripMenuItem";
            this.timeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.timeToolStripMenuItem.Text = "Time";
            this.timeToolStripMenuItem.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // machineToolStripMenuItem
            // 
            this.machineToolStripMenuItem.Name = "machineToolStripMenuItem";
            this.machineToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.machineToolStripMenuItem.Text = "Machine";
            this.machineToolStripMenuItem.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // userNameToolStripMenuItem
            // 
            this.userNameToolStripMenuItem.Name = "userNameToolStripMenuItem";
            this.userNameToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.userNameToolStripMenuItem.Text = "UserName";
            this.userNameToolStripMenuItem.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // ctmFieldRight
            // 
            this.ctmFieldRight.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearStripMenuItem});
            this.ctmFieldRight.Name = "ctmFieldRight";
            this.ctmFieldRight.Size = new System.Drawing.Size(133, 26);
            // 
            // clearStripMenuItem
            // 
            this.clearStripMenuItem.Name = "clearStripMenuItem";
            this.clearStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.clearStripMenuItem.Text = "Clear value";
            this.clearStripMenuItem.Click += new System.EventHandler(this.clearStripMenuItem_Click);
            // 
            // grAction
            // 
            this.grAction.Controls.Add(this.btnSubmit);
            this.grAction.Controls.Add(this.btnRefresh);
            this.grAction.Location = new System.Drawing.Point(631, 3);
            this.grAction.Name = "grAction";
            this.grAction.Size = new System.Drawing.Size(88, 172);
            this.grAction.TabIndex = 23;
            this.grAction.TabStop = false;
            this.grAction.Text = "Action";
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(184, 138);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(76, 29);
            this.btnRandom.TabIndex = 21;
            this.btnRandom.Text = "Run";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(737, 479);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.grUpdate);
            this.tabPage1.Controls.Add(this.grAction);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(729, 453);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Importer";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.txtRandomFolder);
            this.tabPage2.Controls.Add(this.btnConfigRandom);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnStop);
            this.tabPage2.Controls.Add(this.btnRandom);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(729, 453);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Batch Importer";
            // 
            // txtRandomFolder
            // 
            this.txtRandomFolder.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtRandomFolder.Location = new System.Drawing.Point(132, 66);
            this.txtRandomFolder.Multiline = true;
            this.txtRandomFolder.Name = "txtRandomFolder";
            this.txtRandomFolder.ReadOnly = true;
            this.txtRandomFolder.Size = new System.Drawing.Size(479, 21);
            this.txtRandomFolder.TabIndex = 25;
            // 
            // btnConfigRandom
            // 
            this.btnConfigRandom.Location = new System.Drawing.Point(611, 66);
            this.btnConfigRandom.Name = "btnConfigRandom";
            this.btnConfigRandom.Size = new System.Drawing.Size(31, 21);
            this.btnConfigRandom.TabIndex = 24;
            this.btnConfigRandom.Text = "...";
            this.btnConfigRandom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfigRandom.UseVisualStyleBackColor = true;
            this.btnConfigRandom.Click += new System.EventHandler(this.btnConfigRandom_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Watch Folder";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(423, 138);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(76, 29);
            this.btnStop.TabIndex = 22;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 498);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Importer";
            this.Load += new System.EventHandler(this.WatchFolder_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabContent.ResumeLayout(false);
            this.tabContentField.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdContentField)).EndInit();
            this.tabLibraryField.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdLibraryField)).EndInit();
            this.tabContentParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdContentParent)).EndInit();
            this.tabLibraryParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdLibraryParent)).EndInit();
            this.grUpdate.ResumeLayout(false);
            this.grUpdate.PerformLayout();
            this.ctmWatchFolder.ResumeLayout(false);
            this.ctmDetails.ResumeLayout(false);
            this.ctmFieldRight.ResumeLayout(false);
            this.grAction.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBrank;
        private System.Windows.Forms.ComboBox cboLibrary;
        private System.Windows.Forms.Label lbLibrary;
        private System.Windows.Forms.ComboBox cboWorkflow;
        private System.Windows.Forms.ComboBox cboWorkflowStep;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnUploadFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.ComboBox cboContentType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabContent;
        private System.Windows.Forms.TabPage tabContentField;
        private System.Windows.Forms.TabPage tabLibraryField;
        private System.Windows.Forms.GroupBox grUpdate;
        private System.Windows.Forms.ContextMenuStrip ctmWatchFolder;
        private System.Windows.Forms.ToolStripMenuItem PathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConfigToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctmDetails;
        private System.Windows.Forms.ToolStripMenuItem ctmiDocumentType;
        private System.Windows.Forms.ToolStripMenuItem ctmiDate;
        private System.Windows.Forms.ToolStripMenuItem timeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem machineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userNameToolStripMenuItem;
        private System.Windows.Forms.DataGridView grdContentField;
        private System.Windows.Forms.ContextMenuStrip ctmFieldRight;
        private System.Windows.Forms.ToolStripMenuItem clearStripMenuItem;
        private System.Windows.Forms.DataGridView grdLibraryField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblParentContent;
        private System.Windows.Forms.ComboBox cboParentContentType;
        private System.Windows.Forms.GroupBox grAction;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.TabPage tabContentParent;
        private System.Windows.Forms.TabPage tabLibraryParent;
        private System.Windows.Forms.DataGridView grdContentParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridView grdLibraryParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtName;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtNameId;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtlcNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtlcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtlcValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtlcNameId;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtRandomFolder;
        private System.Windows.Forms.Button btnConfigRandom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnStop;
    }
}