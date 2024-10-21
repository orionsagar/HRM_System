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
    public interface IShortLeaveSetup
    {
        Task<int> Upsert(List<ShortLeaveSetup> entity);
        Task<ShortLeaveSetup> GetById(int ShortLeaveSetupId);
        Task<List<ShortLeaveSetup>> GetByEmpId(int EmpId);
        Task<List<ShortLeaveSetup>> GetByEmpIds(List<int> empIds);
        Task<int> DeleteByEmpIds(List<int> empIds);
        Task<int> Delete(int id);
    }
    public class ShortLeaveSetupRepository : IShortLeaveSetup
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public ShortLeaveSetupRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }

        public async Task<int> Upsert(List<ShortLeaveSetup> entity)
        {
            var empIds = entity.Select(a => a.EmpId).ToList();
            var exist = _db.ShortLeaveSetup.Where(a => empIds.Contains(a.EmpId)).ToList();

            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                if (exist.Any())
                {
                    _db.ShortLeaveSetup.RemoveRange(exist);
                }

                _db.ShortLeaveSetup.AddRange(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.Count;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<ShortLeaveSetup> GetById(int ShortLeaveSetupId)
        {
            return await _db.ShortLeaveSetup.Where(l => l.ShortLeaveSetupId == ShortLeaveSetupId).FirstOrDefaultAsync();
        }

        public async Task<List<ShortLeaveSetup>> GetByEmpId(int EmpId)
        {
            var result = await _db.ShortLeaveSetup.Where(l => l.EmpId == EmpId).ToListAsync();
            return result;
        }

        public async Task<List<ShortLeaveSetup>> GetByEmpIds(List<int> empIds)
        {
            var result = await _db.ShortLeaveSetup.Where(l => empIds.Contains(l.EmpId)).ToListAsync();
            return result;
        }

        public async Task<int> DeleteByEmpIds(List<int> empIds)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = _db.ShortLeaveSetup.Where(a => empIds.Contains(a.EmpId)).ToList();

                if (exist == null) return 0;

                _db.ShortLeaveSetup.RemoveRange(exist);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return empIds.Count;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.ShortLeaveSetup.Remove(exist);
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
    }
}
