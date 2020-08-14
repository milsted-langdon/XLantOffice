using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;


namespace XLantDataStore.Repository
{
    public class MLFSAdvisorRepository : IMLFSAdvisorRepository
    {
        private readonly XLantDbContext _db;
        

        public MLFSAdvisorRepository(XLantDbContext db)
        {
            _db = db;
            
        }

        public async Task<MLFSAdvisor> GetAdvisor(string id)
        {
            MLFSAdvisor adv = await _db.MLFSAdvisors.FindAsync(id);
            return adv;
        }

        public async Task<List<MLFSAdvisor>> GetAdvisors()
        {
            List<MLFSAdvisor> advisors = await _db.MLFSAdvisors.ToListAsync();
            return advisors;
        }
    }
}
