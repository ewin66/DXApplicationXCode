using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNet.WinForm
{
    public partial class FormLogon : Form
    {
        public FormLogon()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if(this.textBoxPassword.Text.Equals("18682122099"))
            {
                this.DialogResult = DialogResult.OK;
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        private void FormLogon_Activated(object sender, EventArgs e)
        {
            this.BringToFront();
            this.TopMost = true;
        }

        private void FormLogon_Load(object sender, EventArgs e)
        {
            //label1.Text = CPUID + Environment.NewLine +
            //            DiskSerialNumber + Environment.NewLine +
            //            MacAddress + Environment.NewLine +
            //            BoardID;
        }

        private void FormLogon_Shown(object sender, EventArgs e)
        {
            this.textBoxPassword.Focus();
        }
    }
}
