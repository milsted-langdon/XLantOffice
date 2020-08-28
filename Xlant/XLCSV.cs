using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XLant
{
    public class XLCSV
    {
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
            for(int i = 0; i < headers.Length; i++)
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
