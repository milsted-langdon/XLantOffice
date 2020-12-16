using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSAdvisor : Staff
    {
        public MLFSAdvisor()
        {

        }
        [Display(Name="Advisor to receive credit")]
        public int ReplacementAdvisorId { get; set; }

        public List<MLFSBudget> Budget { get; set; }
        public List<MLFSCommissionRate> CommissionRates { get; set; }
        public MLFSAdvisor ReplacementAdvisor { get; set; }
        
    }
}
