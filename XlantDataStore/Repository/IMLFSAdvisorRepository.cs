using Microsoft.AspNetCore.Mvc.Rendering;
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
        Task<MLFSAdvisor> GetAdvisor(int id);
        Task<List<MLFSAdvisor>> GetAdvisors();
        Task<SelectList> SelectList(int? advisorId = null);
    }
}
