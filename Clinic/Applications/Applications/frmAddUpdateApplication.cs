using Clinic.Classes;
using Clinic.People;
using Clinic_Buisness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Clinic.Applications
{
    public partial class frmAddUpdateApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;
        private int _ApplicationID = -1;
        private int _SelectedPersonID = -1;
        private clsApplication _Application;

        public frmAddUpdateApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateApplication(int ApplicationID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _ApplicationID = ApplicationID;
        }

        private void _FillApplicationTypesInComboBox()
        {
            cbxType.Items.Clear();

            DataTable dt = clsApplicationType.GetAll();

            foreach (DataRow row in dt.Rows)
                cbxType.Items.Add(row["Name"]);
        }

        private void _FillApplicationSubTypesInComboBox(int applicationTypeID)
        {
            cbApplicationSubType.Items.Clear();

            DataTable dt =
                clsApplicationSubType.GetApplicationSubTypesByApplicationTypeID(applicationTypeID);

            foreach (DataRow row in dt.Rows)
                cbApplicationSubType.Items.Add(row["Name"]);
        }

        private void _FillDoctorsInComboBox()
        {
            DataTable dt = clsDoctor.GetAllDoctors();

            cbDoctor.DataSource = dt;
            cbDoctor.DisplayMember = "FullName";
            cbDoctor.ValueMember = "DoctorID";
        }

        private void _FillStatusComboBox()
        {
            cbStatus.Items.Clear();

            foreach (string item in Enum.GetNames(typeof(clsApplication.enApplicationStatus)))
                cbStatus.Items.Add(item);
        }

        private void _ResetDefaultValues()
        {
            _FillApplicationTypesInComboBox();
            _FillDoctorsInComboBox();
            _FillStatusComboBox();

            cbxType.SelectedIndexChanged += cbxType_SelectedIndexChanged;
            cbApplicationSubType.SelectedIndexChanged += cbApplicationType_SelectedIndexChanged;

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Clinic Application";
                Text = "New Clinic Application";

                _Application = new clsApplication();

                ctrlPersonCardWithFilter1.FilterFocus();

                tpApplicationDetails.Enabled = false;

                if (cbxType.Items.Count > 0)
                    cbxType.SelectedIndex = 0;

                if (cbApplicationSubType.Items.Count > 0)
                    cbApplicationSubType.SelectedIndex = 0;

                if (cbDoctor.Items.Count > 0)
                    cbDoctor.SelectedIndex = 0;

                cbStatus.SelectedItem = clsApplication.enApplicationStatus.New.ToString();

                lblApplicationID.Text = "[???]";
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

                lblFees.Text = "0.00";

                dtpAppointmentDate.Value = DateTime.Now;
                txtResourceName.Clear();
                txtPaidFees.Text = "0.00";

            }
            else
            {
                lblTitle.Text = "Update Clinic Application";
                Text = "Update Clinic Application";

                tpApplicationDetails.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void cbxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsApplicationType type = clsApplicationType.FindByTitle(cbxType.Text);

            if (type == null)
                return;

            _FillApplicationSubTypesInComboBox(type.ID == 3 ? 0 : type.ID);
        }

        private void cbApplicationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsApplicationSubType sub =
                clsApplicationSubType.FindByTitle(cbApplicationSubType.Text);

            if (sub != null)
                lblFees.Text = sub.Fees.ToString();
        }
        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            _Application = clsApplication.FindBaseApplication(_ApplicationID);

            if (_Application == null)
            {
                MessageBox.Show("No Application with ID = " + _ApplicationID,
                    "Application Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_Application.PersonID);

            _SelectedPersonID = _Application.PersonID;

            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_Application.ApplicationDate);

            clsApplicationType appType =
                clsApplicationType.FindByID(_Application.ApplicationTypeID);

            if (appType != null)
                cbxType.SelectedIndex = cbxType.FindStringExact(appType.Name);

            clsApplicationSubType subType =
                clsApplicationSubType.Find(_Application.ApplicationSubTypeID);

            if (subType != null)
                cbApplicationSubType.SelectedIndex =
                    cbApplicationSubType.FindStringExact(subType.Name);

            clsDoctor Doctor =
                clsDoctor.FindByDoctorID(_Application.DoctorID);

            if (Doctor != null)
                cbDoctor.SelectedIndex =
                                    cbDoctor.FindStringExact(Doctor.PersonInfo.FullName);

            if (_Application.AppointmentDate.HasValue)
                dtpAppointmentDate.Value = _Application.AppointmentDate.Value;

            txtResourceName.Text = _Application.ResourceName;

            cbStatus.SelectedItem = _Application.ApplicationStatus.ToString();

            lblFees.Text = _Application.Fees.ToString("0.00");

            lblCreatedByUser.Text =
                clsUser.FindByUserID(_Application.CreatedByUserID).UserName;

            txtPaidFees.Text = _Application.PaidFees.ToString();

            cbLock.Text = (_Application.IsLocked == true ? "Yes" : "No");
        }

        private void frmAddUpdateApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                tpApplicationDetails.Enabled = true;
                btnSave.Enabled = true;
                tcApplicationInfo.SelectedTab = tpApplicationDetails;
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show(
                    "Please select a patient first.",
                    "Select Patient",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }

            btnSave.Enabled = true;
            tpApplicationDetails.Enabled = true;
            tcApplicationInfo.SelectedTab = tpApplicationDetails;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show(
                    "Some fields are invalid.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            clsApplicationType type =
                clsApplicationType.FindByTitle(cbxType.Text);

            if (type == null)
            {
                MessageBox.Show(
                    "Please select an application type.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            clsApplicationSubType subType =
                clsApplicationSubType.FindByTitle(cbApplicationSubType.Text);

            if (subType == null)
            {
                MessageBox.Show(
                    "Please select an application sub type.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            int ActiveApplicationID =
                clsApplication.GetActiveApplicationIDForApplicationTypeAndSubType(
                    ctrlPersonCardWithFilter1.PersonID,
                    subType.ApplicationSubTypeID,
                    type.ID);

            if (_Mode == enMode.AddNew &&
                ActiveApplicationID != -1)
            {
                MessageBox.Show(
                    "This patient already has an active application with ID = "
                    + ActiveApplicationID,
                    "Duplicate",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            _Application.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _Application.ApplicationDate = DateTime.Now;

            _Application.ApplicationTypeID = type.ID;
            _Application.ApplicationSubTypeID = subType.ApplicationSubTypeID;

            if (cbDoctor.SelectedItem != null)
                _Application.DoctorID = (int)cbDoctor.SelectedValue;

            _Application.AppointmentDate = dtpAppointmentDate.Value;

            _Application.ResourceName = txtResourceName.Text.Trim();

            _Application.ApplicationStatus =
                (clsApplication.enApplicationStatus)
                Enum.Parse(
                    typeof(clsApplication.enApplicationStatus),
                    cbStatus.Text);

            decimal fees;
            if (!decimal.TryParse(lblFees.Text, out fees))
            {
                MessageBox.Show("Invalid Fees value.");
                return;
            }

            _Application.Fees = fees;


            decimal PaidFees;
            if (!decimal.TryParse(txtPaidFees.Text, out PaidFees))
            {
                MessageBox.Show("Invalid Paid Fees value.");
                return;
            }

            _Application.PaidFees = PaidFees;

            _Application.IsLocked = (cbLock.Text == "Yes" ?  true : false);

            _Application.CreatedByUserID =
                clsGlobal.CurrentUser.UserID;

            _Application.IsLocked = false;
            _Application.Outcome = "";
            if (_Application.Save())
            {
                lblApplicationID.Text = _Application.ApplicationID.ToString();

                _Mode = enMode.Update;

                lblTitle.Text = "Update Clinic Application";
                Text = "Update Clinic Application";

                MessageBox.Show(
                    "Application saved successfully.",
                    "Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Error: Application was not saved.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int PersonID)
        {
            _SelectedPersonID = PersonID;
        }

        private void frmAddUpdateApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}