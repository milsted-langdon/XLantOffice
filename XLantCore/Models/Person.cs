using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Person
    {
        public Person()
        {
            Addresses = new List<Address>();
            Numbers = new List<Number>();
            EmailAddresses = new List<EmailAddress>();
        }

        public int Id { get; set; }
        public string PrimaryID { get; set; }
        public Title Title { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Number> Numbers { get; set; }
        public List<EmailAddress> EmailAddresses { get; set; }
    }
}
