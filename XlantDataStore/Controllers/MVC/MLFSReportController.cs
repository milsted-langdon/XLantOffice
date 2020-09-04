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
        public async Task<IActionResult> Index()
        {
            MLFSReportingPeriod current = await _periodData.GetCurrent();
            ViewBag.ReportingPeriodId = await _periodData.SelectList(current.Id);
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

            return PartialView("_DirectorsReport", report);
        }

        public async Task<IActionResult> SalesReport(int? periodId, string pivot = "Advisor")
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
            List<MLFSDebtorAdjustment> adjs = await _adjustmentData.GetAdjustments(period);
            List<MLFSBudget> budgets = await _budgetData.GetBudgets(period);

            if (pivot.ToLower() == "advisor")
            {
                foreach (MLFSAdvisor adv in advisors)
                {
                    SalesReport line = new SalesReport(income, adjs, budgets, adv, period);
                    report.Add(line);
                } 
            }
            else if (pivot.ToLower() == "organisation")
            {
                report = ViewModels.SalesReport.CreateByOrganisation(income, adjs, period);
            }
            else if (pivot.ToLower() == "campaign")
            {
                report = ViewModels.SalesReport.CreateByCampaign(income, period);
            }
            else
            {
                return NotFound();
            }

            return PartialView("_SalesReport", report.OrderBy(x => x.PivotEntity));
        }

        public async Task<IActionResult> FCIReport(int? periodId)
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
            return PartialView("_FCIReport", report.OrderBy(x => x.Advisor));
        }
    }
}
