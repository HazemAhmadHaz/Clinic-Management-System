using Clinic.Applications;
using Clinic.Classes;
using Clinic.Doctors;
//using Clinic.Drivers;
using Clinic.Login;
using Clinic.People;
using Clinic.User;
using Clinic.Applications.ApplicationTypes;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace Clinic
{

    public partial class frmMain : Form
    {
        frmLogin _frmLogin;

        public frmMain( frmLogin frm )
        {
            InitializeComponent();
            _frmLogin= frm;

        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();

        }



        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmListUsers();
            frm.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            lblLoggedInUser.Text = "LoggedIn User: " + clsGlobal.CurrentUser.UserName;
            this.Refresh();

        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();

        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _frmLogin.Show();
            this.Close();
        }


        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListApplicationSubTypes frm = new frmListApplicationSubTypes();
            frm.ShowDialog();
        }



      
        private void vehiclesLicensesServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmListDrivers frm = new frmListDrivers();
            //frm.ShowDialog();

        }

      

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frm = new frmListDoctors();
            frm.ShowDialog();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void drivingLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void oNewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tsMManageApplications_Click(object sender, EventArgs e)
        {
            frmListApplications frm = new frmListApplications();
            frm.ShowDialog();
        }
    }
}
