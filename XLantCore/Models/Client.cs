using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Client
    {
        public Client()
        {
        }

        public string PrimaryID { get; set; }
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public bool IsIndividual { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public Staff ClientOwner { get; set; }
        public Person Person { get; set; }
        public Organisation Organisation { get; set; }
    }
}
