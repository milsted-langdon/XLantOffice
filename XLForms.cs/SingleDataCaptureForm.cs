using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XLForms
{
    public partial class SingleDataCaptureForm : Form
    {
        public DialogResult result = DialogResult.Cancel;
        public string data;
        
        public SingleDataCaptureForm(string title, string label, string prompt)
        {
            InitializeComponent();
            this.CenterToParent();
            this.Text = title;
            DataLabel.Text = label;
            DataTextBox.Text = prompt;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            data = DataTextBox.Text;
            result = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }



    }
}
