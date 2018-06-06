namespace XLForms
{
    partial class VCForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FileSectionlbl = new System.Windows.Forms.Label();
            this.DescTB = new System.Windows.Forms.TextBox();
            this.ToBeActionDDL = new System.Windows.Forms.ComboBox();
            this.FileSectionDDL = new System.Windows.Forms.ComboBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.IndexBtn = new System.Windows.Forms.Button();
            this.ListDDL1 = new System.Windows.Forms.ComboBox();
            this.Listlabel1 = new System.Windows.Forms.Label();
            this.ListDDL2 = new System.Windows.Forms.ComboBox();
            this.ListLabel2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "To Be Actioned by";
            // 
            // FileSectionlbl
            // 
            this.FileSectionlbl.AutoSize = true;
            this.FileSectionlbl.Location = new System.Drawing.Point(25, 107);
            this.FileSectionlbl.Name = "FileSectionlbl";
            this.FileSectionlbl.Size = new System.Drawing.Size(62, 13);
            this.FileSectionlbl.TabIndex = 2;
            this.FileSectionlbl.Text = "File Section";
            // 
            // DescTB
            // 
            this.DescTB.Location = new System.Drawing.Point(147, 23);
            this.DescTB.Name = "DescTB";
            this.DescTB.Size = new System.Drawing.Size(279, 20);
            this.DescTB.TabIndex = 0;
            // 
            // ToBeActionDDL
            // 
            this.ToBeActionDDL.FormattingEnabled = true;
            this.ToBeActionDDL.Location = new System.Drawing.Point(147, 63);
            this.ToBeActionDDL.Name = "ToBeActionDDL";
            this.ToBeActionDDL.Size = new System.Drawing.Size(195, 21);
            this.ToBeActionDDL.TabIndex = 1;
            // 
            // FileSectionDDL
            // 
            this.FileSectionDDL.FormattingEnabled = true;
            this.FileSectionDDL.Location = new System.Drawing.Point(147, 104);
            this.FileSectionDDL.Name = "FileSectionDDL";
            this.FileSectionDDL.Size = new System.Drawing.Size(195, 21);
            this.FileSectionDDL.TabIndex = 2;
            this.FileSectionDDL.SelectedIndexChanged += new System.EventHandler(this.FileSectionDDL_SelectedIndexChanged);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(364, 224);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // IndexBtn
            // 
            this.IndexBtn.Location = new System.Drawing.Point(283, 224);
            this.IndexBtn.Name = "IndexBtn";
            this.IndexBtn.Size = new System.Drawing.Size(75, 23);
            this.IndexBtn.TabIndex = 5;
            this.IndexBtn.Text = "Index";
            this.IndexBtn.UseVisualStyleBackColor = true;
            this.IndexBtn.Click += new System.EventHandler(this.IndexBtn_Click);
            // 
            // ListDDL1
            // 
            this.ListDDL1.FormattingEnabled = true;
            this.ListDDL1.Location = new System.Drawing.Point(147, 144);
            this.ListDDL1.Name = "ListDDL1";
            this.ListDDL1.Size = new System.Drawing.Size(195, 21);
            this.ListDDL1.TabIndex = 3;
            this.ListDDL1.Visible = false;
            // 
            // Listlabel1
            // 
            this.Listlabel1.AutoSize = true;
            this.Listlabel1.Location = new System.Drawing.Point(25, 147);
            this.Listlabel1.Name = "Listlabel1";
            this.Listlabel1.Size = new System.Drawing.Size(0, 13);
            this.Listlabel1.TabIndex = 8;
            this.Listlabel1.Visible = false;
            // 
            // ListDDL2
            // 
            this.ListDDL2.FormattingEnabled = true;
            this.ListDDL2.Location = new System.Drawing.Point(147, 186);
            this.ListDDL2.Name = "ListDDL2";
            this.ListDDL2.Size = new System.Drawing.Size(195, 21);
            this.ListDDL2.TabIndex = 4;
            this.ListDDL2.Visible = false;
            // 
            // ListLabel2
            // 
            this.ListLabel2.AutoSize = true;
            this.ListLabel2.Location = new System.Drawing.Point(25, 189);
            this.ListLabel2.Name = "ListLabel2";
            this.ListLabel2.Size = new System.Drawing.Size(0, 13);
            this.ListLabel2.TabIndex = 10;
            this.ListLabel2.Visible = false;
            // 
            // VCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 258);
            this.Controls.Add(this.ListDDL2);
            this.Controls.Add(this.ListLabel2);
            this.Controls.Add(this.ListDDL1);
            this.Controls.Add(this.Listlabel1);
            this.Controls.Add(this.IndexBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.FileSectionDDL);
            this.Controls.Add(this.ToBeActionDDL);
            this.Controls.Add(this.DescTB);
            this.Controls.Add(this.FileSectionlbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "VCForm";
            this.Text = "Confirm Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label FileSectionlbl;
        private System.Windows.Forms.TextBox DescTB;
        private System.Windows.Forms.ComboBox ToBeActionDDL;
        private System.Windows.Forms.ComboBox FileSectionDDL;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button IndexBtn;
        private System.Windows.Forms.ComboBox ListDDL1;
        private System.Windows.Forms.Label Listlabel1;
        private System.Windows.Forms.ComboBox ListDDL2;
        private System.Windows.Forms.Label ListLabel2;
    }
}