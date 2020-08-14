using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSPlan
    {
        public MLFSPlan()
        {

        }

        public MLFSPlan(string id)
        {
            PrimaryID = id;
        }

        public MLFSPlan(JObject plan)
        {
            dynamic p = plan;
            PrimaryID = p.id;
            if (p.productProvider != null)
            {
                Provider = p.productProvider.name; 
            }
            Reference = p.reference;
            StartDate = p.startOn;
            if (p.sellingAdviser != null)
            {
                string advisorID = p.sellingAdviser.id;
                Advisor = new Staff(advisorID);
            }
            if (p.paraplanner != null)
            {
                string paraID = p.paraplanner.id;
                ParaPlanner = new Staff(paraID);
            }
            if (p.administrator != null)
            {
                string adminID = p.administrator.id;
                ParaPlanner = new Staff(adminID);
            }
            PlanType = p.planType.name;
            IsPreExistingClient = p.isPreExisting;
            ProductName = p.productName;
            Status = MLFSPlan.ParsePlanStatus(p.currentStatus.Value);
            Clients = MLFSClient.CreateSummaryList(JArray.FromObject(p.owners));
            IsTopUp = p.isTopup;
            if (p.latestValuation != null)
            {
                CurrentValuation = p.latestValuation.value.amount;
            }
        }
        public string PrimaryID { get; set; }
        public string Provider { get; set; }
        public string Reference { get; set; }
        public string StartDate { get; set; }
        public Staff Advisor { get; set; }
        public Staff ParaPlanner { get; set; }
        public string PlanType { get; set; }
        public bool IsPreExistingClient { get; set; }
        public Staff Administrator { get; set; }
        public string ProductName { get; set; }
        public PlanStatus Status { get; set; }
        public List<MLFSClient> Clients { get; set; }
        public bool IsTopUp { get; set; }
        public decimal CurrentValuation { get; set; }
        public decimal ContributionsToDate { get; set; }
    }
}
