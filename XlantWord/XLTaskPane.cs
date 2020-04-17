using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using XLant;
using XLForms;

namespace XlantWord
{
    public partial class XLTaskPane : UserControl
    {
        public static string clientId;
        public static List<XLMain.Address> allAddresses = new List<XLMain.Address>();
        public static XLMain.Staff user;
        public static XLMain.Client client = new XLMain.Client();
        public static XLMain.Staff sender = new XLMain.Staff();
        public static XLMain.Salutation sal;
        public static XLMain.Address add;
        public static List<XLMain.EntityCouplet> users = new List<XLMain.EntityCouplet>();
        public string docType;

        public XLTaskPane()
        {
            try
            {
                InitializeComponent();
                string str = XLDocument.ReadParameter("CRMid");
                docType = XLDocument.ReadParameter("DocType");

                client = XLMain.Client.FetchClient(str);
                //ClientIDLabel.Text = client.crmID;

                user = XLMain.Staff.StaffFromUser(Environment.UserName);
                users = XLtools.StaffList(user, client, false, true);

                SenderDDL.DataSource = users;
                SenderDDL.DisplayMember = "name";
                SenderDDL.ValueMember = "crmID";
                SenderDDL.SelectedItem = user;
                sender = user;
                XLDocument.UpdateParameter("Sender", sender.username);

                if (client != null)
                {
                    //set basic client info
                    string clientStr = client.clientcode + " - " + client.name;
                    ClientLbl.Text = clientStr;
                    SubjectTB.Text = client.name;
                    string fileStore = XLVirtualCabinet.FileStore(client.manager.office, client.department);
                    XLDocument.UpdateParameter("Cabinet", fileStore);
                    XLDocument.UpdateParameter("ClientStr", clientStr);

                    //Deal with salutations
                    if (client.salutations != null)
                    {
                        SalDDL.DataSource = client.salutations;
                        SalDDL.DisplayMember = "Salutation";
                        SalDDL.ValueMember = "Addressee";
                        sal = (XLMain.Salutation)SalDDL.SelectedItem;
                        if (sal != null)
                        {
                            AddresseeTB.Text = sal.addressee;
                            SalutationTb.Text = sal.salutation;
                        }
                    }

                    //Add the appropriate header
                    XLDocument.Header clientHeader = XLDocument.MapHeader(client.office, client.department);


                    //Set up depending on whether it is insolvency or not
                    if (client.department == "INS")
                    {
                        //Deal with when calling ddl
                        WhencallingDDL.DataSource = users;
                        WhencallingDDL.DisplayMember = "name";
                        WhencallingDDL.ValueMember = "crmID";
                        WhenCallingCheck.Checked = true;
                        SubjectTB.Text = XLInsol.GetSubject(client.crmID);
                    }
                    else
                    {
                        FAOBCheck.Checked = true;
                        PandCCheck.Checked = true;
                        WhenCallingCheck.Checked = false;
                    }

                    //Deal with addresses or fax no as appropriate
                    if (docType == "Letter")
                    {
                        if (client.addresses != null)
                        {
                            allAddresses = client.addresses;
                            addressesDDL.DataSource = allAddresses;
                            addressesDDL.DisplayMember = "address1";
                            addressesDDL.ValueMember = "addressBlock";
                            add = (XLMain.Address)addressesDDL.SelectedItem;
                            if (add != null)
                            {
                                if (client.IsIndividual)
                                {
                                    addTB.Text = add.addressBlock;
                                }
                                else
                                {
                                    addTB.Text = client.name + Environment.NewLine + add.addressBlock;
                                }
                            }
                        }
                        if (clientHeader != null)
                        {
                            try
                            {
                                XLDocument.DeployHeader(clientHeader);
                            }
                            catch
                            {
                                MessageBox.Show("Unable to map header automatically." + Environment.NewLine + "Please run the Header/Footer menu.");
                            }
                        }
                    }
                    else if (docType == "Fax")
                    {
                        XLMain.Number fax = XLMain.Number.GetNumber(client.crmID, "Fax");
                        if (fax != null)
                        {
                            FaxTB.Text = fax.number;
                        }
                        if (clientHeader != null)
                        {
                            try
                            {
                                XLDocument.DeployHeader(clientHeader);
                            }
                            catch
                            {
                                MessageBox.Show("Unable to map header automatically." + Environment.NewLine + "Please run the Header/Footer menu.");
                            }
                        }
                    }

                    //set status
                    XLDocument.ChangeStatus("Draft");

                    //Alter fields dependant on doc type;
                    SetVisibility(docType);

                    if (client.department == "INS")
                    {
                        WhencallingDDL.Visible = true;
                        IPSContactBtn.Visible = true;
                    }

                    //Update bookmarks dependant on doc type
                    UpdateBookmarks(docType);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open taskpane");
                XLtools.LogException("TaskPane", ex.ToString());
            }
        }

        private void UpdateBookmarks(string docType)
        {
            try
            {
                //Build reference
                string ourRef = OurRef(client, sender, user);
                if (docType == "Letter")
                {
                    XLDocument.UpdateBookmark("Address", addTB.Text);
                    XLDocument.UpdateBookmark("Salutation", SalutationTb.Text);
                    XLDocument.UpdateBookmark("Subject", SubjectTB.Text);
                    XLDocument.UpdateBookmark("Date", DateTB.Value.Date.ToString("d MMMM yyyy"));
                    XLDocument.UpdateBookmark("Date2", DateTB.Value.Date.ToString("d MMMM yyyy"));
                    XLDocument.UpdateBookmark("Addressee", AddresseeTB.Text);
                    XLDocument.UpdateBookmark("Addressee2", AddresseeTB.Text);
                    DeploySignature(client, sender, AddresseeTB.Text);
                    XLDocument.UpdateBookmark("OurRef", ourRef, 0);
                }
                else if (docType == "Fax")
                {
                    XLDocument.UpdateBookmark("Subject", SubjectTB.Text);
                    XLDocument.UpdateBookmark("Client", client.name);
                    XLDocument.UpdateBookmark("Date", DateTB.Value.Date.ToString("d MMMM yyyy"));
                    XLDocument.UpdateBookmark("Date2", DateTB.Value.Date.ToString("d MMMM yyyy"));
                    XLDocument.UpdateBookmark("Addressee", AddresseeTB.Text);
                    XLDocument.UpdateBookmark("Addressee2", AddresseeTB.Text);
                    DeploySignature(client, sender, AddresseeTB.Text);
                    XLDocument.UpdateBookmark("Sender2", sender.name);
                    XLDocument.UpdateBookmark("Fax", FaxTB.Text);
                    XLDocument.UpdateBookmark("OurRef", ourRef, 0);
                }
                else if (docType == "FileNote")
                {
                    XLDocument.UpdateBookmark("Subject", SubjectTB.Text, 1);
                    XLDocument.UpdateBookmark("Date", DateTB.Value.Date.ToString("d MMMM yyyy"));
                    XLDocument.UpdateBookmark("Date2", DateTB.Value.Date.ToString("d MMMM yyyy"));
                    XLDocument.UpdateBookmark("Sender", sender.name, 1);
                    XLDocument.UpdateBookmark("ClientStr", XLDocument.ReadParameter("ClientStr"));
                    XLDocument.UpdateBookmark("OurRef", ourRef, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update bookmarks");
                XLtools.LogException("TaskPane-UpdateBookmarks", ex.ToString());
            }
        }

        private void SalutationTb_Leave(object sender, EventArgs e)
        {
            string salutation = SalutationTb.Text;
            XLDocument.UpdateBookmark("Salutation", salutation);
            if (salutation == "" || salutation == "Sir" || salutation == "Madam" || salutation == "Sirs" || salutation == "Mesdames")
            {
                XLDocument.UpdateBookmark("FaithSincere", "Yours faithfully");
            }
            else
            {
                XLDocument.UpdateBookmark("FaithSincere", "Yours sincerely");
            }
        }

        private void addTB_Leave(object sender, EventArgs e)
        {
            XLDocument.UpdateBookmark("Address", addTB.Text);
        }

        private void SubjectTB_Leave(object sender, EventArgs e)
        {
            XLDocument.UpdateBookmark("Subject", SubjectTB.Text);
        }

        private void DateTB_Leave(object sender, EventArgs e)
        {
            XLDocument.UpdateBookmark("Date", DateTB.Value.Date.ToString("d MMMM yyyy"));
        }

        private void PandCCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (PandCCheck.Checked)
            {
                XLDocument.UpdateBookmark("PandC", "Private and Confidential");
            }
            else
            {
                XLDocument.UpdateBookmark("PandC", "");
            }
        }

        private void FAOBCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (FAOBCheck.Checked)
            {
                XLDocument.UpdateBookmark("FAOBOML", "Milsted Langdon");
            }
            else
            {
                XLDocument.UpdateBookmark("FAOBOML", "");
            }
        }

        private void GetAddressBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ContForm myForm = new ContForm();
                myForm.ShowDialog();
                XLMain.Contact selectContact = myForm.selectedContact;
                if (selectContact != null)
                {
                    if (docType == "Letter")
                    {
                        if (selectContact.addresses != null)
                        {
                            allAddresses = selectContact.addresses;
                            addressesDDL.DataSource = allAddresses;
                            addressesDDL.DisplayMember = "address1";
                            addressesDDL.ValueMember = "addressBlock";
                            add = (XLMain.Address)addressesDDL.SelectedItem;
                            if (add != null)
                            {
                                addTB.Text = add.addressBlock;
                            }
                        }
                    }
                    else if (docType == "Fax")
                    {
                        XLMain.Number fax = XLMain.Number.GetNumber(client.crmID, "Fax");
                        if (fax != null)
                        {
                            FaxTB.Text = fax.number;
                        }
                    }

                    //Deal with salutations
                    if (selectContact.salutations != null)
                    {
                        SalDDL.DataSource = selectContact.salutations;
                        SalDDL.DisplayMember = "Salutation";
                        SalDDL.ValueMember = "Addressee";
                        sal = (XLMain.Salutation)SalDDL.SelectedItem;
                        if (sal != null)
                        {
                            AddresseeTB.Text = sal.addressee;
                            SalutationTb.Text = sal.salutation;
                            SalutationTb_Leave(this, null);
                        }
                    }
                    else
                    {
                        SalDDL.DataSource = null;
                        SalDDL.DisplayMember = "Salutation";
                        SalDDL.ValueMember = "Addressee";
                        AddresseeTB.Text = "";
                        SalutationTb.Text = "";
                    }
                    RevertBtn.Visible = true;
                    XLDocument.UpdateBookmark("Salutation", SalutationTb.Text);
                    XLDocument.UpdateBookmark("Addressee", AddresseeTB.Text);
                    XLDocument.UpdateBookmark("Addressee2", AddresseeTB.Text);
                    XLDocument.UpdateBookmark("Address", addTB.Text);
                    XLDocument.UpdateBookmark("Fax", FaxTB.Text);
                }
                else
                {
                    MessageBox.Show("No address returned");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to fetch contact");
                XLtools.LogException("TaskPane-ContactBtn", ex.ToString());
            }
        }

        private void AddresseeTB_Leave(object sender, EventArgs e)
        {
            string addressee = AddresseeTB.Text;
            XLDocument.UpdateBookmark("Addressee", addressee);
            XLDocument.UpdateBookmark("Addressee2", addressee);

        }

        private void SalDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            sal = (XLMain.Salutation)SalDDL.SelectedItem;
            AddresseeTB.Text = sal.addressee;
            SalutationTb.Text = sal.salutation;
            XLDocument.UpdateBookmark("Salutation", SalutationTb.Text);
            XLDocument.UpdateBookmark("Addressee", AddresseeTB.Text);
            XLDocument.UpdateBookmark("Addressee2", AddresseeTB.Text);
        }

        private void addressLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string str = addressesDDL.SelectedValue.ToString();
                addTB.Text = str;
            }
            catch
            {
                addTB.Text = "";
            }
            XLDocument.UpdateBookmark("Address", addTB.Text);
        }

        private void SenderDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Collect the changed record
                XLMain.EntityCouplet tempSend = (XLMain.EntityCouplet)SenderDDL.SelectedItem;
                //Convert the entity couplet to a full blown user now we know which one we want
                if (tempSend.crmID != "")
                {
                    XLMain.Staff send = XLMain.Staff.FetchStaff(tempSend.crmID);
                    DeploySignature(client, send, AddresseeTB.Text);
                    XLDocument.UpdateParameter("Sender", send.username);

                    //Build reference
                    string ourRef = OurRef(client, send, user);
                    XLDocument.UpdateBookmark("OurRef", ourRef, 0);
                }
                else
                {
                    XLMain.Staff send = new XLMain.Staff();
                    send.name = tempSend.name;
                    DeploySignature(client, send, AddresseeTB.Text);
                    XLDocument.UpdateParameter("Sender", "");

                    //Build reference
                    string ourRef = OurRef(client, send, user);
                    XLDocument.UpdateBookmark("OurRef", ourRef, 0);

                    //For Milsted Langdon remove for and on behalf
                    if (tempSend.name == "Milsted Langdon LLP")
                    {
                        if (FAOBCheck.Checked)
                        {
                            FAOBCheck.Checked = false;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to change sender");
                XLtools.LogException("TaskPane-SenderDDL", ex.ToString());
            }
        }

        private void RevertBtn_Click(object sender, EventArgs e)
        {

            try
            {
                //Deal with salutations
                if (client.salutations != null)
                {
                    SalDDL.DataSource = client.salutations;
                    SalDDL.DisplayMember = "Salutation";
                    SalDDL.ValueMember = "Addressee";
                    sal = (XLMain.Salutation)SalDDL.SelectedItem;
                    if (sal != null)
                    {
                        AddresseeTB.Text = sal.addressee;
                        SalutationTb.Text = sal.salutation;
                    }
                }
                //Deal with addresses
                if (docType == "Letter")
                {
                    if (client.addresses != null)
                    {
                        allAddresses = client.addresses;
                        addressesDDL.DataSource = allAddresses;
                        addressesDDL.DisplayMember = "address1";
                        addressesDDL.ValueMember = "addressBlock";
                        add = (XLMain.Address)addressesDDL.SelectedItem;
                        if (add != null)
                        {
                            addTB.Text = add.addressBlock;
                        }
                    }
                }
                else if (docType == "Fax")
                {
                    XLMain.Number fax = XLMain.Number.GetNumber(client.crmID, "Fax");
                    if (fax != null)
                    {
                        FaxTB.Text = fax.number;
                    }
                }
                RevertBtn.Visible = false;
                XLDocument.UpdateBookmark("Salutation", SalutationTb.Text);
                XLDocument.UpdateBookmark("Addressee", AddresseeTB.Text);
                XLDocument.UpdateBookmark("Addressee2", AddresseeTB.Text);
                XLDocument.UpdateBookmark("Address", addTB.Text);
                XLDocument.UpdateBookmark("Fax", FaxTB.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to revert");
                XLtools.LogException("RevertBtn", ex.ToString());
            }
        }

        private void WhenCallingCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (WhenCallingCheck.Checked)
            {
                if (WhencallingDDL.DataSource == null)
                {
                    //Deal with when calling ddl
                    WhencallingDDL.DataSource = users;
                    WhencallingDDL.DisplayMember = "name";
                    WhencallingDDL.ValueMember = "crmID";
                    WhenCallingCheck.Checked = true;
                }
                WhencallingDDL.Visible = true;
                WhencallingDDL.SelectedItem = user;
                WhenCallingString(user);
            }
            else
            {
                WhencallingDDL.Visible = false;
                WhenCallingString(null);
            }
        }

        private void WhencallingDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collect the changed record
            XLMain.EntityCouplet temp = (XLMain.EntityCouplet)WhencallingDDL.SelectedItem;
            //Convert the entity couplet to a full blown user now we know which one we want
            XLMain.Staff whenCalling = XLMain.Staff.FetchStaff(temp.crmID);

            WhenCallingString(whenCalling);
        }

        private void FaxTB_Leave(object sender, EventArgs e)
        {
            XLDocument.UpdateBookmark("Fax", FaxTB.Text);
        }

        private static void WhenCallingString(XLMain.Staff wcStaff)
        {
            try
            {
                if (wcStaff == null)
                {
                    XLDocument.UpdateBookmark("Whencalling", "");
                    XLDocument.UpdateBookmark("Calling", "");
                    XLDocument.UpdateBookmark("WcEmail", "");
                    XLDocument.UpdateBookmark("WcEmailAddress", "");
                }
                else
                {
                    XLDocument.UpdateBookmark("Whencalling", "When calling please ask for: ", 1);
                    XLDocument.UpdateBookmark("Calling", wcStaff.name);
                    if (wcStaff.emails != null)
                    {
                        XLDocument.UpdateBookmark("WcEmail", "e-mail: ", 1);
                        XLDocument.UpdateBookmark("WcEmailAddress", wcStaff.emails[0].email);
                    }
                    else
                    {
                        XLDocument.UpdateBookmark("WcEmail", "");
                        XLDocument.UpdateBookmark("WcEmailAddress", "");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to build when calling for");
                XLtools.LogException("WhenCallingString", ex.ToString());
            }
        }

        private void SetVisibility(string docType)
        {
            try
            {
                if (docType == "Fax")
                {
                    SubjectTB.Visible = true;
                    Subjectlbl.Visible = true;
                    DateTB.Visible = true;
                    Datelbl.Visible = true;
                    SenderDDL.Visible = true;
                    Senderlbl.Visible = true;
                    Senderlbl.Text = "Sender";
                    Salutationslbl.Visible = true;
                    SalDDL.Visible = true;
                    Addresseelbl.Visible = true;
                    AddresseeTB.Visible = true;
                    Salutationlbl.Visible = true;
                    SalutationTb.Visible = true;
                    Addresseslbl.Visible = false;
                    addressesDDL.Visible = false;
                    Addresseslbl.Text = "Fax No.";
                    Addresslbl.Visible = false;
                    addTB.Visible = false;
                    PandClbl.Visible = false;
                    PandCCheck.Visible = false;
                    FAOBHOlbl.Visible = true;
                    FAOBCheck.Visible = true;
                    WhenCallinglbl.Visible = false;
                    WhencallingDDL.Visible = true;
                    WhenCallingCheck.Visible = false;
                    FaxTB.Visible = true;

                }
                else if (docType == "Letter")
                {
                    SubjectTB.Visible = true;
                    Subjectlbl.Visible = true;
                    DateTB.Visible = true;
                    Datelbl.Visible = true;
                    SenderDDL.Visible = true;
                    Senderlbl.Visible = true;
                    Senderlbl.Text = "Sender";
                    Salutationslbl.Visible = true;
                    SalDDL.Visible = true;
                    Addresseelbl.Visible = true;
                    AddresseeTB.Visible = true;
                    Salutationlbl.Visible = true;
                    SalutationTb.Visible = true;
                    Addresseslbl.Visible = true;
                    Addresseslbl.Text = "Alt. Addresses";
                    addressesDDL.Visible = true;
                    Addresslbl.Visible = true;
                    addTB.Visible = true;
                    PandClbl.Visible = true;
                    PandCCheck.Visible = true;
                    FAOBHOlbl.Visible = true;
                    FAOBCheck.Visible = true;
                    WhenCallinglbl.Visible = true;
                    WhencallingDDL.Visible = false;
                    WhenCallingCheck.Visible = true;
                    FaxTB.Visible = false;
                }
                else if (docType == "FileNote")
                {
                    SubjectTB.Visible = true;
                    Subjectlbl.Visible = true;
                    DateTB.Visible = true;
                    Datelbl.Visible = true;
                    SenderDDL.Visible = true;
                    Senderlbl.Visible = true;
                    Senderlbl.Text = "Orignator";
                    Salutationslbl.Visible = false;
                    SalDDL.Visible = false;
                    Addresseelbl.Visible = false;
                    AddresseeTB.Visible = false;
                    Salutationlbl.Visible = false;
                    SalutationTb.Visible = false;
                    Addresseslbl.Visible = false;
                    addressesDDL.Visible = false;
                    Addresslbl.Visible = false;
                    addTB.Visible = false;
                    PandClbl.Visible = false;
                    PandCCheck.Visible = false;
                    FAOBHOlbl.Visible = false;
                    FAOBCheck.Visible = false;
                    WhenCallinglbl.Visible = false;
                    WhencallingDDL.Visible = false;
                    WhenCallingCheck.Visible = false;
                    FaxTB.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to determine visibility settings");
                XLtools.LogException("SetVisibility", ex.ToString());
            }
        }

        private static string OurRef(XLMain.Client client, XLMain.Staff sender, XLMain.Staff user)
        {
            try
            {
                string ourRef = "";

                ourRef = client.clientcode;
                if (client.partner != null)
                {
                    ourRef += @"/" + client.partner.initials.ToUpper();
                }
                if (client.manager != null)
                {
                    ourRef += @"/" + client.manager.initials.ToUpper();
                }
                if (sender.crmID == "")
                {
                    if (sender.name != client.partner.name)
                    {
                        if (sender.name != client.manager.name)
                        {
                            ourRef += @"/" + sender.initials.ToUpper();
                        }
                    }
                }
                if (user.name != client.partner.name)
                {
                    if (user.name != client.manager.name)
                    {
                        if (user.name != sender.name)
                        {
                            ourRef += @"/" + user.initials.ToLower();
                        }
                    }
                }
                return ourRef;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to build reference");
                XLtools.LogException("OurRef", ex.ToString());
                return null;
            }
        }

        private static void DeploySignature(XLMain.Client client, XLMain.Staff sender, string salutation)
        {
            try
            {
                //set yours faithfully/Sincerely            
                if (salutation == "" || salutation == "Sir" || salutation == "Madam" || salutation == "Sirs")
                {
                    XLDocument.UpdateBookmark("FaithSincere", "Yours faithfully");
                }
                else
                {
                    XLDocument.UpdateBookmark("FaithSincere", "Yours sincerely");
                }
                //dependant on department build and deploy signature block

                if (client.department == "INS")
                {
                    if (sender.crmID == "")
                    {
                        XLDocument.UpdateBookmark("Sender", sender.name);
                        XLDocument.UpdateBookmark("Sender2", sender.name);
                    }
                    else
                    {
                        XLInsol.KeyData data = XLInsol.KeyData.FetchKeyData(client.crmID);
                        string str = sender.name;
                        if (data.sign != null && data.sign != "" && data.sign != sender.name)
                        {
                            str += Environment.NewLine + "FOR " + data.sign;
                        }
                        XLDocument.UpdateBookmark("Sender", str);
                        XLDocument.UpdateBookmark("Sender2", sender.name);

                        if (data.sign != null && data.sign != "")
                        {
                            if (data.caseType == "IVA" || data.caseType == "CVA")
                            {
                                str = data.title + " to the voluntary arrangement of";
                            }
                            else
                            {
                                str = data.title;
                            }
                            str += Environment.NewLine + client.name;
                            XLDocument.UpdateBookmark("FAOBOML", str);
                        }
                    }
                }
                else
                {
                    if (sender.crmID == "")
                    {
                        XLDocument.UpdateBookmark("Sender", sender.name);
                        XLDocument.UpdateBookmark("Sender2", sender.name);
                    }
                    else
                    {
                        string title = XLMain.Staff.GetJobTitle(sender);
                        string sndr = "";
                        sndr = sender.name;
                        sndr += Environment.NewLine + title;
                        XLDocument.UpdateBookmark("Sender", sndr);
                        XLDocument.UpdateBookmark("Sender2", sender.name);

                        //if (sender.emails != null)
                        //{
                        //    XLDocument.UpdateBookmark("SenderEmail", "email: " + sender.emails[0].email.ToLower(), bold: 1, styleName: "ML Main");
                        //}
                        //else
                        //{
                        //    XLDocument.UpdateBookmark("SenderEmail", "");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to deploy signature");
                XLtools.LogException("DeploySignature", ex.ToString());
            }
        }

        private void IPSContactBtn_Click(object sender, EventArgs e)
        {
            try
            {
                IPSContForm myForm = new IPSContForm(client);
                myForm.ShowDialog();
                XLInsol.Contact selectContact = myForm.selectedContact;
                if (selectContact != null)
                {
                    if (docType == "Letter")
                    {
                        if (selectContact != null)
                        {
                            allAddresses = null;
                            addressesDDL.DataSource = null;
                            string add = selectContact.name;
                            add += Environment.NewLine + selectContact.addressBlock;
                            addTB.Text = add;
                        }
                    }
                    else if (docType == "Fax")
                    {
                        if (selectContact.fax != null)
                        {
                            FaxTB.Text = selectContact.fax;
                        }
                    }
                    SalDDL.DataSource = null;
                    AddresseeTB.Text = "";
                    SalutationTb.Text = "";

                    RevertBtn.Visible = true;
                    XLDocument.UpdateBookmark("Salutation", SalutationTb.Text);
                    XLDocument.UpdateBookmark("Addressee", AddresseeTB.Text);
                    XLDocument.UpdateBookmark("Addressee2", AddresseeTB.Text);
                    XLDocument.UpdateBookmark("Address", addTB.Text);
                    XLDocument.UpdateBookmark("Fax", FaxTB.Text);
                }
                else
                {
                    MessageBox.Show("No address returned");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to fetch insolvency contact");
                XLtools.LogException("InsolContactBtn", ex.ToString());
            }
        }

        private void EncChk_CheckedChanged(object sender, EventArgs e)
        {
            if (EncChk.Checked)
            {
                XLDocument.UpdateBookmark("Enc", "Enc", bold: 0, styleName: "ML Main");
            }
            else
            {
                XLDocument.UpdateBookmark("Enc", "", bold: 0, styleName: "ML Main");
            }
        }
    }
}
