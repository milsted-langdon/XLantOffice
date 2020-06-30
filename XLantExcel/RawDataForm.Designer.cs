namespace XLantExcel
{
    partial class RawDataForm
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
            this.ViewDDL = new System.Windows.Forms.ComboBox();
            this.ViewLbl = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.Delimiter1Ddl = new System.Windows.Forms.ComboBox();
            this.Delimiter1Lbl = new System.Windows.Forms.Label();
            this.Delimiter2Ddl = new System.Windows.Forms.ComboBox();
            this.Delimiter2Lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ViewDDL
            // 
            this.ViewDDL.FormattingEnabled = true;
            this.ViewDDL.Location = new System.Drawing.Point(263, 45);
            this.ViewDDL.Name = "ViewDDL";
            this.ViewDDL.Size = new System.Drawing.Size(273, 21);
            this.ViewDDL.TabIndex = 0;
            // 
            // ViewLbl
            // 
            this.ViewLbl.AutoSize = true;
            this.ViewLbl.Location = new System.Drawing.Point(61, 48);
            this.ViewLbl.Name = "ViewLbl";
            this.ViewLbl.Size = new System.Drawing.Size(30, 13);
            this.ViewLbl.TabIndex = 1;
            this.ViewLbl.Text = "View";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(522, 204);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(441, 204);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // Delimiter1Ddl
            // 
            this.Delimiter1Ddl.FormattingEnabled = true;
            this.Delimiter1Ddl.Location = new System.Drawing.Point(263, 96);
            this.Delimiter1Ddl.Name = "Delimiter1Ddl";
            this.Delimiter1Ddl.Size = new System.Drawing.Size(273, 21);
            this.Delimiter1Ddl.TabIndex = 4;
            this.Delimiter1Ddl.Visible = false;
            // 
            // Delimiter1Lbl
            // 
            this.Delimiter1Lbl.AutoSize = true;
            this.Delimiter1Lbl.Location = new System.Drawing.Point(61, 99);
            this.Delimiter1Lbl.Name = "Delimiter1Lbl";
            this.Delimiter1Lbl.Size = new System.Drawing.Size(35, 13);
            this.Delimiter1Lbl.TabIndex = 5;
            this.Delimiter1Lbl.Text = "label1";
            this.Delimiter1Lbl.Visible = false;
            // 
            // Delimiter2Ddl
            // 
            this.Delimiter2Ddl.FormattingEnabled = true;
            this.Delimiter2Ddl.Location = new System.Drawing.Point(263, 151);
            this.Delimiter2Ddl.Name = "Delimiter2Ddl";
            this.Delimiter2Ddl.Size = new System.Drawing.Size(273, 21);
            this.Delimiter2Ddl.TabIndex = 6;
            this.Delimiter2Ddl.Visible = false;
            // 
            // Delimiter2Lbl
            // 
            this.Delimiter2Lbl.AutoSize = true;
            this.Delimiter2Lbl.Location = new System.Drawing.Point(61, 154);
            this.Delimiter2Lbl.Name = "Delimiter2Lbl";
            this.Delimiter2Lbl.Size = new System.Drawing.Size(35, 13);
            this.Delimiter2Lbl.TabIndex = 7;
            this.Delimiter2Lbl.Text = "label1";
            this.Delimiter2Lbl.Visible = false;
            // 
            // RawDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 240);
            this.Controls.Add(this.Delimiter2Lbl);
            this.Controls.Add(this.Delimiter2Ddl);
            this.Controls.Add(this.Delimiter1Lbl);
            this.Controls.Add(this.Delimiter1Ddl);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.ViewLbl);
            this.Controls.Add(this.ViewDDL);
            this.Name = "RawDataForm";
            this.Text = "RawDataForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ViewDDL;
        private System.Windows.Forms.Label ViewLbl;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ComboBox Delimiter1Ddl;
        private System.Windows.Forms.Label Delimiter1Lbl;
        private System.Windows.Forms.ComboBox Delimiter2Ddl;
        private System.Windows.Forms.Label Delimiter2Lbl;
    }
}