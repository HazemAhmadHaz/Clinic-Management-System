using Clinic_Buisness;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Clinic.Applications
{
    public partial class frmListApplications : Form
    {
        private DataTable _dtAllApplications;

        public frmListApplications()
        {
            InitializeComponent();
        }

        private void frmListApplications_Load(object sender, EventArgs e)
        {
            _dtAllApplications = clsApplication.GetAllApplications();
            dgvApplications.DataSource = _dtAllApplications;

            lblRecordsCount.Text = dgvApplications.Rows.Count.ToString();

            if (dgvApplications.Rows.Count > 0)
            {
                dgvApplications.Columns["ApplicationID"].HeaderText = "Application ID";
                dgvApplications.Columns["ApplicationID"].Width = 100;

                dgvApplications.Columns["PersonID"].HeaderText = "Person ID";
                dgvApplications.Columns["PersonID"].Width = 90;

                dgvApplications.Columns["PersonName"].HeaderText = "Patient";
                dgvApplications.Columns["PersonName"].Width = 180;

                dgvApplications.Columns["ApplicationType"].HeaderText = "Application Type";
                dgvApplications.Columns["ApplicationType"].Width = 150;

                dgvApplications.Columns["ApplicationSubType"].HeaderText = "Sub Type";
                dgvApplications.Columns["ApplicationSubType"].Width = 170;

                dgvApplications.Columns["DoctorName"].HeaderText = "Doctor";
                dgvApplications.Columns["DoctorName"].Width = 180;

                dgvApplications.Columns["ApplicationDate"].HeaderText = "Application Date";
                dgvApplications.Columns["ApplicationDate"].Width = 130;

                dgvApplications.Columns["AppointmentDate"].HeaderText = "Appointment";
                dgvApplications.Columns["AppointmentDate"].Width = 130;

                dgvApplications.Columns["ResourceName"].HeaderText = "Resource";
                dgvApplications.Columns["ResourceName"].Width = 130;

                dgvApplications.Columns["ApplicationStatus"].HeaderText = "Status";
                dgvApplications.Columns["ApplicationStatus"].Width = 70;

                dgvApplications.Columns["Fees"].HeaderText = "Fees";
                dgvApplications.Columns["Fees"].Width = 80;

                dgvApplications.Columns["IsLocked"].HeaderText = "Locked";
                dgvApplications.Columns["IsLocked"].Width = 70;

                dgvApplications.Columns["PaidFees"].HeaderText = "Paid";
                dgvApplications.Columns["PaidFees"].Width = 80;

                dgvApplications.Columns["Outcome"].HeaderText = "Outcome";
                dgvApplications.Columns["Outcome"].Width = 180;
            }

            cbFilterBy.SelectedIndex = 0;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationID = (int)dgvApplications.CurrentRow.Cells["ApplicationID"].Value;

            frmApplicationInfo frm = new frmApplicationInfo(ApplicationID);
            frm.ShowDialog();

            frmListApplications_Load(null, null);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }

            if (_dtAllApplications != null)
            {
                _dtAllApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvApplications.Rows.Count.ToString();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilterBy.Text)
            {
                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "Patient":
                    FilterColumn = "PersonName";
                    break;

                case "Doctor":
                    FilterColumn = "DoctorName";
                    break;

                case "Application Type":
                    FilterColumn = "ApplicationType";
                    break;

                case "Sub Type":
                    FilterColumn = "ApplicationSubType";
                    break;

                case "Status":
                    FilterColumn = "ApplicationStatus";
                    break;

                default:
                    FilterColumn = "";
                    break;
            }

            if (string.IsNullOrWhiteSpace(txtFilterValue.Text) || FilterColumn == "")
            {
                _dtAllApplications.DefaultView.RowFilter = "";
            }
            else
            {
                if (FilterColumn == "ApplicationID" || FilterColumn == "PersonID")
                {
                    _dtAllApplications.DefaultView.RowFilter =
                        string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
                }
                else
                {
                    _dtAllApplications.DefaultView.RowFilter =
                        string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim().Replace("'", "''"));
                }
            }

            lblRecordsCount.Text = dgvApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Application ID" ||
                cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateApplication frm = new frmAddUpdateApplication();
            frm.ShowDialog();

            frmListApplications_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationID = (int)dgvApplications.CurrentRow.Cells["ApplicationID"].Value;

            frmAddUpdateApplication frm = new frmAddUpdateApplication(ApplicationID);
            frm.ShowDialog();

            frmListApplications_Load(null, null);
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationID = (int)dgvApplications.CurrentRow.Cells["ApplicationID"].Value;

            clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

            if (Application != null)
            {
                if (Application.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.",
                        "Deleted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    frmListApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not delete application.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int ApplicationID = (int)dgvApplications.CurrentRow.Cells["ApplicationID"].Value;

            clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

            if (Application != null)
            {
                bool isNew =
                    Application.ApplicationStatus ==
                    clsApplication.enApplicationStatus.New;

                editToolStripMenuItem.Enabled = isNew;
                DeleteApplicationToolStripMenuItem.Enabled = isNew;
            }
        }
    }
}