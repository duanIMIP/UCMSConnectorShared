namespace UCMS.ImportController
{
    partial class frmWatchFolder
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
            this.chkJPG = new System.Windows.Forms.CheckBox();
            this.chkJPEG = new System.Windows.Forms.CheckBox();
            this.chkGIJ = new System.Windows.Forms.CheckBox();
            this.chkPNG = new System.Windows.Forms.CheckBox();
            this.chkJFIF = new System.Windows.Forms.CheckBox();
            this.chkPDF = new System.Windows.Forms.CheckBox();
            this.chkXLSX = new System.Windows.Forms.CheckBox();
            this.chkDOCX = new System.Windows.Forms.CheckBox();
            this.chkXLS = new System.Windows.Forms.CheckBox();
            this.chkDOC = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRename = new System.Windows.Forms.CheckBox();
            this.chkMoveTo = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMoveTo = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkJPG
            // 
            this.chkJPG.AutoSize = true;
            this.chkJPG.Location = new System.Drawing.Point(41, 28);
            this.chkJPG.Name = "chkJPG";
            this.chkJPG.Size = new System.Drawing.Size(47, 17);
            this.chkJPG.TabIndex = 0;
            this.chkJPG.Text = "*.jpg";
            this.chkJPG.UseVisualStyleBackColor = true;
            // 
            // chkJPEG
            // 
            this.chkJPEG.AutoSize = true;
            this.chkJPEG.Location = new System.Drawing.Point(41, 51);
            this.chkJPEG.Name = "chkJPEG";
            this.chkJPEG.Size = new System.Drawing.Size(53, 17);
            this.chkJPEG.TabIndex = 1;
            this.chkJPEG.Text = "*.jpeg";
            this.chkJPEG.UseVisualStyleBackColor = true;
            // 
            // chkGIJ
            // 
            this.chkGIJ.AutoSize = true;
            this.chkGIJ.Location = new System.Drawing.Point(41, 74);
            this.chkGIJ.Name = "chkGIJ";
            this.chkGIJ.Size = new System.Drawing.Size(44, 17);
            this.chkGIJ.TabIndex = 2;
            this.chkGIJ.Text = "*.gif";
            this.chkGIJ.UseVisualStyleBackColor = true;
            // 
            // chkPNG
            // 
            this.chkPNG.AutoSize = true;
            this.chkPNG.Location = new System.Drawing.Point(41, 97);
            this.chkPNG.Name = "chkPNG";
            this.chkPNG.Size = new System.Drawing.Size(51, 17);
            this.chkPNG.TabIndex = 3;
            this.chkPNG.Text = "*.png";
            this.chkPNG.UseVisualStyleBackColor = true;
            // 
            // chkJFIF
            // 
            this.chkJFIF.AutoSize = true;
            this.chkJFIF.Location = new System.Drawing.Point(41, 120);
            this.chkJFIF.Name = "chkJFIF";
            this.chkJFIF.Size = new System.Drawing.Size(43, 17);
            this.chkJFIF.TabIndex = 4;
            this.chkJFIF.Text = "*.jfif";
            this.chkJFIF.UseVisualStyleBackColor = true;
            // 
            // chkPDF
            // 
            this.chkPDF.AutoSize = true;
            this.chkPDF.Location = new System.Drawing.Point(118, 120);
            this.chkPDF.Name = "chkPDF";
            this.chkPDF.Size = new System.Drawing.Size(48, 17);
            this.chkPDF.TabIndex = 10;
            this.chkPDF.Text = "*.pdf";
            this.chkPDF.UseVisualStyleBackColor = true;
            // 
            // chkXLSX
            // 
            this.chkXLSX.AutoSize = true;
            this.chkXLSX.Location = new System.Drawing.Point(118, 97);
            this.chkXLSX.Name = "chkXLSX";
            this.chkXLSX.Size = new System.Drawing.Size(50, 17);
            this.chkXLSX.TabIndex = 9;
            this.chkXLSX.Text = "*.xlsx";
            this.chkXLSX.UseVisualStyleBackColor = true;
            // 
            // chkDOCX
            // 
            this.chkDOCX.AutoSize = true;
            this.chkDOCX.Location = new System.Drawing.Point(118, 74);
            this.chkDOCX.Name = "chkDOCX";
            this.chkDOCX.Size = new System.Drawing.Size(56, 17);
            this.chkDOCX.TabIndex = 8;
            this.chkDOCX.Text = "*.docx";
            this.chkDOCX.UseVisualStyleBackColor = true;
            // 
            // chkXLS
            // 
            this.chkXLS.AutoSize = true;
            this.chkXLS.Location = new System.Drawing.Point(118, 51);
            this.chkXLS.Name = "chkXLS";
            this.chkXLS.Size = new System.Drawing.Size(45, 17);
            this.chkXLS.TabIndex = 7;
            this.chkXLS.Text = "*.xls";
            this.chkXLS.UseVisualStyleBackColor = true;
            // 
            // chkDOC
            // 
            this.chkDOC.AutoSize = true;
            this.chkDOC.Location = new System.Drawing.Point(118, 28);
            this.chkDOC.Name = "chkDOC";
            this.chkDOC.Size = new System.Drawing.Size(51, 17);
            this.chkDOC.TabIndex = 6;
            this.chkDOC.Text = "*.doc";
            this.chkDOC.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(207, 116);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(186, 20);
            this.textBox1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 12;
            // 
            // chkRename
            // 
            this.chkRename.AutoSize = true;
            this.chkRename.Checked = true;
            this.chkRename.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRename.Location = new System.Drawing.Point(40, 23);
            this.chkRename.Name = "chkRename";
            this.chkRename.Size = new System.Drawing.Size(66, 17);
            this.chkRename.TabIndex = 16;
            this.chkRename.Text = "Rename";
            this.chkRename.UseVisualStyleBackColor = true;
            // 
            // chkMoveTo
            // 
            this.chkMoveTo.AutoSize = true;
            this.chkMoveTo.Location = new System.Drawing.Point(40, 47);
            this.chkMoveTo.Name = "chkMoveTo";
            this.chkMoveTo.Size = new System.Drawing.Size(69, 17);
            this.chkMoveTo.TabIndex = 17;
            this.chkMoveTo.Text = "Move To";
            this.chkMoveTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMoveTo.UseVisualStyleBackColor = true;
            this.chkMoveTo.Click += new System.EventHandler(this.chkMoveTo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkDOCX);
            this.groupBox1.Controls.Add(this.chkJPG);
            this.groupBox1.Controls.Add(this.chkJPEG);
            this.groupBox1.Controls.Add(this.chkGIJ);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkPNG);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.chkJFIF);
            this.groupBox1.Controls.Add(this.chkPDF);
            this.groupBox1.Controls.Add(this.chkXLSX);
            this.groupBox1.Controls.Add(this.chkDOC);
            this.groupBox1.Controls.Add(this.chkXLS);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 162);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Attach Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblMoveTo);
            this.groupBox2.Controls.Add(this.chkRename);
            this.groupBox2.Controls.Add(this.chkMoveTo);
            this.groupBox2.Location = new System.Drawing.Point(13, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 81);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Format";
            // 
            // lblMoveTo
            // 
            this.lblMoveTo.AutoSize = true;
            this.lblMoveTo.Location = new System.Drawing.Point(153, 51);
            this.lblMoveTo.Name = "lblMoveTo";
            this.lblMoveTo.Size = new System.Drawing.Size(0, 13);
            this.lblMoveTo.TabIndex = 18;
            this.lblMoveTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(381, 267);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmWatchFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 299);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmWatchFolder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Config Attachment";
            this.Load += new System.EventHandler(this.frmWatchFolder_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkJPG;
        private System.Windows.Forms.CheckBox chkJPEG;
        private System.Windows.Forms.CheckBox chkGIJ;
        private System.Windows.Forms.CheckBox chkPNG;
        private System.Windows.Forms.CheckBox chkJFIF;
        private System.Windows.Forms.CheckBox chkPDF;
        private System.Windows.Forms.CheckBox chkXLSX;
        private System.Windows.Forms.CheckBox chkDOCX;
        private System.Windows.Forms.CheckBox chkXLS;
        private System.Windows.Forms.CheckBox chkDOC;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkRename;
        private System.Windows.Forms.CheckBox chkMoveTo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblMoveTo;
    }
}