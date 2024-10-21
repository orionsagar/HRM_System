using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Tax
{
    public interface ITaxMasterRepo : IGenericRepository<TaxMaster>
    {
        Task<IEnumerable<TaxMaster>> GetAll(int OrgId);
    }
    public class TaxMasterRepository : ITaxMasterRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public TaxMasterRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _readDb = readDb;
            _db = db;
        }
        public async Task<int> Add(TaxMaster entity)
        {
            _db.TaxMasters.Add(entity);
            int returnId = await _db.SaveChangesAsync();
            return returnId;
        }

        public async Task<int> Delete(int id)
        {
            var exist = await GetById(id);
            if (exist == null) return 0;
            _db.TaxMasters.Remove(exist);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaxMaster>> GetAll()
        {
            var taxlist = await _db.TaxMasters.ToListAsync();
            return taxlist.ToList();
        }

        public async Task<TaxMaster> GetById(int id)
        {
            return await _db.TaxMasters.Where(p => p.TaxId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(TaxMaster entity)
        {
            _db.TaxMasters.Update(entity);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaxMaster>> GetAll(int OrgId)
        {
            return await _db.TaxMasters.Where(lt => lt.OrgId == OrgId).ToListAsync();
        }
    }
}
