using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore
{
    public class Tools
    {
        /// <summary>
        /// Takes a response string and strips out the items array
        /// </summary>
        /// <param name="response">the full response or Serialized JObject</param>
        /// <param name="arrayContainer">the name of the array within the response, default is "items"</param>
        /// <returns>JArray</returns>
        public static JArray ExtractItemsArrayFromJsonString(string content, string arrayContainer = "items")
        {
            JObject obj = JObject.Parse(content);
            JArray _array = (JArray)obj[arrayContainer];
            return _array;


        }
    }
}
