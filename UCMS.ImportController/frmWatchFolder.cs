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
        public String _ReName;
        public String _MoveTo;
        public String _Type;

        public frmWatchFolder()
        {
            InitializeComponent();            
        }

        private void frmWatchFolder_Load(object sender, EventArgs e)
        {
            txtExtension.Text = _ReName;
            txtExtension.ContextMenu = new ContextMenu();
            chkMoveTo.Checked = !string.IsNullOrEmpty(_MoveTo);
            txtMoveTo.Text = _MoveTo;
            txtMoveTo.ContextMenu = new ContextMenu();
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
            //_Type = "Image Files(" + _Type + ")|" + _Type;
            _ReName = txtExtension.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void chkMoveTo_Click(object sender, EventArgs e)
        {
            if (chkMoveTo.Checked)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtMoveTo.Text = fbd.SelectedPath;
                    _MoveTo = fbd.SelectedPath;
                }
                else
                {
                    chkMoveTo.Checked = false;
                    txtMoveTo.Text = "";
                    _MoveTo = "";
                }
            }
            else
            {
                txtMoveTo.Text = "";
                _MoveTo = "";
            }
        }

        private void txtExtension_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox txtSender = (TextBox)sender;
            Point ptLowerLeft = new Point(0, txtSender.Height);
            ptLowerLeft = txtSender.PointToScreen(ptLowerLeft);
            ctmRRenameExtension.Show(ptLowerLeft);
        }

        private void txtExtension_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                TextBox txtSender = (TextBox)sender;
                Point ptLowerLeft = new Point(0, txtSender.Height);
                ptLowerLeft = txtSender.PointToScreen(ptLowerLeft);
                ctmRRenameExtension.Show(ptLowerLeft);
            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem stripObj = sender as ToolStripMenuItem;
            if(stripObj.Text == "Other...")
            {
                txtExtension.ReadOnly = false;
                txtExtension.BackColor = System.Drawing.Color.White;
                txtExtension.ContextMenu = null;
            }
            else
            {
                txtExtension.Text = stripObj.Text;
                txtExtension.ReadOnly = true;
                txtExtension.BackColor = System.Drawing.SystemColors.Menu;
                txtExtension.ContextMenu = new ContextMenu();
            }
        }

        private void btnRenameExtension_Click(object sender, EventArgs e)
        {
            Button txtSender = (Button)sender;
            Point ptLowerLeft = new Point(0, txtSender.Height);
            ptLowerLeft = txtSender.PointToScreen(ptLowerLeft);
            ctmRRenameExtension.Show(ptLowerLeft);
        }
    }
}
