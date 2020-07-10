using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using XLant;

namespace XLantCore.Models
{
    public partial class MLFSCommissionRate
    {

        public MLFSCommissionRate()
        {

        }

        public MLFSCommissionRate(DataRow row)
        {
            Id = (int)row["Id"];
            AdvisorId = (int)row["AdvisorId"];
            StartingValue = XLtools.HandleNull(row["StartingValue"].ToString());
            EndingValue = XLtools.HandleNull(row["EndingValue"].ToString());
            Percentage = XLtools.HandleNull(row["Percentage"].ToString());
        }

        public int Id { get; set; }
        public int AdvisorId { get; set; }
        public decimal StartingValue { get; set; }
        public decimal EndingValue { get; set; }
        public decimal Percentage { get; set; }
    }
}
