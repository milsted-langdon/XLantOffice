using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XLantCore.Models;
using XLantDataStore.ViewModels;
using XLantDataStore;

namespace XLantDataStore.Controllers.MVC
{
    public class MLFSReportController : Controller
    {
        private readonly Repository.IMLFSSaleRepository _salesData;
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;
        private readonly Repository.IMLFSIncomeRepository _incomeData;
        private readonly Repository.IMLFSAdvisorRepository _advisorData;
        private readonly Repository.IMLFSDebtorAdjustmentRepository _adjustmentData;
        private readonly Repository.IMLFSBudgetRepository _budgetData;

        public MLFSReportController(XLantDbContext context)
        {
            _salesData = new Repository.MLFSSaleRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _incomeData = new Repository.MLFSIncomeRepository(context);
            _advisorData = new Repository.MLFSAdvisorRepository(context);
            _adjustmentData = new Repository.MLFSDebtorAdjustmentRepository(context);
            _budgetData = new Repository.MLFSBudgetRepository(context);
        }

        // GET: Index
        public async Task<IActionResult> Index(int? periodId)
        {
            if (periodId == null)
            {
                MLFSReportingPeriod current = await _periodData.GetCurrent();
                periodId = current.Id;
            }
            ViewBag.ReportingPeriodId = await _periodData.SelectList(periodId);
            return View();
        }

        public async Task<IActionResult> DirectorsReport(int? periodId)
        {
            if (periodId == null)
            {
                return NotFound();
            }
            MLFSReportingPeriod period = await _periodData.GetPeriodById((int)periodId);
            List<MLFSSale> sales = await _salesData.GetSales(period);

            List<DirectorsReport> report = ViewModels.DirectorsReport.Create(sales.OrderBy(x => x.ClientName).ToList());

            return PartialView("_DirectorsReport", report.OrderBy(x => x.ClientId));
        }

        public async Task<IActionResult> SalesReport(int? periodId, string entity = "advisor")
        {
            MLFSReportingPeriod period;
            List<SalesReport> report = new List<SalesReport>();
            //get period
            if (periodId == null)
            {
                return NotFound();
            }
            period = await _periodData.GetPeriodById((int)periodId);
            if (period == null)
            {
                return NotFound();
            }
            
            //get advsiors
            List<MLFSAdvisor> advisors = await _advisorData.GetAdvisors();
            List<MLFSIncome> income = await _incomeData.GetIncome(period);
            List<MLFSSale> sales = await _salesData.GetSales(period);
            List<MLFSDebtorAdjustment> adjs = await _adjustmentData.GetAdjustments(period);
            List<MLFSBudget> budgets = await _budgetData.GetBudgets(period);

            if (entity.ToLower() == "advisor")
            {
                foreach (MLFSAdvisor adv in advisors)
                {
                    SalesReport line = new SalesReport(income, sales, adjs, budgets, adv, period);
                    report.Add(line);
                } 
            }
            if (entity.ToLower() == "organisation")
            {
                report = report.GroupBy(x => x.Organisation).Select(y => new ViewModels.SalesReport()
                {
                    Period = y.FirstOrDefault().Period,
                    PeriodId = y.FirstOrDefault().PeriodId,
                    Advisor = y.Key,
                    AdvisorId = 0,
                    Organisation = y.Key,
                    Budget = y.Sum(z => z.Budget),
                    New_Business = y.Sum(z => z.New_Business),
                    Renewals = y.Sum(z => z.Renewals),
                    Unallocated = y.Sum(z => z.Unallocated),
                    Clawback = y.Sum(z => z.Clawback),
                    NotTakenUp = y.Sum(z => z.NotTakenUp),
                    Adjustment = y.Sum(z => z.Adjustment),
                    Debtors_Variance = y.Sum(z => z.Debtors_Variance),
                    Debtors_Adjustment = y.Sum(z => z.Debtors_Adjustment),
                    Total = y.Sum(z => z.Total)
                }).ToList();
            }
            else if (entity.ToLower() == "campaign")
            {
                report = ViewModels.SalesReport.CreateByCampaign(income, period);
            }

            return PartialView("_SalesReport", report.OrderBy(x => x.Advisor));
        }

        public async Task<IActionResult> FCIReport(int? periodId, string entity = "advisor")
        {
            MLFSReportingPeriod period;
            List<ViewModels.FCIReport> report = new List<ViewModels.FCIReport>();
            //get period
            if (periodId == null)
            {
                return NotFound();
            }
            period = await _periodData.GetPeriodById((int)periodId);
            if (period == null)
            {
                return NotFound();
            }
            List<MLFSIncome> income = await _incomeData.GetIncome(period);
            report = ViewModels.FCIReport.CreateFromList(income);
            if (entity == "organisation")
            {
                report = report.GroupBy(x => x.Organisation).Select(y => new ViewModels.FCIReport()
                {
                    Advisor = y.Key,
                    Adhoc = y.Sum(z => z.Adhoc),
                    FundBased = y.Sum(z => z.FundBased),
                    Initial = y.Sum(z => z.Initial),
                    Ongoing = y.Sum(z => z.Ongoing),
                    Renewal = y.Sum(z => z.Renewal),
                    Other = y.Sum(z => z.Other),
                    Total = y.Sum(z => z.Total)
                }).ToList();
            }
            return PartialView("_FCIReport", report.OrderBy(x => x.Advisor));
        }

        
        public async Task<IActionResult> SalesSummary(int? periodId, string entity = "advisor")
        {
            MLFSReportingPeriod finalPeriod;
            List<SalesReport> report = new List<SalesReport>();
            //get period
            if (periodId == null)
            {
                return NotFound();
            }
            finalPeriod = await _periodData.GetPeriodById((int)periodId);
            if (finalPeriod == null)
            {
                return NotFound();
            }
            List<MLFSReportingPeriod> periods = await _periodData.GetFinancialYear(finalPeriod);
            periods = periods.Where(x => x.ReportOrder <= finalPeriod.ReportOrder).ToList();
            List<MLFSAdvisor> advisors = await _advisorData.GetAdvisors();
            foreach (MLFSReportingPeriod period in periods)
            {   
                List<MLFSIncome> income = await _incomeData.GetIncome(period);
                List<MLFSSale> sales = await _salesData.GetSales(period);
                List<MLFSDebtorAdjustment> adjs = await _adjustmentData.GetAdjustments(period);
                List<MLFSBudget> budgets = await _budgetData.GetBudgets(period);

                foreach (MLFSAdvisor adv in advisors)
                {
                    SalesReport line = new SalesReport(income, sales, adjs, budgets, adv, period);
                    report.Add(line);
                }
            }
            if (entity.ToLower() == "organisation")
            {
                report = report.GroupBy(x => x.Organisation).Select(y => new ViewModels.SalesReport()
                {
                    Period = y.FirstOrDefault().Period,
                    PeriodId = y.FirstOrDefault().PeriodId,
                    Advisor = y.Key,
                    AdvisorId = 0,
                    Organisation = y.Key,
                    Budget = y.Sum(z => z.Budget),
                    New_Business = y.Sum(z => z.New_Business),
                    Renewals = y.Sum(z => z.Renewals),
                    Unallocated = y.Sum(z => z.Unallocated),
                    Clawback = y.Sum(z => z.Clawback),
                    NotTakenUp = y.Sum(z => z.NotTakenUp),
                    Adjustment = y.Sum(z => z.Adjustment),
                    Debtors_Variance = y.Sum(z => z.Debtors_Variance),
                    Debtors_Adjustment = y.Sum(z => z.Debtors_Adjustment),
                    Total = y.Sum(z => z.Total)
                }).ToList();
            }

            List<SalesSummary> yearToDate = ViewModels.SalesSummary.CreateFromSalesReport(report, advisors);

            return PartialView("_SalesSummary", yearToDate.OrderBy(x => x.Advisor));
        }

        public async Task<IActionResult> NewBusinessSummary(int? periodId, string entity = "advisor")
        {
            MLFSReportingPeriod period;
            //get period
            if (periodId == null)
            {
                return NotFound();
            }
            period = await _periodData.GetPeriodById((int)periodId);
            if (period == null)
            {
                return NotFound();
            }
            List<MLFSSale> sales = await _salesData.GetSales(period);
            List<NewBusiness> report = NewBusiness.CreateList(sales, period);
            if (entity.ToLower() == "organisation")
            {
                report = report.GroupBy(x => x.Organisation).Select(y => new ViewModels.NewBusiness()
                {
                    Period = y.FirstOrDefault().Period,
                    PeriodId = y.FirstOrDefault().PeriodId,
                    Advisor = y.Key,
                    AdvisorId = 0,
                    Organisation = y.Key,
                    NewClients = y.Sum(z => z.NewClients),
                    ExistingClients = y.Sum(z => z.ExistingClients),
                    Total = y.Sum(z => z.Total)
                }).ToList();
            }
            return PartialView("_NewBusiness", report);
        }

        public async Task<IActionResult> VATReview(int? periodId)
        {
            MLFSReportingPeriod period;
            //get period
            if (periodId == null)
            {
                return NotFound();
            }
            period = await _periodData.GetPeriodById((int)periodId);
            if (period == null)
            {
                return NotFound();
            }
            List<MLFSSale> sales = await _salesData.GetSales(period);
            List<MLFSIncome> income = await _incomeData.GetIncome(period);
            List<VATReview> review = ViewModels.VATReview.CreateList(sales, income, period);
            return View(review);
        }
    }
}
