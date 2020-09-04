using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using XLantCore;
using System.ComponentModel.DataAnnotations;

namespace XLantDataStore.ViewModels
{
    public class Upload
    {
        [Display(Name="Reporting Period")]
        public int? ReportingPeriodId { get; set; }
        public MLFSReportingPeriod ReportingPeriod { get; set; }
        public IFormFileCollection Files { get; set; }
        public List<MLFSSale> Sales { get; set; }
        public List<MLFSIncome> Income { get; set; }

        public async Task<string> CreateEntities(List<MLFSAdvisor> advisors)
        {
            string response = "failure";

            DataTable salesTable = new DataTable();
            DataTable incomeTable = new DataTable();
            DataTable planTable = new DataTable();
            DataTable commissionTable = new DataTable();
            foreach (IFormFile file in Files)
            {

                if (file.Length > 0)
                {
                    string newFilePath = Path.GetTempFileName();
                    using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    if (file.FileName.Contains("MLFS Fees"))
                    {
                        salesTable = Tools.ConvertCSVToDataTable(newFilePath);
                    }
                    else if (file.FileName.Contains("Plans"))
                    {
                        planTable = Tools.ConvertCSVToDataTable(newFilePath);
                    }
                    else if (file.FileName.Contains("AdviserMonthlyFCI"))
                    {
                        incomeTable = Tools.ConvertCSVToDataTable(newFilePath);
                    }
                    else if (file.FileName.Contains("Commission"))
                    {
                        commissionTable = Tools.ConvertCSVToDataTable(newFilePath);
                    }
                    else
                    {
                        return response;
                    }
                }
                else
                {
                    return response;
                }
            }
            Sales = MLFSSale.ConvertFromDataTable(salesTable, planTable, commissionTable, advisors, ReportingPeriod);
            Income = MLFSIncome.CreateFromDataTable(incomeTable, advisors, ReportingPeriod);
            response = "Success";
            return response;
        }
    }
}
