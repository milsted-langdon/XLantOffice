using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace XLantCore
{
    public class XLMain
    {
        public class EntityCouplet
        {
            public string crmID { get; set; }
            public string name { get; set; }

            public EntityCouplet()
            {
                crmID = "";
                name = "";
            }

            public EntityCouplet(string iD, string setName)
            {
                crmID = iD;
                name = setName;
            }
        }

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
            public Boolean IsIndividual { get; set; }
            public List<Address> addresses { get; set; }
            public List<Number> numbers { get; set; }
            public List<EmailAddress> emails { get; set; }
            public List<Salutation> salutations { get; set; }
            public List<Parameter> parameters { get; set; }
            public Staff partner { get; set; }
            public Staff manager { get; set; }

            public static Client FetchClient(string ID)
            {
                try
                {
                    if (String.IsNullOrEmpty(ID))
                    {
                        return null;
                    }
                    //Get client data from Client
                    Client client = new Client();
                    //SqlDataReader xlReader = XLSQL.ReaderQuery("Select * from Client where CRMID='" + ID + "'");
                    DataTable xlReader = XLSQL.ReturnTable("Select * from Client where CRMID='" + ID + "'");
                    //if we get more than one result we have a problem
                    if (xlReader.Rows.Count == 1)
                    {
                        client.crmID = ID;
                        client.clientcode = xlReader.Rows[0]["clientcode"].ToString();
                        client.name = xlReader.Rows[0]["name"].ToString();
                        client.relationship = "";//only used in collections
                        client.type = xlReader.Rows[0]["type"].ToString();
                        client.status = xlReader.Rows[0]["status"].ToString();
                        if (client.status.ToUpper() == "ACTIVE" || client.status.ToUpper() == "NEW")
                        {
                            client.isLive = true;
                        }
                        else
                        {
                            client.isLive = false;
                        }
                        client.department = xlReader.Rows[0]["department"].ToString();
                        client.office = xlReader.Rows[0]["office"].ToString();
                        client.partner = Staff.GetStaff("Partner", client.crmID);
                        client.manager = Staff.GetStaff("Manager", client.crmID);
                        if (client.type == "Ltd" || client.type == "Plc" || client.type == "LLP" || client.type == "PartnerS" || client.type == "Charity")
                        {
                            client.IsIndividual = false;
                        }
                        else
                        {
                            client.IsIndividual = true;
                        }
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
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-FetchClient", ex.ToString());
                    return null;
                }
            }

            public static Client FetchClientFromCode(string clientCode)
            {
                try
                {
                    Client client = new Client();
                    DataTable xlReader = XLSQL.ReturnTable("Select crmid from Client where clientcode='" + clientCode + "'");
                    client = FetchClient(xlReader.Rows[0]["crmid"].ToString());
                    return client;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-FetchClientfromcode", ex.ToString());
                    return null;
                }
            }

            public static List<Client> Clients(string id)
            {
                try
                {
                    string type = DiscoverType(id);
                    List<Client> group = new List<Client>();
                    Client tempClient = null;
                    DataTable xlReader = XLSQL.ReturnTable("SELECT * from Connections('" + id + "', '" + type + "') where contacttype='Client' and ISNULL(contactid,'')!=''");
                    if (xlReader.Rows.Count!=0)
                    {
                        foreach (DataRow row in xlReader.Rows)
                        {
                            tempClient = Client.FetchClient(row["crmid"].ToString());
                            tempClient.relationship = row["reldesc"].ToString();
                            group.Add(tempClient);
                        }
                    }
                    return group;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-Clients", ex.ToString());
                    return null;
                }
            }

            public static List<EntityCouplet> AllClients(bool activeOnly = true)
            {
                try
                {
                    List<EntityCouplet> group = new List<EntityCouplet>();
                    string query = "SELECT clientcode, name from client";
                    if (activeOnly)
                    {
                        query = query + " where status in ('Active', 'New')";
                    }
                    query = query + " order by clientcode";
                    DataTable xlReader = XLSQL.ReturnTable(query);
                    if (xlReader.Rows.Count != 0)
                    {
                        foreach (DataRow row in xlReader.Rows)
                        {
                            group.Add(new EntityCouplet(row["clientcode"].ToString(), row["clientcode"].ToString() + " - " + row["name"].ToString()));
                        }
                    }
                    return group;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-AllClients", ex.ToString());
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
                try
                {
                    Contact cont = new Contact();
                    //Get contact data from MainContact
                    DataTable xlReader = XLSQL.ReturnTable("Select * from Contact where CRMID='" + iD + "'");
                    if (xlReader.Rows.Count == 1)
                    {
                        cont.crmID = iD;
                        cont.firstname = xlReader.Rows[0]["first_name"].ToString();
                        cont.lastname = xlReader.Rows[0]["last_name"].ToString();
                        cont.relationship = ""; //only used in collections
                        cont.type = xlReader.Rows[0]["type"].ToString();
                        //see whether the title exists in the enum
                        if (Enum.IsDefined(typeof(Title), xlReader.Rows[0]["title"].ToString()))
                        {
                            cont.title = (Title)Enum.Parse(typeof(Title), xlReader.Rows[0]["title"].ToString(), true);
                        }
                        else
                        {
                            cont.title = (Title)Enum.Parse(typeof(Title), "Mr", true);                        
                        }
                        cont.position = xlReader.Rows[0]["position"].ToString();
                    }
                    //Organisation
                    cont.organisation = Organisation.GetOrganisation(cont.crmID);
                    //Addresses
                    cont.addresses = Address.GetAddresses(cont.crmID);
                    //Update the address block with the organisation name
                    if (cont.organisation != null)
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
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-FetchContact", ex.ToString());
                    return null;
                }

            }
            
            public static List<Contact> Contacts(string id)
            {
                try
                {
                	string type = DiscoverType(id);
                    List<Contact> contacts = new List<Contact>();
                    DataTable xlReader = XLSQL.ReturnTable("SELECT * from Connections('" + id + "', '" + type + "') where contacttype='Contact' and ISNULL(contactid,'')!=''");
                    if (xlReader.Rows.Count!=0)
                    {
                        foreach (DataRow row in xlReader.Rows)
                        {
                            Contact cont = Contact.FetchContact(row["contact_id"].ToString());
                            cont.relationship = row["reldesc"].ToString();
                            contacts.Add(cont);
                        }
                    }
                    contacts.Sort();
                    return contacts;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-Contacts", ex.ToString());
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
                try
                {
                    //Get contact data from Organisation
                    Organisation org = new Organisation();
                    DataTable xlReader = XLSQL.ReturnTable("Select * from Organisations where CRMID='" + iD + "'");
                    if (xlReader.Rows.Count == 1)
                    {
                        org.crmID = iD;
                        org.name = xlReader.Rows[0]["name"].ToString();

                        //Addresses
                        org.addresses = Address.GetAddresses(org.crmID);
                        //Numbers
                        org.numbers = Number.GetNumbers(org.crmID);
                    }
                    return org;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-FetchOrganisation", ex.ToString());
                    return null;
                }
            }

            public static Organisation GetOrganisation(string contactID)
            {
                try
                {
                    Organisation org = new Organisation();
                    //Get contact data from Organisation
                    DataTable xlReader = XLSQL.ReturnTable("SELECT * from Connections('" + contactID + "', 'Contact') where contacttype='Organisation' and ISNULL(contactid,'')!=''");
                    if (xlReader.Rows.Count == 1)
                    {
                        org = Organisation.FetchOrganisation(xlReader.Rows[0]["contactID"].ToString());
                    }
                    else
                    {
                        org = null;
                    }
                    return org;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-FetchOrganisation", ex.ToString());
                    return null;
                }
            }

            public static List<Contact> GetEmployees(string iD)
            {
                try
                {
                    List<Contact> connectedContacts = new List<Contact>();
                    return connectedContacts = Contact.Contacts(iD);
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-GetEmployees", ex.ToString());
                    return null;
                }
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
                try
                {
                    string type = DiscoverType(crmID);
                    List<Address> addresses = new List<Address>();
                    DataTable xlReader = XLSQL.ReturnTable("Select * from [XLant].[dbo].[Addresses] ('" + crmID + "','" + type + "')");
                    if (xlReader.Rows.Count != 0)
                    {
                        foreach (DataRow row in xlReader.Rows)
                        {
                            addresses.Add(new Address(Convert.ToBoolean(row["IsPrimary"].ToString()), row["address1"].ToString(), row["address2"].ToString(), row["address3"].ToString(), row["address4"].ToString(), row["address5"].ToString(), row["postcode"].ToString()));
                        }
                        return addresses;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-GetAddresses", ex.ToString());
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
                try
                {
                    string type = DiscoverType(crmID);
                    List<Number> numbers = new List<Number>();
                    DataTable xlReader = XLSQL.ReturnTable("SELECT * FROM [XLant].[dbo].[Numbers] ('" + crmID + "','" + type + "') where ISNULL(number,'')!=''");
                        if (xlReader.Rows.Count != 0)
                        {
                            foreach (DataRow row in xlReader.Rows)
                            {
                                numbers.Add(new Number(row["ndesc"].ToString(), row["number"].ToString(), Convert.ToBoolean(row["isPrimary"].ToString())));
                            }
                        }
                    return numbers;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-GetNumbers", ex.ToString());
                    return null;
                }
            }

            public static Number GetNumber(string crmID, string ndesc)
            {
                try
                {
                    string type = DiscoverType(crmID);
                    DataTable xlReader = XLSQL.ReturnTable("SELECT Top 1 * FROM [XLant].[dbo].[Numbers] ('" + crmID + "','" + type + "') where ndesc='" + ndesc + "' and ISNULL(number,'')!='' order by isPrimary desc");
                    if (xlReader.Rows.Count == 1)
                    {
                        Number number = new Number(xlReader.Rows[0]["ndesc"].ToString(), xlReader.Rows[0]["number"].ToString(), Convert.ToBoolean(xlReader.Rows[0]["isPrimary"].ToString()));
                        return number;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-GetNumber", ex.ToString());
                    return null;
                }
            }
        }

        public class Error
        {
            public int id { get; set; }
            public DateTime Date { get; set; }
            public string User { get; set; }
            public string Machine { get; set; }
            public string Command { get; set; }
            public string ErrorMessage { get; set; }

            public Error(DateTime date, string user, string machine, string command, string error)
            {
                Date = date;
                User = user;
                Machine = machine;
                Command = command;
                ErrorMessage = error;
            }

            public bool SavetoDb()
            {
                try
                {
                    string query = "set dateformat dmy; INSERT INTO [XLant].[dbo].[Errors]([date],[user],[machine],[command],[error]) Values (@date, @user, @machine, @command, @errorMessage)";
                    List<SqlParameter> parameterCollection = new List<SqlParameter>(); 
                    parameterCollection.Add(new SqlParameter("date", Date.ToShortDateString()));
                    parameterCollection.Add(new SqlParameter("user", User));
                    parameterCollection.Add(new SqlParameter("machine", Machine));
                    parameterCollection.Add(new SqlParameter("command", Command));
                    parameterCollection.Add(new SqlParameter("errorMessage", ErrorMessage));
                    bool result = XLSQL.RunCommand(query, parameterCollection);
                    return result;
                }
                catch
                {
                    return false;
                    //no error handling we would just create a cycle
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
                    DataTable xlReader = XLSQL.ReturnTable("Select * from Emails where CRMID='" + crmID + "'and module='" + type + "' and deleted=0");
                    if (xlReader.Rows.Count != 0)
                    {
                        foreach (DataRow row in xlReader.Rows)
                        {
                            string crmid = row["crmid"].ToString();
                            string email = row["email"].ToString();
                            bool isPrimary = Convert.ToBoolean(row["isprimary"]);
                            bool doNotMail = Convert.ToBoolean(row["donotmail"]);
                            emails.Add(new EmailAddress(crmid, email, email, isPrimary, doNotMail));
                        }
                    }
                    return emails;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-GetEmails", ex.ToString());
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
            public List<Address> addresses { get; set; }
            public List<Salutation> salutations { get; set; }

            public static Staff FetchStaff(string iD)
            {
                try
                {
                    Staff staff = new Staff();
                    DataTable xlReader = XLSQL.ReturnTable("Select * from Staff where crmID='" + iD + "'");
                    if (xlReader.Rows.Count == 1)
                    {
                        staff.crmID = xlReader.Rows[0]["crmid"].ToString();
                        staff.relationship = ""; //Only used in collections
                        staff.username = xlReader.Rows[0]["username"].ToString();
                        staff.name = xlReader.Rows[0]["fullname"].ToString();
                        staff.initials = xlReader.Rows[0]["initials"].ToString(); 
                        staff.department = xlReader.Rows[0]["department"].ToString(); 
                        staff.office = xlReader.Rows[0]["office"].ToString();
                        staff.grade = xlReader.Rows[0]["grade"].ToString();
                        staff.emails = EmailAddress.GetEmails(staff.crmID);
                        staff.addresses = Address.GetAddresses(staff.crmID);
                        staff.salutations = Salutation.GetSalutations(staff.crmID);
                        staff.numbers = Number.GetNumbers(staff.crmID);   
                    }
                    return staff;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-FetchStaff", ex.ToString());
                    return null;
                }
            }

            public static Staff StaffFromUser(string username)
            {
                try
                {
                    Staff staff = new Staff();
                    if (@username.Contains("\\"))
                    {
                        //Contains domain so take everything after the \ doubled to handle escaping
                        username = username.Substring(@username.LastIndexOf("\\"));
                    }
                    DataTable xlReader = XLSQL.ReturnTable("Select Top 1 * from Staff where username='" + username + "'");
                    if (xlReader.Rows.Count == 1)
                    {
                        staff = FetchStaff(xlReader.Rows[0]["crmid"].ToString());
                    }
                return staff;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-FetchStaffFromUser", ex.ToString());
                    return null;
                }
            }
           
            public static List<Staff> connectedStaff(string id)
            {
                try
                {
                    string type = DiscoverType(id);
                    List<Staff> staff = new List<Staff>();
                    DataTable xlReader = XLSQL.ReturnTable("SELECT * from Connections('" + id + "', '" + type + "') where contacttype='Staff' and ISNULL(contactid,'')!=''");
                    if (xlReader.Rows.Count != 0)
                    {
                        foreach (DataRow row in xlReader.Rows)
                        {
                            Staff emp = FetchStaff(row["contactid"].ToString());
                            emp.relationship = row["reldesc"].ToString();
                            staff.Add(emp);
                        }
                    }
                    return staff;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-ConnectedStaff", ex.ToString());
                    return null;
                }
            }

            public static Staff GetStaff(string grade, string iD)
            {
                try
                {
                    Staff staff = new Staff();
                    string type = DiscoverType(iD);
                    DataTable xlReader = XLSQL.ReturnTable("SELECT TOP 1 * from Connections('" + iD + "', '" + type + "') where contacttype='Staff' and reldesc = '" + grade + "' and ISNULL(contactid,'')!=''");
                    if (xlReader.Rows.Count == 1)
                    {
                        staff = FetchStaff(xlReader.Rows[0]["contactid"].ToString());
                    }
                    return staff;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-GetStaff", ex.ToString());
                    return null;
                }
            }

            public static List<EntityCouplet> AllStaff()
            {
                try
                {
                    List<EntityCouplet> staff = new List<EntityCouplet>();
                    DataTable xlReader = XLSQL.ReturnTable("SELECT fullname, crmid from VCStaffView order by fullname");
                    if (xlReader.Rows.Count != 0)
                    {
                        foreach (DataRow row in xlReader.Rows)
                        {
                            EntityCouplet tempStaff = new EntityCouplet();
                            tempStaff.crmID = row["crmid"].ToString();
                            tempStaff.name = row["fullname"].ToString();
                            staff.Add(tempStaff);
                        }
                    }
                    return staff;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-Allstaff", ex.ToString());
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
                   
                        DataTable xlReader = XLSQL.ReturnTable("Select * from [XLant].[dbo].[Salutations] ('" + crmID + "','" + type + "') where ISNULL(salutation,'')!=''");
                        if (xlReader.Rows.Count != 0)
                        {
                            foreach (DataRow row in xlReader.Rows)
                            {
                                salutations.Add(new Salutation("", row["desc"].ToString(), row["addressee"].ToString(), row["Salutation"].ToString(), Convert.ToBoolean(row["isprimary"].ToString())));
                            }
                        }
                    return salutations;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("XLMain-GetSalutations", ex.ToString());
                    return null;
                }
            }
        }

        public class FPIClient : Client
        {

            public FPIClient()
            {
            
            }

            public FPIClient(DataRow row)
            {
                crmID = row["id"].ToString();
                clientcode = row["clientcode"].ToString();
                name = row["Lead_Name"].ToString();
                type = row["client_type"].ToString();
                office = row["office"].ToString();
                Manager = row["manager"].ToString();
                ManagerRole = row["ManagerRole"].ToString();
                Mgr = row["Mgr"].ToString();
                Username = row["manager_user"].ToString();
                Ptr = row["Ptr"].ToString();
                addresses = new List<Address>();
                addresses.Add(new Address(true, row["Add1"].ToString(), row["Add2"].ToString(), row["Add3"].ToString(), row["Add4"].ToString(), "", row["Postcode"].ToString()));
                Address = addresses.FirstOrDefault().addressBlock;
                Salutation = row["Salutation"].ToString();
                Addressee = row["Addressee"].ToString();
                DirectDebit = Boolean.Parse(row["direct_debit"].ToString());
                ddref1 = clientcode.Substring(0, 1);
                ddref2 = clientcode.Substring(1, 1);
                ddref3 = clientcode.Substring(2, 1);
                ddref4 = clientcode.Substring(3, 1);
                ddref5 = clientcode.Substring(4, 1);
                ddref6 = clientcode.Substring(5, 1);
                LastYear = Boolean.Parse(row["last_year"].ToString());
                decimal premium = 0;
                if (decimal.TryParse(row["TotalPremium"].ToString(), out premium))
                {
                    TotalPremium = premium;
                }
                else
                {
                    TotalPremium = 0;
                }
                InvoiceLines = row["InvoiceLines"].ToString();
                Email = row["email"].ToString();
                if (Addressee != name)
                {
                    AddresseeBlock = Addressee + Environment.NewLine + name;
                }
                else
                {
                    AddresseeBlock = name;
                }
            }
            public decimal TotalPremium { get; set; }
            public bool DirectDebit { get; set; }
            public string ddref1 { get; set; }
            public string ddref2 { get; set; }
            public string ddref3 { get; set; }
            public string ddref4 { get; set; }
            public string ddref5 { get; set; }
            public string ddref6 { get; set; }
            public string InvoiceLines { get; set; }
            public string Manager { get; set; }
            public string ManagerRole { get; set; }
            public string Email { get; set; }
            public string Mgr { get; set; }
            public string Ptr { get; set; }
            public string Address { get; set; }
            public string AddresseeBlock { get; set; }
            public string Addressee { get; set; }
            public string Salutation { get; set; }
            public string Username { get; set; }
            public bool LastYear { get; set; }

            /// <summary>
            /// Returns the FPI clients for a manager
            /// </summary>
            /// <param name="managerCRMId">THe Id of the manager</param>
            /// <param name="additionalQuery">Allows you to filter the list at the sql end format "where x = y"</param>
            /// <returns>The populated list of clients</returns>
            public static List<FPIClient> GetFPIClients(string managerCRMId, string additionalQuery = null, bool domestic = true)
            {
                List<FPIClient> list = new List<FPIClient>();
                Staff manager = Staff.FetchStaff(managerCRMId);
                string query = "SELECT * FROM [dbo].[FPIManager] ('" + manager.name + "')";
                if (additionalQuery != null)
                {
                    query += " " + additionalQuery;
                }
                if (query.Contains("where"))
                {
                    query += " and";
                }
                else
                {
                    query += " where";
                }
                if(domestic)
                {
                    
                    query += " (isnull(add4,'') = '' or add4 in ('UK', 'United Kingdom', 'England', 'Wales', 'Scotland', 'Northern Ireland', 'GB', 'NULL', ' '))";
                }
                else
                {
                    query += " (isnull(add4,'') != '' and add4 not in ('UK', 'United Kingdom', 'England', 'Wales', 'Scotland', 'Northern Ireland', 'GB', 'NULL', ' '))";
                }
                DataTable xlReader = XLSQL.ReturnTable(query);
                if (xlReader != null)
                {
                    for (int i = 0; i < xlReader.Rows.Count; i++)
                    {
                        FPIClient client = new FPIClient(xlReader.Rows[i]);
                        list.Add(client);
                    } 
                }
                return list;
            }

            public static List<FPIClient> GetFPIClients(Client client)
            {
                List<FPIClient> list = new List<FPIClient>();
                list = GetFPIClients(client.manager.crmID, "where clientcode = '" + client.clientcode + "'");
                return list;
            }

            public static FPIClient GetFPIClientInvoice(Client client)
            {
                DataTable table = XLSQL.ReturnTable("select * from FPIInvoice('" + client.clientcode + "')");
                FPIClient fpiClient = new FPIClient(table.Rows[0]);
                return fpiClient;
            }
        }

        public static string DiscoverType(string crmID)
        {
            try
            {
                string type = null;
                //first find out whether it is a client
                DataTable xlReader = XLSQL.ReturnTable("Select count(crmID) as number from Client where crmID='" + crmID + "'");
                if (xlReader.Rows.Count != 0)
                {
                    if (Convert.ToInt16(xlReader.Rows[0]["number"].ToString()) != 0)
                    {
                        type = "Client";
                    }
                }
                if (type == null)
                {
                    //contact
                    xlReader = XLSQL.ReturnTable("Select count(crmID) as number from Contact where crmID='" + crmID + "'");
                    if (xlReader.Rows.Count != 0)
                    {
                        if (Convert.ToInt16(xlReader.Rows[0]["number"].ToString()) != 0)
                        {
                            type = "Contact";
                        }
                    }
                }
                if (type == null)
                {
                    //staff
                    xlReader = XLSQL.ReturnTable("Select count(crmID) as number from Staff where crmID='" + crmID + "'");
                    if (xlReader.Rows.Count != 0)
                    {
                        if (Convert.ToInt16(xlReader.Rows[0]["number"].ToString()) != 0)
                        {
                            type = "Staff";
                        }
                    }
                }
                if (type == null)
                {
                    //organisation
                    xlReader = XLSQL.ReturnTable("Select count(crmID) as number from Organisations where crmID='" + crmID + "'");
                    if (xlReader.Rows.Count != 0)
                    {
                        if (Convert.ToInt16(xlReader.Rows[0]["number"].ToString()) != 0)
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
            catch (Exception ex)
            {
                XLtools.LogException("XLMain-DiscoverType", ex.ToString());
                return null;
            }
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
            Lord,

        }
    }
}
