using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Leave
{
    public interface ILeavePayMethodRepo : IGenericRepository<LeavePayMethod>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int compId, int OrgId);
        Task<IEnumerable<LeavePayMethod>> GetAll(int OrgId);
    }
    public class LeavePayMethodRepository : ILeavePayMethodRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public LeavePayMethodRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }
        public async Task<int> Add(LeavePayMethod entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeavePayMethod.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.LeavePayMethodId;
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

                _db.LeavePayMethod.Remove(exist);
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

        public async Task<IEnumerable<LeavePayMethod>> GetAll()
        {
            return await _db.LeavePayMethod.ToListAsync();
        }

        public async Task<LeavePayMethod> GetById(int id)
        {
            return await _db.LeavePayMethod.Where(a => a.LeavePayMethodId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(LeavePayMethod entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeavePayMethod.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.LeavePayMethodId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int compId, int OrgId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(LeavePayMethod.LeavePayMethodId)} Value, {nameof(LeavePayMethod.PayMethodName)} Text  from {nameof(_db.LeavePayMethod)} where {nameof(LeavePayMethod.OrgId)}=@OrgId";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql, new { OrgId });
            _db.Connection.Close();
            return data;
        }

        public async Task<IEnumerable<LeavePayMethod>> GetAll(int OrgId)
        {
            return await _db.LeavePayMethod.Where(l => l.OrgId == OrgId).ToListAsync();
        }
    }
}
