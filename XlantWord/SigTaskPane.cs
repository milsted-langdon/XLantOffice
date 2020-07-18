using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using XLantCore;

namespace XlantWord
{
    public partial class SigTaskPane : UserControl
    {
        public SigTaskPane()
        {
            InitializeComponent();
            //Document currentDoc = Globals.ThisAddIn.Application.ActiveDocument;
            PaneRefresh();
        }

        private void PaneRefresh()
        {
            System.Data.DataTable t = new System.Data.DataTable();
            t.Columns.Add("User");
            t.Columns.Add("Grade");
            t.Columns.Add("Date");
            t.Columns.Add("Signature");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Width = 325;

            for (int i = 0; i < 10; i++)
            {
                //add to grid
                
                string str = XLDocument.ReadParameter("Sign" + i.ToString());
                if (!String.IsNullOrEmpty(str))
                {
                    DataRow r = t.NewRow();
                    string[] sArray = str.Split(new Char[] {';'});
                    r["User"] = sArray[0];
                    r["Grade"] = sArray[1];
                    r["Date"] = sArray[2];
                    //handle old 3 item entries
                    if (sArray.Length > 3)
                    {
                        r["Signature"] = sArray[3]; 
                    }
                    else
                    {
                        r["Signature"] = "";
                    }
                    
                    t.Rows.Add(r);
                    dataGridView1.DataSource = t;
                }
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            PaneRefresh();
        }

    }
}
