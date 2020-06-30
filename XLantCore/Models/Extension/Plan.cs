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

        public static List<Plan> CreateList(string jsonResponse)
        {
            List<Plan> plans = new List<Plan>();
            JArray jarray = Tools.ExtractItemsArrayFromJsonString(jsonResponse);
            foreach (JObject p in jarray)
            {
                Plan plan = new Plan(JsonConvert.SerializeObject(p));
                plans.Add(plan);
            }
            return plans;
        }
    }
}
