using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace XLantCore.Models
{
    public partial class MLFSReportingPeriod
    {
        public MLFSReportingPeriod()
        {
        
        }

        public MLFSReportingPeriod(DataRow row)
        {
            Id = (int)row["id"];
            Description = row["Description"].ToString();
            Month = (int)row["Month"];
            Year = (int)row["Year"];
            FinancialYear = row["FinancialYear"].ToString();
            ReportOrder = (int)row["ReportOrder"];
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string FinancialYear { get; set; }
        public int ReportOrder { get; set; }
    }
}
