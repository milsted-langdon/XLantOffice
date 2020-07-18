using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSSale
    {
        public MLFSSale()
        {

        }

        public MLFSSale(DataRow row)
        {
            IOId = row["Id"].ToString();
            IOReference = row["Reference Number"].ToString();
            ClientName =  row["Fee Owner.Full Name"].ToString();
            ClientId = row["Fee Owner.Id"].ToString();
            JointClientName = row["Fee Owner 2.Full Name"].ToString();
            JointClientId = row["Fee Owner 2.Id"].ToString();
            AdvisorName = row["Selling Adviser.Full Name"].ToString();
            AdvisorId = row["Selling Adviser.Id"].ToString();
            PlanType = row["Related Plan Type"].ToString();
            IsNew = false;
            RelevantDate = DateTime.Parse(row["Invoice Date"].ToString());
            NetAmount = Tools.HandleNull(row["Net Amount"].ToString());
            VAT = Tools.HandleNull(row["VAT"].ToString());
            PlanReference = row["Related Plan Reference"].ToString();
            Investment = 0;
            OnGoingPercentage = 0;
            Paid = 0;
            Adjusted = 0;
            NotTakenUp = false;
            IncomeId = null;

        }

        public int? Id { get; set; }
        public string IOId { get; set; }
        public string IOReference { get; set; }
        public int? ReportPeriodId { get; set; }
        public string Organisation { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string JointClientName { get; set; }
        public string JointClientId { get; set; }
        public string AdvisorId { get; set; }
        public string AdvisorName { get; set; }
        public string ProviderName { get; set; }
        public string PlanType { get; set; }
        public bool IsNew { get; set; }
        public DateTime RelevantDate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VAT { get; set; }
        public decimal Investment { get; set; }
        public decimal OnGoingPercentage { get; set; }
        public decimal Paid { get; set; }
        public decimal Adjusted { get; set; }
        public bool NotTakenUp { get; set; }
        public int? IncomeId { get; set; }
        public string PlanReference { get; set; }

        [NotMapped]
        public virtual MLFSClient Client { get; set; }
        public virtual MLFSAdvisor Advisor { get; set; }
        public virtual MLFSIncome Income { get; set; }
        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
    }
}
