using Domains.Models;
using Domains.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Leave
{
    public interface ILeaveTransactionRepo : IGenericRepository<LeaveTransaction>
    {
        Task<List<LeaveDetailsVM>> SP_LeaveDetails(int EmpId, int FiscalYearId, int LeaveTypeId, int OrgId);
        Task<List<LeaveDetailsVM>> SP_LeaveBalance(int FiscalYearId, int SectId, int DesigId, int DeptId, int EmpId);

        Task<List<LeaveTransaction>> GetLeaveTransactions(int LeaveTransectionId, int EmpId, int LeaveTypeId, int CompId);
        Task<List<LeaveTransactionVM>> SP_Dt_LeaveTransactionList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, string dtFrom, string dtTo,int OrgId);
        Task<bool> HasLeaveTransaction(DateTime date, int empId);
        Task<bool> UpdateLeaveApprovalStatus(int LeaveTransectionId, string Status);
    }
    public class LeaveTransactionRepository : ILeaveTransactionRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;
        private IApplicationWriteDbConnection _writeDb;

        public LeaveTransactionRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db, IApplicationWriteDbConnection writeDb)
        {
            _readDb = readDb;
            _db = db;
            _writeDb = writeDb;
        }
        public async Task<int> Add(LeaveTransaction entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeaveTransaction.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.LeaveTransactionId;
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

                _db.LeaveTransaction.Remove(exist);
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

        public async Task<IEnumerable<LeaveTransaction>> GetAll()
        {
            var data = await _db.LeaveTransaction.ToListAsync();
            return data;
        }

        public async Task<LeaveTransaction> GetById(int id)
        {
            return await _db.LeaveTransaction.Where(a => a.LeaveTransactionId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(LeaveTransaction entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeaveTransaction.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.LeaveTransactionId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<LeaveDetailsVM>> SP_LeaveDetails(int EmpId, int FiscalYearId, int LeaveTypeId, int OrgId)
        {
            _db.Connection.Open();
            string sql = $"EXEC SP_LeaveDetails @EmpId,@FiscalYearId,@LeaveTypeId,@OrgId";
            var data = await _readDb.QueryAsync<LeaveDetailsVM>(sql, new { EmpId, FiscalYearId, LeaveTypeId, OrgId });
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<List<LeaveTransaction>> GetLeaveTransactions(int LeaveTransactionId, int EmpId, int LeaveTypeId, int CompId)
        {
            _db.Connection.Open();
            string sql = $"select * from LeaveTransaction where EmpId = @EmpId and CompId = @CompId";
            if(LeaveTransactionId > 0)
            {
                sql = $"select * from LeaveTransaction where LeaveTransactionId = @LeaveTransactionId";
            }
            var data = await _readDb.QueryAsync<LeaveTransaction>(sql, new { LeaveTransactionId, EmpId, LeaveTypeId, CompId});
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<List<LeaveTransactionVM>> SP_Dt_LeaveTransactionList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, string dtFrom, string dtTo, int OrgId)
        {
            _db.Connection.Open();
            string sql = $"EXEC SP_Dt_LeaveTransactionList @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@ComId,@dtFrom,@dtTo,@OrgId";
            var data = await _readDb.QueryAsync<LeaveTransactionVM>(sql, new { DisplayLength, DisplayStart, SortCol, SortDir, Search, ComId, dtFrom, dtTo,OrgId });
            _db.Connection.Close();
            return data.ToList();
        }
        

        public async Task<bool> HasLeaveTransaction(DateTime date,int empId)
        {
            _db.Connection.Open();
            string sql = $"SELECT LeaveTransactionId FROM  LeaveTransaction WHERE     (EmpId = @empId) AND (@date BETWEEN StartDate AND EndDate)";
            var data = await _readDb.QueryAsync<int>(sql, new { date, empId });
            _db.Connection.Close();
            return data.Count > 0;
        }

        public async Task<List<LeaveDetailsVM>> SP_LeaveBalance(int FiscalYearId, int SectId, int DesigId, int DeptId, int EmpId)
        {
            _db.Connection.Open();
            string query = $@"EXEC SP_LeaveBalance {FiscalYearId},{SectId},{DesigId},{DeptId},{EmpId}";
            string sql = $"EXEC SP_LeaveBalance @FiscalYearId,@SectId,@DesigId,@DeptId,@EmpId";
            var data = await _readDb.QueryAsync<LeaveDetailsVM>(sql, new { FiscalYearId, SectId, DesigId, DeptId, EmpId });
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<bool> UpdateLeaveApprovalStatus(int LeaveTransectionId, string Status)
        {
            try
            {
                _db.Connection.Open();
                var approvedDate = DateTime.Now;
                string sql = $"Update LeaveTransaction set GrantDate = @GrantDate, Status = @Status where LeaveTransactionId = @LeaveTransactionId";
                var data = await _writeDb.ExecuteAsync(sql, new { GrantDate = approvedDate, LeaveTransactionId = LeaveTransectionId, Status, });
                _db.Connection.Close();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

    }
}
