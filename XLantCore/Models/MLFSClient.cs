using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSClient: Client
    {
        public MLFSClient()
        {
            Plans = new List<Plan>();
            Fees = new List<Fee>();
        }

        public MLFSClient(string id)
        {
            Plans = new List<Plan>();
            Fees = new List<Fee>();
            PrimaryID = id;
        }

        public MLFSClient(JToken jsonResponse)
        {
            Plans = new List<Plan>();
            Fees = new List<Fee>();
            
            dynamic obj = jsonResponse;
            if (obj != null)
            {

                PrimaryID = obj.id;
                ClientCode = "";
                Name = obj.name;
                if (obj.currentAdviser != null)
                {
                    string advisorId = obj.currentAdviser.id;
                    string advisorName = obj.currentAdviser.name;
                    ClientOwner = new Staff(advisorId, advisorName);
                    Advisor = ClientOwner;
                }
                IsActive = true;
                if (obj.person != null)
                {
                    IsIndividual = true;
                    Person = new Person();
                    Person.Title = obj.person.title;
                    Person.FirstName = obj.person.firstName;
                    Person.LastName = obj.person.lastName;
                    Person.DateOfBirth = obj.person.dateOFBirth;
                }
                else
                {
                    IsIndividual = false;
                    Organisation = new Organisation();
                    Organisation.Name = obj.corporate.name;
                }
                if (obj.servicingAdministrator != null)
                {
                    string adminName = obj.servicingAdministrator.name;
                    string adminId = obj.servicingAdministrator.id;
                    Administrator = new Staff(adminId, adminName);
                }
                if (obj.paraplanner != null)
                {
                    string paraName = obj.paraplanner.name;
                    string paraId = obj.paraplanner.id;
                    ParaPlanner = new Staff(paraId, paraName);
                }
                Category = obj.category;
                Type = obj.Type;
            }
        }

        public Staff Administrator { get; set; }
        public Staff ParaPlanner { get; set; }
        public Staff Advisor { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }

        public List<Plan> Plans { get; set; }
        public List<Fee> Fees { get; set; }
    }
}
