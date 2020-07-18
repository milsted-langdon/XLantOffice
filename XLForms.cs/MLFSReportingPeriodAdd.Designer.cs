namespace XLForms
{
    partial class MLFSReportingPeriodAdd
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
            this.MonthDDL = new System.Windows.Forms.ComboBox();
            this.YearDDL = new System.Windows.Forms.ComboBox();
            this.DescriptionTb = new System.Windows.Forms.TextBox();
            this.ReportOrderTb = new System.Windows.Forms.NumericUpDown();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ReportOrderTb)).BeginInit();
            this.SuspendLayout();
            // 
            // MonthDDL
            // 
            this.MonthDDL.FormattingEnabled = true;
            this.MonthDDL.Location = new System.Drawing.Point(218, 52);
            this.MonthDDL.Name = "MonthDDL";
            this.MonthDDL.Size = new System.Drawing.Size(160, 21);
            this.MonthDDL.TabIndex = 0;
            this.MonthDDL.SelectedIndexChanged += new System.EventHandler(this.MonthDDL_SelectedIndexChanged);
            // 
            // YearDDL
            // 
            this.YearDDL.FormattingEnabled = true;
            this.YearDDL.Location = new System.Drawing.Point(413, 52);
            this.YearDDL.Name = "YearDDL";
            this.YearDDL.Size = new System.Drawing.Size(121, 21);
            this.YearDDL.TabIndex = 1;
            this.YearDDL.SelectedIndexChanged += new System.EventHandler(this.YearDDL_SelectedIndexChanged);
            // 
            // DescriptionTb
            // 
            this.DescriptionTb.Location = new System.Drawing.Point(218, 109);
            this.DescriptionTb.Name = "DescriptionTb";
            this.DescriptionTb.Size = new System.Drawing.Size(316, 20);
            this.DescriptionTb.TabIndex = 2;
            this.DescriptionTb.TextChanged += new System.EventHandler(this.DescriptionTb_TextChanged);
            // 
            // ReportOrderTb
            // 
            this.ReportOrderTb.Location = new System.Drawing.Point(218, 167);
            this.ReportOrderTb.Name = "ReportOrderTb";
            this.ReportOrderTb.Size = new System.Drawing.Size(160, 20);
            this.ReportOrderTb.TabIndex = 3;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(52, 55);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(83, 13);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "Month and Year";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(52, 115);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(60, 13);
            this.Label2.TabIndex = 5;
            this.Label2.Text = "Description";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(52, 169);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(68, 13);
            this.Label3.TabIndex = 6;
            this.Label3.Text = "Report Order";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(537, 208);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 7;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(456, 208);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // MLFSReportingPeriodAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 245);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.ReportOrderTb);
            this.Controls.Add(this.DescriptionTb);
            this.Controls.Add(this.YearDDL);
            this.Controls.Add(this.MonthDDL);
            this.Name = "MLFSReportingPeriodAdd";
            this.Text = "Add Reporting Period";
            ((System.ComponentModel.ISupportInitialize)(this.ReportOrderTb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MonthDDL;
        private System.Windows.Forms.ComboBox YearDDL;
        private System.Windows.Forms.TextBox DescriptionTb;
        private System.Windows.Forms.NumericUpDown ReportOrderTb;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}