using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;
using System.Linq;

namespace XLantCore.Models
{
    public partial class MLFSSale
    {
        public MLFSSale()
        {
            Adjustments = new HashSet<MLFSDebtorAdjustment>();
        }

        public MLFSSale(DataRow row, List<MLFSAdvisor> advisors)
        {
            IOId = row["Id"].ToString();
            IOReference = row["Reference Number"].ToString();
            ClientName =  row["Fee Owner.Full Name"].ToString();
            ClientId = row["Fee Owner.Id"].ToString();
            JointClientName = row["Fee Owner 2.Full Name"].ToString();
            JointClientId = row["Fee Owner 2.Id"].ToString();
            MLFSAdvisor adv = MLFSAdvisor.Assign(row["Selling Adviser.Id"].ToString(), advisors);
            AdvisorId = adv.Id;
            Advisor = adv;
            PlanType = row["Related Plan Type"].ToString();
            IsNew = false;
            RelevantDate = DateTime.Parse(row["Invoice Date"].ToString());
            NetAmount = Tools.HandleNull(row["Net Amount"].ToString());
            VAT = Tools.HandleNull(row["VAT"].ToString());
            PlanReference = row["Related Plan Reference"].ToString();
            Investment = 0;
            OnGoingPercentage = 0;
            EstimatedOtherIncome = 0;
        }

        public int? Id { get; set; }
        public string IOId { get; set; }
        [Display(Name="IO Reference")]
        public string IOReference { get; set; }
        [Display(Name = "Period")]
        public int? ReportingPeriodId { get; set; }
        public string Organisation { get; set; }
        [Display(Name = "Client")]
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string JointClientName { get; set; }
        public string JointClientId { get; set; }
        public int AdvisorId { get; set; }
        [Display(Name = "Provider")]
        public string ProviderName { get; set; }
        [Display(Name = "Plan Type")]
        public string PlanType { get; set; }
        [Display(Name = "New")]
        public bool IsNew { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime RelevantDate { get; set; }
        [Display(Name = "Net Amount")]
        [DataType(DataType.Currency)]
        public decimal NetAmount { get; set; }
        [DataType(DataType.Currency)]
        public decimal VAT { get; set; }
        [DataType(DataType.Currency)]
        public decimal Investment { get; set; }
        [Display(Name = "On Going Percentage")]
        public decimal OnGoingPercentage { get; set; }
        [Display(Name = "Reference")]
        public string PlanReference { get; set; }
        public decimal EstimatedOtherIncome { get; set; }

        [NotMapped]
        public virtual MLFSClient Client { get; set; }
        public virtual MLFSAdvisor Advisor { get; set; }
        public virtual MLFSIncome Income { get; set; }
        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
        public virtual ICollection<MLFSDebtorAdjustment> Adjustments { get; set; }
    }
}
