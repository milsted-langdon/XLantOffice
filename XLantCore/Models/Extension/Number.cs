using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Number
    {
        /// <summary>
        /// Create a list of numbers based on an array passed from io
        /// </summary>
        /// <param name="_array">the json array</param>
        /// <returns>a list of number objects</returns>
        public static List<Number> CreateList(JArray _array)
        {
            List<Number> numbers = new List<Number>();
            if (_array == null)
            {
                return null;
            }
            else
            {
                foreach (JObject obj in _array)
                {
                    Number n = new Number(obj);
                    numbers.Add(n);
                }
                return numbers; 
            }
        }
    }
}
