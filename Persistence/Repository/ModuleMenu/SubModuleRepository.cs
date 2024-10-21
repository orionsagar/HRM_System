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

namespace Persistence.Repository.ModuleMenu
{
    public interface ISubModuleRepo : IGenericRepository<SubModule>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int CompId);
        Task<IEnumerable<SubModuleVM>> SP_Dt_SubModuelList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ModuleId, int ComId);
        Task<IEnumerable<SubModuleVM>> GetSubModuleByModuleId(int ModuleId);
        Task<int> GetSubModuleByUaToolId(int UAToolId);
    }
    public class SubModuleRepository : ISubModuleRepo
    {
        readonly IApplicationDbContext _dbContext;
        private IApplicationReadDbConnection readDbConnection;
        private IApplicationWriteDbConnection writeDbConnection;

        public SubModuleRepository(IApplicationDbContext db, IApplicationWriteDbConnection writeDbConnection, IApplicationReadDbConnection readDbConnection)
        {
            _dbContext = db;
            this.readDbConnection = readDbConnection;
            this.writeDbConnection = writeDbConnection;
        }
        public async Task<int> Add(SubModule entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.SubModule.Add(entity);
                int returnId = await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return returnId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _dbContext.SubModule.Remove(exist);
                int returnId = await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return returnId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SubModule>> GetAll()
        {
            var smodule = await _dbContext.SubModule.Include(m=>m.MainModule).ToListAsync();
            return smodule;
        }

        public async Task<SubModule> GetById(int id)
        {
            return await _dbContext.SubModule.FirstOrDefaultAsync(a => a.SubModuleID == id);
        }

        public async Task<int> Update(SubModule entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.SubModule.Update(entity);
                int returnId = await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return returnId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SubModuleVM>> SP_Dt_SubModuelList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ModuleId, int ComId)
        {
            _dbContext.Connection.Open();
            string sql = $"EXEC SP_Dt_SubModuelList @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@ModuleId,@ComId";
            var data = await readDbConnection.QueryAsync<SubModuleVM>(sql,new { DisplayLength, DisplayStart, SortCol, SortDir, Search, ModuleId, ComId });
            _dbContext.Connection.Close();
            return data;
        }

        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int CompId)
        {
            _dbContext.Connection.Open();
            string sql = $"Select {nameof(SubModule.SubModuleID)} Value, {nameof(SubModule.SubModuleName)} Text  from {nameof(_dbContext.SubModule)} Where CompId = {CompId}";
            var data = await readDbConnection.QueryAsync<SelectListItemModel>(sql);
            _dbContext.Connection.Close();
            return data;
        }
        public async Task<IEnumerable<SubModuleVM>> GetSubModuleByModuleId(int ModuleId)
        {
            _dbContext.Connection.Open();
            string sql = $"Select {nameof(SubModule.SubModuleID)}, {nameof(SubModule.SubModuleName)}  from {nameof(_dbContext.SubModule)} Where {nameof(SubModule.ModuleID)} = {ModuleId}";
            var data = await readDbConnection.QueryAsync<SubModuleVM>(sql);
            _dbContext.Connection.Close();
            return data;
        }
        public async Task<int> GetSubModuleByUaToolId(int UAToolId)
        {
            _dbContext.Connection.Open();
            string sql = $"Select {nameof(UserAccessTools.SubModuleID)} from {nameof(_dbContext.UserAccessTools)} Where {nameof(UserAccessTools.UAToolid)} = {UAToolId}";
            var data = await readDbConnection.QueryAsync<int>(sql);
            _dbContext.Connection.Close();
            return data.FirstOrDefault();
        }

    }
}
