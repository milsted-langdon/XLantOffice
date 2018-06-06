using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XlantWord
{
    public class XLSQL
    {
        private static string BuildConnectionString()
        {
            string conn = "";
            XDocument settingsDoc = XLDocument.settingsDoc;
            //query the setting files and try to find a match
            XElement setting = (from map in settingsDoc.Descendants("ConnectionStr")
                                select map).FirstOrDefault();
            if (setting != null)
            {
                conn = "user id=XLantLogin;password=mswlBtCk11OF;server=";
                conn += setting.Attribute("Server").Value;
                conn += ";Trusted_Connection=yes;database=";
                conn += setting.Attribute("db").Value;
                conn += ";connection timeout=15";
            }
            return conn;
        }
        
        public static SqlConnection ConnecttoSQL()
        {

            string conn = BuildConnectionString();
            SqlConnection xLConnection = new SqlConnection(conn);
            try
            {
                xLConnection.Open();
                return xLConnection;
            }
            catch(Exception e)
            {
                MessageBox.Show("The connection to the database was declined: " + e.ToString());
                return null;
            }
        }

        public static SqlDataReader ReaderQuery(string query)
        {
            SqlDataReader xLReader = null;
            using (SqlCommand xLCommand = new SqlCommand(query, ConnecttoSQL()))
            {
                try
                {
                    xLReader = xLCommand.ExecuteReader();
                }
                catch (Exception e)
                {
                    MessageBox.Show("The query was rejected: " + e.ToString());
                    return null;
                }
            }
            return xLReader;
        }
    }
}
