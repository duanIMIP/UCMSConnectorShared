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
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabContentField = new System.Windows.Forms.TabPage();
            this.tabLibraryField = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ctmWatchFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmDetails = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctmiDocumentType = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmiDate = new System.Windows.Forms.ToolStripMenuItem();
            this.timeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ctmWatchFolder.SuspendLayout();
            this.ctmDetails.SuspendLayout();
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
            this.cboWorkflowStep.Location = new System.Drawing.Point(275, 73);
            this.cboWorkflowStep.Name = "cboWorkflowStep";
            this.cboWorkflowStep.Size = new System.Drawing.Size(168, 21);
            this.cboWorkflowStep.TabIndex = 7;
            this.cboWorkflowStep.SelectedIndexChanged += new System.EventHandler(this.cboWorkflowStep_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Watch Folder";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(124, 139);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(97, 39);
            this.btnSubmit.TabIndex = 15;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnUploadFolder
            // 
            this.btnUploadFolder.Location = new System.Drawing.Point(613, 100);
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
            this.txtFolder.Location = new System.Drawing.Point(103, 100);
            this.txtFolder.Multiline = true;
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(510, 21);
            this.txtFolder.TabIndex = 17;
            // 
            // cboContentType
            // 
            this.cboContentType.FormattingEnabled = true;
            this.cboContentType.Location = new System.Drawing.Point(452, 73);
            this.cboContentType.Name = "cboContentType";
            this.cboContentType.Size = new System.Drawing.Size(161, 21);
            this.cboContentType.TabIndex = 18;
            this.cboContentType.SelectedIndexChanged += new System.EventHandler(this.cboContentType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Process";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(421, 139);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(97, 39);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl2);
            this.groupBox1.Location = new System.Drawing.Point(9, 248);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(699, 265);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabContentField);
            this.tabControl2.Controls.Add(this.tabLibraryField);
            this.tabControl2.Location = new System.Drawing.Point(6, 20);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(687, 240);
            this.tabControl2.TabIndex = 1;
            // 
            // tabContentField
            // 
            this.tabContentField.BackColor = System.Drawing.SystemColors.Control;
            this.tabContentField.Location = new System.Drawing.Point(4, 22);
            this.tabContentField.Name = "tabContentField";
            this.tabContentField.Padding = new System.Windows.Forms.Padding(3);
            this.tabContentField.Size = new System.Drawing.Size(679, 214);
            this.tabContentField.TabIndex = 0;
            this.tabContentField.Text = "Content Field";
            // 
            // tabLibraryField
            // 
            this.tabLibraryField.Location = new System.Drawing.Point(4, 22);
            this.tabLibraryField.Name = "tabLibraryField";
            this.tabLibraryField.Padding = new System.Windows.Forms.Padding(3);
            this.tabLibraryField.Size = new System.Drawing.Size(679, 214);
            this.tabLibraryField.TabIndex = 1;
            this.tabLibraryField.Text = "Library Field";
            this.tabLibraryField.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnRefresh);
            this.groupBox2.Controls.Add(this.txtFolder);
            this.groupBox2.Controls.Add(this.btnSubmit);
            this.groupBox2.Controls.Add(this.cboBrank);
            this.groupBox2.Controls.Add(this.btnUploadFolder);
            this.groupBox2.Controls.Add(this.cboLibrary);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lbLibrary);
            this.groupBox2.Controls.Add(this.cboContentType);
            this.groupBox2.Controls.Add(this.cboWorkflow);
            this.groupBox2.Controls.Add(this.cboWorkflowStep);
            this.groupBox2.Location = new System.Drawing.Point(9, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(699, 184);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Update attributes of content";
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
            this.ctmDetails.Size = new System.Drawing.Size(181, 136);
            // 
            // ctmiDocumentType
            // 
            this.ctmiDocumentType.Name = "ctmiDocumentType";
            this.ctmiDocumentType.Size = new System.Drawing.Size(180, 22);
            this.ctmiDocumentType.Text = "Document Type";
            this.ctmiDocumentType.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // ctmiDate
            // 
            this.ctmiDate.Name = "ctmiDate";
            this.ctmiDate.Size = new System.Drawing.Size(180, 22);
            this.ctmiDate.Text = "Date";
            this.ctmiDate.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // timeToolStripMenuItem
            // 
            this.timeToolStripMenuItem.Name = "timeToolStripMenuItem";
            this.timeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.timeToolStripMenuItem.Text = "Time";
            this.timeToolStripMenuItem.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // machineToolStripMenuItem
            // 
            this.machineToolStripMenuItem.Name = "machineToolStripMenuItem";
            this.machineToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.machineToolStripMenuItem.Text = "Machine";
            this.machineToolStripMenuItem.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // userNameToolStripMenuItem
            // 
            this.userNameToolStripMenuItem.Name = "userNameToolStripMenuItem";
            this.userNameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.userNameToolStripMenuItem.Text = "UserName";
            this.userNameToolStripMenuItem.Click += new System.EventHandler(this.ConfigContentField_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 517);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WatchFolder";
            this.Load += new System.EventHandler(this.WatchFolder_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ctmWatchFolder.ResumeLayout(false);
            this.ctmDetails.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabContentField;
        private System.Windows.Forms.TabPage tabLibraryField;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ContextMenuStrip ctmWatchFolder;
        private System.Windows.Forms.ToolStripMenuItem PathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConfigToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctmDetails;
        private System.Windows.Forms.ToolStripMenuItem ctmiDocumentType;
        private System.Windows.Forms.ToolStripMenuItem ctmiDate;
        private System.Windows.Forms.ToolStripMenuItem timeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem machineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userNameToolStripMenuItem;
    }
}