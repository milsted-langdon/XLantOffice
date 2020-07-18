using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using XLantCore;

namespace XLForms
{
    public partial class VCForm : Form
    {
        public XLVirtualCabinet.BondResult outcome = new XLVirtualCabinet.BondResult();
        private XLMain.Client client = new XLMain.Client();
        private string docPath = null;
        private string status = null;
        private string docDate = DateTime.Now.ToString("dd/mm/yyyy");
        XLVirtualCabinet.IndexList list1 = new XLVirtualCabinet.IndexList();
        XLVirtualCabinet.IndexList list2 = new XLVirtualCabinet.IndexList();

        public VCForm(XLMain.Staff writer, XLMain.Client clientPass, string docPathPass, string desc = "", string statusPass = "Draft", string docDatePass = null, string todo = "user", List<XLMain.EntityCouplet> staffList=null)
        {
            InitializeComponent();
            this.CenterToParent();
            client = clientPass;
            docPath = docPathPass;
            status = statusPass;
            docDate = docDatePass ?? DateTime.Now.ToString("dd/MM/yyyy");
            XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);

            //Set the list of sections for the cabinet
            List<string> sections = XLVirtualCabinet.SectionValues(client.manager.office, client.department);

            if (sections != null)
            {
                FileSectionDDL.DataSource = sections;
            }
            //if one of the items is correspondence use it otherwise just pick number one.
            try
            {
                FileSectionDDL.SelectedItem = "Correspondence";
            }
            catch
            {
                FileSectionDDL.SelectedIndex = 1;
            }
            
            DescTB.Text = desc;   

            //populate the sender
            List<XLMain.EntityCouplet> users = XLtools.StaffList(user, client, true, userList: staffList);
            if (todo == "user")
            {
                if (writer != null)
                {
                    ToBeActionDDL.DataSource = users;
                    ToBeActionDDL.DisplayMember = "name";
                    ToBeActionDDL.ValueMember = "crmID";
                    ToBeActionDDL.SelectedItem = writer.crmID;
                }
            }
            else
            {
                XLMain.EntityCouplet blank = new XLMain.EntityCouplet();
                blank.crmID= "";
                blank.name="";
                users.Insert(0,blank);
                ToBeActionDDL.DataSource = users;
                ToBeActionDDL.DisplayMember = "name";
                ToBeActionDDL.ValueMember = "crmID";
                ToBeActionDDL.SelectedItem = blank;
            }

            //Check for any sub-section

        }

        private void IndexBtn_Click(object sender, EventArgs e)
        {

            string fullPath = docPath;
            string cabinet = XLVirtualCabinet.FileStore(client.manager.office, client.department);
            string clientStr = client.clientcode + " - " + client.name;
            string toBeActionedBy = null;
            if (ToBeActionDDL.SelectedValue == null)
            {
                toBeActionedBy = "";
            }
            else
            {
                XLMain.EntityCouplet toBe = (XLMain.EntityCouplet)ToBeActionDDL.SelectedItem;
                toBeActionedBy = toBe.name;
            }
            //collect information from the two optional ddls use whether they are visible or not to acsertain whether 
            //to include them - They may not be visible but still hold data.
            XLVirtualCabinet.IndexPair list1Data = new XLVirtualCabinet.IndexPair();
            XLVirtualCabinet.IndexPair list2Data = new XLVirtualCabinet.IndexPair();
            if (ListDDL1.Visible)
            {
                list1Data.index = list1.index;
                list1Data.value = ListDDL1.Text;

            }
            if (ListDDL2.Visible)
            {
                list2Data.index = list2.index;
                list2Data.value = ListDDL2.Text;
            }

            string desc = DescTB.Text;
            string section = "";
            section = FileSectionDDL.SelectedItem.ToString();
            //Launch the index process and collect the result
            outcome = XLVirtualCabinet.IndexDocument(fullPath, cabinet, clientStr, status, toBeActionedBy, section, desc, docDate, list1Data, list2Data);
            
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FileSectionDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Check to see whether there are lists attributed to the selected option
            
            List<XLVirtualCabinet.IndexList> lists = new List<XLVirtualCabinet.IndexList>();
            lists = XLVirtualCabinet.SectionLists(client.office, client.department, FileSectionDDL.SelectedItem.ToString());

            //only allowing up to two other fields
            if (lists != null)
            {
                if (lists.Count > 0)
                {
                    list1 = lists[0];
                    Listlabel1.Text = list1.name;
                    ListDDL1.DataSource = list1.items;
                    ListDDL1.SelectedItem = null;

                    Listlabel1.Visible = true;
                    ListDDL1.Visible = true;

                    if (lists.Count > 1)
                    {
                        list2 = lists[1];
                        ListLabel2.Text = list2.name;
                        ListDDL2.DataSource = list2.items;

                        ListLabel2.Visible = true;
                        ListDDL2.Visible = true;
                    }
                    else
                    {
                        ListLabel2.Visible = false;
                        ListDDL2.Visible = false;
                    }
                    //Any other detail is not handled - We could have iterated through them but more than two would be a mistake so no need to make it dynamic.
                }
                else
                {
                    Listlabel1.Visible = false;
                    ListDDL1.Visible = false;
                    ListLabel2.Visible = false;
                    ListDDL2.Visible = false;
                }
            }
            else
            {
                Listlabel1.Visible = false;
                ListDDL1.Visible = false;
                ListLabel2.Visible = false;
                ListDDL2.Visible = false;
            }

        }
    }
}
