using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XlantWord
{
    public static class XLtools
    {
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
                MessageBox.Show("Unable to locate column " + colname);
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
    }
}
