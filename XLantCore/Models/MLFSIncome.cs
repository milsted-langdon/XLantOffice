using System;
using System.Collections.Generic;
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

        public MLFSIncome(DataRow row, List<MLFSAdvisor> advisors)
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
            Amount = Tools.HandleNull(row["GrossFCI Excl.VAT"].ToString());
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
        }


        public int? Id { get; set; }
        public string IOReference { get; set; }
        public int? ReportingPeriodId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RelevantDate { get; set; }
        public string Organisation { get; set; }
        public int AdvisorId { get; set; }
        public string ProviderName { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string JointClientName { get; set; }
        public string JointClientId { get; set; }
        public string Campaign { get; set; }
        public string CampaignSource { get; set; }
        public decimal Amount { get; set; }
        public string FeeStatus { get; set; }
        public string PlanType { get; set; }
        public string PlanNumber { get; set; }
        public bool IsTopUp { get; set; }
        public string IncomeType { get; set; }
        public DateTime? ClientOnBoardDate { get; set; }
        public bool IsClawBack { get; set; }
        public bool IsAdjustment { get; set; }

        public virtual MLFSAdvisor Advisor { get; set; }
        [NotMapped]
        public virtual MLFSClient Client { get; set; }
        [NotMapped]
        public virtual MLFSClient JointClient { get; set; }
        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
        public virtual MLFSDebtorAdjustment MLFSDebtorAdjustment { get; set; }
    }
}
