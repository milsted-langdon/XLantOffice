using Microsoft.AspNetCore.Http;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XLantCore;

namespace XLantExcel
{
    public class XLSheet
    {
        /// <summary>
        /// Create a new worksheet and add and format data
        /// </summary>
        /// <param name="table">the table of data to be displayed</param>
        /// <param name="sheetName">The name to be given to the sheet (and tables)</param>
        /// <param name="splitTableOn">If you are going to split the table in two parts</param>
        /// <param name="isJsonData">Whether the data is json or SQL, sql data has more information about data types</param>
        /// <param name="firstNumberColumn">If provided everything after this will be formatted as a number</param>
        /// <param name="extraWideColumns">The columns with lots of text in</param>
        public static void CreateWorkSheet(System.Data.DataTable table, string sheetName, string splitTableOn = "", bool isJsonData = false, string firstNumberColumn = "", int[] extraWideColumns = null)
        {
            Microsoft.Office.Interop.Excel.Worksheet worksheet = new Microsoft.Office.Interop.Excel.Worksheet();
            List<System.Data.DataTable> tables = new List<System.Data.DataTable>();
            if (sheetName == null)
            {
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
            }
            else
            {
                worksheet = Globals.ThisAddIn.Application.Worksheets.Add(After: Globals.ThisAddIn.Application.ActiveSheet);
                worksheet.Name = sheetName;
            }
            worksheet.UsedRange.Clear();
            if (!String.IsNullOrEmpty(splitTableOn))
            {
                //split the data set in two to handle the two parts
                var rows = table.AsEnumerable().Where(x => String.IsNullOrEmpty(x.Field<string>(splitTableOn)));
                System.Data.DataTable topPartTable = rows.Any() ? rows.CopyToDataTable() : table.Clone();
                topPartTable.TableName = sheetName + "_1";
                rows = table.AsEnumerable().Where(x => !String.IsNullOrEmpty(x.Field<string>(splitTableOn)));
                System.Data.DataTable bottomPartTable = rows.Any() ? rows.CopyToDataTable() : table.Clone();
                bottomPartTable.TableName = sheetName + "_2";
                tables.Add(topPartTable);
                tables.Add(bottomPartTable);
            }
            else
            {
                table.TableName = sheetName;
                tables.Add(table);
            }
            int lastRow = 1;
            foreach (System.Data.DataTable t in tables)
            {
                RemoveNonDisplayColumns(t);
                Microsoft.Office.Interop.Excel.Range rng = AddDataToSheet(t, worksheet, lastRow);
                AddExcelTable(rng, t, t.TableName, true, isJsonData, firstNumberColumn);
                FormatTable(t, rng, isJsonData, firstNumberColumn, extraWideColumns);
                lastRow += t.Rows.Count + 8; //allow for total and a gap
            }
        }
        /// <summary>
        /// Handles the peculiarity and complexity of the claims experience data
        /// </summary>
        /// <param name="package">a data object containing the data relating to the Claims Experience</param>
        public static void HandleClaimsExperience(JObject package)
        {
            //sort summary data
            JArray summary = (JArray)package["Summary"];
            System.Data.DataTable summaryTable = FlattenCRYearSummaries(summary);
            summaryTable.TableName = "Summary";
            CreateWorkSheet(summaryTable, "Summary", "", true, "Case_Reserve", new int[] { 1 });

            //sort each of the practitioner tabs
            foreach (JObject p in package["Practitioner"])
            {
                string name = p["Practitioner"].ToString();
                if (p["Closed"].ToString() == "True")
                {
                    name = "Closed-" + name;
                }
                System.Data.DataTable table = FlattenCRYearSummaries((JArray)p["Entries"]);
                table.TableName = name;
                CreateWorkSheet(table, name, "NonDisplay_Claim", true, "SPS_Value", new int[] { 2, 3 });
            }
        }

        /// <summary>
        /// If any columns are pre-fixed with "NonDisplay_" they will be removed before the table is created, useful if needed for sorting or splitting but not for display
        /// </summary>
        /// <param name="table">the data table to assess</param>
        private static void RemoveNonDisplayColumns(System.Data.DataTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string columnName = table.Columns[i].ColumnName;
                if (columnName.Contains("NonDisplay_"))
                {
                    table.Columns.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Formats numerical and date columns (dates are best provided in yyyy-MM-dd format)
        /// </summary>
        /// <param name="table">The DataTable - if sql data we can assess the data types based on this</param>
        /// <param name="range">our table in excel</param>
        /// <param name="isJsonData">If JsonData we can't use datatypes (they are all strings) so the following is useful - it will format as data where the column contains "Date"</param>
        /// <param name="firstMoneyColumn">The first column after which everything is numerical, dates are formatted afterwards so they will automatically override</param>
        /// <param name="extraWideColumns">Any columns which have lots of text which could spaced better</param>
        /// <param name="decimalPlaces">Either 0 or 2, 2 is the default</param>
        private static void FormatTable(System.Data.DataTable table, Range range, bool isJsonData = false, string firstMoneyColumn = "", int[] extraWideColumns = null, int decimalPlaces = 2)
        {
            List<int> dateColumns = GetColumnList(table, "DateTime", isJsonData);
            List<int> moneyColumns = GetColumnList(table, "Decimal", isJsonData, firstMoneyColumn);
            foreach (int i in moneyColumns)
            {
                Range column = range.Columns.EntireColumn[i];
                if (decimalPlaces == 0)
                {
                    column.NumberFormat = "#,##0;-#,##0;-";
                }
                else
                {
                    column.NumberFormat = "#,##0.00;-#,##0.00;-";
                }
                
            }
            foreach (int i in dateColumns)
            {
                Range column = range.Columns.EntireColumn[i];
                column.NumberFormat = "dd/mm/yyyy";
            }
            foreach (Range col in range.Columns)
            {
                if (extraWideColumns != null && extraWideColumns.Contains(col.Column))
                {
                    col.ColumnWidth = 40;
                }
                else
                {
                    col.ColumnWidth = 25;
                }
            }
        }

        /// <summary>
        /// Places the data in the sheet
        /// </summary>
        /// <param name="dataTable">The data table we are adding</param>
        /// <param name="sheetToAddTo">The sheet we are adding it to</param>
        /// <param name="startingRow">By default it will start at row 1 otherwise provde a different value</param>
        /// <returns>The range of the table created</returns>
        private static Microsoft.Office.Interop.Excel.Range AddDataToSheet(System.Data.DataTable dataTable, Microsoft.Office.Interop.Excel.Worksheet sheetToAddTo, int startingRow = 1)
        {
            //create the object to store the column names
            object[,] columnNames;
            columnNames = new object[1, dataTable.Columns.Count];

            //add the columns names from the datatable
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                columnNames[0, i] = PrettyColumnName(dataTable.Columns[i].ColumnName);
            }

            //get a range object that the columns will be added to
            Range openingCell = sheetToAddTo.Cells[startingRow, 1];
            Range closingCell = sheetToAddTo.Cells[startingRow, dataTable.Columns.Count];
            Range columnsNamesRange = sheetToAddTo.Range[openingCell, closingCell];

            //a simple assignement allows the data to be transferred quickly
            columnsNamesRange.Value2 = columnNames;

            //release the columsn range object now it is finished with
            columnsNamesRange = null;

            //create the object to store the dataTable data
            object[,] rowData;
            rowData = new object[dataTable.Rows.Count, dataTable.Columns.Count];

            //insert the data into the object[,]
            for (int iRow = 0; iRow < dataTable.Rows.Count; iRow++)
            {
                for (int iCol = 0; iCol < dataTable.Columns.Count; iCol++)
                {
                    rowData[iRow, iCol] = dataTable.Rows[iRow][iCol];
                }
            }

            //get a range to add the table data into 
            //it is one row down to avoid the previously added columns
            openingCell = sheetToAddTo.Cells[startingRow + 1, 1];
            closingCell = sheetToAddTo.Cells[dataTable.Rows.Count + startingRow, dataTable.Columns.Count];
            Range dataCells = sheetToAddTo.Range[openingCell, closingCell];

            //assign data to worksheet
            dataCells.Value2 = rowData;

            //release range
            dataCells = null;

            //return the range to the new data
            openingCell = sheetToAddTo.Cells[startingRow, 1];
            return sheetToAddTo.Range[openingCell, closingCell];

        }


        /// <summary>
        /// Converts a table of data to an Excel Table with corresponding filters and totals
        /// </summary>
        /// <param name="tableRange">The range of our data</param>
        /// <param name="table">The datatable on which that data is based</param>
        /// <param name="tableName">The name to give to the Table</param>
        /// <param name="addTotals">Whether to add totals or not</param>
        /// <param name="isJsonData">If json we have less datatype information</param>
        /// <param name="firstTotalColumn">The first total column to add, everything afterwards will also be totalled</param>
        private static void AddExcelTable(Microsoft.Office.Interop.Excel.Range tableRange, System.Data.DataTable table, string tableName, bool addTotals, bool isJsonData, string firstTotalColumn)
        {
            Worksheet activeSheet = (Microsoft.Office.Interop.Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
            ListObject newList = tableRange.Worksheet.ListObjects.Add(Microsoft.Office.Interop.Excel.XlListObjectSourceType.xlSrcRange, tableRange, null, Microsoft.Office.Interop.Excel.XlYesNoGuess.xlYes, tableRange);
            newList.Name = tableName;
            for (int x = 1; x == newList.ListColumns.Count; x++)
            {
                var column = newList.ListColumns.get_Item(x);
            }
            if (addTotals)
            {
                newList.ShowTotals = true;

                List<int> moneyColumns = GetColumnList(table, "Decimal", isJsonData, firstTotalColumn);
                foreach (int i in moneyColumns)
                {
                    newList.ListColumns[i].TotalsCalculation = XlTotalsCalculation.xlTotalsCalculationSum;
                }
            }

        }

        /// <summary>
        /// Provides a list of columns which match the criteria
        /// </summary>
        /// <param name="table">The table you want to search</param>
        /// <param name="dataType">the DataType you are looking for</param>
        /// <param name="isJsonData">if the data has come from sql it carries datatypes and can use this but Json doesn't</param>
        /// <param name="firstColumn">This will just return this column and all that follow, useful for when all the numbers are to the right of the description columns</param>
        /// <returns>List of column indicies</returns>
        private static List<int> GetColumnList(System.Data.DataTable table, string dataType, bool isJsonData = false, string firstColumn = "")
        {
            int x = 1;
            List<int> columns = new List<int>();
            bool startCounting = false;
            foreach (DataColumn column in table.Columns)
            {
                if (!isJsonData)
                {
                    if (column.DataType.Name == dataType)
                    {
                        columns.Add(x);
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(firstColumn))
                    {
                        if (dataType == "DateTime")
                        {
                            if (column.ColumnName.Contains("Date"))
                            {
                                columns.Add(x);
                            }
                        }
                    }
                    else
                    {
                        if (column.ColumnName == firstColumn)
                        {
                            startCounting = true;
                        }
                        if (startCounting)
                        {
                            columns.Add(x);
                        }
                    }
                }
                x++;
            }
            return columns;
        }

        /// <summary>
        /// Remove any underscores to make column headers more human readable
        /// </summary>
        /// <param name="name">The column string</param>
        /// <returns>the string with spaces rather than underscores</returns>
        private static string PrettyColumnName(string name)
        {
            return name.Replace("_", " ");
        }


        /// <summary>
        /// For the claim register flatten nested year summaries to make an excel table
        /// </summary>
        /// <param name="array">An array of entries</param>
        /// <returns>a flattened datatable</returns>
        public static System.Data.DataTable FlattenCRYearSummaries(JArray array)
        {

            JArray flatArray = new JArray();
            foreach (JObject o in array)
            {
                JObject flattenedO = JObject.FromObject(FlattenCREntries(o));
                flatArray.Add(flattenedO);
            }
            System.Data.DataTable table = (System.Data.DataTable)JsonConvert.DeserializeObject<System.Data.DataTable>(flatArray.ToString());
            return table;
        }

        /// <summary>
        /// Flatten the YearSummaries of entries for the claims experience
        /// </summary>
        /// <param name="entry">the individual entry to flatten</param>
        /// <returns>Dictionary which reflect the json string pairings</returns>
        private static Dictionary<string, string> FlattenCREntries(JObject entry)
        {
            IEnumerable<JToken> jTokens = entry.Descendants().Where(p => p.Count() == 0);
            string year = "";
            Dictionary<string, string> results = jTokens.Aggregate(new Dictionary<string, string>(), (properties, jToken) =>
            {
                string name = jToken.Path;
                if (name.Contains("YearSummaries"))
                {
                    if (name.EndsWith("Year"))
                    {
                        year = jToken.ToString();
                    }
                    name = year + " - " + name.Substring(name.LastIndexOf('.') + 1);
                }
                else
                {
                    name = name.Substring(name.LastIndexOf('.') + 1);
                }
                properties.Add(name, jToken.ToString());
                return properties;
            });
            return results;
        }

        /// <summary>
        /// Builds the MLFS Director's report from two tables
        /// </summary>
        /// <param name="periodId">The ID of the period we are adding these to</param>
        /// <param name="FeeCSV">THe location of the fee csv</param>
        /// <param name="PlanCSV">The location of the plan csv</param>
        /// <param name="fciCSV">The location of the FCI csv</param>
        /// <returns>Success or Failure</returns>
        public static async Task<string> BuildMLFSDirectorsReport(int periodId, string feeCSV, string planCSV, string fciCSV)
        {
            Uri webApi = new Uri(APIAccess.baseURL + "/MLFSSale/PostMonthlyData?periodId=" + periodId);
            FileStream saleStream = File.OpenRead(feeCSV);
            FileStream planStream = File.OpenRead(planCSV);
            FileStream fciStream = File.OpenRead(fciCSV);
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(saleStream), "salesCSV", Path.GetFileName(feeCSV));
            content.Add(new StreamContent(planStream), "planCSV", Path.GetFileName(planCSV));
            content.Add(new StreamContent(fciStream), "feeCSV", Path.GetFileName(fciCSV));

            var client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(webApi, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return "Success";
            }
            else
            {
                return "Failure";
            }
        }
    }
}
