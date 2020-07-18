using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IMLFSClientRepository
    {
        Task<MLFSClient> GetClient(string id);
        Task<List<Plan>> GetClientPlans(string clientId, string url=null);
        Task<List<Fee>> GetClientFees(string clientId, string url=null);

    }
}
