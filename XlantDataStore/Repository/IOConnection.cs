using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XLantDataStore.Repository
{
    public class IOConnection
    {
        private static string clientId = "app-87d0541-tcf-9ec5dadde6e84f6fb4daea56cca578e6";
        private static string clientSecret = "WZ@R7GN3K$-_HX3469z$s6s8?!_0QS";
        private static string tenantId = "12551";
        private static string apiKey = "app-87d0541-6c93b0849c29424291ce4ff10580935a";
        private static string credentials = String.Format("tenant_id={0}&client_id={1}&client_secret={2}", tenantId, clientId, clientSecret);
        private static string tokenUrl = "https://identity.intelliflo.com/core/connect/token";
        private static string apiBaseUrl = "https://api.intelliflo.com/v2/";
        public static APIToken currentToken = new APIToken();
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

        public static IRestResponse GetResponse(string url)
        {
            url = apiBaseUrl + url;
            var contRestClient = new RestClient(url);
            var request = IOConnection.BuildRestRequest(Method.GET);
            IRestResponse response = contRestClient.Execute(request);
            return response;
        }
    }
}
