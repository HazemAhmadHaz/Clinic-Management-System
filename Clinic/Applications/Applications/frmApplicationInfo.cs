using Clinic.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Applications
{
    public partial class frmApplicationInfo : Form
    {
        private int _ApplicationID = -1;

        public frmApplicationInfo(int ApplicationID)
        {
            InitializeComponent();
            _ApplicationID = ApplicationID;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmApplicationInfo_Load(object sender, EventArgs e)
        {
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_ApplicationID);
        }
    }
}
