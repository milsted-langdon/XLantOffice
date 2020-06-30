using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Number
    {
        public static List<Number> CreateList(string jsonResponse)
        {
            List<Number> numbers = new List<Number>();
            JArray _array = Tools.ExtractItemsArrayFromJsonString(jsonResponse);
            foreach (JObject obj in _array)
            {
                Number n = new Number(JsonConvert.SerializeObject(obj));
                numbers.Add(n);
            }
            return numbers;
        }
    }
}
