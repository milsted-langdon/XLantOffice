using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class DirectorsReport
    {
        public DirectorsReport()
        {

        }
        public DirectorsReport(MLFSSale sale)
        {
            if (sale.Advisor != null)
            {
                Advisor = sale.Advisor.Fullname;
            }
            else
            {
                Advisor = "Unknown";
            }
            ClientName = sale.ClientName;
            ClientId = sale.ClientId;
            JointClientId = sale.JointClientId;
            PlanId = sale.PlanReference;
            ReportingPeriodId = (int)sale.ReportingPeriodId;
            Provider = sale.ProviderName;
            if (sale.IsNew)
            {
                NewExisting = "New";
            }
            else
            {
                NewExisting = "Existing";
            }
            InitialFee = sale.NetAmount;
            Investment = sale.Investment;
            OngoingPercentage = sale.OnGoingPercentage;
            if (Investment != 0 && sale.OnGoingPercentage != 0)
            {
                TwelveMonthsTrail = Investment * sale.OnGoingPercentage / 100; 
            }
            else
            {
                TwelveMonthsTrail = 0;
            }
            OtherIncome = sale.EstimatedOtherIncome;
            RelatedClients = sale.RelatedClients;
        }

        public int ReportingPeriodId { get; set; }
        public string Advisor { get; set; }
        [Display(Name="Client Name")]
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string JointClientId { get; set; }
        public string PlanId { get; set; }
        public string Provider { get; set; }
        [Display(Name="New/Existing")]
        public string NewExisting { get; set; }
        [Display(Name = "Initial Fee")]
        [DataType(DataType.Currency)]
        public decimal InitialFee { get; set; }
        [DataType(DataType.Currency)]
        public decimal Investment { get; set; }
        [Display(Name = "Ongoing Percentage")]
        public decimal OngoingPercentage { get; set; }
        [Display(Name = "12 Month Trail")]
        [DataType(DataType.Currency)]
        public decimal TwelveMonthsTrail { get; set; }
        [Display(Name = "Other Income")]
        [DataType(DataType.Currency)]
        public decimal OtherIncome { get; set; }
        public string[] RelatedClients { get; set; }

        /// <summary>
        /// Creates a list for a report based on the sales provided
        /// </summary>
        /// <param name="sales">The list of sales to be converted</param>
        /// <returns>A list of report lines</returns>
        public static List<DirectorsReport> Create(List<MLFSSale> sales)
        {
            List<DirectorsReport> report = new List<DirectorsReport>();
            foreach (MLFSSale sale in sales)
            {
                DirectorsReport line = new DirectorsReport(sale);
                report.Add(line);
            }

            //group where they are the same client
            report = report.GroupBy(x => x.ClientId).Select(y => new DirectorsReport()
            {
                ReportingPeriodId = y.FirstOrDefault().ReportingPeriodId,
                Advisor = y.FirstOrDefault().Advisor,
                ClientName = y.FirstOrDefault().ClientName,
                ClientId = y.Key,
                JointClientId = y.Where(z => z.JointClientId != null).FirstOrDefault().JointClientId,
                NewExisting = y.FirstOrDefault().NewExisting,
                InitialFee = y.Sum(z => z.InitialFee),
                Investment = y.Sum(z => z.Investment),
                TwelveMonthsTrail = y.Sum(z => z.TwelveMonthsTrail),
                OngoingPercentage = y.Sum(z => z.TwelveMonthsTrail) == 0 ? 0 : y.Sum(z => z.TwelveMonthsTrail) / y.Sum(z => z.Investment) * 100,
                OtherIncome = y.FirstOrDefault().OtherIncome,
                RelatedClients = y.FirstOrDefault().RelatedClients
            }).ToList();

            //group where they are related
            List<string> checkedIds = new List<string>();
            for (int i = 0; i < report.Count; i++)
            {
                DirectorsReport rep = report[i];
                if (!checkedIds.Contains(rep.ClientId))
                {
                    for(int j = 0; j < report.Count; j++)
                    {
                        DirectorsReport r = report[j];
                        if (rep.JointClientId == r.ClientId || (rep.RelatedClients != null && rep.RelatedClients.Contains(r.ClientId)))
                        {
                            //Add the initial fees together
                            rep.InitialFee += r.InitialFee;
                            //log the id
                            checkedIds.Add(r.ClientId);
                            //update the name
                            rep.ClientName += " & " + r.ClientName;
                            report.Remove(r);
                        }
                    }
                }
            }
            return report;
        }

        [Display(Name = "Initial Passed")]
        public bool InitialPassed
        {
            get
            {
                bool pass = false;

                if (InitialFee >= 450)
                {
                    pass = true;
                }
                else
                {
                    if (Investment != 0)
                    {
                        if (InitialFee / Investment >= (decimal)0.03)
                        {
                            pass = true;
                        }
                    }
                }
                return pass;

            }
        }

        /// <summary>
        /// Read Only - Tests whether the ongoing fee criteria is met is it greater that £500 for 12 months if not is it >= 1%
        /// </summary>
        [Display(Name = "Ongoing Passed")]
        public bool OngoingPassed
        {
            get
            {
                bool pass = false;
                decimal twelveMonthsIncome = Investment * OngoingPercentage / 100;
                twelveMonthsIncome += OtherIncome;
                if (twelveMonthsIncome >= 500)
                {
                    pass = true;
                }
                else
                {
                    if (OngoingPercentage >= (decimal)1.00)
                    {
                        pass = true;
                    }
                }
                return pass;
            }
        }

    }
}
