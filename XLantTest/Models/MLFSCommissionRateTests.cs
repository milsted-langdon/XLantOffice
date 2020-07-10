using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSCommissionRateTests
    {
        [TestMethod()]
        public void CreateListTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("AdvisorId", typeof(int));
            table.Columns.Add("StartingValue", typeof(decimal));
            table.Columns.Add("EndingValue", typeof(decimal));
            table.Columns.Add("Percentage", typeof(decimal));
            for (int i=1; i < 4; i++)
            {
                DataRow row = table.NewRow();
                row["Id"] = i;
                row["AdvisorId"] = 4;
                row["StartingValue"] = 100000 * (i-1);
                row["EndingValue"] = 100000 * i;
                row["Percentage"] = 0.50 - (i*0.10);
                table.Rows.Add(row);
            }

            //act
            List<MLFSCommissionRate> rates = MLFSCommissionRate.CreateList(table);

            //assert
            Assert.AreEqual(0, rates[0].StartingValue, "First entry doesn't start at 0");
            Assert.AreEqual((decimal)0.20, rates[2].Percentage, "Percentage does not match");
        }

        public void CreateEntryFromDataRow()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("AdvisorId", typeof(int));
            table.Columns.Add("StartingValue", typeof(decimal));
            table.Columns.Add("EndingValue", typeof(decimal));
            table.Columns.Add("Percentage", typeof(decimal));
            
            DataRow row = table.NewRow();
            row["Id"] = 1;
            row["AdvisorId"] = 4;
            row["StartingValue"] = 0;
            row["EndingValue"] = 250000;
            row["Percentage"] = 0.40;
            
            //act
            MLFSCommissionRate rate = new MLFSCommissionRate(row);

            //assert
            Assert.AreEqual(250000, rate.EndingValue, "The ending value doesnot match");
            Assert.AreEqual(4, rate.AdvisorId, "AdvisorId does not match");
        }
    }
}