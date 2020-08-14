using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Address
    {
        /// <summary>
        /// Create a list of addresses when provided with a JSON array of data from IO
        /// </summary>
        /// <param name="_array">the array stripped from the json object</param>
        /// <returns>a list of address objects</returns>
        public static List<Address> CreateList(JArray _array)
        {
            List<Address> addresses = new List<Address>();
            foreach(JObject obj in _array)
            {
                Address a = new Address(obj);
                addresses.Add(a);
            }
            return addresses;
        }
    }
}
