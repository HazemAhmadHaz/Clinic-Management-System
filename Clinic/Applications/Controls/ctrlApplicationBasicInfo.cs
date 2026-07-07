using Clinic.Classes;
using Clinic.People;
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

namespace Clinic.Controls.ApplicationControls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsApplication _Application;
        private int _ApplicationID = -1;

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        public clsApplication SelectedApplicationInfo
        {
            get { return _Application; }
        }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.FindBaseApplication(ApplicationID);

            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _FillApplicationInfo();
            }
        }

        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;

            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _Application.ApplicationStatus.ToString();

            lblType.Text = clsApplicationType.FindByID(_Application.ApplicationTypeID).Name;

            lblFees.Text = _Application.PaidFees.ToString();
            lblApplicant.Text = _Application.ApplicantFullName;

            lblDate.Text = clsFormat.DateToShort(_Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_Application.ApplicationDate);

            lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;

            // =========================
            // MISSING FIELDS (ADD THESE)
            // =========================

            lblDoctor.Text =
                clsDoctor.FindByDoctorID(_Application.DoctorID).PersonInfo.FullName;

            lblSubType.Text =
                clsApplicationSubType.Find(_Application.ApplicationSubTypeID)?.Name ?? "[????]";

            lblAppointmentDate.Text =_Application.AppointmentDate.ToString();

            // Outcome (if exists in DB/model)
            lblOutcome.Text =
                _Application.Outcome ?? "";
            
            lblPaidFees.Text =
                _Application.PaidFees.ToString() ?? "";

            llViewPersonInfo.Enabled = true;
        }
        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;
            _Application = null;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";

            // Disable link label when no data is loaded
            llViewPersonInfo.Enabled = false;
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Application == null) return;

            frmShowPersonInfo frm = new frmShowPersonInfo(_Application.PersonID);
            frm.ShowDialog();

            // Refresh control data in case applicant information was updated inside the dialog
            LoadApplicationInfo(_ApplicationID);
        }
    }
}