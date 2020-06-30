using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XLant
{
    public class XLAPI
    {
        public class Result
        {
            public Result()
            {

            }
            public string Message { get; set; }
            public bool WasSuccessful { get; set; }
            public JToken Data { get; set; }
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
    }
}
