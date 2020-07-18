using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace XLantCore
{
    public class Tools
    {
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

        /// <summary>
        /// Takes a response string and strips out the items array
        /// </summary>
        /// <param name="response">the full response or Serialized JObject</param>
        /// <param name="arrayContainer">the name of the array within the response, default is "items"</param>
        /// <returns>JArray</returns>
        public static JArray ExtractItemsArrayFromJsonString(string content, string arrayContainer = "items")
        {
            if (content == null)
            {
                return null;
            }
            else
            {

                //check whether it is already an array
                JArray _array = new JArray();
                JToken token = JToken.Parse(content);
                if (token is JObject)
                {
                    JObject obj = JObject.Parse(content);
                    _array = (JArray)obj[arrayContainer];
                }
                else
                {
                    _array = token as JArray;
                }

                return _array;
            }
        }

        /// <summary>
        /// Takes the contact details and seperates numbers from emails
        /// </summary>
        /// <param name="_array">The array from the json response from IO</param>
        /// <param name="emails">whether to return emails or numbers default is false thereby providing numbers</param>
        /// <returns>Searlialised JArray</returns>
        public static JArray SplitContactDetails(JArray _array, bool emails = false)
        {
            JArray filteredArray = new JArray();
            if (emails)
            {
                //how many do we have
                int elementCount = _array.AsEnumerable().Where(x => x["type"].ToString().Contains("Email")).Count();
                if (elementCount == 1)
                {
                    filteredArray.Add(_array.AsEnumerable().Where(x => x["type"].ToString().Contains("Email")));
                }
                else if (elementCount > 1)
                {
                    filteredArray = _array.AsEnumerable().Where(x => x["type"].ToString().Contains("Email")) as JArray;
                }
            }
            else
            {
                //how many do we have
                int elementCount = _array.AsEnumerable().Where(x => !x["type"].ToString().Contains("Email")).Count();
                if (elementCount == 1)
                {
                    filteredArray.Add(_array.AsEnumerable().Where(x => !x["type"].ToString().Contains("Email")));
                }
                else if (elementCount > 1)
                {
                    filteredArray = _array.AsEnumerable().Where(x => !x["type"].ToString().Contains("Email")) as JArray;
                }
            }

            return filteredArray;
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

        /// <summary>
        /// Converts a csv to datatable
        /// Credit to - https://immortalcoder.blogspot.com/2013/12/convert-csv-file-to-datatable-in-c.html
        /// </summary>
        /// <param name="fileLocation">the location of the csv first row should contain the header</param>
        /// <param name="isQuotes">if the value is in "" speachmarks true</param>
        /// <returns>DataTable</returns>
        public static DataTable ConvertCSVToDataTable(string fileLocation, bool isQuotes = true)
        {
            DataTable table = new DataTable();
            StreamReader sr = new StreamReader(fileLocation);
            string[] headers = sr.ReadLine().Split(',');
            for (int i = 0; i < headers.Length; i++)
            {
                if (isQuotes)
                {
                    headers[i] = headers[i].Trim('"');
                }
                table.Columns.Add(headers[i]);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = table.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    if (isQuotes)
                    {
                        rows[i] = rows[i].Trim('"');
                    }
                    dr[i] = rows[i];
                }
                table.Rows.Add(dr);
            }
            return table;
        }
    }
}
