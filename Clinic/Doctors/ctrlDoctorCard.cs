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

namespace Clinic.Controls
{
    public partial class ctrlDoctorCard : UserControl
    {
        private clsDoctor _Doctor;
        private int _DoctorID = -1;

        public int DoctorID
        {
            get { return _DoctorID; }
        }

        public ctrlDoctorCard()
        {
            InitializeComponent();
        }

        public void LoadDoctorInfo(int DoctorID)
        {
            _Doctor = clsDoctor.FindByDoctorID(DoctorID);

            _FillDoctorInfo();
        }

        private void _FillDoctorInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_Doctor.PersonID);
            lblDoctorID.Text = _Doctor.DoctorID.ToString();
            lblSpecialisation.Text = _Doctor.Specialisation.ToString();

            if (_Doctor.IsAvailable)
                lblIsAvailable.Text = "Yes";
            else
                lblIsAvailable.Text = "No";

        }

        private void _ResetPersonInfo()
        {

            ctrlPersonCard1.ResetPersonInfo();
            lblDoctorID.Text = "[???]";
            lblSpecialisation.Text = "[???]";
            lblIsAvailable.Text = "[???]";
        }
    }
}
