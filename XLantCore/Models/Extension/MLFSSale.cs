using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSSale
    {
        public decimal GrossAmount
        {
            get
            {
                return Tools.HandleNull(NetAmount) + Tools.HandleNull(VAT);
            }
        }

        public bool InitialFeePass
        {
            get
            {
                bool pass = false;
                if (NetAmount >= 450)
                {
                    pass = true;
                }
                else
                {
                    if (NetAmount/Investment >= (decimal)0.03)
                    {
                        pass = true;
                    }
                }
                return pass;
            }
        }

        public bool OngoingFeePass
        {
            get
            {
                bool pass = false;
                decimal twelveMonthsIncome = Investment * OnGoingPercentage * 12;
                if (twelveMonthsIncome >= 500)
                {
                    pass = true;
                }
                else
                {
                    if (OnGoingPercentage >= (decimal)0.01)
                    {
                        pass = true;
                    }
                }
                return pass;
            }
        }

        public void AddPlanData(DataRow row)
        {
            ProviderName = row["Provider.Name"].ToString();
            DateTime creationDate = DateTime.Parse(row["Owner 1.Creation Date"].ToString());
            if (creationDate > ReportingPeriod.StartDate.AddMonths(-3))
            {
                IsNew = true;
            }
            Investment = Tools.HandleNull(row["Total Premiums to Date"].ToString());
            OnGoingPercentage = Tools.HandleNull(row["On-going Fee Percentage"].ToString());
            Organisation = row["Selling Adviser.Group.Name"].ToString();
        }

        public void AddPlanData(Plan plan, Fee fee)
        {
            ProviderName = plan.Provider;
            if (!plan.IsPreExistingClient)
            {
                IsNew = true;
            }
            Investment = plan.ContributionsToDate;
            if (fee != null)
            {
                OnGoingPercentage = fee.FeePercentage;
                Organisation = "";
            }
            
        }
    }
}
