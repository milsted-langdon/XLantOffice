using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IDataMapRepository
    {
        Task<IEnumerable<DataMap>> GetMaps(string csvName);
        Task<DataMap> GetDataMapById(int mapId);
        void Update(DataMap map);
        void Delete(int mapId);
        void Insert(DataMap map);

    }
}
