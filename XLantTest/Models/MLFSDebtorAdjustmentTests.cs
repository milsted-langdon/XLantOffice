using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSDebtorAdjustmentTests
    {
        [TestMethod()]
        public void CloneTest()
        {
            //arrange
            MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment()
            {
                Id = 1,
                ReportingPeriodId = 2,
                DebtorId = 3,
                ReceiptId = 4,
                Amount = 100,
                IsVariance = false,
                NotTakenUp = false
            };

            //act
            MLFSDebtorAdjustment adj2 = adj.Clone();

            //assert
            Assert.AreEqual(adj.Amount, adj2.Amount, "Amounts don't match");
            Assert.AreEqual(adj.Receipt, adj2.ReceiptId, "Receipt Id don't match");
            Assert.AreEqual(adj.NotTakenUp, adj2.NotTakenUp, "NTU status doesn't match");
        }
    }
}