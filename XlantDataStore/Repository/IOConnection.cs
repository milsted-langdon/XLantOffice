using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace XLantDataStore.Repository
{
    public class IOConnection
    {
        private static string clientId = DotNetEnv.Env.GetString("IOCLIENTID");
        private static string clientSecret = DotNetEnv.Env.GetString("IOCLIENTSECRET");
        private static string tenantId = "10946";//"12551";//
        private static string apiKey = DotNetEnv.Env.GetString("IOAPIKEY");
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

        public static async Task<APIToken> GetToken()
        {
            var client = new RestClient(tokenUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=tenant_client_credentials&scope=client_data client_financial_data&" + credentials, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);
            APIToken token = JsonConvert.DeserializeObject<APIToken>(response.Content);
            return token;
        }

        private static async Task<RestRequest> BuildRestRequest(Method method)
        {
            if (String.IsNullOrEmpty(currentToken.AccessToken))
            {
                currentToken = await GetToken();
            }
            var request = new RestRequest(method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", String.Format("Bearer {0}", currentToken.AccessToken));
            request.AddHeader("x-api-key", apiKey);
            return request;
        }

        public static async Task<IRestResponse> GetResponse(string url)
        {
            url = apiBaseUrl + url;
            
            var contRestClient = new RestClient(url);
            var request = await IOConnection.BuildRestRequest(Method.GET);
            IRestResponse response = await contRestClient.ExecuteAsync(request);
            return response;
        }
    }
}
