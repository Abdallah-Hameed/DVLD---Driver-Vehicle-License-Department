namespace DVLDtraining.Applications.Forms.Local_License
{
    partial class frmShowLocalLicenseApplicationInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAddUpdatePerson = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlLicenseApplicationInfo1 = new DVLDtraining.Applications.Controls.ctrlLicenseApplicationInfo();
            this.SuspendLayout();
            // 
            // lblAddUpdatePerson
            // 
            this.lblAddUpdatePerson.AutoSize = true;
            this.lblAddUpdatePerson.Font = new System.Drawing.Font("Segoe Print", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddUpdatePerson.ForeColor = System.Drawing.Color.Gold;
            this.lblAddUpdatePerson.Location = new System.Drawing.Point(265, 0);
            this.lblAddUpdatePerson.Name = "lblAddUpdatePerson";
            this.lblAddUpdatePerson.Size = new System.Drawing.Size(564, 65);
            this.lblAddUpdatePerson.TabIndex = 38;
            this.lblAddUpdatePerson.Text = "Local license application info";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Image = global::DVLDtraining.Properties.Resources.close__5_;
            this.btnClose.Location = new System.Drawing.Point(1091, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(41, 40);
            this.btnClose.TabIndex = 37;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
            this.btnClose.MouseHover += new System.EventHandler(this.btnClose_MouseHover);
            // 
            // ctrlLicenseApplicationInfo1
            // 
            this.ctrlLicenseApplicationInfo1.BackColor = System.Drawing.Color.Black;
            this.ctrlLicenseApplicationInfo1.Location = new System.Drawing.Point(7, 96);
            this.ctrlLicenseApplicationInfo1.Name = "ctrlLicenseApplicationInfo1";
            this.ctrlLicenseApplicationInfo1.Size = new System.Drawing.Size(1133, 451);
            this.ctrlLicenseApplicationInfo1.TabIndex = 39;
            // 
            // frmShowLocalLicenseApplicationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1144, 625);
            this.Controls.Add(this.ctrlLicenseApplicationInfo1);
            this.Controls.Add(this.lblAddUpdatePerson);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmShowLocalLicenseApplicationInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmShowLocalLicenseApplicationInfo";
            this.Load += new System.EventHandler(this.frmShowLocalLicenseApplicationInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAddUpdatePerson;
        private System.Windows.Forms.Button btnClose;
        private Controls.ctrlLicenseApplicationInfo ctrlLicenseApplicationInfo1;
    }
}