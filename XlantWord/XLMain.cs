using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace XlantWord
{
    public class XLMain
    {
        public class Client
        {
            public String crmID { get; set; }
            public String clientcode { get; set; }
            public String name {get; set;}
            public String relationship { get; set; }
            public String type { get; set; }
            public String status { get; set; }
            public Boolean isLive {get; set;}
            public float wip { get; set; }
            public float debtor { get; set; }
            public String department { get; set; }
            public String office { get; set; }
            public List<Address> addresses { get; set; }
            public List<Number> numbers { get; set; }
            public List<EmailAddress> emails { get; set; }
            public List<Salutation> salutations { get; set; }
            public List<Parameter> parameters { get; set; }
            public Staff partner { get; set; }
            public Staff manager { get; set; }

            public static Client FetchClient(string ID)
            {
                //Get client data from Client
                Client client = new Client();
                SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from Client where CRMID='" + ID + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    client.crmID = ID;
                    client.clientcode = xlReader.NiceString("clientcode");
                    client.name = xlReader.NiceString("name");
                    client.relationship = "";//only used in collections
                    client.type = xlReader.NiceString("type");
                    client.status = xlReader.NiceString("status");
                    if (client.status.ToUpper() == "ACTIVE" || client.status.ToUpper() == "NEW")
                    {
                        client.isLive = true;
                    }
                    else
                    {
                        client.isLive = false;
                    }
                    client.department = xlReader.NiceString("department");
                    client.office = xlReader.NiceString("office");
                    client.partner = Staff.GetStaff("Partner", client.crmID);
                    client.manager = Staff.GetStaff("Manager", client.crmID);
                }
                    
                //Start building additional data
                //WIP
                //xlReader = XLSQL.ReaderQuery("SELECT ISNULL((SELECT ISNULL(SUM(WIPOutstanding),0) as WIPOutstanding FROM wipentries WHERE [Clientcode] = '" + clientcode + "'),0)");
                //if (xlReader.HasRows)
                //{
                //    xlReader.Read();
                //    wip = float.Parse(xlReader["wipoutstanding"].ToString());
                //}
                //Debtor
                //xlReader = XLSQL.ReaderQuery("SELECT ISNULL((SELECT ISNULL(SUM(DebtTranUnpaid),0) As DRSTotal FROM [Engine_MLDB].[dbo].[view_debtors] WHERE ([Clientcode] = '" + clientcode + "')),0)");
                //if (xlReader.HasRows)
                //{
                    //xlReader.Read();
                client.wip = 0;
                client.debtor = 0;
                //}
                //Addresses
                client.addresses = Address.GetAddresses(client.crmID);
                //Numbers
                client.numbers = Number.GetNumbers(client.crmID);
                //email
                client.emails = EmailAddress.GetEmails(client.crmID);
                //salutations
                client.salutations = Salutation.GetSalutations(client.crmID);
                //parameters
                client.parameters = null;
                //connections
                return client;
            }

            public static List<Client> Clients(string id)
            {
                try
                {
                    string type = DiscoverType(id);
                    List<Client> group = new List<Client>();
                    Client tempClient = null;
                    SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT * from Connections('" + id + "', '" + type + "') where contacttype='Client' and ISNULL(contactid,'')!=''");
                    if (xlReader.HasRows)
                    {
                        while (xlReader.Read())
                        {
                            tempClient = Client.FetchClient(xlReader["crmid"].ToString());
                            tempClient.relationship = xlReader["reldesc"].ToString();
                            group.Add(tempClient);
                        }
                    }
                    return group;
                }
                catch
                {
                    return null;
                }
            }
        }

        public class Contact
        {
            public String crmID { get; set; }
            public String firstname { get; set; }
            public String lastname { get; set; }
            public String relationship { get; set; }
            public String type { get; set; }
            public Title title { get; set; }
            public String position { get; set; }
            public Organisation organisation { get; set; }
            public List<Address> addresses { get; set; }
            public List<Number> numbers { get; set; }
            public List<EmailAddress> emails { get; set; }
            public List<Salutation> salutations { get; set; }
            public List<Parameter> parameters { get; set; }


            //Set default sorting by last name
            public int CompareTo(Contact cont)
            {
                return this.lastname.CompareTo(cont.lastname);
            }

            public static Contact FetchContact(string iD)
            {
                Contact cont = new Contact();
                //Get contact data from MainContact
                SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from Contact where CRMID='" + iD + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    cont.crmID = iD;
                    cont.firstname = xlReader.NiceString("first_name");
                    cont.lastname = xlReader.NiceString("last_name");
                    cont.relationship = ""; //only used in collections
                    cont.type = xlReader.NiceString("type");
                    cont.title = (Title)Enum.Parse(typeof(Title), xlReader.NiceString("title"), true);
                    cont.position = xlReader.NiceString("position");
                }
                //Organisation
                cont.organisation = Organisation.GetOrganisation(cont.crmID);
                //Addresses
                cont.addresses = Address.GetAddresses(cont.crmID);
                //Update the address block with the organisation name
                if (cont.organisation.name != "")
                {
                    foreach (Address add in cont.addresses)
                    {
                        add.addressBlock = cont.organisation.name + Environment.NewLine + add.addressBlock;
                    }
                }
                //Numbers
                cont.numbers = Number.GetNumbers(cont.crmID);
                //email
                cont.emails = EmailAddress.GetEmails(cont.crmID);
                //salutations
                cont.salutations = Salutation.GetSalutations(cont.crmID);
                //parameters
                cont.parameters = null;
                return cont;
            }
            
            public static List<Contact> Contacts(string id)
            {
                try
                {
                	string type = DiscoverType(id);
                    List<Contact> contacts = new List<Contact>();
                    SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT * from Connections('" + id + "', '" + type + "') where contacttype='Contact' and ISNULL(contactid,'')!=''");
                    if (xlReader.HasRows)
                    {
                        while (xlReader.Read())
                        {
                            Contact cont = Contact.FetchContact(xlReader.NiceString("contactid"));
                            cont.relationship = xlReader.NiceString("reldesc");
                            contacts.Add(cont);
                        }
                    }
                    contacts.Sort();
                    return contacts;
                }
                catch
                {
                    return null;
                }
            }
        }
        
        public class Organisation
        {
            public String crmID { get; set; }
            public String name { get; set; }
            public List<Address> addresses { get; set; }
            public List<Number> numbers { get; set; }

            //Set default sorting by name
            public int CompareTo(Organisation org)
            {
                return this.name.CompareTo(org.name);
            }

            public static Organisation FetchOrganisation(string iD)
            {
                //Get contact data from Organisation
                Organisation org = new Organisation();
                SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from Organisations where CRMID='" + iD + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    org.crmID = iD;
                    org.name = xlReader.NiceString("name");

                    //Addresses
                    org.addresses = Address.GetAddresses(org.crmID);
                    //Numbers
                    org.numbers = Number.GetNumbers(org.crmID);
                }
                return org;
            }

            public static Organisation GetOrganisation(string contactID)
            {
                try
                {
                    Organisation org = new Organisation();
                    //Get contact data from Organisation
                    SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT * from Connections('" + contactID + "', 'Contact') where contacttype='Organisation' and ISNULL(contactid,'')!=''");
                    if (xlReader.HasRows)
                    {
                        xlReader.Read();
                        org = Organisation.FetchOrganisation(xlReader.NiceString("contactID"));
                    }
                    else
                    {
                        org = null;
                    }
                    return org;
                }
                catch
                {
                    return null;
                }
            }

            public static List<Contact> GetEmployees(string iD)
            {
                List<Contact> connectedContacts = new List<Contact>();
                return connectedContacts = Contact.Contacts(iD);
            }
            
        }

        public class Address
        {
            public String crmID { get; set; }
            public Boolean primary { get; set; }
            public String address1 { get; set; }
            public String address2 { get; set; }
            public String address3 { get; set; }
            public String address4 { get; set; }
            public String address5 { get; set; }
            public String postcode { get; set; }
            public String addressBlock { get; set; }



            public Address(Boolean isPrimary, string add1, string add2, string add3, string add4, string add5, string newPostcode)
            {
                crmID = "";
                address1 = add1;
                address2 = add2;
                address3 = add3;
                address4 = add4;
                address5 = add5;
                postcode = newPostcode;
                if (address1 != "")
                {
                    addressBlock = address1 + Environment.NewLine;
                }
                if (address2 != "")
                {
                    addressBlock += address2 + Environment.NewLine;
                }
                if (address3 != "")
                {
                    addressBlock += address3 + Environment.NewLine;
                }
                if (address4 != "")
                {
                    addressBlock += address4 + Environment.NewLine;
                }
                if (address5 != "")
                {
                    addressBlock += address5 + Environment.NewLine;
                }
                if (postcode != "")
                {
                    addressBlock += postcode;
                }
            }

            public static List<Address> GetAddresses(string crmID)
            {
                string type = DiscoverType(crmID);
                List<Address> addresses = new List<Address>();
                SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from [XLant].[dbo].[Addresses] ('" + crmID + "','" + type + "')");
                if (xlReader.HasRows)
                {
                    while (xlReader.Read())
                    {
                        addresses.Add(new Address(Convert.ToBoolean(xlReader["IsPrimary"].ToString()), xlReader.NiceString("address1"), xlReader.NiceString("address2"), xlReader.NiceString("address3"), xlReader.NiceString("address4"), xlReader.NiceString("address5"), xlReader.NiceString("postcode")));
                    }
                    return addresses;
                }
                else
                {
                    return null;
                }
            }
        }

        public class Number
        {
            public String crmID { get; set; }
            public String relID { get; set; }
            public String desc { get; set; }
            public String number { get; set; }
            public Boolean primary { get; set; }

            public Number()
            {
                crmID = "";
                relID = "";
                desc = null;
                number = null;
                primary = false;
            }

            public Number(string newDesc, string newNumber, Boolean newPrimary, string id = "")
            {
                crmID = "";
                relID = "";
                desc = newDesc;
                number = newNumber;
                primary = newPrimary;
            }

            public void Save(string connectedId)
            {
                //if crmid is blank then it is brand new and needs insert otherwise insert.
                if (crmID != "")
                {
                    //Insert query to insert number
                }
                else
                {
                    //insert query for update
                }
            }

            public static List<Number> GetNumbers(string crmID)
            {
                string type = DiscoverType(crmID);
                List<Number> numbers = new List<Number>();
                SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT * FROM [XLant].[dbo].[Numbers] ('" + crmID + "','" + type + "') where ISNULL(number,'')!=''");
                    if (xlReader.HasRows)
                    {
                        while(xlReader.Read())
                        {
                            numbers.Add(new Number(xlReader.NiceString("ndesc"), xlReader.NiceString("number"), Convert.ToBoolean(xlReader["isPrimary"].ToString())));
                        }
                    }
                return numbers;
            }

            public static Number GetNumber(string crmID, string ndesc)
            {
                string type = DiscoverType(crmID);
                SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT Top 1 * FROM [XLant].[dbo].[Numbers] ('" + crmID + "','" + type + "') where ndesc='" + ndesc + "' and ISNULL(number,'')!='' order by isPrimary desc");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    Number number = new Number(xlReader.NiceString("ndesc"), xlReader.NiceString("number"), Convert.ToBoolean(xlReader["isPrimary"].ToString()));
                    return number;
                }
                else
                {
                    return null;
                }
            }
        }

        public class EmailAddress
        {
            public String crmID { get; set; }
            public String email { get; set; }
            public String displayAs { get; set; }
            public Boolean primary { get; set; }
            public Boolean doNotMail { get; set; }

            public EmailAddress(string id, string address, string displayName, Boolean isPrimary, Boolean notForMail)
            {
                crmID = id;
                email = address;
                displayAs = displayName;
                primary = isPrimary;
                doNotMail = notForMail;
            }

            public static List<EmailAddress> GetEmails(string crmID)
            {
                try
                {
                    string type = DiscoverType(crmID);
                    List<EmailAddress> emails = new List<EmailAddress>();
                    SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from Emails where CRMID='" + crmID + "'and module='" + type + "' and deleted=0");
                    if (xlReader.HasRows)
                    {
                        while (xlReader.Read())
                        {
                            emails.Add(new EmailAddress(xlReader.NiceString("crmid"), xlReader.NiceString("email"), xlReader.NiceString("email"), Convert.ToBoolean(xlReader["isprimary"].ToString()), Convert.ToBoolean(xlReader["donotmail"].ToString())));
                        }
                    }
                    return emails;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return null;
                }
            }
        }

        public class Staff
        {
            public String crmID { get; set; }
            public String name { get; set; }
            public String username { get; set; }
            public String relationship { get; set; }
            public String initials { get; set; }
            public String department { get; set; }
            public String office { get; set; }
            public String grade { get; set; }
            public List<EmailAddress> emails { get; set; }
            public List<Number> numbers { get; set; }

            public static Staff FetchStaff(string iD)
            {
                Staff staff = new Staff();
                SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from Staff where crmID='" + iD + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    staff.crmID = xlReader.NiceString("crmid");
                    staff.relationship = ""; //Only used in collections
                    staff.username = xlReader.NiceString("username");
                    staff.name = xlReader.NiceString("fullname");
                    staff.initials = xlReader.NiceString("initials");
                    staff.department = xlReader.NiceString("department");
                    staff.office = xlReader.NiceString("office");
                    staff.grade = xlReader.NiceString("grade");
                    staff.emails = EmailAddress.GetEmails(staff.crmID);
                    staff.numbers = null;   
                }
                return staff;
            }

            public static Staff StaffFromUser(string username)
            {
                Staff staff = new Staff();
                if (@username.Contains("\\"))
                {
                    //Contains domain so take everything after the \ doubled to handle escaping
                    username = username.Substring(@username.LastIndexOf("\\"));
                }
                SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from Staff where username='" + username + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    staff.crmID = xlReader["crmid"].ToString();
                    staff.relationship = ""; //Only used in collections
                    staff.username = xlReader.NiceString("username");
                    staff.name = xlReader["fullname"].ToString();
                    staff.initials = xlReader["initials"].ToString();
                    staff.department = xlReader["department"].ToString();
                    staff.office = xlReader["office"].ToString();
                    staff.grade = xlReader["grade"].ToString();
                    staff.emails = EmailAddress.GetEmails(staff.crmID);
                    staff.numbers = null;
                }
            return staff;
            }
           
            public static List<Staff> connectedStaff(string id)
            {
                try
                {
                    string type = DiscoverType(id);
                    List<Staff> staff = new List<Staff>();
                    SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT * from Connections('" +id + "', '" + type + "') where contacttype='Staff' and ISNULL(contactid,'')!=''");
                    if (xlReader.HasRows)
                    {
                        while (xlReader.Read())
                        {
                            Staff emp = FetchStaff(xlReader.NiceString("contactid"));
                            emp.relationship = xlReader.NiceString("reldesc");
                            staff.Add(emp);
                        }
                    }
                    return staff;
                }
                catch
                {
                    return null;
                }
            }

            public static Staff GetStaff(string grade, string iD)
            {
                try
                {
                    Staff staff = new Staff();
                    string type = DiscoverType(iD);
                    SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT TOP 1 * from Connections('" + iD + "', '" + type + "') where contacttype='Staff' and reldesc = '" + grade + "' and ISNULL(contactid,'')!=''");
                    if (xlReader.HasRows)
                    {
                        xlReader.Read();
                        staff = FetchStaff(xlReader.NiceString("contactid"));
                    }
                    return staff;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("We failed the GetStaff try because: " + ex.ToString());
                    return null;
                }
            }

            public static List<Staff> AllStaff()
            {
                try
                {
                    List<Staff> staff = new List<Staff>();
                    SqlDataReader xlReader = XLSQL.ReaderQuery("SELECT * from Staff");
                    if (xlReader.HasRows)
                    {
                        while (xlReader.Read())
                        {
                            Staff tempStaff = new Staff();
                            tempStaff.crmID = xlReader.NiceString("crmid");
                            tempStaff.relationship = ""; //Only used in collections
                            tempStaff.username = xlReader.NiceString("username");
                            tempStaff.name = xlReader.NiceString("fullname");
                            tempStaff.initials = xlReader.NiceString("initials");
                            tempStaff.department = xlReader.NiceString("department");
                            tempStaff.office = xlReader.NiceString("office");
                            tempStaff.grade = xlReader.NiceString("grade");
                            tempStaff.emails = null; //EmailAddress.GetEmails(tempStaff.crmID);  **removed to reduce extra calls on the db
                            tempStaff.numbers = null;
                            staff.Add(tempStaff);
                        }
                    }
                    return staff;
                }
                catch
                {
                    return null;
                }
            }
        }

        public class Salutation
        {
            public String crmID { get; set; }
            public String desc { get; set; }
            public String addressee { get; set; }
            public String salutation { get; set; }
            public Boolean primary { get; set; }

            public Salutation(string newID, string newDesc, string newAddressee, string newSalutation, Boolean newPrimary)
            {
                crmID = newID;
                desc = newDesc;
                addressee = newAddressee;
                salutation = newSalutation;
                primary = newPrimary;
            }

            public static List<Salutation> GetSalutations(string crmID)
            {
                try
                {
                    string type = DiscoverType(crmID);
                    List<Salutation> salutations = new List<Salutation>();
                   
                        SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from [XLant].[dbo].[Salutations] ('" + crmID + "','" + type + "') where ISNULL(salutation,'')!=''");
                        if (xlReader.HasRows)
                        {
                            while (xlReader.Read())
                            {
                                salutations.Add(new Salutation("", xlReader.NiceString("desc"), xlReader.NiceString("addressee"), xlReader.NiceString("Salutation"), Convert.ToBoolean(xlReader["isprimary"].ToString())));
                            }
                        }
                    return salutations;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string DiscoverType(string crmID)
        {
            string type = null;
            //first find out whether it is a client
            SqlDataReader xlReader = XLSQL.ReaderQuery("Select count(crmID) as number from Client where crmID='" + crmID + "'");
            if (xlReader.HasRows)
            {
                xlReader.Read();
                if (Convert.ToInt16(xlReader["number"].ToString()) != 0)
                {
                    type = "Client";
                }
            }
            if (type == null)
            {
                //contact
                xlReader = XLSQL.ReaderQuery("Select count(crmID) as number from Contact where crmID='" + crmID + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    if (Convert.ToInt16(xlReader["number"].ToString()) != 0)
                    {
                        type = "Contact";
                    }
                }
            }
            if (type == null)
            {
                //staff
                xlReader = XLSQL.ReaderQuery("Select count(crmID) as number from Staff where crmID='" + crmID + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    if (Convert.ToInt16(xlReader["number"].ToString()) != 0)
                    {
                        type = "Staff";
                    }
                }
            }
            if (type == null)
            {
                //organisation
                xlReader = XLSQL.ReaderQuery("Select count(crmID) as number from Organisations where crmID='" + crmID + "'");
                if (xlReader.HasRows)
                {
                    xlReader.Read();
                    if (Convert.ToInt16(xlReader["number"].ToString()) != 0)
                    {
                        type = "Organisation";
                    }
                }
            }
            if (type == null)
            {
                type = "Unknown";
            }

            return type;
        }

        public class Parameter
        {
            public String crmID { get; set; }
            public String desc { get; set; }
            public String group { get; set; }
            public ParameterType type { get; set; } 
            public String str { get; set; }
            public Int64 number { get; set; }
            public DateTime date { get; set; }
            public Boolean truefalse { get; set; }
        }

        public enum ParameterType
        {
            pString,
            pBool,
            pDate,
            pInt,
            pMoney
        }

        public enum Title
        {
            Mr,
            Mrs,
            Miss,
            Ms,
            Dr,
            Sir,
            Prof,
            Lord
        }
    }
}
