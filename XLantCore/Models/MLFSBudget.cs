using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XLantCore;

namespace XLantCore.Models
{
    public partial class MLFSBudget
    {
        public MLFSBudget()
        {

        }

        public int Id { get; set; }
        public int ReportingPeriodId { get; set; }
        public decimal Budget { get; set; }
        public int AdvisorId { get; set; }

        public virtual MLFSAdvisor Advisor { get; set; }
        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
    }
}
