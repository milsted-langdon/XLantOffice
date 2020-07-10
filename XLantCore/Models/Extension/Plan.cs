using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Plan
    {

        public static List<Plan> CreateList(JArray jArray)
        {
            List<Plan> plans = new List<Plan>();
            
            foreach (JObject p in jArray)
            {
                Plan plan = new Plan(p);
                plans.Add(plan);
            }
            return plans;
        }
    }
}
