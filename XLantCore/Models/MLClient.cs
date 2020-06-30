using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLClient: Client
    {
        public string Department { get; set; }
        public string Office { get; set; }
        public Staff Partner { get; set; }
        public Staff Manager { get; set; }
        public decimal CurrentWIP { get; set; }
        public decimal DebtorTotal { get; set; }
    }
}
