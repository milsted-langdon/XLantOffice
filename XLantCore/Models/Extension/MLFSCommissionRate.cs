using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSCommissionRate
    {
        public static List<MLFSCommissionRate> CreateList(DataTable table)
        {
            List<MLFSCommissionRate> budgets = new List<MLFSCommissionRate>();
            foreach(DataRow row in table.Rows)
            {
                MLFSCommissionRate budget = new MLFSCommissionRate(row);
                budgets.Add(budget);
            }
            return budgets;
        }
    }
}
