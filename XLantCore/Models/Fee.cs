using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Fee
    {
        public string PrimaryID { get; set; }
        public DateTime? SentToClient { get; set; }
        public string FeeType { get; set; }
        public Staff Advisor { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VAT { get; set; }
        public bool IsRecurring { get; set; }
        public int RecuringFrequency { get; set;}
        public DateTime? RecurringStart { get; set; }
        public DateTime? RecurringEnd { get; set; }
        public string PaidBy { get; set; }
        public int InitialPeriod { get; set; }
        public Plan Plan { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountTotal { get; set; }
        public List<MLFSClient> Clients { get; set; }
        public decimal FeePercentage { get; set; }
    }
}
