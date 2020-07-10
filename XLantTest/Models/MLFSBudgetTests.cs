using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSBudgetTests
    {
        [TestMethod()]
        public void CreateListTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("AdvisorId", typeof(int));
            table.Columns.Add("MLFSReportPeriodId", typeof(int));
            table.Columns.Add("Budget", typeof(decimal));
            for (int i=1; i < 4; i++)
            {
                DataRow row = table.NewRow();
                row["Id"] = i;
                row["AdvisorId"] = 4;
                row["MLFSReportPeriodId"] = i;
                row["Budget"] = 100000 * i;
                table.Rows.Add(row);
            }

            //act
            List<MLFSBudget> budgets = MLFSBudget.CreateList(table);

            //assert
            Assert.AreEqual("4", budgets[0].AdvisorId, "Advisor Id doesn't match");
            Assert.AreEqual(300000, budgets[2].Budget, "Budget does not match");
        }

        public void CreateEntryFromDataRow()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("AdvisorId", typeof(int));
            table.Columns.Add("MLFSReportPeriodId", typeof(int));
            table.Columns.Add("Budget", typeof(decimal));
            
            DataRow row = table.NewRow();
            row["Id"] = 1;
            row["AdvisorId"] = 4;
            row["MLFSReportingPeriodId"] = 1;
            row["Budget"] = 250000;
            
            //act
            MLFSBudget budget = new MLFSBudget(row);

            //assert
            Assert.AreEqual(250000, budget.Budget, "The budget does not match");
            Assert.AreEqual(1, budget.MLFSReportPeriodId, "The reportingperiod id does not match");
        }
    }
}