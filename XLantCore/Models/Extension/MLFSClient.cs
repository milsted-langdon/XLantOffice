using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSClient
    {
        /// <summary>
        /// Create a list of client objects from an array of IO data where only the Id is present
        /// </summary>
        /// <param name="array">the array from IO</param>
        /// <returns>the list of clients</returns>
        public static List<MLFSClient> CreateSummaryList(JArray array)
        {
            List<MLFSClient> clients = new List<MLFSClient>();
            foreach(JObject obj in array)
            {
                MLFSClient c = new MLFSClient(obj["id"].ToString());
                clients.Add(c);
            }
            return clients;
        }

        /// <summary>
        /// Create a list of client objects from an array of IO data
        /// </summary>
        /// <param name="array">the array from IO</param>
        /// <returns>the list of clients</returns>
        public static List<MLFSClient> CreateList(JArray array)
        {
            List<MLFSClient> clients = new List<MLFSClient>();
            foreach (JObject obj in array)
            {
                MLFSClient c = new MLFSClient(obj);
                clients.Add(c);
            }
            return clients;
        }
    }
}
