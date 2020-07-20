using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSIncome
    {
        public bool IsNew
        {
            get
            {
                if (IncomeType == "Ongoing" || IncomeType == "Renewal Commission")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
