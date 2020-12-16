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
    public class VATIssueFeeRepository : IVATIssueFeeRepository
    {
        private readonly XLantDbContext _db;
        

        public VATIssueFeeRepository(XLantDbContext db)
        {
            _db = db;
            
        }

        public void Add(string feeReference)
        {
            VATIssueFee fee = new VATIssueFee()
            {
                IOReference = feeReference
            };
            _db.VATIssueFees.Add(fee);
        }

        public async Task<List<string>> GetIssues()
        {
            List<string> fees = await _db.VATIssueFees.Select(x => x.IOReference).ToListAsync();
            return fees;

        }
    }
}
