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
            if (sale.InitialFeePass)
            {
                InitialPassed = "Passed"; 
            }
            else
            {
                InitialPassed = "Failed";
            }
            if (sale.OngoingFeePass)
            {
                OngoingPassed = "Passed";
            }
            else
            {
                OngoingPassed = "Failed";
            }
            if (Investment != 0 && sale.OnGoingPercentage != 0)
            {
                TwelveMonthsTrail = Investment * sale.OnGoingPercentage / 100; 
            }
            else
            {
                TwelveMonthsTrail = 0;
            }
            OtherIncome = sale.EstimatedOtherIncome;
        }

        public int ReportingPeriodId { get; set; }
        public string Advisor { get; set; }
        [Display(Name="Client Name")]
        public string ClientName { get; set; }
        public string ClientId { get; set; }
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
        [Display(Name = "Initial Passed")]
        public string InitialPassed { get; set; }
        [Display(Name = "Ongoing Passed")]
        public string OngoingPassed { get; set; }
        [Display(Name = "Other Income")]
        [DataType(DataType.Currency)]
        public decimal OtherIncome { get; set; }

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
            
            return report;
        }
    }
}
