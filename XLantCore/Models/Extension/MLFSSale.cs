using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSSale
    {
        public decimal GrossAmount
        {
            get
            {
                return Tools.HandleNull(NetAmount) + Tools.HandleNull(VAT);
            }
        }
    }
}
