using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XLantCore;
using System.ComponentModel.DataAnnotations;

namespace XLantCore.Models
{
    public partial class MLFSCommissionRate
    {

        public MLFSCommissionRate()
        {

        }

        public int Id { get; set; }
        [Display(Name = "Advsior")]
        public int AdvisorId { get; set; }
        [Display(Name = "Lower Bound")]
        public decimal StartingValue { get; set; }
        [Display(Name = "Upper Bound")]
        public decimal EndingValue { get; set; }
        public decimal Percentage { get; set; }

        public virtual MLFSAdvisor Advisor { get; set; }
    }
}
