using Clinic.Classes;
using Clinic_Buisness;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Clinic.Applications
{
    public partial class frmEditApplicationSubTypes : Form
    {
        private int _SubApplicationTypeID = -1;
        private clsApplicationSubType _SubApplicationType;

        public frmEditApplicationSubTypes(int SubApplicationTypeID)
        {
            InitializeComponent();
            _SubApplicationTypeID = SubApplicationTypeID;
        }
        private void frmEditSubApplicationTypes_Load(object sender, EventArgs e)
        {
            lblID.Text = _SubApplicationTypeID.ToString();

            _SubApplicationType = clsApplicationSubType.Find(_SubApplicationTypeID);

            if (_SubApplicationType != null)
            {
                txtName.Text = _SubApplicationType.Name;
                txtFees.Text = _SubApplicationType.Fees.ToString();
                txtApplicationTypeID.Text = _SubApplicationType.ApplicationSubTypeID == 1 ? "Consultation" : "Surgery";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show(
                    "Some fields are not valid! Put the mouse over the red icon(s).",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            _SubApplicationType.Name = txtName.Text.Trim();
            _SubApplicationType.Fees = Convert.ToDecimal(txtFees.Text.Trim());
            _SubApplicationType.ApplicationSubTypeID = txtApplicationTypeID.Text.Trim() == "Consultation" ? 1 : 2;

            if (_SubApplicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data was not saved.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtName, "Name cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtName, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
                return;
            }

            if (!decimal.TryParse(txtFees.Text, out _))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid number!");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void txtApplicationTypeID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtApplicationTypeID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeID, "Application Type ID cannot be empty!");
                return;
            }

            if (!int.TryParse(txtApplicationTypeID.Text, out _))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeID, "Invalid ID!");
            }
            else
            {
                errorProvider1.SetError(txtApplicationTypeID, null);
            }
        }


    }
}