using Domains.Models;
using Domains.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repository.UserLog
{
    public interface ILogRepo
    {
        Task TransectionLog(EditDeleteInfo log);
        Task<List<EditDeleteInfoVM>> GetLogList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, EditDeleteInfoFilterVM filterVM);

        Task<IEnumerable<SelectListItemModel>> CommandTypeDropdown(int compId);

        Task<int> AddProcessLog(ProcessLog entity);
        Task<List<ProcessLogVM>> GetProcessLogList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, ProcessLogFilterVM filterVM);
    }

    public class LogRepo : ILogRepo
    {
        private IApplicationDbContext _db;
        private IApplicationWriteDbConnection _writeDb;
        private IApplicationReadDbConnection _readDb;


        public LogRepo(IApplicationDbContext db, IApplicationWriteDbConnection writeDb, IApplicationReadDbConnection readDb)
        {
            _db = db;
            _writeDb = writeDb;
            _readDb = readDb;
        }

        public async Task TransectionLog(EditDeleteInfo log)
        {
            try
            {
                await _db.EditDeleteInfo.AddAsync(log);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<EditDeleteInfoVM>> GetLogList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, EditDeleteInfoFilterVM filterVM)
        {
            _db.Connection.Open();
            string sql = $"EXEC [SP_EditDeleteInfoList] @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@ComId,@UserId,@FromDate,@ToDate,@Controller,@Action,@CommandType";
            var data = await _readDb.QueryAsync<EditDeleteInfoVM>(sql, new
            {
                DisplayLength,
                DisplayStart,
                SortCol,
                SortDir,
                Search,
                ComId,
                filterVM.UserId,
                filterVM.FromDate,
                filterVM.ToDate,
                filterVM.Controller,
                filterVM.Action,
                filterVM.CommandType,
            });

            _db.Connection.Close();
            return data.ToList();
        }


        public async Task<List<ProcessLogVM>> GetProcessLogList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, ProcessLogFilterVM filterVM)
        {
            _db.Connection.Open();
            string sql = $"EXEC [SP_ProcessLogList] @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@ComId,@UserId,@FromDate,@ToDate,@ProcessType";
            var data = await _readDb.QueryAsync<ProcessLogVM>(sql, new
            {
                DisplayLength,
                DisplayStart,
                SortCol,
                SortDir,
                Search,
                ComId,
                filterVM.UserId,
                filterVM.FromDate,
                filterVM.ToDate,
                filterVM.ProcessType,
            });

            _db.Connection.Close();
            return data.ToList();
        }




        public async Task<IEnumerable<SelectListItemModel>> CommandTypeDropdown(int compId)
        {
            _db.Connection.Open();
            string sql = $"Select distinct({nameof(EditDeleteInfo.CommandType)})  Value  from {nameof(_db.EditDeleteInfo)} Where CompId=@compId";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql, new { compId });
            _db.Connection.Close();
            return data;
        }


        public async Task<int> AddProcessLog(ProcessLog entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.ProcessLog.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.LogId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException.Message);
            }
        }
    }
}
