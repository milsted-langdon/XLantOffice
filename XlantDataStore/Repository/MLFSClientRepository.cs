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
    public class MLFSClientRepository : IMLFSClientRepository
    {

        public MLFSClientRepository()
        {

        }


        public async Task<MLFSClient> GetClient(string id)
        {
            //Build the client
            MLFSClient client = await GetBasicClientData(id);
            //then get the plans
            client.Plans = await GetClientPlans(client.PrimaryID);
            //for each plan get the other owners but only basic data
            foreach(Plan plan in client.Plans)
            {
                plan.ContributionsToDate = await GetContributionTotal(plan);
                for (int i = 0; i < plan.Clients.Count; i++)
                {
                    if (plan.Clients[i].PrimaryID != client.PrimaryID)
                    {
                        plan.Clients[i] = await GetBasicClientData(plan.Clients[i].PrimaryID);
                    }
                }
            }
            //get addresses
            GetAdditionalData(client, "addresses");
            GetAdditionalData(client, "contactdetails");
            GetAdditionalData(client, "relationships");
            client.Fees.AddRange(await GetClientFees(client.PrimaryID));
            return client;
        }

        private async Task<decimal> GetContributionTotal(Plan plan)
        {
            string url = String.Format("clients/{0}/plans/{1}/contributions", plan.Clients[0].PrimaryID, plan.PrimaryID);
            IRestResponse response = await IOConnection.GetResponse(url);
            if (response.Content.Length != 0)
            {
                JArray array = Tools.ExtractItemsArrayFromJsonString(response.Content);
                decimal total = 0;
                foreach (JObject obj in array)
                {
                    total += decimal.Parse(obj["value"]["Amount"].ToString());
                }
                return total; 
            }
            else
            {
                return 0;
            }
        }

        private static async Task<MLFSClient> GetBasicClientData(string id)
        {
            string url = String.Format("clients/{0}", id);
            IRestResponse response = await IOConnection.GetResponse(url);
            if (response.Content.Length != 0)
            {
                JToken token = JToken.Parse(response.Content);
                MLFSClient client = new MLFSClient(token);
                return client; 
            }
            else
            {
                return null;
            }
        }

        private static async void GetAdditionalData(MLFSClient client, string endpoint)
        {
            string url = String.Format("clients/{0}/{1}", client.PrimaryID, endpoint);
            IRestResponse response = await IOConnection.GetResponse(url);
            if (response.Content.Length != 0)
            {
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
        }

        public async Task<List<Plan>> GetClientPlans(string clientId, string url = "")
        {
            if (String.IsNullOrEmpty(url))
            {
                url = String.Format("clients/{0}/plans", clientId);
            }
            IRestResponse response = await IOConnection.GetResponse(url);
            if (response.Content.Length != 0 && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JArray jarray = Tools.ExtractItemsArrayFromJsonString(response.Content);
                List<Plan> plans = Plan.CreateList(jarray);
                foreach (Plan p in plans)
                {
                    url = String.Format("clients/{0}/plans/{1}/contributions", clientId, p.PrimaryID);
                    response = await IOConnection.GetResponse(url);
                    JArray conts = Tools.ExtractItemsArrayFromJsonString(response.Content);
                    p.ContributionsToDate = conts.Sum(x => Decimal.Parse(x["value"]["amount"].ToString()));
                }
                return plans;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Fee>> GetClientFees(string clientId, string url = "")
        {
            List<Fee> fees = new List<Fee>();
            if (String.IsNullOrEmpty(url))
            {
                url = String.Format("clients/{0}/fees", clientId);
            }
            IRestResponse response = await IOConnection.GetResponse(url);
            if (response.Content.Length != 0 && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JArray jarray = Tools.ExtractItemsArrayFromJsonString(response.Content);
                fees = Fee.CreateList(jarray);
                return fees; 
            }
            else
            {
                return null;
            }
        }
    }
}
