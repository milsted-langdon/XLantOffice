using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IVATIssueFeeRepository
    {
        Task<List<string>> GetIssues();
        void Add(string feeReference);
    }
}
