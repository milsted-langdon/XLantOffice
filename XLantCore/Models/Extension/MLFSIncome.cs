using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSIncome
    {
        /// <summary>
        /// Read Only - Whether the income is generated from new business or represents trail income
        /// </summary>
        public bool IsNewBusiness
        {
            get
            {
                if (IncomeType == "Ongoing Fee" || IncomeType == "Renewal Commission" || IncomeType == "Fund Based Commission")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Read Only - Reflects whether the client has been added in the last 9 months (if so treated as new)
        /// If the system can't assertain the join date it returns false
        /// </summary>
        public bool IsNewClient
        {
            get
            {
                if (RelevantDate != null)
                {
                    DateTime relevantDate = (DateTime)RelevantDate;
                    if (ClientOnBoardDate != null && ClientOnBoardDate >= relevantDate.AddMonths(-9))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Based on the client list it will update the income lines with the relevant created on date
        /// </summary>
        /// <param name="incomeList">List of income you want to update</param>
        /// <param name="clients">the client data you want to use for reference</param>
        public static void UpdateFromIO(List<MLFSIncome> incomeList, List<MLFSClient> clients)
        {
            foreach(MLFSClient client in clients)
            {
                List<MLFSIncome> sublist = incomeList.Where(x => x.ClientId == client.PrimaryID).ToList();
                foreach (MLFSIncome income in sublist)
                {
                    income.ClientOnBoardDate = client.CreatedOn;
                }
            }
        }
    }
}
