using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace XLantCore
{
    public class APIAccess
    {
        public static string baseURL = "https://localhost:44393/api";
        public class Result
        {
            public Result()
            {

            }
            public string Message { get; set; }
            public bool WasSuccessful { get; set; }
            public object Data { get; set; }
            public string RawData { get; set; }
        }
        public static Result GetDataFromAPI(string url)
        {
            WebClient web = new WebClient();
            Result result = new Result();
            try
            {
                string rawData = web.DownloadString(url);
                JToken token = JToken.Parse(rawData);
                result.Data = token;
                result.WasSuccessful = true;
            }
            catch
            {
                result.WasSuccessful = false;
                result.Message = "Unable to reach server";
            }
            finally
            {
                web.Dispose();
            }
            return result;
        }

        public static Result GetDataFromXLAPI<TEntity>(string url) where TEntity : class
        {
            WebClient web = new WebClient();
            Result result = new Result();
            try
            {
                Uri address = new Uri(baseURL + url);
                string rawData = web.DownloadString(address);
                JToken token = JToken.Parse(rawData);
                result.RawData = rawData;
                result.Data = JsonConvert.DeserializeObject<TEntity>(rawData);
                result.WasSuccessful = true;
            }
            catch
            {
                result.WasSuccessful = false;
                result.Message = "Unable to reach server";
            }
            finally
            {
                web.Dispose();
            }
            return result;
        }

        public static Result PostDataToXLAPI(string url, object itemToPost)
        {
            WebClient web = new WebClient();
            Result result = new Result();
            try
            {
                string content = JsonConvert.SerializeObject(itemToPost);
                Uri address = new Uri(baseURL + url);
                web.UploadStringAsync(address, content);
                result.WasSuccessful = true;
            }
            catch
            {
                result.WasSuccessful = false;
                result.Message = "Unable to reach server";
            }
            finally
            {
                web.Dispose();
            }
            return result;
        }
    }
}
