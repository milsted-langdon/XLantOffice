using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSAdvisor : Staff
    {
        public MLFSAdvisor()
        {

        }
        public int ReplacementAdvisorId { get; set; }

        public List<MLFSBudget> Budget { get; set; }
        public List<MLFSCommissionRate> CommissionRates { get; set; }
        
    }
}
