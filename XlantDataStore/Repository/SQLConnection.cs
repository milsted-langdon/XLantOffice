using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using XLantCore;


namespace XLantDataStore.Repository
{
    public class SQLConnection
    {
        private static string BuildConnectionString()
        {
            string conn = "";
            XDocument settingsDoc = Tools.settingsDoc;
            //query the setting files and try to find a match
            XElement setting = (from map in settingsDoc.Descendants("ConnectionStr")
                                select map).FirstOrDefault();
            if (setting != null)
            {
                conn = "user id=XLantLogin;password=;server=";
                conn += setting.Attribute("Server").Value;
                conn += ";database=";
                conn += setting.Attribute("db").Value;
                conn += ";connection timeout=15";
            }
            return conn;
        }

        private static SqlConnection ConnecttoSQL()
        {

            string conn = BuildConnectionString();
            SqlConnection xLConnection = new SqlConnection(conn);
            try
            {
                xLConnection.Open();
                return xLConnection;
            }
            catch (Exception e)
            {
                //Logging
                //XLtools.LogException("XLSQL-Connection", e.ToString());
                return null;
            }
        }

        public static DataTable ReturnTable(string query, string param1 = null, string param2 = null)
        {
            try
            {
                SqlDataReader xLReader = null;
                DataTable xlDataTable = new DataTable();
                using (SqlConnection xlConnection = ConnecttoSQL())
                {
                    using (SqlCommand xLCommand = new SqlCommand(query, xlConnection))
                    {
                        if (param1 != null)
                        {
                            xLCommand.Parameters.AddWithValue("param1", param1);
                            if (param2 != null)
                            {
                                xLCommand.Parameters.AddWithValue("param2", param2);
                            }
                        }
                        xLReader = xLCommand.ExecuteReader();
                        xlDataTable.Load(xLReader);
                        return xlDataTable;
                    }
                }
            }
            catch (Exception e)
            {
                //Logging
                //XLtools.LogException("XLSQL-Returntable", e.ToString());
                return null;
            }
        }

        public static bool RunCommand(string query, List<SqlParameter> parameterCollection)
        {
            try
            {
                int i = 0;
                using (SqlConnection xlConnection = ConnecttoSQL())
                {
                    using (SqlCommand xLCommand = new SqlCommand(query, xlConnection))
                    {
                        if (parameterCollection != null)
                        {
                            foreach (SqlParameter p in parameterCollection)
                            {
                                xLCommand.Parameters.Add(p);
                            }
                        }

                        i = xLCommand.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //Logging
                //XLtools.LogException("XLSQL-RunCommand", e.ToString());
                return false;
            }
        }
    }
}
