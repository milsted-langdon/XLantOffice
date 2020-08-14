using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSPlanTests
    {
        [TestMethod()]
        public void ParsePlanStatusTest()
        {
            //arrange
            string s = "In force";
            string u = "fjg9odjfgd";

            //act
            PlanStatus status = MLFSPlan.ParsePlanStatus(s);
            PlanStatus status2 = MLFSPlan.ParsePlanStatus(u);

            //assert
            Assert.AreEqual(PlanStatus.InForce, status, "Status not in force");
            Assert.AreEqual(PlanStatus.Unknown, status2, "Status not set to unknown where no match");
        }
    }
}