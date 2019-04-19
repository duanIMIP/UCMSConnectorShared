namespace IMIP.UniversalScan.Profile
{
    partial class DocumentSeparation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentSeparation));
            this.grbSepDetail = new System.Windows.Forms.GroupBox();
            this.tabSeparation = new System.Windows.Forms.TabControl();
            this.tabPageFixPage = new System.Windows.Forms.TabPage();
            this.panelFixPage = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.nudPageCount = new System.Windows.Forms.NumericUpDown();
            this.cboFPFormType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPageBlankPage = new System.Windows.Forms.TabPage();
            this.panelBlankPage = new System.Windows.Forms.Panel();
            this.cboBPFormType = new System.Windows.Forms.ComboBox();
            this.chkBPDeleteBlankPage = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nudThreshold = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPageBarcode = new System.Windows.Forms.TabPage();
            this.panelBarcode = new System.Windows.Forms.Panel();
            this.dgvBarcode = new System.Windows.Forms.DataGridView();
            this.colFormType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colBarcode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colComparision = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeleteSeparator = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCombineWithPageCount = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPageCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageCustom = new System.Windows.Forms.TabPage();
            this.panelCustom = new System.Windows.Forms.Panel();
            this.textBoxCustomFile = new System.Windows.Forms.TextBox();
            this.cmdCustomFile = new System.Windows.Forms.Button();
            this.grbSeparationType = new System.Windows.Forms.GroupBox();
            this.radCustom = new System.Windows.Forms.RadioButton();
            this.radBarcode = new System.Windows.Forms.RadioButton();
            this.radBlankPage = new System.Windows.Forms.RadioButton();
            this.radFixPage = new System.Windows.Forms.RadioButton();
            this.chkEnabledSeparation = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.grbSepDetail.SuspendLayout();
            this.tabSeparation.SuspendLayout();
            this.tabPageFixPage.SuspendLayout();
            this.panelFixPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageCount)).BeginInit();
            this.tabPageBlankPage.SuspendLayout();
            this.panelBlankPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).BeginInit();
            this.tabPageBarcode.SuspendLayout();
            this.panelBarcode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarcode)).BeginInit();
            this.tabPageCustom.SuspendLayout();
            this.panelCustom.SuspendLayout();
            this.grbSeparationType.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbSepDetail
            // 
            resources.ApplyResources(this.grbSepDetail, "grbSepDetail");
            this.grbSepDetail.Controls.Add(this.tabSeparation);
            this.grbSepDetail.Name = "grbSepDetail";
            this.grbSepDetail.TabStop = false;
            // 
            // tabSeparation
            // 
            resources.ApplyResources(this.tabSeparation, "tabSeparation");
            this.tabSeparation.Controls.Add(this.tabPageFixPage);
            this.tabSeparation.Controls.Add(this.tabPageBlankPage);
            this.tabSeparation.Controls.Add(this.tabPageBarcode);
            this.tabSeparation.Controls.Add(this.tabPageCustom);
            this.tabSeparation.Name = "tabSeparation";
            this.tabSeparation.SelectedIndex = 0;
            this.tabSeparation.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabSeparation_Selecting);
            // 
            // tabPageFixPage
            // 
            this.tabPageFixPage.Controls.Add(this.panelFixPage);
            resources.ApplyResources(this.tabPageFixPage, "tabPageFixPage");
            this.tabPageFixPage.Name = "tabPageFixPage";
            this.tabPageFixPage.UseVisualStyleBackColor = true;
            // 
            // panelFixPage
            // 
            this.panelFixPage.Controls.Add(this.label1);
            this.panelFixPage.Controls.Add(this.nudPageCount);
            this.panelFixPage.Controls.Add(this.cboFPFormType);
            this.panelFixPage.Controls.Add(this.label11);
            resources.ApplyResources(this.panelFixPage, "panelFixPage");
            this.panelFixPage.Name = "panelFixPage";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // nudPageCount
            // 
            resources.ApplyResources(this.nudPageCount, "nudPageCount");
            this.nudPageCount.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudPageCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPageCount.Name = "nudPageCount";
            this.nudPageCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPageCount.ValueChanged += new System.EventHandler(this.nudPageCount_ValueChanged);
            // 
            // cboFPFormType
            // 
            this.cboFPFormType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFPFormType.FormattingEnabled = true;
            resources.ApplyResources(this.cboFPFormType, "cboFPFormType");
            this.cboFPFormType.Name = "cboFPFormType";
            this.cboFPFormType.SelectedIndexChanged += new System.EventHandler(this.cboFPFormType_SelectedIndexChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // tabPageBlankPage
            // 
            this.tabPageBlankPage.Controls.Add(this.panelBlankPage);
            resources.ApplyResources(this.tabPageBlankPage, "tabPageBlankPage");
            this.tabPageBlankPage.Name = "tabPageBlankPage";
            this.tabPageBlankPage.UseVisualStyleBackColor = true;
            // 
            // panelBlankPage
            // 
            this.panelBlankPage.Controls.Add(this.cboBPFormType);
            this.panelBlankPage.Controls.Add(this.chkBPDeleteBlankPage);
            this.panelBlankPage.Controls.Add(this.label13);
            this.panelBlankPage.Controls.Add(this.nudThreshold);
            this.panelBlankPage.Controls.Add(this.label12);
            resources.ApplyResources(this.panelBlankPage, "panelBlankPage");
            this.panelBlankPage.Name = "panelBlankPage";
            // 
            // cboBPFormType
            // 
            this.cboBPFormType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBPFormType.FormattingEnabled = true;
            resources.ApplyResources(this.cboBPFormType, "cboBPFormType");
            this.cboBPFormType.Name = "cboBPFormType";
            this.cboBPFormType.SelectedIndexChanged += new System.EventHandler(this.cboBPFormType_SelectedIndexChanged);
            // 
            // chkBPDeleteBlankPage
            // 
            resources.ApplyResources(this.chkBPDeleteBlankPage, "chkBPDeleteBlankPage");
            this.chkBPDeleteBlankPage.Name = "chkBPDeleteBlankPage";
            this.chkBPDeleteBlankPage.UseVisualStyleBackColor = true;
            this.chkBPDeleteBlankPage.CheckedChanged += new System.EventHandler(this.chkBPDeleteBlankPage_CheckedChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // nudThreshold
            // 
            resources.ApplyResources(this.nudThreshold, "nudThreshold");
            this.nudThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThreshold.Name = "nudThreshold";
            this.nudThreshold.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudThreshold.ValueChanged += new System.EventHandler(this.nudThreshold_ValueChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tabPageBarcode
            // 
            this.tabPageBarcode.Controls.Add(this.panelBarcode);
            resources.ApplyResources(this.tabPageBarcode, "tabPageBarcode");
            this.tabPageBarcode.Name = "tabPageBarcode";
            this.tabPageBarcode.UseVisualStyleBackColor = true;
            // 
            // panelBarcode
            // 
            this.panelBarcode.Controls.Add(this.dgvBarcode);
            resources.ApplyResources(this.panelBarcode, "panelBarcode");
            this.panelBarcode.Name = "panelBarcode";
            // 
            // dgvBarcode
            // 
            this.dgvBarcode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBarcode.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFormType,
            this.colBarcode,
            this.colComparision,
            this.colValue,
            this.colDeleteSeparator,
            this.colCombineWithPageCount,
            this.colPageCount});
            resources.ApplyResources(this.dgvBarcode, "dgvBarcode");
            this.dgvBarcode.Name = "dgvBarcode";
            this.dgvBarcode.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBarcode_CellValueChanged);
            this.dgvBarcode.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvBarcode_DataError);
            this.dgvBarcode.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBarcode_RowEnter);
            this.dgvBarcode.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvBarcode_RowValidating);
            this.dgvBarcode.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvBarcode_UserAddedRow);
            this.dgvBarcode.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvBarcode_UserDeletingRow);
            // 
            // colFormType
            // 
            this.colFormType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            resources.ApplyResources(this.colFormType, "colFormType");
            this.colFormType.Name = "colFormType";
            // 
            // colBarcode
            // 
            this.colBarcode.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            resources.ApplyResources(this.colBarcode, "colBarcode");
            this.colBarcode.Name = "colBarcode";
            // 
            // colComparision
            // 
            this.colComparision.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            resources.ApplyResources(this.colComparision, "colComparision");
            this.colComparision.Name = "colComparision";
            // 
            // colValue
            // 
            resources.ApplyResources(this.colValue, "colValue");
            this.colValue.Name = "colValue";
            // 
            // colDeleteSeparator
            // 
            resources.ApplyResources(this.colDeleteSeparator, "colDeleteSeparator");
            this.colDeleteSeparator.Name = "colDeleteSeparator";
            // 
            // colCombineWithPageCount
            // 
            resources.ApplyResources(this.colCombineWithPageCount, "colCombineWithPageCount");
            this.colCombineWithPageCount.Name = "colCombineWithPageCount";
            // 
            // colPageCount
            // 
            resources.ApplyResources(this.colPageCount, "colPageCount");
            this.colPageCount.Name = "colPageCount";
            this.colPageCount.ReadOnly = true;
            // 
            // tabPageCustom
            // 
            this.tabPageCustom.Controls.Add(this.panelCustom);
            resources.ApplyResources(this.tabPageCustom, "tabPageCustom");
            this.tabPageCustom.Name = "tabPageCustom";
            this.tabPageCustom.UseVisualStyleBackColor = true;
            // 
            // panelCustom
            // 
            this.panelCustom.Controls.Add(this.textBoxCustomFile);
            this.panelCustom.Controls.Add(this.cmdCustomFile);
            resources.ApplyResources(this.panelCustom, "panelCustom");
            this.panelCustom.Name = "panelCustom";
            // 
            // textBoxCustomFile
            // 
            resources.ApplyResources(this.textBoxCustomFile, "textBoxCustomFile");
            this.textBoxCustomFile.Name = "textBoxCustomFile";
            // 
            // cmdCustomFile
            // 
            resources.ApplyResources(this.cmdCustomFile, "cmdCustomFile");
            this.cmdCustomFile.Name = "cmdCustomFile";
            this.cmdCustomFile.UseVisualStyleBackColor = true;
            this.cmdCustomFile.Click += new System.EventHandler(this.cmdCustomFile_Click);
            // 
            // grbSeparationType
            // 
            resources.ApplyResources(this.grbSeparationType, "grbSeparationType");
            this.grbSeparationType.Controls.Add(this.radCustom);
            this.grbSeparationType.Controls.Add(this.radBarcode);
            this.grbSeparationType.Controls.Add(this.radBlankPage);
            this.grbSeparationType.Controls.Add(this.radFixPage);
            this.grbSeparationType.Name = "grbSeparationType";
            this.grbSeparationType.TabStop = false;
            // 
            // radCustom
            // 
            resources.ApplyResources(this.radCustom, "radCustom");
            this.radCustom.Name = "radCustom";
            this.radCustom.TabStop = true;
            this.radCustom.UseVisualStyleBackColor = true;
            this.radCustom.CheckedChanged += new System.EventHandler(this.radCustom_CheckedChanged);
            // 
            // radBarcode
            // 
            resources.ApplyResources(this.radBarcode, "radBarcode");
            this.radBarcode.Name = "radBarcode";
            this.radBarcode.TabStop = true;
            this.radBarcode.UseVisualStyleBackColor = true;
            this.radBarcode.CheckedChanged += new System.EventHandler(this.radBarcode_CheckedChanged);
            // 
            // radBlankPage
            // 
            resources.ApplyResources(this.radBlankPage, "radBlankPage");
            this.radBlankPage.Name = "radBlankPage";
            this.radBlankPage.TabStop = true;
            this.radBlankPage.UseVisualStyleBackColor = true;
            this.radBlankPage.CheckedChanged += new System.EventHandler(this.radBlankPage_CheckedChanged);
            // 
            // radFixPage
            // 
            resources.ApplyResources(this.radFixPage, "radFixPage");
            this.radFixPage.Checked = true;
            this.radFixPage.Name = "radFixPage";
            this.radFixPage.TabStop = true;
            this.radFixPage.UseVisualStyleBackColor = true;
            this.radFixPage.CheckedChanged += new System.EventHandler(this.radFixPage_CheckedChanged);
            // 
            // chkEnabledSeparation
            // 
            resources.ApplyResources(this.chkEnabledSeparation, "chkEnabledSeparation");
            this.chkEnabledSeparation.Name = "chkEnabledSeparation";
            this.chkEnabledSeparation.UseVisualStyleBackColor = true;
            this.chkEnabledSeparation.CheckedChanged += new System.EventHandler(this.chkEnabledSeparation_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DocumentSeparation
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grbSepDetail);
            this.Controls.Add(this.grbSeparationType);
            this.Controls.Add(this.chkEnabledSeparation);
            this.Name = "DocumentSeparation";
            this.grbSepDetail.ResumeLayout(false);
            this.tabSeparation.ResumeLayout(false);
            this.tabPageFixPage.ResumeLayout(false);
            this.panelFixPage.ResumeLayout(false);
            this.panelFixPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageCount)).EndInit();
            this.tabPageBlankPage.ResumeLayout(false);
            this.panelBlankPage.ResumeLayout(false);
            this.panelBlankPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).EndInit();
            this.tabPageBarcode.ResumeLayout(false);
            this.panelBarcode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBarcode)).EndInit();
            this.tabPageCustom.ResumeLayout(false);
            this.panelCustom.ResumeLayout(false);
            this.panelCustom.PerformLayout();
            this.grbSeparationType.ResumeLayout(false);
            this.grbSeparationType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbSepDetail;
        private System.Windows.Forms.TabControl tabSeparation;
        private System.Windows.Forms.TabPage tabPageFixPage;
        private System.Windows.Forms.Panel panelFixPage;
        private System.Windows.Forms.NumericUpDown nudPageCount;
        private System.Windows.Forms.ComboBox cboFPFormType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabPageBlankPage;
        private System.Windows.Forms.Panel panelBlankPage;
        private System.Windows.Forms.ComboBox cboBPFormType;
        private System.Windows.Forms.CheckBox chkBPDeleteBlankPage;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudThreshold;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPageBarcode;
        private System.Windows.Forms.Panel panelBarcode;
        private System.Windows.Forms.DataGridView dgvBarcode;
        private System.Windows.Forms.GroupBox grbSeparationType;
        private System.Windows.Forms.RadioButton radBarcode;
        private System.Windows.Forms.RadioButton radBlankPage;
        private System.Windows.Forms.RadioButton radFixPage;
        private System.Windows.Forms.CheckBox chkEnabledSeparation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFormType;
        private System.Windows.Forms.DataGridViewComboBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewComboBoxColumn colComparision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDeleteSeparator;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCombineWithPageCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPageCount;
        private System.Windows.Forms.RadioButton radCustom;
        private System.Windows.Forms.TabPage tabPageCustom;
        private System.Windows.Forms.Button cmdCustomFile;
        private System.Windows.Forms.TextBox textBoxCustomFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panelCustom;
    }
}
