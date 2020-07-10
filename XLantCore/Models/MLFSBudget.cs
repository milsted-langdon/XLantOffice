using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XLant;

namespace XLantCore.Models
{
    public partial class MLFSBudget
    {
        public MLFSBudget()
        {

        }

        public MLFSBudget(DataRow row)
        {
            Id = (int)row["Id"];
            AdvisorId = row["AdvisorId"].ToString();
            MLFSReportPeriodId = (int)row["MLFSReportPeriodId"];
            Budget = XLtools.HandleNull(row["Budget"].ToString());
        }

        public int Id { get; set; }
        public string AdvisorId { get; set; }
        public int MLFSReportPeriodId { get; set; }
        public decimal Budget { get; set; }
    }
}
