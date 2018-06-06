namespace XLForms
{
    partial class DescForm
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
            this.DescTB = new System.Windows.Forms.TextBox();
            this.OkBtn = new System.Windows.Forms.Button();
            this.SenderDDL = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Description:";
            // 
            // DescTB
            // 
            this.DescTB.Location = new System.Drawing.Point(110, 25);
            this.DescTB.Name = "DescTB";
            this.DescTB.Size = new System.Drawing.Size(293, 20);
            this.DescTB.TabIndex = 1;
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(344, 106);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "Submit";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // SenderDDL
            // 
            this.SenderDDL.FormattingEnabled = true;
            this.SenderDDL.Location = new System.Drawing.Point(110, 71);
            this.SenderDDL.Name = "SenderDDL";
            this.SenderDDL.Size = new System.Drawing.Size(215, 21);
            this.SenderDDL.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Confirm recipiant";
            // 
            // DescForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 141);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SenderDDL);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.DescTB);
            this.Controls.Add(this.label1);
            this.Name = "DescForm";
            this.Text = "Confirm Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DescTB;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.ComboBox SenderDDL;
        private System.Windows.Forms.Label label2;
    }
}