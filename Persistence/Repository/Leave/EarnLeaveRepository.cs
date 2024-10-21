using Domains.Models;
using Domains.ViewModels;
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
    public interface IEarnLeaveRepo : IGenericRepository<EarnLeave>
    {
        Task<List<EarnLeaveVM>> SP_EarnLeave(string CardNo, int DesigId, int SectId,int DeptId, int FloorId, int LineId, int CompId, int FiscalYearId);
        Task<EarnLeave> GetLeaveByYearIdAndEmpId(int FiscalYearId,int EmpId);
        Task<int> UpdateLeaveTransection(int FiscalYearId, int EmpId, int TotalDays, int LeaveTransactionId);
        Task<int> SP_ClosingFiscalYear(EarnLeaveVM el);
        Task<int> SP_UpdateEarnLeave(int FiscalYearId, int CompId);
    }
    public class EarnLeaveRepository : IEarnLeaveRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;
        private IApplicationWriteDbConnection _writeDb;

        public EarnLeaveRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db, IApplicationWriteDbConnection writeDb)
        {
            _readDb = readDb;
            _db = db;
            _writeDb = writeDb;
        }
        public async Task<int> Add(EarnLeave entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.EarnLeave.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.EarnLeaveId;
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

                _db.EarnLeave.Remove(exist);
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

        public async Task<IEnumerable<EarnLeave>> GetAll()
        {
            return await _db.EarnLeave.ToListAsync();
        }

        public async Task<EarnLeave> GetById(int id)
        {
            return await _db.EarnLeave.Where(a => a.EarnLeaveId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(EarnLeave entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.EarnLeave.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.EarnLeaveId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<EarnLeaveVM>> SP_EarnLeave(string CardNo,int DesigId,int SectId,int DeptId, int FloorId,int LineId,int CompId,int FiscalYearId)
        {
            _db.Connection.Open();
            string sqltest = $"exec [SP_EarnLeave] '{CardNo}', {DesigId}, {SectId},{DeptId}, {FloorId}, '{LineId}', '{CompId}', {FiscalYearId}";
            string sql = $"exec [SP_EarnLeave] @CardNo,@DesigId,@SectId,@DeptId,@FloorId,@LineId,@CompId,@FiscalYearId";
            var data = await _readDb.QueryAsync<EarnLeaveVM>(sql, new
            {
                CardNo = CardNo,
                DesigId = DesigId,
                SectId = SectId,
                DeptId = DeptId,
                FloorId = FloorId,
                LineId = LineId,
                CompId = CompId,
                FiscalYearId = FiscalYearId
            });
            _db.Connection.Close();
            return data.ToList();
        }
        public async Task<int> SP_ClosingFiscalYear(EarnLeaveVM el)
        {
            _db.Connection.Open();
            string sqltest = $"EXEC [SP_ClosingFiscalYear] '{el.FiscalYearId}', {el.OldFiscalYearId}, {el.UserId},'{el.AddDate}','{el.UpdateDate}', {el.CompId}";
            string sql = $"exec [SP_ClosingFiscalYear] @NewFiscalYearId,@OldFiscalYearId,@UserId,@AddDate,@UpdateDate,@CompId";
            var data = await _writeDb.ExecuteAsync(sql, new
            {
                NewFiscalYearId = el.FiscalYearId,
                OldFiscalYearId = el.OldFiscalYearId,
                UserId = el.UserId,
                AddDate = el.AddDate,
                UpdateDate = el.UpdateDate, 
                CompId = el.CompId 
            });
            _db.Connection.Close();
            return data;
        }
        public async Task<int> SP_UpdateEarnLeave(int FiscalYearId, int CompId)
        {
            _db.Connection.Open();
            string sqltest = $"EXEC [SP_UpdateEarnLeave] {CompId},{FiscalYearId}";
            string sql = $"exec [SP_UpdateEarnLeave] @CompId,@FiscalYearId";
            var data = await _writeDb.ExecuteAsync(sql, new
            {
                CompId = CompId,
                FiscalYearId = FiscalYearId,
            });
            _db.Connection.Close();
            return data;
        }

        public async Task<int> UpdateLeaveTransection(int FiscalYearId, int EmpId, int TotalDays, int LeaveTransactionId)
        {
            _db.Connection.Open();
            string sql = "";
            if (LeaveTransactionId == 0)
            {
                sql = $"UPDATE EarnLeave SET LeaveTransaction = isnull(LeaveTransaction,0) + @TotalDays WHERE (FiscalYearId = @FiscalYearId) AND (EmpId = @EmpId)";
            }
            else
            {
                sql = $"UPDATE EarnLeave SET LeaveTransaction = isnull(LeaveTransaction,0) + @TotalDays - (SELECT TotalDays FROM LeaveTransaction WHERE (LeaveTransactionId = @LeaveTransactionId)) WHERE (FiscalYearId = @FiscalYearId) AND (EmpId = @EmpId)";
            }
             
            var data = await _writeDb.ExecuteAsync(sql, new
            {
                TotalDays = TotalDays,
                EmpId = EmpId,
                FiscalYearId = FiscalYearId,
                LeaveTransactionId = LeaveTransactionId
            });
            _db.Connection.Close();
            return data;
        }

        public async Task<EarnLeave> GetLeaveByYearIdAndEmpId(int FiscalYearId, int EmpId)
        {
            return await _db.EarnLeave.Where(a => a.FiscalYearId == FiscalYearId && a.EmpId == EmpId).FirstOrDefaultAsync();
        }
    }
}
