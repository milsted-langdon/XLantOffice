namespace XlantWord
{
    partial class XLTaskPane
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
            this.Addresslbl = new System.Windows.Forms.Label();
            this.Salutationlbl = new System.Windows.Forms.Label();
            this.SalutationTb = new System.Windows.Forms.TextBox();
            this.SubjectTB = new System.Windows.Forms.TextBox();
            this.Subjectlbl = new System.Windows.Forms.Label();
            this.DateTB = new System.Windows.Forms.DateTimePicker();
            this.Datelbl = new System.Windows.Forms.Label();
            this.SenderDDL = new System.Windows.Forms.ComboBox();
            this.Senderlbl = new System.Windows.Forms.Label();
            this.FAOBHOlbl = new System.Windows.Forms.Label();
            this.PandClbl = new System.Windows.Forms.Label();
            this.FAOBCheck = new System.Windows.Forms.CheckBox();
            this.PandCCheck = new System.Windows.Forms.CheckBox();
            this.ClientIDLabel = new System.Windows.Forms.Label();
            this.ClientLbl = new System.Windows.Forms.Label();
            this.GetAddressBtn = new System.Windows.Forms.Button();
            this.AddresseeTB = new System.Windows.Forms.TextBox();
            this.Addresseelbl = new System.Windows.Forms.Label();
            this.addTB = new System.Windows.Forms.TextBox();
            this.Addresseslbl = new System.Windows.Forms.Label();
            this.addressesDDL = new System.Windows.Forms.ComboBox();
            this.SalDDL = new System.Windows.Forms.ComboBox();
            this.Salutationslbl = new System.Windows.Forms.Label();
            this.RevertBtn = new System.Windows.Forms.Button();
            this.WhenCallingCheck = new System.Windows.Forms.CheckBox();
            this.WhenCallinglbl = new System.Windows.Forms.Label();
            this.WhencallingDDL = new System.Windows.Forms.ComboBox();
            this.FaxTB = new System.Windows.Forms.TextBox();
            this.IPSContactBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.EncChk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Addresslbl
            // 
            this.Addresslbl.AutoSize = true;
            this.Addresslbl.Location = new System.Drawing.Point(23, 382);
            this.Addresslbl.Name = "Addresslbl";
            this.Addresslbl.Size = new System.Drawing.Size(45, 13);
            this.Addresslbl.TabIndex = 6;
            this.Addresslbl.Text = "Address";
            // 
            // Salutationlbl
            // 
            this.Salutationlbl.AutoSize = true;
            this.Salutationlbl.Location = new System.Drawing.Point(23, 303);
            this.Salutationlbl.Name = "Salutationlbl";
            this.Salutationlbl.Size = new System.Drawing.Size(54, 13);
            this.Salutationlbl.TabIndex = 7;
            this.Salutationlbl.Text = "Salutation";
            // 
            // SalutationTb
            // 
            this.SalutationTb.Location = new System.Drawing.Point(122, 300);
            this.SalutationTb.Name = "SalutationTb";
            this.SalutationTb.Size = new System.Drawing.Size(192, 20);
            this.SalutationTb.TabIndex = 5;
            this.SalutationTb.DoubleClick += new System.EventHandler(this.SalutationTb_Leave);
            this.SalutationTb.Leave += new System.EventHandler(this.SalutationTb_Leave);
            // 
            // SubjectTB
            // 
            this.SubjectTB.Location = new System.Drawing.Point(122, 73);
            this.SubjectTB.Multiline = true;
            this.SubjectTB.Name = "SubjectTB";
            this.SubjectTB.Size = new System.Drawing.Size(192, 50);
            this.SubjectTB.TabIndex = 0;
            this.SubjectTB.DoubleClick += new System.EventHandler(this.SubjectTB_Leave);
            this.SubjectTB.Leave += new System.EventHandler(this.SubjectTB_Leave);
            // 
            // Subjectlbl
            // 
            this.Subjectlbl.AutoSize = true;
            this.Subjectlbl.Location = new System.Drawing.Point(23, 73);
            this.Subjectlbl.Name = "Subjectlbl";
            this.Subjectlbl.Size = new System.Drawing.Size(43, 13);
            this.Subjectlbl.TabIndex = 13;
            this.Subjectlbl.Text = "Subject";
            // 
            // DateTB
            // 
            this.DateTB.CustomFormat = "dd MMMM yyyy";
            this.DateTB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTB.Location = new System.Drawing.Point(122, 145);
            this.DateTB.Name = "DateTB";
            this.DateTB.Size = new System.Drawing.Size(192, 20);
            this.DateTB.TabIndex = 1;
            this.DateTB.Leave += new System.EventHandler(this.DateTB_Leave);
            // 
            // Datelbl
            // 
            this.Datelbl.AutoSize = true;
            this.Datelbl.Location = new System.Drawing.Point(23, 151);
            this.Datelbl.Name = "Datelbl";
            this.Datelbl.Size = new System.Drawing.Size(30, 13);
            this.Datelbl.TabIndex = 24;
            this.Datelbl.Text = "Date";
            // 
            // SenderDDL
            // 
            this.SenderDDL.FormattingEnabled = true;
            this.SenderDDL.Location = new System.Drawing.Point(122, 183);
            this.SenderDDL.Name = "SenderDDL";
            this.SenderDDL.Size = new System.Drawing.Size(192, 21);
            this.SenderDDL.TabIndex = 2;
            this.SenderDDL.SelectedIndexChanged += new System.EventHandler(this.SenderDDL_SelectedIndexChanged);
            // 
            // Senderlbl
            // 
            this.Senderlbl.AutoSize = true;
            this.Senderlbl.Location = new System.Drawing.Point(23, 186);
            this.Senderlbl.Name = "Senderlbl";
            this.Senderlbl.Size = new System.Drawing.Size(41, 13);
            this.Senderlbl.TabIndex = 26;
            this.Senderlbl.Text = "Sender";
            // 
            // FAOBHOlbl
            // 
            this.FAOBHOlbl.AutoSize = true;
            this.FAOBHOlbl.Location = new System.Drawing.Point(23, 590);
            this.FAOBHOlbl.Name = "FAOBHOlbl";
            this.FAOBHOlbl.Size = new System.Drawing.Size(69, 13);
            this.FAOBHOlbl.TabIndex = 30;
            this.FAOBHOlbl.Text = "FAOBHO ML";
            // 
            // PandClbl
            // 
            this.PandClbl.AutoSize = true;
            this.PandClbl.Location = new System.Drawing.Point(23, 552);
            this.PandClbl.Name = "PandClbl";
            this.PandClbl.Size = new System.Drawing.Size(107, 13);
            this.PandClbl.TabIndex = 29;
            this.PandClbl.Text = "Private && Confidential";
            // 
            // FAOBCheck
            // 
            this.FAOBCheck.AutoSize = true;
            this.FAOBCheck.Location = new System.Drawing.Point(148, 589);
            this.FAOBCheck.Name = "FAOBCheck";
            this.FAOBCheck.Size = new System.Drawing.Size(15, 14);
            this.FAOBCheck.TabIndex = 9;
            this.FAOBCheck.UseVisualStyleBackColor = true;
            this.FAOBCheck.CheckedChanged += new System.EventHandler(this.FAOBCheck_CheckedChanged);
            // 
            // PandCCheck
            // 
            this.PandCCheck.AutoSize = true;
            this.PandCCheck.Location = new System.Drawing.Point(148, 551);
            this.PandCCheck.Name = "PandCCheck";
            this.PandCCheck.Size = new System.Drawing.Size(15, 14);
            this.PandCCheck.TabIndex = 8;
            this.PandCCheck.UseVisualStyleBackColor = true;
            this.PandCCheck.CheckedChanged += new System.EventHandler(this.PandCCheck_CheckedChanged);
            // 
            // ClientIDLabel
            // 
            this.ClientIDLabel.AutoSize = true;
            this.ClientIDLabel.Location = new System.Drawing.Point(23, 29);
            this.ClientIDLabel.Name = "ClientIDLabel";
            this.ClientIDLabel.Size = new System.Drawing.Size(0, 13);
            this.ClientIDLabel.TabIndex = 31;
            this.ClientIDLabel.Visible = false;
            // 
            // ClientLbl
            // 
            this.ClientLbl.AutoSize = true;
            this.ClientLbl.Location = new System.Drawing.Point(71, 29);
            this.ClientLbl.Name = "ClientLbl";
            this.ClientLbl.Size = new System.Drawing.Size(33, 13);
            this.ClientLbl.TabIndex = 32;
            this.ClientLbl.Text = "Client";
            // 
            // GetAddressBtn
            // 
            this.GetAddressBtn.Location = new System.Drawing.Point(26, 441);
            this.GetAddressBtn.Name = "GetAddressBtn";
            this.GetAddressBtn.Size = new System.Drawing.Size(75, 23);
            this.GetAddressBtn.TabIndex = 12;
            this.GetAddressBtn.Text = "Contact";
            this.GetAddressBtn.UseVisualStyleBackColor = true;
            this.GetAddressBtn.Click += new System.EventHandler(this.GetAddressBtn_Click);
            // 
            // AddresseeTB
            // 
            this.AddresseeTB.Location = new System.Drawing.Point(122, 259);
            this.AddresseeTB.Name = "AddresseeTB";
            this.AddresseeTB.Size = new System.Drawing.Size(192, 20);
            this.AddresseeTB.TabIndex = 4;
            this.AddresseeTB.DoubleClick += new System.EventHandler(this.AddresseeTB_Leave);
            this.AddresseeTB.Leave += new System.EventHandler(this.AddresseeTB_Leave);
            // 
            // Addresseelbl
            // 
            this.Addresseelbl.AutoSize = true;
            this.Addresseelbl.Location = new System.Drawing.Point(23, 262);
            this.Addresseelbl.Name = "Addresseelbl";
            this.Addresseelbl.Size = new System.Drawing.Size(57, 13);
            this.Addresseelbl.TabIndex = 35;
            this.Addresseelbl.Text = "Addressee";
            // 
            // addTB
            // 
            this.addTB.Location = new System.Drawing.Point(122, 382);
            this.addTB.Multiline = true;
            this.addTB.Name = "addTB";
            this.addTB.Size = new System.Drawing.Size(192, 144);
            this.addTB.TabIndex = 7;
            this.addTB.DoubleClick += new System.EventHandler(this.addTB_Leave);
            this.addTB.Leave += new System.EventHandler(this.addTB_Leave);
            // 
            // Addresseslbl
            // 
            this.Addresseslbl.AutoSize = true;
            this.Addresseslbl.Location = new System.Drawing.Point(23, 342);
            this.Addresseslbl.Name = "Addresseslbl";
            this.Addresseslbl.Size = new System.Drawing.Size(74, 13);
            this.Addresseslbl.TabIndex = 39;
            this.Addresseslbl.Text = "Alt. Addresses";
            // 
            // addressesDDL
            // 
            this.addressesDDL.FormattingEnabled = true;
            this.addressesDDL.Location = new System.Drawing.Point(122, 339);
            this.addressesDDL.Name = "addressesDDL";
            this.addressesDDL.Size = new System.Drawing.Size(192, 21);
            this.addressesDDL.TabIndex = 6;
            this.addressesDDL.SelectedIndexChanged += new System.EventHandler(this.addressLB_SelectedIndexChanged);
            // 
            // SalDDL
            // 
            this.SalDDL.FormattingEnabled = true;
            this.SalDDL.Location = new System.Drawing.Point(122, 225);
            this.SalDDL.Name = "SalDDL";
            this.SalDDL.Size = new System.Drawing.Size(192, 21);
            this.SalDDL.TabIndex = 3;
            this.SalDDL.SelectedIndexChanged += new System.EventHandler(this.SalDDL_SelectedIndexChanged);
            // 
            // Salutationslbl
            // 
            this.Salutationslbl.AutoSize = true;
            this.Salutationslbl.Location = new System.Drawing.Point(23, 228);
            this.Salutationslbl.Name = "Salutationslbl";
            this.Salutationslbl.Size = new System.Drawing.Size(77, 13);
            this.Salutationslbl.TabIndex = 41;
            this.Salutationslbl.Text = "Alt. Salutations";
            // 
            // RevertBtn
            // 
            this.RevertBtn.Location = new System.Drawing.Point(25, 470);
            this.RevertBtn.Name = "RevertBtn";
            this.RevertBtn.Size = new System.Drawing.Size(75, 23);
            this.RevertBtn.TabIndex = 13;
            this.RevertBtn.Text = "Revert";
            this.RevertBtn.UseVisualStyleBackColor = true;
            this.RevertBtn.Visible = false;
            this.RevertBtn.Click += new System.EventHandler(this.RevertBtn_Click);
            // 
            // WhenCallingCheck
            // 
            this.WhenCallingCheck.AutoSize = true;
            this.WhenCallingCheck.Location = new System.Drawing.Point(148, 627);
            this.WhenCallingCheck.Name = "WhenCallingCheck";
            this.WhenCallingCheck.Size = new System.Drawing.Size(15, 14);
            this.WhenCallingCheck.TabIndex = 10;
            this.WhenCallingCheck.UseVisualStyleBackColor = true;
            this.WhenCallingCheck.CheckedChanged += new System.EventHandler(this.WhenCallingCheck_CheckedChanged);
            // 
            // WhenCallinglbl
            // 
            this.WhenCallinglbl.AutoSize = true;
            this.WhenCallinglbl.Location = new System.Drawing.Point(23, 628);
            this.WhenCallinglbl.Name = "WhenCallinglbl";
            this.WhenCallinglbl.Size = new System.Drawing.Size(72, 13);
            this.WhenCallinglbl.TabIndex = 45;
            this.WhenCallinglbl.Text = "When calling:";
            // 
            // WhencallingDDL
            // 
            this.WhencallingDDL.FormattingEnabled = true;
            this.WhencallingDDL.Location = new System.Drawing.Point(122, 647);
            this.WhencallingDDL.Name = "WhencallingDDL";
            this.WhencallingDDL.Size = new System.Drawing.Size(192, 21);
            this.WhencallingDDL.TabIndex = 11;
            this.WhencallingDDL.Visible = false;
            this.WhencallingDDL.SelectedIndexChanged += new System.EventHandler(this.WhencallingDDL_SelectedIndexChanged);
            // 
            // FaxTB
            // 
            this.FaxTB.Location = new System.Drawing.Point(122, 339);
            this.FaxTB.Name = "FaxTB";
            this.FaxTB.Size = new System.Drawing.Size(192, 20);
            this.FaxTB.TabIndex = 14;
            this.FaxTB.Visible = false;
            this.FaxTB.DoubleClick += new System.EventHandler(this.FaxTB_Leave);
            this.FaxTB.Leave += new System.EventHandler(this.FaxTB_Leave);
            // 
            // IPSContactBtn
            // 
            this.IPSContactBtn.Location = new System.Drawing.Point(26, 412);
            this.IPSContactBtn.Name = "IPSContactBtn";
            this.IPSContactBtn.Size = new System.Drawing.Size(75, 23);
            this.IPSContactBtn.TabIndex = 46;
            this.IPSContactBtn.Text = "IPS Contact";
            this.IPSContactBtn.UseVisualStyleBackColor = true;
            this.IPSContactBtn.Visible = false;
            this.IPSContactBtn.Click += new System.EventHandler(this.IPSContactBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 552);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Enc";
            // 
            // EncChk
            // 
            this.EncChk.AutoSize = true;
            this.EncChk.Location = new System.Drawing.Point(280, 552);
            this.EncChk.Name = "EncChk";
            this.EncChk.Size = new System.Drawing.Size(15, 14);
            this.EncChk.TabIndex = 47;
            this.EncChk.UseVisualStyleBackColor = true;
            this.EncChk.CheckedChanged += new System.EventHandler(this.EncChk_CheckedChanged);
            // 
            // XLTaskPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EncChk);
            this.Controls.Add(this.IPSContactBtn);
            this.Controls.Add(this.FaxTB);
            this.Controls.Add(this.WhencallingDDL);
            this.Controls.Add(this.WhenCallinglbl);
            this.Controls.Add(this.WhenCallingCheck);
            this.Controls.Add(this.RevertBtn);
            this.Controls.Add(this.SalDDL);
            this.Controls.Add(this.Salutationslbl);
            this.Controls.Add(this.addressesDDL);
            this.Controls.Add(this.Addresseslbl);
            this.Controls.Add(this.addTB);
            this.Controls.Add(this.AddresseeTB);
            this.Controls.Add(this.Addresseelbl);
            this.Controls.Add(this.GetAddressBtn);
            this.Controls.Add(this.ClientLbl);
            this.Controls.Add(this.ClientIDLabel);
            this.Controls.Add(this.FAOBHOlbl);
            this.Controls.Add(this.PandClbl);
            this.Controls.Add(this.FAOBCheck);
            this.Controls.Add(this.PandCCheck);
            this.Controls.Add(this.SubjectTB);
            this.Controls.Add(this.Subjectlbl);
            this.Controls.Add(this.Senderlbl);
            this.Controls.Add(this.SenderDDL);
            this.Controls.Add(this.Datelbl);
            this.Controls.Add(this.SalutationTb);
            this.Controls.Add(this.Salutationlbl);
            this.Controls.Add(this.DateTB);
            this.Controls.Add(this.Addresslbl);
            this.Name = "XLTaskPane";
            this.Size = new System.Drawing.Size(340, 759);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Addresslbl;
        private System.Windows.Forms.Label Salutationlbl;
        private System.Windows.Forms.TextBox SalutationTb;
        private System.Windows.Forms.TextBox SubjectTB;
        private System.Windows.Forms.Label Subjectlbl;
        private System.Windows.Forms.DateTimePicker DateTB;
        private System.Windows.Forms.Label Datelbl;
        private System.Windows.Forms.ComboBox SenderDDL;
        private System.Windows.Forms.Label Senderlbl;
        private System.Windows.Forms.Label FAOBHOlbl;
        private System.Windows.Forms.Label PandClbl;
        private System.Windows.Forms.CheckBox FAOBCheck;
        private System.Windows.Forms.CheckBox PandCCheck;
        private System.Windows.Forms.Label ClientIDLabel;
        private System.Windows.Forms.Label ClientLbl;
        private System.Windows.Forms.Button GetAddressBtn;
        private System.Windows.Forms.TextBox AddresseeTB;
        private System.Windows.Forms.Label Addresseelbl;
        private System.Windows.Forms.TextBox addTB;
        private System.Windows.Forms.Label Addresseslbl;
        private System.Windows.Forms.ComboBox addressesDDL;
        private System.Windows.Forms.ComboBox SalDDL;
        private System.Windows.Forms.Label Salutationslbl;
        private System.Windows.Forms.Button RevertBtn;
        private System.Windows.Forms.CheckBox WhenCallingCheck;
        private System.Windows.Forms.Label WhenCallinglbl;
        private System.Windows.Forms.ComboBox WhencallingDDL;
        private System.Windows.Forms.TextBox FaxTB;
        private System.Windows.Forms.Button IPSContactBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox EncChk;
    }
}
