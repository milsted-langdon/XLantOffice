using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSIncome
    {
        public MLFSIncome()
        {

        }

        public MLFSIncome(DataRow row, List<MLFSAdvisor> advisors, List<string> VATIssueFees = null)
        {
            IOReference = row["IORef"].ToString();
            RelevantDate = Tools.HandleStringToDate(row["CashReceiptDate"].ToString());
            Organisation = row["GroupOne"].ToString();
            MLFSAdvisor adv = MLFSAdvisor.Assign(row["CRMContactId"].ToString(), advisors);
            AdvisorId = adv.Id;
            Advisor = adv;
            ProviderName = row["Provider"].ToString();
            ClientName = row["ClientName"].ToString();
            ClientId = row["ClientId"].ToString();
            JointClientName = row["JointClientName"].ToString();
            JointClientId = row["JointClientId"].ToString();
            Campaign = row["CampaignType"].ToString();
            CampaignSource = row["CampaignSource"].ToString();
            Amount = Tools.HandleNull(row["FCIRecognition"].ToString());
            if (Tools.HandleNull(row["ReceivedVAT/GST"].ToString()) != 0)
            {
                if (VATIssueFees != null)
                {
                    if (VATIssueFees.Contains(IOReference))
                    {
                        Amount += Tools.HandleNull(row["ReceivedVAT"].ToString());
                    }
                    else
                    {
                        VAT = Tools.HandleNull(row["ReceivedVAT"].ToString());
                    }
                }
            }
            else
            {
                VAT = 0;
            }
            FeeStatus = row["FeeStatus"].ToString();
            PlanType = row["PlanType"].ToString();
            PlanNumber = row["PlanNumber"].ToString();
            if (row["IsTopUp"].ToString() == "" || row["IsTopUp"].ToString().ToLower() == "no")
            {
                IsTopUp = false;
            }
            else
            {
                IsTopUp = true;
            }
            IncomeType = row["IncomeType"].ToString();
            IgnoreFromCommission = false;
        }


        public int? Id { get; set; }
        [Display(Name = "External ID")]
        public string IOReference { get; set; }
        [Display(Name = "Reporting Period")]
        public int? ReportingPeriodId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? RelevantDate { get; set; }
        public string Organisation { get; set; }
        [Display(Name = "Advisor")]
        public int AdvisorId { get; set; }
        [Display(Name = "Provider")]
        public string ProviderName { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        [Display(Name = "Joint Client Name")]
        public string JointClientName { get; set; }
        public string JointClientId { get; set; }
        public string Campaign { get; set; }
        [Display(Name = "Campaign Source")]
        public string CampaignSource { get; set; }
        public decimal Amount { get; set; }
        [DefaultValue(0.00)]
        public decimal VAT { get; set; }
        [Display(Name = "Fee Status")]
        public string FeeStatus { get; set; }
        [Display(Name = "Plan Type")]
        public string PlanType { get; set; }
        [Display(Name = "Plan Number")]
        public string PlanNumber { get; set; }
        [Display(Name = "Top Up")]
        public bool IsTopUp { get; set; }
        [Display(Name = "Income Type")]
        public string IncomeType { get; set; }
        [Display(Name = "Client Created Date")]
        public DateTime? ClientOnBoardDate { get; set; }
        [Display(Name = "Clawback")]
        public bool IsClawBack { get; set; }
        [Display(Name = "Adjustment")]
        public bool IsAdjustment { get; set; }
        [Display(Name = "Ignore for Commission Calculation")]
        public bool IgnoreFromCommission { get; set; }


        public virtual MLFSAdvisor Advisor { get; set; }
        [NotMapped]
        public virtual MLFSClient Client { get; set; }
        [NotMapped]
        public virtual MLFSClient JointClient { get; set; }
        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
        public virtual MLFSDebtorAdjustment MLFSDebtorAdjustment { get; set; }
    }
}
