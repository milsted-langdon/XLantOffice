using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XLant
{
    public static class XLtools
    {
        //The location of the settings file
        public static XDocument settingsDoc = XDocument.Load(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\settings.xml");
        
        //Personal settings file which can be created on the fly if it doesn't exist
        public static XDocument personalDoc()
        {
            XDocument xDoc;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\personal.xml";
            if (File.Exists(docPath))
            {
                xDoc = XDocument.Load(docPath);
            }
            else
            {
                xDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("PersonalSettings", ""));
                var pSettings = xDoc.Descendants("PersonalSettings").FirstOrDefault();
                pSettings.Add(new XElement("Prompt", "true"));
                xDoc.Save(docPath);
            }
            return xDoc;
        }

        //This handles null values given the column number
        public static string NiceString(this SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? "" : reader.GetString(ordinal);
        }

        //This handles null values given the column name gets the column number and calls the above
        public static string NiceString(this SqlDataReader reader, string colname)
        {
            try
            {
                int ordinal = reader.GetOrdinal(colname);
                return reader.NiceString(ordinal);
            }
            catch
            {
                return null;
            }
            
        }

        //This method is to handle if element is missing
        public static string ElementValueNull(this XElement element)
        {
            if (element != null)
                return element.Value;

            return "";
        }

        //This method is to handle if attribute is missing
        public static string AttributeValueNull(this XElement element, string attributeName)
        {
            if (element == null)
                return "";
            else
            {
                XAttribute attr = element.Attribute(attributeName);
                return attr == null ? "" : attr.Value;
            }
        }

        //This method is to handle if attribute is missing
        public static int AttributeIntNull(this XElement element, string attributeName)
        {
            if (element == null)
                return 0;
            else
            {
                XAttribute attr = element.Attribute(attributeName);
                if (attr == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt16(attr.Value);
                }
            }
        }

        public static void Clear(this XLMain.EntityCouplet eC)
        {
            eC.crmID = "";
            eC.name = "";
        }

        public static List<XLMain.EntityCouplet> StaffList(XLMain.Staff user, XLMain.Client client, Boolean incSec, Boolean incML=false, List<XLMain.EntityCouplet> userList=null)
        {
            //create list
            List<XLMain.EntityCouplet> users = new List<XLMain.EntityCouplet>();
            //check whether we have been given a list
            if (userList==null)
            {
            	//populate it with everyone
            	userList = XLMain.Staff.AllStaff();
            }
            users = userList;
            //assign list to temporary list for further adjustment
            int i = 0;
            //place current and connected users at the top of the list
            if (user != null)
            {
                users.Insert(i, new XLMain.EntityCouplet(user.crmID, user.name));
                i++;
            }
            if (client != null)
            {
                if (client.partner != null)
                {
                    users.Insert(i, new XLMain.EntityCouplet(client.partner.crmID, client.partner.name));
                    i++;
                }
                if (client.manager != null)
                {
                    users.Insert(i, new XLMain.EntityCouplet(client.manager.crmID, client.manager.name)); i++;
                }
                List<XLMain.Staff> conStaff = XLMain.Staff.connectedStaff(client.crmID);
                if (conStaff != null)
                {
                    foreach (XLMain.Staff staff in conStaff)
                    {
                        users.Insert(i, new XLMain.EntityCouplet(staff.crmID, staff.name));
                        i++;
                    }
                }

                //add secretarial
                if (incSec)
                {
                    XLMain.EntityCouplet secretarial = new XLMain.EntityCouplet();
                    secretarial.name = "Secretarial - " + client.manager.office;
                    users.Insert(i, secretarial);
                    i++;
                }
            }
            //add ML
            if (incML)
            {
                XLMain.EntityCouplet mL = new XLMain.EntityCouplet();
                mL.name = "Milsted Langdon LLP";
                users.Insert(i, mL);
                i++;
            }

            List<XLMain.EntityCouplet> dedupedList = users.GroupBy(p => p.crmID).Select(g => g.First()).ToList(); 
            return dedupedList;
        }

        public static void LogException(string app, string e)
        {
            
            //try to store it to the database for easier access.
            bool toDb = DatabaseLog(app, e);
        }

        private static bool DatabaseLog(string command, string err)
        {
            try
            {
                bool success = false;
                string user = Environment.UserName;
                string machine = Environment.MachineName;
                XLMain.Error error = new XLMain.Error(DateTime.Now, user, machine, command, err);
                success = error.SavetoDb();
                return success;
            }
            catch 
            {
                return false;
            }
            
        }

        public static string TempPath()
        {
            //blanked in an attempt to handle the weird random saving
            string folder = "";
            folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";

            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch
                {
                    folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\temp\\";
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                }
            }
            return folder;
        }

        /// <summary>
        /// Converts a linq list to a data table
        /// Credit to - https://www.c-sharpcorner.com/UploadFile/0c1bb2/join-two-datatable-using-linq-in-Asp-Net-C-Sharp/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Linqlist"></param>
        /// <returns></returns>
        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type IcolType = GetProperty.PropertyType;

                        if ((IcolType.IsGenericType) && (IcolType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            IcolType = IcolType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, IcolType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo p in columns)
                {
                    dr[p.Name] = p.GetValue(Record, null) == null ? DBNull.Value : p.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Converts nullable decimal to decimal, repalcing null with 0
        /// </summary>
        /// <param name="value">Decimal to alter</param>
        /// <returns>decimal</returns>
        public static decimal HandleNull(decimal? value)
        {
            decimal d = value != null ? (decimal)value : 0;
            return d;
        }

        /// <summary>
        /// Converts string to decimal, repalcing null or empty with 0
        /// </summary>
        /// <param name="value">string to convert and alter</param>
        /// <returns>decimal</returns>
        public static decimal HandleNull(string value)
        {
            decimal d = 0;
            if (!decimal.TryParse(value, out d))
            {
                d = 0;
            }
            return d;
        }

        public static DateTime? HandleStringToDate(string value)
        {
            DateTime? nullableDate = new DateTime();
            DateTime date = new DateTime();
            if (DateTime.TryParse(value, out date))
            {
                nullableDate = (DateTime?)date;
            }
            else
            {
                nullableDate = null;
            }
            return nullableDate;
        }
    }
}
