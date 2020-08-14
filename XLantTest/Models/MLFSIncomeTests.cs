using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSIncomeTests
    {
        [TestMethod()]
        public void MLFSIncomeCreateFromIODataRowTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("IORef", typeof(string));
            table.Columns.Add("GroupOne", typeof(string));
            table.Columns.Add("Submitted", typeof(DateTime));
            table.Columns.Add("CRMContactId", typeof(int));
            table.Columns.Add("Provider", typeof(string));
            table.Columns.Add("ClientName", typeof(string));
            table.Columns.Add("ClientId", typeof(string));
            table.Columns.Add("JointClientName", typeof(string));
            table.Columns.Add("JointClientId", typeof(string));
            table.Columns.Add("CampaignType", typeof(string));
            table.Columns.Add("CampaignSource", typeof(string));
            table.Columns.Add("GrossFCI Excl.VAT", typeof(decimal));
            table.Columns.Add("FeeStatus", typeof(string));
            table.Columns.Add("PlanType", typeof(string));
            table.Columns.Add("PlanNumber", typeof(string));
            table.Columns.Add("IsTopup", typeof(bool));
            table.Columns.Add("IncomeType", typeof(string));

            DataRow row = table.NewRow();
            row["IORef"] = "IOF123456";
            row["GroupOne"] = "MLFS";
            row["Submitted"] = DateTime.Now;
            row["CRMContactId"] = 4;
            row["Provider"] = "Elevate";
            row["ClientName"] = "Jeff Bloggs";
            row["ClientId"] = 5;
            row["JointClientName"] = "Jane Bloggs";
            row["JointClientId"] = 6;
            row["Campaigntype"] = "Teachers";
            row["CampaignSource"] = "ML";
            row["GrossFCI Excl.VAT"] = 1100;
            row["FeeStatus"] = "Paid";
            row["PlanType"] = "Pension";
            row["PlanNumber"] = "123456";
            row["IsTopup"] = true;
            row["IncomeType"] = "InitialFee";
            table.Rows.Add(row);

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
            MLFSIncome income = new MLFSIncome(row, advisors);

            //assert
            Assert.AreEqual("Elevate", income.ProviderName, "Provider doesn't match");
            Assert.AreEqual(1100, income.Amount, "Amount does not match");
            Assert.AreEqual(6, income.AdvisorId, "Advisor has not been updated");
        }

        [TestMethod()]
        public void UpdateFromIOTest()
        {
            //arrage
            List<MLFSIncome> income = new List<MLFSIncome>();
            MLFSIncome i = new MLFSIncome()
            {
                Id = 1,
                IOReference = "123456",
                ReportingPeriodId = 2,
                RelevantDate = DateTime.Now,
                ClientId = "234",
                Amount = 100,
                PlanNumber = "9876"
            };
            income.Add(i);
            MLFSIncome i2 = new MLFSIncome()
            {
                Id = 2,
                IOReference = "1234567",
                ReportingPeriodId = 2,
                RelevantDate = DateTime.Now,
                ClientId = "345",
                Amount = 100,
                PlanNumber = "9877"
            };
            income.Add(i2);
            List<MLFSClient> clients = new List<MLFSClient>();
            MLFSClient c = new MLFSClient()
            {
                PrimaryID = "234",
                Name = "John Smith",
                IsActive = true,
                CreatedOn = DateTime.Parse("01/01/2001")
            };
            MLFSClient c2 = new MLFSClient()
            {
                PrimaryID = "231",
                Name = "John Adams",
                IsActive = false,
                CreatedOn = DateTime.Parse("10/01/2001")
            };
            clients.Add(c);
            clients.Add(c2);


            //act
            MLFSIncome.UpdateFromIO(income, clients);

            //assert
            Assert.AreEqual(DateTime.Parse("01/01/2001"), i.ClientOnBoardDate, "On board date not updated");
        }
    }
}