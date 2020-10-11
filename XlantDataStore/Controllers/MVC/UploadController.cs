using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XLantDataStore.ViewModels;
using XLantCore.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using XLantCore;
using System.IO;

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

        public async Task<IActionResult> IncomeOnly()
        {
            ViewBag.ReportingPeriodId = await _periodData.SelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IncomeOnly(Upload upload)
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

            DataTable incomeTable = new DataTable();
            foreach (IFormFile file in upload.Files)
            {
                if (file.Length > 0)
                {
                    string newFilePath = Path.GetTempFileName();
                    using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    if (file.FileName.Contains("AdviserMonthlyFCI"))
                    {
                        incomeTable = Tools.ConvertCSVToDataTable(newFilePath);
                    }
                }
            }
            upload.Income = MLFSIncome.CreateFromDataTable(incomeTable, advisors, period);
            await _incomeData.InsertList(upload.Income);
            List<MLFSDebtorAdjustment> adjs = MLFSSale.CheckForReceipts(upload.Sales, upload.Income);
            _adjustmentData.InsertList(adjs);
            ViewBag.Result = "Uploaded Successfully";
            return RedirectToAction("Index");
        }
    }
}