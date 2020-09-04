using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XLantDataStore.ViewModels;
using XLantCore.Models;
using Microsoft.AspNetCore.Http;

namespace XLantDataStore.Controllers.MVC
{
    public class UploadController : Controller
    {
        private readonly Repository.IMLFSSaleRepository _salesData;
        private readonly Repository.IMLFSIncomeRepository _incomeData;
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;
        private readonly Repository.IMLFSDebtorAdjustmentRepository _adjustmentData;
        private readonly Repository.IMLFSAdvisorRepository _advisorData;

        public UploadController(XLantDbContext context)
        {
            _salesData = new Repository.MLFSSaleRepository(context);
            _incomeData = new Repository.MLFSIncomeRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _adjustmentData = new Repository.MLFSDebtorAdjustmentRepository(context);
            _advisorData = new Repository.MLFSAdvisorRepository(context);
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ReportingPeriodId = await _periodData.SelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Upload upload)
        {
            if (upload.ReportingPeriodId == null)
            {
                return NotFound();
            }
            MLFSReportingPeriod period = await _periodData.GetPeriodById((int)upload.ReportingPeriodId);
            if (period == null)
            {
                return NotFound();
            }
            upload.ReportingPeriod = period;
            upload.ReportingPeriodId = period.Id;
            if (upload.Files == null || upload.Files.Count != 4)
            {
                return NotFound();
            }
            List<MLFSAdvisor> advisors = await _advisorData.GetAdvisors();
            string response = await upload.CreateEntities(advisors);
            if (response != "Success")
            {
                return NotFound();
            }
            await _salesData.InsertList(upload.Sales);
            await _incomeData.InsertList(upload.Income);
            List<MLFSDebtorAdjustment> adjs = MLFSSale.CheckForReceipts(upload.Sales, upload.Income);
            _adjustmentData.InsertList(adjs);
            ViewBag.Result = "Uploaded Successfully";
            return RedirectToAction("Index");
        }
    }
}