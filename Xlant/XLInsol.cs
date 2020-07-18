using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace XLantCore
{
    public class XLInsol
    {
        public class Contact
        {
            public String crmID { get; set; }
            public String name { get; set; }
            public String type { get; set; }
            public String address1 { get; set; }
            public String address2 { get; set; }
            public String address3 { get; set; }
            public String address4 { get; set; }
            public String address5 { get; set; }
            public String postcode { get; set; }
            public String addressBlock { get; set; }
            public String fax { get; set; }

            public static Contact FetchContact(string ID, string clientcrmid)
            {
                try
                {
                    //Get client data from Client
                    Contact contact = new Contact();
                    DataTable xlReader = XLSQL.ReturnTable("Select * from IPSContact('" + clientcrmid + "') where ID='" + ID + "'");
                    if (xlReader.Rows.Count == 1)
                    {
                        contact.crmID = ID;
                        contact.name = xlReader.Rows[0]["name"].ToString();
                        contact.type = xlReader.Rows[0]["ctype"].ToString();
                        contact.address1 = xlReader.Rows[0]["address1"].ToString();
                        contact.address2 = xlReader.Rows[0]["address2"].ToString();
                        contact.address3 = xlReader.Rows[0]["address3"].ToString();
                        contact.address4 = xlReader.Rows[0]["address4"].ToString();
                        contact.address5 = xlReader.Rows[0]["address5"].ToString();
                        contact.postcode = xlReader.Rows[0]["postcode"].ToString();
                        if (contact.address1 != "")
                        {
                            contact.addressBlock = contact.address1 + Environment.NewLine;
                        }
                        if (contact.address2 != "")
                        {
                            contact.addressBlock += contact.address2 + Environment.NewLine;
                        }
                        if (contact.address3 != "")
                        {
                            contact.addressBlock += contact.address3 + Environment.NewLine;
                        }
                        if (contact.address4 != "")
                        {
                            contact.addressBlock += contact.address4 + Environment.NewLine;
                        }
                        if (contact.address5 != "")
                        {
                            contact.addressBlock += contact.address5 + Environment.NewLine;
                        }
                        if (contact.postcode != "")
                        {
                            contact.addressBlock += contact.postcode;
                        }
                        contact.fax = xlReader.Rows[0]["fax"].ToString(); ;
                    }
                    return contact;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("Insol-FetchContact", ex.ToString());
                    return null;
                }
            }
        }

        public class KeyData
        {
            public String caseType { get; set; }
            public String sign { get; set; }
            public String title { get; set; }
            //public DateTime appdate { get; set; }

            public static KeyData FetchKeyData(string ID)
            {
                try
                {
                    KeyData data = new KeyData();
                    DataTable xlReader = XLSQL.ReturnTable("Select * from insol where crmID='" + ID + "'");
                    if (xlReader.Rows.Count == 1)
                    {
                        //data.appdate = Convert.ToDateTime(xlReader["appdate"].ToString()); //DateTime.Parse(xlReader.NiceString("appdate"));
                        data.caseType = xlReader.Rows[0]["type"].ToString();
                        data.sign = xlReader.Rows[0]["sign"].ToString();
                        data.title = xlReader.Rows[0]["app"].ToString();
                    }
                    return data;
                }
                catch (Exception ex)
                {
                    XLtools.LogException("Insol-FetchKeyData", ex.ToString());
                    return null;
                }
            }
        }

        public static string GetSubject(string crmiID)
        {
            try
            {
                Contact contact = new Contact();
                string subject = "";    
                DataTable xlReader = XLSQL.ReturnTable("SELECT [XLant].[dbo].[Insol_Subject] ('" + crmiID + "') as subject");
                if (xlReader.Rows.Count == 1)
                {
                    subject = xlReader.Rows[0]["subject"].ToString(); ;
                }
                return subject;
            }
            catch (Exception ex)
            {
                XLtools.LogException("Insol-GetSubject", ex.ToString());
                return null;
            }
        }
    }
}
