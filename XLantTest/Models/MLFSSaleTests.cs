using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSSaleTests
    {
        [TestMethod()]
        public void MLFSSaleCreateFromIODataRowTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Reference Number", typeof(string));
            table.Columns.Add("Fee Owner.Full Name", typeof(string));
            table.Columns.Add("Fee Owner.Id", typeof(string));
            table.Columns.Add("Fee Owner2.Full Name", typeof(string));
            table.Columns.Add("Fee Owner2.Id", typeof(string));
            table.Columns.Add("Selling Advisor.Id", typeof(int));
            table.Columns.Add("Selling Advisor.Full Name", typeof(string));
            table.Columns.Add("Related Plan Type", typeof(string));
            table.Columns.Add("Net Amount", typeof(decimal));
            table.Columns.Add("VAT", typeof(decimal)); 
            table.Columns.Add("Invoice Date", typeof(DateTime));
            table.Columns.Add("Related Plan Reference", typeof(string));


            DataRow row = table.NewRow();
            row["Id"] = "123465";
            row["Reference Number"] = "IOF123456";
            row["Fee Owner.Id"] = "654987";
            row["Fee Owner.Full Name"] = "John Bloggs";
            row["Fee Owner2.Full Name"] = "Jane Bloggs";
            row["Fee Owner2.Id"] = "321654";
            row["Selling Advisor.Full Name"] = "Joe Smith";
            row["Selling Advisor.Id"] = "9";
            row["Related Plan Type"] = "Pension";
            row["Invoice Date"] = DateTime.Now;
            row["Net Amount"] = 1000;
            row["VAT"] = 100;
            row["Related Plan Reference"] = "IOB098767";
            table.Rows.Add(row);

            //act
            MLFSSale sale = new MLFSSale(row);

            //assert
            Assert.AreEqual("Joe Smith", sale.AdvisorName, "Advisor doesn't match");
            Assert.AreEqual(1100, sale.GrossAmount, "Gross amount does not match");
            Assert.AreEqual(0, sale.Paid, "Paid amount not defaulted to 0");
        }
    }
}