using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSBudget
    {
        public static List<MLFSBudget> CreateList(DataTable table)
        {
            List<MLFSBudget> budgets = new List<MLFSBudget>();
            foreach(DataRow row in table.Rows)
            {
                MLFSBudget budget = new MLFSBudget(row);
                budgets.Add(budget);
            }
            return budgets;
        }
    }
}
