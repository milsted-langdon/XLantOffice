using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class NewBusiness
    {
        public NewBusiness()
        {

        }

        public string Period { get; set; }
        public int PeriodId { get; set; }
        public int Quarter { get; set; }
        public string Advisor { get; set; }
        public int AdvisorId { get; set; }
        public string Organisation { get; set; }
        [Display(Name="New Clients")]
        public decimal NewClients { get; set; }
        [Display(Name = "Existing Clients")]
        public decimal ExistingClients { get; set; }
        public decimal Total { get; set; }


        public static List<NewBusiness> CreateList(List<MLFSSale> sales, MLFSReportingPeriod period)
        {
            List<NewBusiness> reports = sales.GroupBy(x => x.Advisor).Select(y => new NewBusiness()
            {
                Period = period.Description,
                PeriodId = period.Id,
                Quarter = period.Quarter,
                Advisor = y.Key.Fullname,
                AdvisorId = y.Key.Id,
                Organisation = y.Key.Department,
                NewClients = y.Where(a => a.IsNew).Sum(z => z.NetAmount),
                ExistingClients = y.Where(a => !a.IsNew).Sum(z => z.NetAmount),
                Total = y.Sum(z => z.NetAmount)
            }).ToList();

            return reports;
        }
    }
}
