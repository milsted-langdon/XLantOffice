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
        private static string clientId = "app-87d0541-tcf-9ec5dadde6e84f6fb4daea56cca578e6";
        private static string clientSecret = "WZ@R7GN3K$-_HX3469z$s6s8?!_0QS";
        private static string tenantId = "12551";
        private static string apiKey = "app-87d0541-6c93b0849c29424291ce4ff10580935a";
        private static string credentials = String.Format("tenant_id={0}&client_id={1}&client_secret={2}", tenantId, clientId, clientSecret);
        private static string tokenUrl = "https://identity.intelliflo.com/core/connect/token";
        private static string apiBaseUrl = "https://api.intelliflo.com/v2/";
        private static APIToken currentToken = new APIToken();
        public class APIToken
        {
            public APIToken()
            {
            }

            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("token_type")]
            public string TokenType { get; set; }
            [JsonProperty("expires_in")]
            public long ExpiresIn { get; set; }
            [JsonProperty("scope")]
            public string Scope { get; set; }
        }

        public static APIToken GetToken()
        {
            var client = new RestClient(tokenUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=tenant_client_credentials&scope=client_data client_financial_data&" + credentials, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            APIToken token = JsonConvert.DeserializeObject<APIToken>(response.Content);
            return token;
        }

        private static RestRequest BuildRestRequest(Method method)
        {
            if (String.IsNullOrEmpty(currentToken.AccessToken))
            {
                currentToken = GetToken();
            }
            var request = new RestRequest(method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", String.Format("Bearer {0}", currentToken.AccessToken));
            request.AddHeader("x-api-key", apiKey);
            return request;
        }


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
            return client;
        }

        private static MLFSClient GetBasicClientData(string id)
        {
            string url = String.Format("{0}clients/{1}", apiBaseUrl, id);
            var restClient = new RestClient(url);
            var request = BuildRestRequest(Method.GET);
            IRestResponse response = restClient.Execute(request);
            MLFSClient client = new MLFSClient(response.Content);
            return client;
        }

        private static void GetAdditionalData(MLFSClient client, string endpoint)
        {
            string url = String.Format("{0}clients/{1}/{2}", apiBaseUrl, client.PrimaryID, endpoint);
            var restClient = new RestClient(url);
            var request = BuildRestRequest(Method.GET);
            IRestResponse response = restClient.Execute(request);
            JArray _array = Tools.ExtractItemsArrayFromJsonString(response.Content);
            List<Address> addresses = new List<Address>();
            List<Number> numbers = new List<Number>();
            List<EmailAddress> emails = new List<EmailAddress>();
            if (endpoint == "addressess")
            {
                addresses = Address.CreateList(JsonConvert.SerializeObject(_array));
            }
            if (endpoint == "contactdetails")
            {
                string stringArray = SplitContactDetails(_array);
                numbers = Number.CreateList(stringArray);
                stringArray = SplitContactDetails(_array, true);
                emails = EmailAddress.CreateList(stringArray);
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

        /// <summary>
        /// Takes the contact details and seperates numbers from emails
        /// </summary>
        /// <param name="_array">The array from the json response from IO</param>
        /// <param name="emails">whether to return emails or numbers default is false thereby providing numbers</param>
        /// <returns>Searlialised JArray</returns>
        public static string SplitContactDetails(JArray _array, bool emails = false)
        {
            JArray filteredArray = new JArray();
            string searilisedArray = "";
            if (emails)
            {
                //how many do we have
                int elementCount = _array.AsEnumerable().Where(x => x["type"].ToString().Contains("Email")).Count();
                if (elementCount == 1)
                {
                    filteredArray.Add(_array.AsEnumerable().Where(x => x["type"].ToString().Contains("Email")));
                }
                else if (elementCount > 1)
                {
                    filteredArray = _array.AsEnumerable().Where(x => x["type"].ToString().Contains("Email")) as JArray;
                }
            }
            else
            {
                //how many do we have
                int elementCount = _array.AsEnumerable().Where(x => !x["type"].ToString().Contains("Email")).Count();
                if (elementCount == 1)
                {
                    filteredArray.Add(_array.AsEnumerable().Where(x => !x["type"].ToString().Contains("Email")));
                }
                else if (elementCount > 1)
                {
                    filteredArray = _array.AsEnumerable().Where(x => !x["type"].ToString().Contains("Email")) as JArray;
                }
            }
            
            if (filteredArray.Count != 0)
            {
                searilisedArray = JsonConvert.SerializeObject(filteredArray);
            }
            else
            {
                searilisedArray = null;
            }
            return searilisedArray;
        }

        public static List<Plan> GetClientPlans(string clientId, string url = "")
        {
            if (String.IsNullOrEmpty(url))
            {
                url = String.Format("{0}clients/{1}/plans", apiBaseUrl, clientId);
            }
            var restClient = new RestClient(url);
            var request = BuildRestRequest(Method.GET);
            IRestResponse response = restClient.Execute(request);
            List<Plan> plans = Plan.CreateList(response.Content);

            return plans;
        }

        public static List<Fee> GetClientFees(string clientId, string url = "")
        {
            List<Fee> fees = new List<Fee>();

            return fees;
        }
    }
}
