using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSFee
    {
        /// <summary>
        /// Creates a list of fees from a jarray of IO data
        /// </summary>
        /// <param name="jarray">the array of items from IO</param>
        /// <returns>a list of fees</returns>
        public static List<MLFSFee> CreateList(JArray jarray)
        {
            List<MLFSFee> fees = new List<MLFSFee>();
            if (jarray == null)
            {
                return fees;
            }
            foreach (JObject p in jarray)
            {
                if (p != null)
                {
                    MLFSFee fee = new MLFSFee(p);
                    fees.Add(fee);
                }
            }
            return fees;
        }
    }
}
