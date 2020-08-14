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
    public class IncomeReportTests
    {
        
        private List<MLFSReportingPeriod> MockEntries()
        {
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

            List<MLFSIncome> income = new List<MLFSIncome>();
            MLFSIncome inc = new MLFSIncome()
            {
                Id = 1,
                IOReference = "123456",
                Advisor = advisor,
                AdvisorId = advisor.Id,
                Organisation = "FPP",
                ReportingPeriodId = periods[0].Id,
                ReportingPeriod = periods[0],
                RelevantDate = DateTime.Now,
                ClientOnBoardDate = DateTime.Now.AddMonths(-2),
                ClientId = "234",
                Amount = 100,
                PlanNumber = "9876"
            };
            income.Add(inc);
            MLFSIncome inc2 = new MLFSIncome()
            {
                Id = 2,
                IOReference = "1234567",
                Advisor = advisor,
                AdvisorId = advisor.Id,
                Organisation = "MLFS",
                ReportingPeriodId = periods[0].Id,
                ReportingPeriod = periods[0],
                RelevantDate = DateTime.Now,
                ClientOnBoardDate = DateTime.Now.AddMonths(-2),
                ClientId = "345",
                Amount = 100,
                PlanNumber = "9877"
            };
            income.Add(inc2);
            List<MLFSBudget> budgets = new List<MLFSBudget>();
            MLFSBudget budget = new MLFSBudget()
            {
                Id = 9,
                Advisor = advisor,
                AdvisorId = advisor.Id,
                Budget = (decimal)20000,
                ReportingPeriodId = periods[0].Id,
                ReportingPeriod = periods[0]
            };
            budgets.Add(budget);
            periods[0].Receipts = income;
            periods[0].Budgets = budgets;
            return periods;
        }

        [TestMethod()]
        public void CreateReportByAdvisorTest()
        {
            //arrange
            List<MLFSReportingPeriod> periods = MockEntries();

            //act
            List<IncomeReport> report = IncomeReport.CreateReportByAdvisor(periods);

            //assert
            Assert.AreEqual((decimal)200, report.Where(y => y.PeriodId == 1 && y.AdvisorId == 1).Sum(x => x.New_Amount), "New Amount value does not tally");
            Assert.AreEqual(3, report.Count, "Number of entries does not tally");
            Assert.AreEqual((decimal)200, report.Sum(x => x.New_Amount), "New Amount of all entries does not tally");
        }

        [TestMethod()]
        public void CreateFromListTest()
        {
            //arrange
            List<MLFSReportingPeriod> periods = MockEntries();

            //act
            List<IncomeReport> report = IncomeReport.CreateFromList(periods[0].Receipts);

            //assert
            Assert.AreEqual((decimal)200, report.Sum(x => x.New_Amount), "New Amount value does not tally");
            Assert.AreEqual(2, report.Count, "Number of entries does not tally");
            Assert.AreEqual((decimal)0, report.Sum(x => x.Existing_Amount), "Existing Amount does not tally");
        }

        [TestMethod()]
        public void CreateReportByOrganisationTest()
        {
            //arrange
            List<MLFSReportingPeriod> periods = MockEntries();

            //act
            List<IncomeReport> report = IncomeReport.CreateReportByOrganisation(periods);

            //assert
            Assert.AreEqual((decimal)100, report.Where(y => y.PeriodId == 1 && y.Organisation == "MLFS").Sum(x => x.New_Amount), "New Amount for MLFS does not tally");
            Assert.AreEqual(6, report.Count, "Number of entries does not tally");
            Assert.AreEqual((decimal)200, report.Sum(x => x.New_Amount), "New Amount of all entries does not tally");
        }

        
    }
}