using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSReportingPeriodTests
    {
        [TestMethod()]
        public void MLFSReportingPeriodTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Month", typeof(int));
            table.Columns.Add("Year", typeof(int));
            table.Columns.Add("FinancialYear", typeof(string));
            table.Columns.Add("ReportOrder", typeof(int));

            DataRow row = table.NewRow();
            row["Id"] = 1;
            row["Description"] = "May 2020";
            row["Month"] = 5;
            row["Year"] = 2020;
            row["FinancialYear"] = "20/21";
            row["ReportOrder"] = 1;

            //act
            MLFSReportingPeriod period = new MLFSReportingPeriod(row);

            //assert
            Assert.AreEqual("May 2020", period.Description, "The description does not match");
            Assert.AreEqual(2020, period.Year, "The year doesn't match");
        }
    }
}