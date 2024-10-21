using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IFiscalYearRepo : IGenericRepository<FiscalYear>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int compId);
        Task<IEnumerable<FiscalYear>> GetFiscalYears(int FiscalYearId = 0, int CompId = 0);
        int GetFiscalYearsByDate(DateTime Date, int CompId = 0);
    }
    public class FiscalYearRepository : IFiscalYearRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public FiscalYearRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }
        public async Task<int> Add(FiscalYear entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.FiscalYear.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.FiscalYearId;
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

                _db.FiscalYear.Remove(exist);
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

        public async Task<IEnumerable<FiscalYear>> GetAll()
        {
            return await _db.FiscalYear.ToListAsync();
        }

        public async Task<FiscalYear> GetById(int id)
        {
            return await _db.FiscalYear.Where(a => a.FiscalYearId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(FiscalYear entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.FiscalYear.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.FiscalYearId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int compId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(FiscalYear.FiscalYearId)} Value, {nameof(FiscalYear.Title)} Text  from {nameof(_db.FiscalYear)} where {nameof(FiscalYear.CompId)}=@compId Order By {nameof(FiscalYear.FiscalYearId)} desc";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql, new { compId });
            _db.Connection.Close();
            return data;
        }

        public async Task<IEnumerable<FiscalYear>> GetFiscalYears(int FiscalYearId = 0,int CompId = 0)
        {
            var fyear = _db.FiscalYear.Where(f=> (f.FiscalYearId == FiscalYearId || f.FiscalYearId == 0) && (f.CompId == CompId || f.CompId == 0)).OrderByDescending(o=>o.FiscalYearId);
            return await fyear.ToListAsync();
        }

        public int GetFiscalYearsByDate(DateTime Date, int CompId = 0)
        {
            var year = _db.FiscalYear.Where(y => y.startdate <= Date && y.enddate >= Date && y.CompId == CompId).Select(y=>y.FiscalYearId).FirstOrDefault();
            return year;
        }
    }
}
