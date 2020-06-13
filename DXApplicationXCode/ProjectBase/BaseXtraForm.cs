using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using XCode.Membership;

namespace DXApplicationXCode
{
    public partial class BaseXtraForm : DevExpress.XtraEditors.XtraForm
    {
        User<UserX> currentUser = null;
        public BaseXtraForm()
        {
            InitializeComponent();
        }
        public BaseXtraForm(User<UserX> currentUser)
            :this()
        {
            
        }

        private void XtraFormUser_Load(object sender, EventArgs e)
        {
            if (currentUser != null)
            {

            }
        }
    }
}