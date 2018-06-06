namespace XlantWord
{
    partial class ForwardForm
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
            this.ToBeActionDDL = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IndexBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ToBeActionDDL
            // 
            this.ToBeActionDDL.FormattingEnabled = true;
            this.ToBeActionDDL.Location = new System.Drawing.Point(134, 17);
            this.ToBeActionDDL.Name = "ToBeActionDDL";
            this.ToBeActionDDL.Size = new System.Drawing.Size(195, 21);
            this.ToBeActionDDL.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To Be Actioned by";
            // 
            // IndexBtn
            // 
            this.IndexBtn.Location = new System.Drawing.Point(185, 92);
            this.IndexBtn.Name = "IndexBtn";
            this.IndexBtn.Size = new System.Drawing.Size(75, 23);
            this.IndexBtn.TabIndex = 9;
            this.IndexBtn.Text = "OK";
            this.IndexBtn.UseVisualStyleBackColor = true;
            this.IndexBtn.Click += new System.EventHandler(this.IndexBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(266, 92);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // Forward
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 127);
            this.Controls.Add(this.IndexBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ToBeActionDDL);
            this.Controls.Add(this.label2);
            this.Name = "Forward";
            this.Text = "Forward";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ToBeActionDDL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button IndexBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}