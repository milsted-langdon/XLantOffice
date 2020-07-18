using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Address
    {
        public Address()
        {

        }

        public Address(JObject jobject)
        {
            dynamic obj = jobject;
            PrimaryID = obj.id;
            IsPrimary = obj.isDefault;
            Line1 = obj.address.line1 + " " + obj.address.line2;
            Line2 = obj.address.line3;
            Town = obj.address.line4;
            City = obj.address.locality;
            County = obj.address.county.name;
            Postcode = obj.address.postalcode;
        }
        public int Id { get; set; }
        public String PrimaryID { get; set; }
        public Boolean IsPrimary { get; set; }
        public String Line1 { get; set; }
        public String Line2 { get; set; }
        public String Town { get; set; }
        public String City { get; set; }
        public String County { get; set; }
        public String Postcode { get; set; }
    }
}
