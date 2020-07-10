using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSClient
    {
        public static List<MLFSClient> CreateList(JArray array)
        {
            List<MLFSClient> clients = new List<MLFSClient>();
            foreach(JObject obj in array)
            {
                MLFSClient c = new MLFSClient(obj["id"].ToString());
                clients.Add(c);
            }
            return clients;
        }
    }
}
