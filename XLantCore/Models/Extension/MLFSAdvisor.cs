using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSAdvisor
    {
        /// <summary>
        /// Converts the external id from IO into a local Advisor object
        /// </summary>
        /// <param name="externalId">The ID from IO</param>
        /// <param name="advisors">The list of local advisors to check against</param>
        /// <returns>an Advisor or if no match is found the unknown advisor</returns>
        public static MLFSAdvisor Assign(string externalId, List<MLFSAdvisor> advisors)
        {
            MLFSAdvisor adv = advisors.Where(x => x.PrimaryID == externalId).FirstOrDefault();
            if (adv == null)
            {
                adv = advisors.Where(x => x.Username.ToLower() == "unknown").FirstOrDefault();
            }
            return adv;
        }
    }
}
