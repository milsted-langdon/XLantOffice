using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Address
    {

        public static List<Address> CreateList(string jsonResponse)
        {
            List<Address> addresses = new List<Address>();
            JArray _array = Tools.ExtractItemsArrayFromJsonString(jsonResponse);
            foreach(JObject obj in _array)
            {
                Address a = new Address(JsonConvert.SerializeObject(obj));
                addresses.Add(a);
            }
            return addresses;
        }
    }
}
