using Clinic_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic.Doctors
{
    public partial class frmListDoctors : Form
    {
        private static DataTable _dtAllDoctors;
        public frmListDoctors()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListDoctors_Load(object sender, EventArgs e)
        {
            _dtAllDoctors = clsDoctor.GetAllDoctors();

            // TEMP DIAGNOSTIC - remove after fix
            if (_dtAllDoctors == null)
            {
                MessageBox.Show("GetAllDoctors returned NULL — check DAL connection or SP.");
                return;
            }
            if (_dtAllDoctors.Columns.Count < 5)
            {
                MessageBox.Show($"Expected 5 columns, got {_dtAllDoctors.Columns.Count}. SP is still wrong.");
                return;
            }
            dgvDoctors.DataSource = _dtAllDoctors;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvDoctors.Rows.Count.ToString();

            dgvDoctors.Columns[0].HeaderText = "Doctor ID";
            dgvDoctors.Columns[0].Width = 110;

            dgvDoctors.Columns[1].HeaderText = "Person ID";
            dgvDoctors.Columns[1].Width = 120;

            dgvDoctors.Columns[2].HeaderText = "Full Name";
            dgvDoctors.Columns[2].Width = 350;

            dgvDoctors.Columns[3].HeaderText = "Specialisation";
            dgvDoctors.Columns[3].Width = 120;

            dgvDoctors.Columns[4].HeaderText = "Is Available";
            dgvDoctors.Columns[4].Width = 120;


        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbFilterBy.Text == "Is Available")
            {
                txtFilterValue.Visible = false;
                cbIsAvailable.Visible = true;
                cbIsAvailable.Focus();
                cbIsAvailable.SelectedIndex = 0;
            }

            else

            {

                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsAvailable.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                }
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }


        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Doctor ID":
                    FilterColumn = "DoctorID";
                    break;
                case "Specialisation":
                    FilterColumn = "Specialisation";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDoctors.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvDoctors.Rows.Count.ToString();
                return;
            }


            if (FilterColumn != "FullName" && FilterColumn != "Specialisation")
                //in this case we deal with numbers not string.
                _dtAllDoctors.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllDoctors.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = _dtAllDoctors.Rows.Count.ToString();
        }

        private void cbIsAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {


            string FilterColumn = "IsAvailable";
            string FilterValue = cbIsAvailable.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
                _dtAllDoctors.DefaultView.RowFilter = "";
            else
                //in this case we deal with numbers not string.
                _dtAllDoctors.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = _dtAllDoctors.Rows.Count.ToString();


        }
        private void btnAddDoctor_Click(object sender, EventArgs e)
        {
            frmAddUpdateDoctor Frm1 = new frmAddUpdateDoctor();
            Frm1.ShowDialog();
            frmListDoctors_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddUpdateDoctor Frm1 = new frmAddUpdateDoctor((int)dgvDoctors.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();
            frmListDoctors_Load(null, null);

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdateDoctor Frm1 = new frmAddUpdateDoctor();
            Frm1.ShowDialog();
            frmListDoctors_Load(null, null);

        }

        private void dgvDoctors_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmDoctorInfo Frm1 = new frmDoctorInfo((int)dgvDoctors.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDoctorInfo Frm1 = new frmDoctorInfo((int)dgvDoctors.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();

        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or Doctor id is selected.
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "Doctor ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int DoctorID = (int)dgvDoctors.CurrentRow.Cells[0].Value;
            if (clsDoctor.DeleteDoctor(DoctorID))
            {
                MessageBox.Show("Doctor has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmListDoctors_Load(null, null);
            }

            else
                MessageBox.Show("Doctor is not delted due to data connected to it.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);





        }
    }
}
