namespace DVLDtraining.People.Controls
{
    partial class ctrlPersonInfoWithFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ctrlPersonInformation1 = new DVLDtraining.People.UserControls.ctrlPersonInformation();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.btnShowInfo = new System.Windows.Forms.Button();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctrlPersonInformation1
            // 
            this.ctrlPersonInformation1.BackColor = System.Drawing.Color.Black;
            this.ctrlPersonInformation1.ForeColor = System.Drawing.Color.DarkKhaki;
            this.ctrlPersonInformation1.Location = new System.Drawing.Point(3, 75);
            this.ctrlPersonInformation1.Name = "ctrlPersonInformation1";
            this.ctrlPersonInformation1.Size = new System.Drawing.Size(1120, 362);
            this.ctrlPersonInformation1.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.DarkGray;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.Black;
            this.txtSearch.Location = new System.Drawing.Point(287, 32);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(257, 22);
            this.txtSearch.TabIndex = 45;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // cmbFilter
            // 
            this.cmbFilter.BackColor = System.Drawing.Color.LightGray;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbFilter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFilter.ForeColor = System.Drawing.Color.Black;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "Person ID",
            "National No"});
            this.cmbFilter.Location = new System.Drawing.Point(29, 32);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(242, 22);
            this.cmbFilter.TabIndex = 44;
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.BackColor = System.Drawing.Color.Black;
            this.btnAddPerson.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddPerson.Image = global::DVLDtraining.Properties.Resources.add__1_;
            this.btnAddPerson.Location = new System.Drawing.Point(617, 25);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(35, 35);
            this.btnAddPerson.TabIndex = 46;
            this.btnAddPerson.UseVisualStyleBackColor = false;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            this.btnAddPerson.MouseLeave += new System.EventHandler(this.Buttons_MouseLeave);
            this.btnAddPerson.MouseHover += new System.EventHandler(this.Buttons_MouseHover);
            // 
            // btnShowInfo
            // 
            this.btnShowInfo.BackColor = System.Drawing.Color.Black;
            this.btnShowInfo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowInfo.Image = global::DVLDtraining.Properties.Resources.views;
            this.btnShowInfo.Location = new System.Drawing.Point(561, 24);
            this.btnShowInfo.Name = "btnShowInfo";
            this.btnShowInfo.Size = new System.Drawing.Size(35, 35);
            this.btnShowInfo.TabIndex = 52;
            this.btnShowInfo.UseVisualStyleBackColor = false;
            this.btnShowInfo.Click += new System.EventHandler(this.btnShowInfo_Click);
            this.btnShowInfo.MouseLeave += new System.EventHandler(this.Buttons_MouseLeave);
            this.btnShowInfo.MouseHover += new System.EventHandler(this.Buttons_MouseHover);
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnShowInfo);
            this.gbFilter.Controls.Add(this.cmbFilter);
            this.gbFilter.Controls.Add(this.btnAddPerson);
            this.gbFilter.Controls.Add(this.txtSearch);
            this.gbFilter.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFilter.ForeColor = System.Drawing.Color.Gainsboro;
            this.gbFilter.Location = new System.Drawing.Point(167, -1);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(656, 74);
            this.gbFilter.TabIndex = 53;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter:";
            // 
            // ctrlPersonInfoWithFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.ctrlPersonInformation1);
            this.Name = "ctrlPersonInfoWithFilter";
            this.Size = new System.Drawing.Size(1126, 445);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.ctrlPersonInformation ctrlPersonInformation1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.Button btnShowInfo;
        private System.Windows.Forms.GroupBox gbFilter;
    }
}
