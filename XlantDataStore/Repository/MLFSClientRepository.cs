using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XLantCore;

namespace XLantDataStore.Repository
{
    public class MLFSClientRepository
    {
        public static MLFSClient GetMLFSClient(string id)
        {
            //Build the client
            MLFSClient client = GetBasicClientData(id);
            //then get the plans
            client.Plans = GetClientPlans(client.PrimaryID);
            //for each plan get the other owners but only basic data
            foreach(Plan plan in client.Plans)
            {
                for(int i = 0; i < plan.Clients.Count; i++)
                {
                    if (plan.Clients[i].PrimaryID != client.PrimaryID)
                    {
                        plan.Clients[i] = GetBasicClientData(plan.Clients[i].PrimaryID);
                    }
                }
            }
            //get addresses
            GetAdditionalData(client, "addresses");
            GetAdditionalData(client, "contactdetails");
            GetAdditionalData(client, "relationships");
            client.Fees.AddRange(GetClientFees(client.PrimaryID));
            return client;
        }

        private static MLFSClient GetBasicClientData(string id)
        {
            string url = String.Format("clients/{0}", id);
            IRestResponse response = IOConnection.GetResponse(url);
            JToken token = JToken.Parse(response.Content);
            MLFSClient client = new MLFSClient(token);
            return client;
        }

        private static void GetAdditionalData(MLFSClient client, string endpoint)
        {
            string url = String.Format("clients/{0}/{1}", client.PrimaryID, endpoint);
            IRestResponse response = IOConnection.GetResponse(url);
            JArray _array = Tools.ExtractItemsArrayFromJsonString(response.Content);
            List<Address> addresses = new List<Address>();
            List<Number> numbers = new List<Number>();
            List<EmailAddress> emails = new List<EmailAddress>();
            if (endpoint == "addressess")
            {
                addresses = Address.CreateList(_array);
            }
            if (endpoint == "contactdetails")
            {
                JArray jarray = Tools.SplitContactDetails(_array);
                numbers = Number.CreateList(jarray);
                jarray = Tools.SplitContactDetails(_array, true);
                emails = EmailAddress.CreateList(jarray);
            }
            if (client.IsIndividual)
            {
                client.Person.Addresses = addresses;
                client.Person.EmailAddresses = emails;
                client.Person.Numbers = numbers;
            }
            else
            {
                client.Organisation.Addresses = addresses;
                client.Organisation.Numbers = numbers;
            }
        }

        public static List<Plan> GetClientPlans(string clientId, string url = "")
        {
            if (String.IsNullOrEmpty(url))
            {
                url = String.Format("clients/{0}/plans", clientId);
            }
            IRestResponse response = IOConnection.GetResponse(url);
            JArray jarray = Tools.ExtractItemsArrayFromJsonString(response.Content);
            List<Plan> plans = Plan.CreateList(jarray);
            foreach (Plan p in plans)
            {
                url = String.Format("clients/{0}/plans/{1}/contributions", clientId, p.PrimaryID);
                response = IOConnection.GetResponse(url);
                JArray conts = Tools.ExtractItemsArrayFromJsonString(response.Content);
                p.ContributionsToDate = conts.Sum(x => Decimal.Parse(x["value"]["amount"].ToString()));
            }

            return plans;
        }

        public static List<Fee> GetClientFees(string clientId, string url = "")
        {
            List<Fee> fees = new List<Fee>();
            if (String.IsNullOrEmpty(url))
            {
                url = String.Format("clients/{0}/fees", clientId);
            }
            IRestResponse response = IOConnection.GetResponse(url);
            JArray jarray = Tools.ExtractItemsArrayFromJsonString(response.Content);
            fees = Fee.CreateList(jarray);
            return fees;
        }
    }
}
