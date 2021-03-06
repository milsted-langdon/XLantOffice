﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class EmailAddress
    {
        public EmailAddress()
        {

        }

        public EmailAddress(JObject jobject)
        {
            dynamic obj = jobject;
            PrimaryID = obj.id;
            Address = obj.value;
            DisplayName = obj.type;
            IsPrimary = obj.isDefault;
        }

        public int Id { get; set; }
        public String PrimaryID { get; set; }
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public bool IsPrimary { get; set; }
    }
}
