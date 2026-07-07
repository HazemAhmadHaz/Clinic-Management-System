using Clinic.Properties;
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
using Clinic.Classes;
using Clinic.People;
using Clinic.Controls;
using System.Runtime.Remoting.Messaging;

namespace Clinic.Doctors
{
    public partial class frmAddUpdateDoctor : Form
    {

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _DoctorID = -1;
        clsDoctor _Doctor;

        public frmAddUpdateDoctor()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateDoctor(int DoctorID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _DoctorID = DoctorID;
        }

        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Doctor";
                this.Text = "Add New Doctor";
                _Doctor = new clsDoctor();

                tpDoctorInfo.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update Doctor";
                this.Text = "Update Doctor";

                tpDoctorInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            txtSpecialisation.Text = "";
            chkIsAvailable.Checked = true;
        }

        private void _LoadData()
        {
            _Doctor = clsDoctor.FindByDoctorID(_DoctorID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_Doctor == null)
            {
                MessageBox.Show("No Doctor with ID = " + _DoctorID, "Doctor Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            //the following code will not be executed if the person was not found
            lblDoctorID.Text = _Doctor.DoctorID.ToString();
            txtSpecialisation.Text = _Doctor.Specialisation;
            chkIsAvailable.Checked = _Doctor.IsAvailable;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_Doctor.PersonID);
        }

        private void frmAddUpdateDoctor_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Doctor.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _Doctor.Specialisation = txtSpecialisation.Text.Trim();
            _Doctor.IsAvailable = chkIsAvailable.Checked;

            if (_Doctor.Save())
            {
                lblDoctorID.Text = _Doctor.DoctorID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "Update Doctor";
                this.Text = "Update Doctor";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtSpecialisation_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSpecialisation.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtSpecialisation, "Specialisation cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtSpecialisation, null);
            }
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpDoctorInfo.Enabled = true;
                tcDoctorInfo.SelectedTab = tcDoctorInfo.TabPages["tpDoctorInfo"];
                return;
            }

            //incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if (clsDoctor.IsDoctorExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a Doctor record, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpDoctorInfo.Enabled = true;
                    tcDoctorInfo.SelectedTab = tcDoctorInfo.TabPages["tpDoctorInfo"];
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void frmAddUpdateDoctor_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}