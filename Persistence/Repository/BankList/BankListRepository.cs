using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.BankList
{
    public interface IBankListRepo : IGenericRepository<BankLists>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId, int ClientId);
        Task<List<BankLists>> GetAllByOrgId(int OrgId);
    }
    public class BankListRepository : IBankListRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public BankListRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }

        public async Task<int> Add(BankLists entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.BankLists.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.BankId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.BankLists.Remove(exist);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId, int ClientId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(BankLists.BankId)} Value, {nameof(BankLists.BankName)} Text  from {nameof(_db.LeaveType)} where {nameof(BankLists.OrgId)}=@OrgId";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql, new { OrgId});
            _db.Connection.Close();
            return data;
        }

        public async Task<IEnumerable<BankLists>> GetAll()
        {
            try
            {
                var data = await _db.BankLists.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<BankLists>> GetAllByOrgId(int OrgId)
        {
            try
            {
                var data = await _db.BankLists.Where(b=>b.OrgId == OrgId).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<BankLists> GetById(int id)
        {
            return await _db.BankLists.Where(a => a.BankId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(BankLists entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.BankLists.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.BankId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
