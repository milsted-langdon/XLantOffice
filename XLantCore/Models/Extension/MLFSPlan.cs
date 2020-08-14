using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSPlan
    {
        /// <summary>
        /// Convert a string into PlanStatus handles nulls/blanks and no match using Unknown
        /// </summary>
        /// <param name="s">The string to convert</param>
        /// <returns></returns>
        public static PlanStatus ParsePlanStatus(string s)
        {
            PlanStatus status = PlanStatus.Unknown;
            s = s.Replace(" ", string.Empty);
            try
            {
                status = (PlanStatus)Enum.Parse(typeof(PlanStatus), s, true);
            }
            catch (Exception)
            {
                status = PlanStatus.Unknown;
            }
            return status;
        }

        /// <summary>
        /// Creates a list of plans from an array gained from IO
        /// </summary>
        /// <param name="jArray">The array from IO</param>
        /// <returns>a list of plans</returns>
        public static List<MLFSPlan> CreateList(JArray jArray)
        {
            List<MLFSPlan> plans = new List<MLFSPlan>();

            foreach (JObject p in jArray)
            {
                MLFSPlan plan = new MLFSPlan(p);
                plans.Add(plan);
            }
            return plans;
        }
    }
}
