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

        public int Id { get; set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string FinancialYear { get; set; }
        public int ReportOrder { get; set; }
    }
}
