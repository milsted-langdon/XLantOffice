using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IMLFSAdvisorRepository
    {
        Task<MLFSAdvisor> GetAdvisor(string id);
        Task<List<MLFSAdvisor>> GetAdvisors();
    }
}
