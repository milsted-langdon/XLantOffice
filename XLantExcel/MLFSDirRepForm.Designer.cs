namespace XLantExcel
{
    partial class MLFSDirRepForm
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
            this.FeesFileTb = new System.Windows.Forms.TextBox();
            this.ChooseFeeFileBtn = new System.Windows.Forms.Button();
            this.ChoosePlansFileBtn = new System.Windows.Forms.Button();
            this.PlansFileTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DateTb = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.InitialTb = new System.Windows.Forms.TextBox();
            this.TrailTargetTb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fees File";
            // 
            // FeesFileTb
            // 
            this.FeesFileTb.Location = new System.Drawing.Point(135, 56);
            this.FeesFileTb.Name = "FeesFileTb";
            this.FeesFileTb.ReadOnly = true;
            this.FeesFileTb.Size = new System.Drawing.Size(315, 20);
            this.FeesFileTb.TabIndex = 1;
            // 
            // ChooseFeeFileBtn
            // 
            this.ChooseFeeFileBtn.Location = new System.Drawing.Point(474, 54);
            this.ChooseFeeFileBtn.Name = "ChooseFeeFileBtn";
            this.ChooseFeeFileBtn.Size = new System.Drawing.Size(102, 23);
            this.ChooseFeeFileBtn.TabIndex = 2;
            this.ChooseFeeFileBtn.Text = "Choose File...";
            this.ChooseFeeFileBtn.UseVisualStyleBackColor = true;
            this.ChooseFeeFileBtn.Click += new System.EventHandler(this.ChooseFeeFileBtn_Click);
            // 
            // ChoosePlansFileBtn
            // 
            this.ChoosePlansFileBtn.Location = new System.Drawing.Point(474, 98);
            this.ChoosePlansFileBtn.Name = "ChoosePlansFileBtn";
            this.ChoosePlansFileBtn.Size = new System.Drawing.Size(102, 23);
            this.ChoosePlansFileBtn.TabIndex = 5;
            this.ChoosePlansFileBtn.Text = "Choose File...";
            this.ChoosePlansFileBtn.UseVisualStyleBackColor = true;
            this.ChoosePlansFileBtn.Click += new System.EventHandler(this.ChoosePlansFileBtn_Click);
            // 
            // PlansFileTb
            // 
            this.PlansFileTb.Location = new System.Drawing.Point(135, 100);
            this.PlansFileTb.Name = "PlansFileTb";
            this.PlansFileTb.ReadOnly = true;
            this.PlansFileTb.Size = new System.Drawing.Size(315, 20);
            this.PlansFileTb.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Plans File";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Period End Date";
            // 
            // DateTb
            // 
            this.DateTb.Location = new System.Drawing.Point(135, 141);
            this.DateTb.Name = "DateTb";
            this.DateTb.Size = new System.Drawing.Size(315, 20);
            this.DateTb.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Initial Fee Target";
            // 
            // InitialTb
            // 
            this.InitialTb.Location = new System.Drawing.Point(135, 185);
            this.InitialTb.Name = "InitialTb";
            this.InitialTb.Size = new System.Drawing.Size(315, 20);
            this.InitialTb.TabIndex = 9;
            this.InitialTb.Text = "450";
            this.InitialTb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TrailTargetTb
            // 
            this.TrailTargetTb.Location = new System.Drawing.Point(135, 226);
            this.TrailTargetTb.Name = "TrailTargetTb";
            this.TrailTargetTb.Size = new System.Drawing.Size(315, 20);
            this.TrailTargetTb.TabIndex = 11;
            this.TrailTargetTb.Text = "500";
            this.TrailTargetTb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ongoing Fee Target";
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(546, 282);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 12;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(465, 282);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 13;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // MLFSDirRepForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 317);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.TrailTargetTb);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.InitialTb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DateTb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ChoosePlansFileBtn);
            this.Controls.Add(this.PlansFileTb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ChooseFeeFileBtn);
            this.Controls.Add(this.FeesFileTb);
            this.Controls.Add(this.label1);
            this.Name = "MLFSDirRepForm";
            this.Text = "Directors\' Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FeesFileTb;
        private System.Windows.Forms.Button ChooseFeeFileBtn;
        private System.Windows.Forms.Button ChoosePlansFileBtn;
        private System.Windows.Forms.TextBox PlansFileTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DateTb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox InitialTb;
        private System.Windows.Forms.TextBox TrailTargetTb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}