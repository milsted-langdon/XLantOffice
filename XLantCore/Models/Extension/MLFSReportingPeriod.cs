using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSReportingPeriod
    {
        public DateTime StartDate
        {
            get
            {
                DateTime start = DateTime.Parse(String.Format("{0}/{1}/{2}", 1, Month, Year));
                return start;
            }
        }
    }
}
