using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XLantCore;
using Microsoft.EntityFrameworkCore;

namespace XLantDataStore.Repository
{
    public class DataMapRepository : IDataMapRepository
    {
        private readonly XLantDbContext _context;
        public DataMapRepository(XLantDbContext context)
        {
            _context = context;
        }

        public void Delete(int mapId)
        {
            DataMap map = _context.DataMaps.Find(mapId);
            if (map != null)
            {
                _context.DataMaps.Remove(map);
                _context.SaveChanges();
            }
        }

        public async Task<DataMap> GetDataMapById(int mapId)
        {
            DataMap map = await _context.DataMaps.FindAsync(mapId);
            return map;
        }

        public async Task<IEnumerable<DataMap>> GetMaps(string csvName)
        {
            List<DataMap> maps = await _context.DataMaps.Where(x => x.SourceName == csvName).ToListAsync();
            return maps;
        }

        public void Insert(DataMap map)
        {
            _context.DataMaps.Add(map);
            _context.SaveChanges();
        }

        public void Update(DataMap map)
        {
            _context.Entry(map).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }

}
