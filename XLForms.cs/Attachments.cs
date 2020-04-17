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
    public partial class Attachments : Form
    {
        public List<Tuple<string, string>> selectedDocuments = new List<Tuple<string, string>>();
        public List<Tuple<string, string>> documents = new List<Tuple<string, string>>();
        public Attachments(List<Tuple<string,string>> documentsProvided)
        {
            InitializeComponent();
            documents = documentsProvided;
            foreach(Tuple<string, string> doc in documents)
            {
                documentListBox.Items.Add(doc.Item1, false);
            }
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            foreach(string selected in documentListBox.CheckedItems)
            {
                selectedDocuments.AddRange(documents.Where(x => x.Item1 == selected).ToList());
            }
            this.Close();
        }

        private void AddFileBtn_Click(object sender, EventArgs e)
        {
            string filename = "";

            //Open the dialog and find the file
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.ShowReadOnly = true;
            fDialog.Filter = "PDF files|*.pdf";
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                filename = fDialog.FileName;
                //add to the document list and the list box
                documents.Add(new Tuple<string, string>(filename, filename));
                documentListBox.Items.Add(filename, true);
            }
        }
    }
}
