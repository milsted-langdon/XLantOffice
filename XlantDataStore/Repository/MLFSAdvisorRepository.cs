using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<MLFSAdvisor> GetAdvisor(int id)
        {
            MLFSAdvisor adv = await _db.MLFSAdvisors.FindAsync(id);
            return adv;
        }

        public async Task<List<MLFSAdvisor>> GetAdvisors()
        {
            List<MLFSAdvisor> advisors = await _db.MLFSAdvisors.ToListAsync();
            return advisors;
        }

        public async Task<SelectList> SelectList(int? advisorId = null)
        {
            List<MLFSAdvisor> advisors = await GetAdvisors();
            SelectList sList = new SelectList(advisors.OrderBy(x => x.LastName).ThenByDescending(y => y.FirstName), "Id", "Fullname", advisorId);
            return sList;
        }
    }
}
