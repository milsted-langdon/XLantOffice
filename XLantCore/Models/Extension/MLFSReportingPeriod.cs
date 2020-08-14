using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;


namespace XLantCore.Models
{
    public partial class MLFSReportingPeriod
    {
        /// <summary>
        /// Read Only - The first day of the month which the period represents
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                DateTime start = DateTime.Parse(String.Format("{0}/{1}/{2}", 1, Month, Year));
                return start;
            }
        }

        /// <summary>
        /// Read Only - Returns the year prior in the format 2001
        /// </summary>
        public string PriorYear
        {
            get
            {
                int startOfThisYear = int.Parse(FinancialYear.Substring(0, 2));
                string value = (startOfThisYear -1).ToString().Substring(2);
                value += "/" + (startOfThisYear).ToString().Substring(2);
                return value;
            }
        }

        /// <summary>
        /// Read Only - Provides the numerical value of the quarter the period falls into based on the report order.
        /// </summary>
        public int Quarter
        {
            get
            {
                if(ReportOrder <= 3)
                {
                    return 1;
                }
                if (ReportOrder <= 6)
                {
                    return 2;
                }
                if (ReportOrder <= 9)
                {
                    return 3;
                }
                return 4;
            }
        }

    }
}
