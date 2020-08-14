using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantDataStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using XLantCore.Models;
using System.Linq;

namespace XLantDataStore.ViewModels.Tests
{
    [TestClass()]
    public class DirectorsReportTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            //arrange
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
            MLFSAdvisor advisor = new MLFSAdvisor()
            {
                Id = 1,
                FirstName = "Geoff",
                LastName = "Smith"
            };

            List<MLFSSale> debtors = new List<MLFSSale>();
            MLFSSale sale = new MLFSSale()
            {
                Id = 1,
                Advisor = advisor,
                AdvisorId = advisor.Id,
                ClientName = "Billy Bigwallet",
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
                Advisor = advisor,
                AdvisorId = advisor.Id,
                ClientName = "Jonny Comelately",
                IOReference = "1234567",
                ReportingPeriodId = periods[1].Id,
                ReportingPeriod = periods[1],
                Investment = 1000,
                NetAmount = 100,
                OnGoingPercentage = (decimal)0.005,
                RelevantDate = DateTime.Parse("01/02/2020")
            };
            debtors.Add(sale2);


            //act
            List<DirectorsReport> report = DirectorsReport.Create(debtors);

            //assert
            Assert.AreEqual(2, report.Count, "Wrong number of records");
            Assert.AreEqual(104, report.Sum(x => x.InitialFee), "Total fees does not match");
        }
    }
}