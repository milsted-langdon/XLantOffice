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
            RelatedClients = new string[]{};
        }

        public MLFSSale(DataRow row, List<MLFSAdvisor> advisors, MLFSReportingPeriod period, bool isCommission = false)
        {
            Adjustments = new HashSet<MLFSDebtorAdjustment>();
            ReportingPeriod = period;
            ReportingPeriodId = period.Id;
            EstimatedOtherIncome = 0;
            MLFSAdvisor adv = MLFSAdvisor.Assign(row["Selling Adviser.Id"].ToString(), advisors);
            AdvisorId = adv.Id;
            Advisor = adv;
            IsNew = false;

            if (isCommission)
            {
                PlanReference = "IOB" + row["Id"].ToString();
                IOReference = "";
                ClientName = row["Owner 1.Full Name"].ToString();
                ClientId = row["Owner 1.Id"].ToString();
                JointClientName = row["Owner 2.Full Name"].ToString();
                JointClientId = row["Owner 2.Id"].ToString();
                PlanType = row["Plan Type"].ToString();
                ProviderName = row["Provider.Name"].ToString();
                RelevantDate = DateTime.Parse(row["Submitted Date"].ToString());
                if (string.IsNullOrEmpty(row["Expected Commission - Non-Indemnity"].ToString()))
                {
                    NetAmount = Tools.HandleNull(row["Expected Commission - Total Initial"].ToString());
                }
                else
                {
                    NetAmount = Tools.HandleNull(row["Expected Commission - Non-Indemnity"].ToString());
                }
                VAT = 0;
                DateTime creationDate = DateTime.Parse(row["Owner 1.Creation Date"].ToString());
                if (creationDate > ReportingPeriod.StartDate.AddMonths(-6))
                {
                    IsNew = true;
                }
                Investment = Tools.HandleNull(row["Total Premiums to Date"].ToString());
                OnGoingPercentage = Tools.HandleNull(row["On-going Fee Percentage"].ToString());
                Organisation = row["Selling Adviser.Group.Name"].ToString();
            }
            else
            {
                IOId = row["Id"].ToString();
                IOReference = row["Reference Number"].ToString();
                ClientName = row["Fee Owner.Full Name"].ToString();
                ClientId = row["Fee Owner.Id"].ToString();
                JointClientName = row["Fee Owner 2.Full Name"].ToString();
                JointClientId = row["Fee Owner 2.Id"].ToString();
                PlanType = row["Related Plan Type"].ToString();
                RelevantDate = DateTime.Parse(row["Invoice Date"].ToString());
                NetAmount = Tools.HandleNull(row["Net Amount"].ToString());
                VAT = Tools.HandleNull(row["VAT"].ToString());
                PlanReference = row["Related Plan Reference"].ToString();
                Investment = 0;
                OnGoingPercentage = 0;
            }
            RelatedClients = new string[]{};
        }

        public MLFSSale(MLFSIncome income)
        {
            IOId = income.IOReference;
            ReportingPeriodId = income.ReportingPeriodId;
            Organisation = income.Organisation;
            ClientName = income.ClientName;
            ClientId = income.ClientId;
            JointClientId = income.JointClientId;
            JointClientName = income.JointClientName;
            AdvisorId = income.AdvisorId;
            Advisor = income.Advisor;
            ProviderName = income.ProviderName;
            PlanType = income.PlanType;
            IsNew = false;
            RelevantDate = (DateTime)income.RelevantDate;
            NetAmount = income.Amount;
            VAT = 0;
            Investment = 0;
            OnGoingPercentage = 0;
            PlanReference = income.PlanNumber;
            EstimatedOtherIncome = 0;
            RelatedClients = new string[] { };
        }
        
        public int Id { get; set; }
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
        [Display(Name="Advisor")]
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
        public string[] RelatedClients { get; set; }

        [NotMapped]
        public virtual MLFSClient Client { get; set; }
        public virtual MLFSAdvisor Advisor { get; set; }
        public virtual MLFSIncome Income { get; set; }
        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
        public virtual ICollection<MLFSDebtorAdjustment> Adjustments { get; set; }
    }
}
