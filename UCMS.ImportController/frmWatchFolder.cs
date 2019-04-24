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
    public partial class frmWatchFolder : Form
    {
        public Boolean _ReName;
        public String _MoveTo;
        public String _Type;

        public frmWatchFolder()
        {
            InitializeComponent();            
        }

        private void frmWatchFolder_Load(object sender, EventArgs e)
        {
            chkRename.Checked = _ReName;
            if (!string.IsNullOrEmpty(_MoveTo))
            {
                chkMoveTo.Checked = true;
                lblMoveTo.Text = _MoveTo;
            }
            if (!string.IsNullOrEmpty(_Type))
            {
                foreach (var item in groupBox1.Controls)//groupBox1
                {
                    if (item.GetType().Name.Equals("CheckBox"))
                    {
                        var checktemp = item as CheckBox;
                        if (_Type.Contains(checktemp.Text + ";"))
                        {
                            checktemp.Checked = true;
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _Type = "";
            foreach (var item in groupBox1.Controls)//groupBox1
            {
                if (item.GetType().Name.Equals("CheckBox"))
                {
                    var checktemp = item as CheckBox;
                    if (checktemp.Checked)
                    {
                        _Type += checktemp.Text + ";";
                    }
                }
            }
            _Type = "Image Files(" + _Type + ")|" + _Type;
            _ReName = chkRename.Checked;
            this.DialogResult = DialogResult.OK;
        }

        private void chkMoveTo_Click(object sender, EventArgs e)
        {
            if (chkMoveTo.Checked)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    lblMoveTo.Text = fbd.SelectedPath;
                    _MoveTo = fbd.SelectedPath;
                }
                else
                {
                    chkMoveTo.Checked = false;
                    lblMoveTo.Text = "";
                    _MoveTo = "";
                }
            }
            else
            {
                lblMoveTo.Text = "";
                _MoveTo = "";
            }
        }
    }
}
