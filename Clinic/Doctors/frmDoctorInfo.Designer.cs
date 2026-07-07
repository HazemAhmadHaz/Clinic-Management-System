namespace Clinic.Doctors
{
    partial class frmDoctorInfo
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlDoctorCard1 = new Clinic.Controls.ctrlDoctorCard();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Image = global::Clinic.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(720, 413);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 37);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrlDoctorCard1
            // 
            this.ctrlDoctorCard1.BackColor = System.Drawing.Color.White;
            this.ctrlDoctorCard1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ctrlDoctorCard1.Location = new System.Drawing.Point(13, 14);
            this.ctrlDoctorCard1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctrlDoctorCard1.Name = "ctrlDoctorCard1";
            this.ctrlDoctorCard1.Size = new System.Drawing.Size(839, 399);
            this.ctrlDoctorCard1.TabIndex = 0;
            // 
            // frmDoctorInfo
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(859, 462);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlDoctorCard1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDoctorInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Doctor Info";
            this.Load += new System.EventHandler(this.frmDoctorInfo_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private Controls.ctrlDoctorCard ctrlDoctorCard1;
        private System.Windows.Forms.Button btnClose;
    }
}