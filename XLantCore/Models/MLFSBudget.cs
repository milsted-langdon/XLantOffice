using System.ComponentModel.DataAnnotations;

namespace XLantCore.Models
{
    public partial class MLFSBudget
    {
        public MLFSBudget()
        {

        }

        public int Id { get; set; }
        [Display(Name = "Reporting Period")]
        public int ReportingPeriodId { get; set; }
        public decimal Budget { get; set; }
        [Display(Name = "Advisor")]
        public int AdvisorId { get; set; }

        public virtual MLFSAdvisor Advisor { get; set; }
        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
    }
}
