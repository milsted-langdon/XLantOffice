using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;

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
            table.Columns.Add("Fee Owner 2.Full Name", typeof(string));
            table.Columns.Add("Fee Owner 2.Id", typeof(string));
            table.Columns.Add("Selling Adviser.Id", typeof(int));
            table.Columns.Add("Selling Adviser.Full Name", typeof(string));
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
            row["Fee Owner 2.Full Name"] = "Jane Bloggs";
            row["Fee Owner 2.Id"] = "321654";
            row["Selling Adviser.Full Name"] = "Joe Smith";
            row["Selling Adviser.Id"] = "9";
            row["Related Plan Type"] = "Pension";
            row["Invoice Date"] = DateTime.Now;
            row["Net Amount"] = 1000;
            row["VAT"] = 100;
            row["Related Plan Reference"] = "IOB098767";
            table.Rows.Add(row);

            List<MLFSAdvisor> advisors = new List<MLFSAdvisor>();
            MLFSAdvisor advisor = new MLFSAdvisor()
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Smith",
                PrimaryID = "9",
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
            MLFSReportingPeriod period = new MLFSReportingPeriod(1, 2020)
            {
                Id = 2
            };

            //act
            MLFSSale sale = new MLFSSale(row, advisors, period);

            //assert
            Assert.AreEqual("Joe Smith", sale.Advisor.Fullname, "Advisor doesn't match");
            Assert.AreEqual(1100, sale.GrossAmount, "Gross amount does not match");
            Assert.AreEqual(1, sale.AdvisorId, "Id has not been converted");
        }

        [TestMethod()]
        public void FeeCheckTests()
        {
            //arrange
            MLFSSale sale = new MLFSSale()
            {
                Id = 1,
                Investment = 100,
                NetAmount = 4,
                OnGoingPercentage = (decimal)0.005
            };

            //act
            //Nothing to do

            //assert            
            Assert.AreEqual(true, sale.InitialFeePass, "Initial Fee checked passed correctly");
            Assert.AreEqual(false, sale.OngoingFeePass, "Ogoing Fee check failed correctly");
        }

        [TestMethod()]
        public void AddPlanDataTest()
        {
            //arrange
            MLFSReportingPeriod period = new MLFSReportingPeriod(1, 2020)
            {
                Id = 2
            };
            MLFSSale sale = new MLFSSale()
            {
                Id = 1,
                Investment = 100,
                ReportingPeriodId = period.Id,
                ReportingPeriod = period,
                NetAmount = 4,
                OnGoingPercentage = (decimal)0.005,
                RelevantDate = DateTime.Parse("01/02/2020")
            };
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Provider.Name", typeof(string));
            table.Columns.Add("Owner 1.Creation Date", typeof(string));
            table.Columns.Add("Total Premiums to Date", typeof(string));
            table.Columns.Add("On-going Fee Percentage", typeof(string));
            table.Columns.Add("Selling Adviser.Group.Name", typeof(string));

            DataRow row = table.NewRow();
            row["Id"] = "123465";
            row["Provider.Name"] = "Elevate";
            row["Owner 1.Creation Date"] = "01/01/2020";
            row["Total Premiums to Date"] = "10000";
            row["On-going Fee Percentage"] = "0.01";
            row["Selling Adviser.Group.Name"] = "Milsted Langdon Financial Services";
            table.Rows.Add(row);

            //act
            sale.AddPlanData(row);

            //assert
            Assert.AreEqual(true, sale.IsNew, "Client is not reporting as new");
            Assert.AreEqual((decimal)10000, sale.Investment, "Investment not updated");
        }

        [TestMethod()]
        public void CreateNTUTest()
        {
            //arrange
            MLFSSale sale = new MLFSSale()
            {
                Id = 1,
                ReportingPeriodId = 6,
                Investment = 100,
                NetAmount = 4,
                OnGoingPercentage = (decimal)0.005,
                RelevantDate = DateTime.Parse("01/02/2020")
            };
            MLFSReportingPeriod period = new MLFSReportingPeriod(5, 2020)
            {
                Id = 2
            };

            //act
            MLFSDebtorAdjustment adj = sale.CreateNTU(period);

            //assert
            Assert.AreEqual(-4, adj.Amount, "Amount is not the inverse of the debtor");
            Assert.AreEqual(2, adj.ReportingPeriodId, "Correct reporting period not given.");
            Assert.AreEqual(true, adj.NotTakenUp, "Not reporting as an NTU adjustment");
        }

        [TestMethod()]
        public void CheckForReceiptsTest()
        {
            //arrange
            List<MLFSReportingPeriod> periods = new List<MLFSReportingPeriod>();
            for (int i = 1; i < 4; i++)
            {
                MLFSReportingPeriod period = new MLFSReportingPeriod(i, 2020)
                {
                    Id = i
                };
                periods.Add(period);
            }


            List<MLFSSale> debtors = new List<MLFSSale>();
            MLFSSale sale = new MLFSSale()
            {
                Id = 1,
                ReportingPeriodId = periods[0].Id,
                ReportingPeriod = periods[0],
                Investment = 100,
                NetAmount = 4,
                OnGoingPercentage = (decimal)0.005,
                RelevantDate = DateTime.Parse("01/02/2020")
            };
            debtors.Add(sale);
            MLFSSale sale2 = new MLFSSale()
            {
                Id = 1,
                IOReference = "1234567",
                ReportingPeriodId = periods[1].Id,
                ReportingPeriod = periods[1],
                Investment = 1000,
                NetAmount = 100,
                OnGoingPercentage = (decimal)0.005,
                RelevantDate = DateTime.Parse("01/02/2020")
            };
            debtors.Add(sale2);
            List<MLFSIncome> income = new List<MLFSIncome>();
            MLFSIncome inc = new MLFSIncome()
            {
                Id = 1,
                IOReference = "123456",
                ReportingPeriodId = periods[2].Id,
                ReportingPeriod = periods[2],
                RelevantDate = DateTime.Now,
                ClientId = "234",
                Amount = 100,
                PlanNumber = "9876"
            };
            income.Add(inc);
            MLFSIncome inc2 = new MLFSIncome()
            {
                Id = 2,
                IOReference = "1234567",
                ReportingPeriodId = periods[2].Id,
                ReportingPeriod = periods[2],
                RelevantDate = DateTime.Now,
                ClientId = "345",
                Amount = 90,
                PlanNumber = "9877"
            };
            income.Add(inc2);

            //act
            List<MLFSDebtorAdjustment> adjs = MLFSSale.CheckForReceipts(debtors, income);

            //assert
            Assert.AreEqual(1, adjs.Count, "Incorrect number of adjustments created");
            Assert.AreEqual((decimal)10, sale2.Outstanding, "Amount outstanding to sale2 not reflected");
        }

        [TestMethod()]
        public void AddClientDataTest()
        {
            //arrange
            MLFSSale sale = new MLFSSale()
            {
                Id = 1,
                ClientId = "IOB123",
                ClientName = "John",
                NetAmount = 100,
                VAT = 20,
                PlanReference = "IOP123"
            };
            MLFSClient client = new MLFSClient()
            {
                Name = "John",
                PrimaryID = "IOB123",
                CreatedOn = DateTime.Now.AddYears(-1)
            };
            client.Plans.Add(new MLFSPlan()
            {
                PrimaryID = "IOP123",
                Provider = "XYZ Limited",
                ContributionsToDate = 10000,
                CurrentValuation = 11000,
                IsPreExistingClient = true
            });
            client.Fees.Add(new MLFSFee()
            {
                PrimaryID = "IOF123",
                FeePercentage = (decimal)0.1,
                IsRecurring = true,
                Plan = client.Plans.FirstOrDefault()
            });
            client.Plans.Add(new MLFSPlan()
            {
                PrimaryID = "IOP987",
                Provider = "XYZ Limited",
                ContributionsToDate = 200000,
                CurrentValuation = 220000
            });
            client.Fees.Add(new MLFSFee()
            {
                PrimaryID = "IOF987",
                FeePercentage = (decimal)0.1,
                IsRecurring = true,
                Plan = new MLFSPlan()
                {
                    PrimaryID = "IOP987",
                    Provider = "XYZ Limited",
                    ContributionsToDate = 200000,
                    CurrentValuation = 220000
                }
            });

            //act
            sale.AddClientData(client);

            //assert
            Assert.AreEqual("XYZ Limited", sale.ProviderName, "Provider name not updated");
            Assert.AreEqual(false, sale.IsNew, "IsNew not set correctly");
            Assert.AreEqual(11000, sale.Investment, "Investment incorrect");
            Assert.AreEqual((decimal)0.1, sale.OnGoingPercentage, "Percentage incorrect");
            Assert.AreEqual((decimal)22000, sale.EstimatedOtherIncome, "Other income not calculated correctly");
        }

        [TestMethod()]
        public void MatchPlanTest()
        {
            //arrange
            MLFSReportingPeriod period = new MLFSReportingPeriod(1, 2020)
            {
                Id = 2
            };
            MLFSSale sale = new MLFSSale()
            {
                Id = 1,
                ClientId = "IOB123",
                ClientName = "John",
                ReportingPeriod = period,
                ReportingPeriodId = period.Id,
                NetAmount = 100,
                VAT = 20,
                PlanReference = "IOP123",
                IOReference = "IOF345",
                RelevantDate = DateTime.Parse("01/02/2020")
            };
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Provider.Name", typeof(string));
            table.Columns.Add("Owner 1.Creation Date", typeof(string));
            table.Columns.Add("Total Premiums to Date", typeof(string));
            table.Columns.Add("On-going Fee Percentage", typeof(string));
            table.Columns.Add("Selling Adviser.Group.Name", typeof(string));
            table.Columns.Add("Root Sequential Ref", typeof(string));
            table.Columns.Add("Sequential Ref", typeof(string));
            table.Columns.Add("Related Fee Reference", typeof(string));

            DataRow firstRow = table.NewRow();
            firstRow["Id"] = "123465";
            firstRow["Provider.Name"] = "Elevate";
            firstRow["Owner 1.Creation Date"] = "01/01/2020";
            firstRow["Total Premiums to Date"] = "10000";
            firstRow["On-going Fee Percentage"] = "0.01";
            firstRow["Selling Adviser.Group.Name"] = "Milsted Langdon Financial Services";
            firstRow["Sequential Ref"] = "IOP123";
            firstRow["Root Sequential Ref"] = "IOP124";
            firstRow["Related Fee Reference"] = "IOF345";
            table.Rows.Add(firstRow);

            DataRow secondRow = table.NewRow();
            secondRow["Id"] = "123465";
            secondRow["Provider.Name"] = "Elevate";
            secondRow["Owner 1.Creation Date"] = "01/01/2020";
            secondRow["Total Premiums to Date"] = "20000";
            secondRow["On-going Fee Percentage"] = "0.01";
            secondRow["Selling Adviser.Group.Name"] = "Milsted Langdon Financial Services";
            secondRow["Sequential Ref"] = "IOP123";
            secondRow["Root Sequential Ref"] = "IOP124";
            secondRow["Related Fee Reference"] = "IOF346";
            table.Rows.Add(secondRow);

            //act
            sale.MatchPlan(table);

            //assert
            Assert.AreEqual((decimal)10000, sale.Investment, "Investment doesn't match");
        }
    }
}