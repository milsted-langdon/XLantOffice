using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSAdvisorTests
    {
        [TestMethod()]
        public void AssignTest()
        {
            //arrange
            List<MLFSAdvisor> advisors = new List<MLFSAdvisor>();
            MLFSAdvisor advisor = new MLFSAdvisor()
            {
                Id = 6,
                FirstName = "Joe",
                LastName = "Smith",
                PrimaryID = "4",
                Username = "jsmith"
            };
            advisors.Add(advisor);
            MLFSAdvisor unknownAdvisor = new MLFSAdvisor()
            {
                Id = 2,
                FirstName = "Unknown",
                LastName = "",
                PrimaryID = "",
                Username = "unknown"
            };
            advisors.Add(unknownAdvisor);

            //act
            MLFSAdvisor adv = MLFSAdvisor.Assign("4", advisors);

            //assert
            Assert.AreEqual(6, adv.Id, "Assignment has not worked");
        }
        [TestMethod()]
        public void AssignTestWithUnknown()
        {
            //arrange
            List<MLFSAdvisor> advisors = new List<MLFSAdvisor>();
            MLFSAdvisor advisor = new MLFSAdvisor()
            {
                Id = 6,
                FirstName = "Joe",
                LastName = "Smith",
                PrimaryID = "4",
                Username = "jsmith"
            };
            advisors.Add(advisor);
            MLFSAdvisor unknownAdvisor = new MLFSAdvisor()
            {
                Id = 2,
                FirstName = "Unknown",
                LastName = "",
                PrimaryID = "",
                Username = "unknown"
            };
            advisors.Add(unknownAdvisor);

            //act
            MLFSAdvisor adv = MLFSAdvisor.Assign("12", advisors);

            //assert
            Assert.AreEqual(2, adv.Id, "Assignment to unknown has not worked");
        }
    }
}