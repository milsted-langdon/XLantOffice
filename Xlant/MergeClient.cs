using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLant
{
    public class MergeClient : XLMain.Client
    {
        public MergeClient()
        {

        }

        public MergeClient(XLMain.Client client)
        {
            Manager = client.manager.name;
            ManagerRole = XLMain.Staff.GetJobTitle(client.manager);
            ManagerEmail = client.manager.emails.Where(x => x.primary).FirstOrDefault().email;
            Partner = client.partner.name;
            PartnerRole = XLMain.Staff.GetJobTitle(client.partner);
            PartnerEmail = client.partner.emails.Where(x => x.primary).FirstOrDefault().email;
            ManagerImage = client.manager.GetImageLocation();
            PartnerImage = client.partner.GetImageLocation();
            name = client.name;
            clientcode = client.clientcode;
            crmID = client.crmID;
            relationship = client.relationship;
            type = client.type;
            status = client.status;
            isLive = client.isLive;
            wip = client.wip;
            debtor = client.debtor;
            department = client.department;
            office = client.office;
            IsIndividual = client.IsIndividual;
            if (client.salutations.Where(x => x.primary).Count() > 0)
            {
                Addressee = client.salutations.Where(x => x.primary).FirstOrDefault().addressee;
                Salutation = client.salutations.Where(x => x.primary).FirstOrDefault().salutation;
            }
            if (client.salutations.Count > 0)
            {
                Addressee = client.salutations.FirstOrDefault().addressee;
                Salutation = client.salutations.FirstOrDefault().salutation;
            }
            else
            {
                Addressee = "";
                Salutation = "Sirs";
            }
            if (client.addresses.Where(x => x.primary).Count() > 0)
            {
                Address = client.addresses.Where(x => x.primary).FirstOrDefault().addressBlock;
            }
            else if (client.addresses.Count > 0)
            {
                Address = client.addresses.FirstOrDefault().addressBlock;
            }
            else
            {
                Address = "";
            }            
            if (Addressee != name)
            {
                AddresseeBlock = Addressee + Environment.NewLine + name;
            }
            else
            {
                AddresseeBlock = name;
            }
        }

        public string Manager { get; set; }
        public string Partner { get; set; }
        public string ManagerEmail { get; set; }
        public string PartnerEmail { get; set; }
        public string ManagerRole { get; set; }
        public string PartnerRole { get; set; }
        public string ManagerImage { get; set; }
        public string PartnerImage { get; set; }
        public string AddresseeBlock { get; set; }
        public string Address { get; set; }
        public string Addressee { get; set; }
        public string Salutation { get; set; }
    }
}
