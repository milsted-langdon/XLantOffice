using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Number
    {
        public Number()
        {

        }

        public Number(string jsonString)
        {
            dynamic obj = JObject.Parse(jsonString);
            PrimaryID = obj.id;
            Description = obj.type;
            PhoneNumber = obj.value;
            IsPrimary = obj.isDefault;
        }

        public string PrimaryID { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
    }
}
