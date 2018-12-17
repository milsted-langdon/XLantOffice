namespace XlantWord
{
    partial class PrintForm
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
            this.PrinterDDL = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PaperDDL = new System.Windows.Forms.ComboBox();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.AdvPrintBtn = new System.Windows.Forms.Button();
            this.PageSetup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose Printer:";
            // 
            // PrinterDDL
            // 
            this.PrinterDDL.FormattingEnabled = true;
            this.PrinterDDL.Location = new System.Drawing.Point(126, 10);
            this.PrinterDDL.Name = "PrinterDDL";
            this.PrinterDDL.Size = new System.Drawing.Size(236, 21);
            this.PrinterDDL.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select Paper Type:";
            // 
            // PaperDDL
            // 
            this.PaperDDL.FormattingEnabled = true;
            this.PaperDDL.Items.AddRange(new object[] {
            "Plain",
            "Headed & Continuation",
            "Continuation"});
            this.PaperDDL.Location = new System.Drawing.Point(126, 66);
            this.PaperDDL.Name = "PaperDDL";
            this.PaperDDL.Size = new System.Drawing.Size(236, 21);
            this.PaperDDL.TabIndex = 3;
            this.PaperDDL.SelectedIndexChanged += new System.EventHandler(this.PaperDDL_SelectedIndexChanged);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(287, 114);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(75, 23);
            this.PrintBtn.TabIndex = 4;
            this.PrintBtn.Text = "Quick Print";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // AdvPrintBtn
            // 
            this.AdvPrintBtn.Location = new System.Drawing.Point(19, 114);
            this.AdvPrintBtn.Name = "AdvPrintBtn";
            this.AdvPrintBtn.Size = new System.Drawing.Size(75, 23);
            this.AdvPrintBtn.TabIndex = 5;
            this.AdvPrintBtn.Text = "Advanced...";
            this.AdvPrintBtn.UseVisualStyleBackColor = true;
            this.AdvPrintBtn.Click += new System.EventHandler(this.AdvPrintBtn_Click);
            // 
            // PageSetup
            // 
            this.PageSetup.Location = new System.Drawing.Point(126, 114);
            this.PageSetup.Name = "PageSetup";
            this.PageSetup.Size = new System.Drawing.Size(122, 23);
            this.PageSetup.TabIndex = 6;
            this.PageSetup.Text = "Page Setup";
            this.PageSetup.UseVisualStyleBackColor = true;
            this.PageSetup.Click += new System.EventHandler(this.PageSetup_Click);
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 150);
            this.Controls.Add(this.AdvPrintBtn);
            this.Controls.Add(this.PageSetup);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.PaperDDL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PrinterDDL);
            this.Controls.Add(this.label1);
            this.Name = "PrintForm";
            this.Text = "Print";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox PrinterDDL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox PaperDDL;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button AdvPrintBtn;
        private System.Windows.Forms.Button PageSetup;
    }
}