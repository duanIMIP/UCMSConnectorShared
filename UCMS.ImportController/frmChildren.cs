using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UCMS.ImportController
{
    public partial class frmChildren : Form
    {
        public UCMS.Model.Library oLibrary = null;
        public UCMS.Model.ContentType oContentType = null;
        public frmChildren()
        {
            InitializeComponent();            
        }
        private void frmChildren_Load(object sender, EventArgs e)
        {
            if (oLibrary != null)
            {
                for (int i = 0; i < oLibrary.Fields.Count; i++)
                {
                    Label lblnew = new System.Windows.Forms.Label();
                    lblnew.Location = new Point(12, 44 + i * 30);
                    lblnew.Text = oLibrary.Fields[i].Name;
                    lblnew.AutoSize = true;
                    lblnew.BackColor = System.Drawing.Color.LightGray;
                    lblnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.Controls.Add(lblnew);
                    if(oLibrary.Fields[i].DataType ==  UCMS.Model.Enum.DataType.Lookup)
                    {
                        ComboBox txtnew = new System.Windows.Forms.ComboBox();
                        txtnew.Location = new Point(108, 40 + i * 30);
                        txtnew.Text = oLibrary.Fields[i].DefaultValue;
                        txtnew.Name = oLibrary.Fields[i].Name;
                        txtnew.Size = new Size(247, 20);
                        txtnew.BackColor = System.Drawing.Color.White;
                        txtnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        this.Controls.Add(txtnew);
                    }
                    else
                    {
                        TextBox txtnew = new System.Windows.Forms.TextBox();
                        txtnew.Location = new Point(108, 40 + i * 30);
                        txtnew.Text = oLibrary.Fields[i].DefaultValue;
                        txtnew.Name = oLibrary.Fields[i].Name;
                        txtnew.Size = new Size(247, 20);
                        txtnew.BackColor = System.Drawing.Color.White;
                        txtnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        this.Controls.Add(txtnew);
                    }                    
                }
                Button btnSubmit = new System.Windows.Forms.Button();
                btnSubmit.Location = new System.Drawing.Point(155, 50 + oLibrary.Fields.Count * 30);
                btnSubmit.Name = "btnSubmit";
                btnSubmit.Size = new System.Drawing.Size(75, 23);
                btnSubmit.TabIndex = 0;
                btnSubmit.Text = "Submit";
                btnSubmit.UseVisualStyleBackColor = true;
                btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
                this.Controls.Add(btnSubmit);
            }
            else if (oContentType != null)
            {
                for (int i = 0; i < oContentType.Fields.Count; i++)
                {
                    Label lblnew = new System.Windows.Forms.Label();
                    lblnew.Location = new Point(12, 44 + i * 30);
                    lblnew.Text = oContentType.Fields[i].Name;
                    lblnew.AutoSize = true;
                    lblnew.BackColor = System.Drawing.Color.LightGray;
                    lblnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.Controls.Add(lblnew);

                    TextBox txtnew = new System.Windows.Forms.TextBox();
                    txtnew.Location = new Point(108, 40 + i * 30);
                    txtnew.Text = oContentType.Fields[i].DefaultValue;
                    txtnew.Name = oContentType.Fields[i].Name;
                    txtnew.Size = new Size(247, 20);
                    txtnew.BackColor = System.Drawing.Color.White;
                    txtnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.Controls.Add(txtnew);
                }
                Button btnSubmit = new System.Windows.Forms.Button();
                btnSubmit.Location = new System.Drawing.Point(155, 50 + oContentType.Fields.Count * 30);
                btnSubmit.Name = "btnSubmit";
                btnSubmit.Size = new System.Drawing.Size(75, 23);
                btnSubmit.TabIndex = 0;
                btnSubmit.Text = "Submit";
                btnSubmit.UseVisualStyleBackColor = true;
                btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
                this.Controls.Add(btnSubmit);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
