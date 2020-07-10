using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Address
    {

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
