using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace XLantOutlook
{
    public class XLSQL
    {
        public static SqlConnection ConnecttoSQL()
        {
            SqlConnection xLConnection = new SqlConnection("user id=XLReader;password=XL111415; server=Sauron\\SQLExpress;Trusted_Connection=yes;database=Northwnd;connection timeout=15");
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
            SqlCommand xLCommand = new SqlCommand(query, ConnecttoSQL());
            xLReader = xLCommand.ExecuteReader();
            return xLReader;
        }
    }
}
