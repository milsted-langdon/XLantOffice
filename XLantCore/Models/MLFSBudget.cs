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
        public string AdvisorId { get; set; }
        public int MLFSReportPeriodId { get; set; }
        public decimal Budget { get; set; }
    }
}
