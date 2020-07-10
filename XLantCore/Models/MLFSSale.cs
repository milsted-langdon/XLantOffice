using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSSale
    {
        public MLFSSale()
        {

        }

        public MLFSSale(DataRow row, bool isIOData)
        {
            if (isIOData)
            {
                IOId = row["Id"].ToString();
                IOReference = row["Reference Number"].ToString();
                ClientName =  row["Fee Owner.Full Name"].ToString();
                ClientId = row["Fee Owner.Id"].ToString();
                JointClientName = row["Fee Owner2.Full Name"].ToString();
                JointClientId = row["Fee Owner2.Id"].ToString();
                AdvisorName = row["Selling Advisor.Full Name"].ToString();
                AdvisorId = row["Selling Advisor.Id"].ToString();
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
            else
            {
                Id = (int)row["Id"];
                IOReference = row["IOReference"].ToString();
                MLFSReportPeriodId = (int)row["ReportingPeriodId"];
                Organisation = row["Organisation"].ToString();
                ClientName = row["ClientName"].ToString();
                ClientId = row["ClientId"].ToString();
                JointClientName = row["JointClientName"].ToString();
                JointClientId = row["JointClientId"].ToString();
                AdvisorId = row["AdvisorId"].ToString();
                AdvisorName = row["AdvisorName"].ToString();
                ProviderName = row["ProviderName"].ToString();
                PlanType = row["PlanType"].ToString();
                IsNew = (bool)row["IsNew"];
                RelevantDate = DateTime.Parse(row["RelevantDate"].ToString());
                NetAmount = Tools.HandleNull(row["NetAmount"].ToString());
                VAT = Tools.HandleNull(row["VAT"].ToString());
                Investment = Tools.HandleNull(row["Investment"].ToString());
                OnGoingPercentage = Tools.HandleNull(row["OngoingPercentage"].ToString());
                Paid = Tools.HandleNull(row["Paid"].ToString());
                Adjusted = Tools.HandleNull(row["Adjusted"].ToString());
                NotTakenUp = (bool)row["NotTakenUp"];
                IncomeId = (int)row["IncomeId"];
                PlanReference = row["PlanReference"].ToString();
            }
        }

        public int? Id { get; set; }
        public string IOId { get; set; }
        public string IOReference { get; set; }
        public int? MLFSReportPeriodId { get; set; }
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

        public virtual MLFSClient Client { get; set; }
        public virtual MLFSAdvisor Advisor { get; set; }
        public virtual MLFSIncome Income { get; set; }
    }
}
