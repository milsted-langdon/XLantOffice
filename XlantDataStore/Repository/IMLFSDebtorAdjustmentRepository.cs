using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IMLFSDebtorAdjustmentRepository
    {
        Task<List<MLFSDebtorAdjustment>> GetAdjustments(int debtorId);
        Task<MLFSDebtorAdjustment> GetAdjustmentById(int adjustmentId);
        void Update(MLFSDebtorAdjustment adjustment);
        void Delete(int adjustmentId);
        void Insert(MLFSDebtorAdjustment adjustment);
        void InsertList(List<MLFSDebtorAdjustment> adjs);
        Task<List<MLFSDebtorAdjustment>> GetAdjustments(MLFSReportingPeriod period);
    }
}
