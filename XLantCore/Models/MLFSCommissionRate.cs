using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XLantCore;

namespace XLantCore.Models
{
    public partial class MLFSCommissionRate
    {

        public MLFSCommissionRate()
        {

        }

        public int Id { get; set; }
        public int AdvisorId { get; set; }
        public decimal StartingValue { get; set; }
        public decimal EndingValue { get; set; }
        public decimal Percentage { get; set; }

        public virtual MLFSAdvisor Advisor { get; set; }
    }
}
