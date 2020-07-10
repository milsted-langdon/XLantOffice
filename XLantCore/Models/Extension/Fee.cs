using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Fee
    {

        public static List<Fee> CreateList(JArray jarray)
        {
            List<Fee> fees = new List<Fee>();
            foreach (JObject p in jarray)
            {
                Fee fee = new Fee(p);
                fees.Add(fee);
            }
            return fees;
        }
    }
}
