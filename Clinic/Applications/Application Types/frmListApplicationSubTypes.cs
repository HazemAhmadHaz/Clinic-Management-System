using Clinic_Buisness;
using System;
using System.Data;
using System.Windows.Forms;

namespace Clinic.Applications.ApplicationTypes
{
    public partial class frmListApplicationSubTypes : Form
    {
        private DataTable _dtAllApplicationSubType;

        public frmListApplicationSubTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageApplicationSubType_Load(object sender, EventArgs e)
        {
            _dtAllApplicationSubType = clsApplicationSubType.GetAll();
            dgvApplicationSubTypes.DataSource = _dtAllApplicationSubType;

            lblRecordsCount.Text = dgvApplicationSubTypes.Rows.Count.ToString();

            // Columns formatting
            dgvApplicationSubTypes.Columns[0].HeaderText = "ID";
            dgvApplicationSubTypes.Columns[0].Width = 110;

            dgvApplicationSubTypes.Columns[1].HeaderText = "Name";
            dgvApplicationSubTypes.Columns[1].Width = 300;

            dgvApplicationSubTypes.Columns[2].HeaderText = "Fees";
            dgvApplicationSubTypes.Columns[2].Width = 120;

            dgvApplicationSubTypes.Columns[3].HeaderText = "Application Type";
            dgvApplicationSubTypes.Columns[3].Width = 150;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplicationSubTypes.CurrentRow == null)
                return;

            int id = (int)dgvApplicationSubTypes.CurrentRow.Cells[0].Value;

            frmEditApplicationSubTypes frm = new frmEditApplicationSubTypes(id);
            frm.ShowDialog();

            frmManageApplicationSubType_Load(null, null);
        }
    }
}