namespace XLForms
{
    partial class ClientForm
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
            this.ClientListBox = new System.Windows.Forms.ListBox();
            this.IncLostCheck = new System.Windows.Forms.CheckBox();
            this.SearchTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.RecentClientListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ClientListBox
            // 
            this.ClientListBox.FormattingEnabled = true;
            this.ClientListBox.Location = new System.Drawing.Point(12, 71);
            this.ClientListBox.Name = "ClientListBox";
            this.ClientListBox.Size = new System.Drawing.Size(279, 238);
            this.ClientListBox.TabIndex = 43;
            this.ClientListBox.SelectedIndexChanged += new System.EventHandler(this.ClientListBox_SelectedIndexChanged);
            // 
            // IncLostCheck
            // 
            this.IncLostCheck.AutoSize = true;
            this.IncLostCheck.Location = new System.Drawing.Point(383, 16);
            this.IncLostCheck.Name = "IncLostCheck";
            this.IncLostCheck.Size = new System.Drawing.Size(84, 17);
            this.IncLostCheck.TabIndex = 42;
            this.IncLostCheck.Text = "Include Lost";
            this.IncLostCheck.UseVisualStyleBackColor = true;
            // 
            // SearchTB
            // 
            this.SearchTB.Location = new System.Drawing.Point(53, 16);
            this.SearchTB.Name = "SearchTB";
            this.SearchTB.Size = new System.Drawing.Size(324, 20);
            this.SearchTB.TabIndex = 39;
            this.SearchTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTB_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Client";
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(53, 42);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(75, 23);
            this.SearchBtn.TabIndex = 41;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click_1);
            // 
            // RecentClientListBox
            // 
            this.RecentClientListBox.FormattingEnabled = true;
            this.RecentClientListBox.Location = new System.Drawing.Point(310, 71);
            this.RecentClientListBox.Name = "RecentClientListBox";
            this.RecentClientListBox.Size = new System.Drawing.Size(279, 238);
            this.RecentClientListBox.TabIndex = 44;
            this.RecentClientListBox.SelectedIndexChanged += new System.EventHandler(this.RecentClientListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(397, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "No Recent Clients";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 400);
            this.Controls.Add(this.RecentClientListBox);
            this.Controls.Add(this.ClientListBox);
            this.Controls.Add(this.IncLostCheck);
            this.Controls.Add(this.SearchTB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.label2);
            this.Name = "ClientForm";
            this.Text = "EntityForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTB_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ClientListBox;
        private System.Windows.Forms.CheckBox IncLostCheck;
        private System.Windows.Forms.TextBox SearchTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.ListBox RecentClientListBox;
        private System.Windows.Forms.Label label2;
    }
}