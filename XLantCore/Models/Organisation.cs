using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Organisation
    {
        public string PrimaryID { get; set; }
        public string Name { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Number> Numbers { get; set; }
        public List<Person> Employees { get; set; }
        public Person PrimaryContact { get; set; }
    }
}
