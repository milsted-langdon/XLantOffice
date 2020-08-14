using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSFee
    {
        public MLFSFee()
        {
            Clients = new List<MLFSClient>();
        }

        public MLFSFee(JObject fee)
        {
            Clients = new List<MLFSClient>();
            dynamic f = fee;
            PrimaryID = f.id;
            SentToClient = f.sentToClientOn;
            FeeType = f.feeChargingType.name;
            if (f.sellingAdvisor != null)
            {
                string advisorID = f.sellingAdvisor.id;
                Advisor = new Staff(advisorID);
            }
            NetAmount = f.net.amount;
            VAT = f.vat.amount;
            if (f.recurring == null)
            {
                IsRecurring = false;
                RecurringFrequency = "";
                RecurringStart = null;
                RecurringEnd = null;
            }
            else
            {
                IsRecurring = true;
                RecurringFrequency = f.recurring.frequency;
                if (f.recurring.startsOn != null)
                {
                    RecurringStart = Tools.HandleStringToDate(f.recurring.startsOn.ToString()); 
                }
                if (f.recurring.endsOn != null)
                {
                    RecurringEnd = Tools.HandleStringToDate(f.recurring.endsOn.ToString()); 
                }
            }
            PaidBy = f.paymentType.paidBy;
            InitialPeriod = f.initialPeriod;
            if (f.plan_href != null)
            {
                string planId = f.plan_href.ToString();
                planId = planId.Substring(planId.IndexOf('(') + 1, planId.LastIndexOf(')') - planId.IndexOf('(') - 1);
                Plan = new MLFSPlan(planId); 
            }
            if (f.discount != null)
            {
                DiscountPercentage = f.discount.percentage;
                DiscountTotal = f.discount.total.amount;
            }
            Clients = MLFSClient.CreateSummaryList(JArray.FromObject(f.clients));
            FeePercentage = f.feePercentage;
        }

        public string PrimaryID { get; set; }
        public DateTime? SentToClient { get; set; }
        public string FeeType { get; set; }
        public Staff Advisor { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VAT { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurringFrequency { get; set;}
        public DateTime? RecurringStart { get; set; }
        public DateTime? RecurringEnd { get; set; }
        public string PaidBy { get; set; }
        public int? InitialPeriod { get; set; }
        public MLFSPlan Plan { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountTotal { get; set; }
        public List<MLFSClient> Clients { get; set; }
        public decimal FeePercentage { get; set; }
    }
}
