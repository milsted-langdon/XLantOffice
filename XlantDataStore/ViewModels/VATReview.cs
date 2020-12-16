using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class VATReview
    {
        public VATReview()
        {

        }

        public string Period { get; set; }
        public int PeriodId { get; set; }
        public int Quarter { get; set; }
        public bool NewBusiness { get; set; }
        public int Id { get; set; }
        public string IOReference { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VAT { get; set; }
        public decimal GrossAmount { get; set; }


        public static List<VATReview> CreateList(List<MLFSSale> sales, List<MLFSIncome> income, MLFSReportingPeriod period)
        {
            List<VATReview> review = new List<VATReview>();
            sales = sales.Where(x => x.VAT != 0).ToList();
            income = income.Where(x => x.VAT != 0).ToList();
            foreach (MLFSSale s in sales)
            {
                review.Add(new VATReview()
                {
                    Period = period.Description,
                    PeriodId = period.Id,
                    Quarter = period.Quarter,
                    NewBusiness = true,
                    Id = s.Id,
                    IOReference = s.IOReference,
                    ClientName = s.ClientName,
                    ClientId = s.ClientId,
                    NetAmount = s.NetAmount,
                    VAT = s.VAT,
                    GrossAmount = s.GrossAmount
                });
            }
            foreach (MLFSIncome i in income)
            {
                review.Add(new VATReview()
                {
                    Period = period.Description,
                    PeriodId = period.Id,
                    Quarter = period.Quarter,
                    NewBusiness = false,
                    Id = (int)i.Id,
                    IOReference = i.IOReference,
                    ClientName = i.ClientName,
                    ClientId = i.ClientId,
                    NetAmount = i.Amount - i.VAT,
                    VAT = i.VAT,
                    GrossAmount = i.Amount,
                });
            }
            return review;
        }
    }
}
