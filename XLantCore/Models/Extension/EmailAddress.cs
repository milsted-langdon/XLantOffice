using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class EmailAddress
    {
        public static List<EmailAddress> CreateList(string jsonResponse)
        {
            List<EmailAddress> emails = new List<EmailAddress>();
            JArray _array = Tools.ExtractItemsArrayFromJsonString(jsonResponse);
            foreach (JObject obj in _array)
            {
                EmailAddress a = new EmailAddress(JsonConvert.SerializeObject(obj));
                emails.Add(a);
            }
            return emails;
        }
    }
}
