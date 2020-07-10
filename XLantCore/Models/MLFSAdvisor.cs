using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSAdvisor :Staff
    {
        public MLFSAdvisor(DataRow row)
        {
            Name = row["name"].ToString();
            PrimaryID = row["id"].ToString();
            Department = row["Organisation"].ToString();
        }

        public List<MLFSBudget> Budget { get; set; }
        public List<MLFSCommissionRate> CommissionRates { get; set; }
    }
}
