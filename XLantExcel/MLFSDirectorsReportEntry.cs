using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLant;

namespace XLantExcel
{
    class MLFSDirectorsReportEntry
    {
        public MLFSDirectorsReportEntry()
        {

        }

        public MLFSDirectorsReportEntry(DataRow row)
        {
            Advisor_Name = row["Fee Owner.Servicing Adviser.Full Name"].ToString();
            Client_Name = row["Fee Owner.Full Name"].ToString();
            Policy_Number = row["Policy Number"].ToString();
            Plan_Type = row["Related Plan Type"].ToString();
            New_Or_Existing = "Existing";
            Status = row["Fee Status"].ToString();
            STP_Date = XLtools.HandleStringToDate(row["Invoice Date"].ToString());
            Net_Amount = XLtools.HandleNull(row["Net Amount"].ToString());
            VAT = XLtools.HandleNull(row["VAT"].ToString());
            Total_Amount = XLtools.HandleNull(row["Total Amount"].ToString());
            Trail_12mths = 0;
            Initial_Passed = false;
            Trail_Passed = false;
            Charging_Type = row["Charging Type"].ToString();
            Payment_Type = row["Payment Type"].ToString();
            Paid_By = row["Paid By"].ToString();
            Campaign_Type = "";
            Campaign_Source = "";
            Provider = "";
            NonDisplay_Client_Creation_Date = DateTime.Now;
            Investment = 0;
            Ongoing_Percentage = 0;
        }

        public string Advisor_Name { get; set; }
        public string Client_Name { get; set; }
        public string Provider { get; set; }
        public string Policy_Number { get; set; }
        public string Plan_Type { get; set; }
        public DateTime? NonDisplay_Client_Creation_Date { get; set; }
        public string New_Or_Existing { get; set; }
        public string Status { get; set; }
        public DateTime? STP_Date { get; set; }
        public decimal Net_Amount { get; set; }
        public decimal VAT { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal Investment { get; set; }
        public decimal Ongoing_Percentage { get; set; }
        public decimal Trail_12mths { get; set; }
        public bool Initial_Passed { get; set; }
        public bool Trail_Passed { get; set; }
        public string Charging_Type { get; set; }
        public string Payment_Type { get; set; }
        public string Paid_By { get; set; }
        public string Campaign_Type { get; set; }
        public string Campaign_Source { get; set; }

    }
}
