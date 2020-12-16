using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name="External ID")]
        public string PrimaryID { get; set; }
        public Title Title { get; set; }
        public string Name { get; set; }
        [Display(Name = "Forename")]
        public string FirstName { get; set; }
        [Display(Name = "Surname")]
        public string LastName { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Number> Numbers { get; set; }
        [Display(Name = "emails")]
        public List<EmailAddress> EmailAddresses { get; set; }
    }
}
