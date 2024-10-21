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
    public interface IModuleMenuRepo : IGenericRepository<UserAccessTools>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int CompId);
        Task<IEnumerable<UserAccessToolsVM>> SP_Dt_ModuelMenuList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ModuleId, int? SubModuleId, int ComId);
        Task<int> GetMenuSortIndex(int ModuleId, int SubModuleId);
    }
    public class ModuleMenuRepository : IModuleMenuRepo
    {
        readonly IApplicationDbContext _dbContext;
        private IApplicationReadDbConnection readDbConnection;
        private IApplicationWriteDbConnection writeDbConnection;

        public ModuleMenuRepository(IApplicationDbContext db, IApplicationWriteDbConnection writeDbConnection, IApplicationReadDbConnection readDbConnection)
        {
            _dbContext = db;
            this.readDbConnection = readDbConnection;
            this.writeDbConnection = writeDbConnection;
        }
        public async Task<int> Add(UserAccessTools entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.UserAccessTools.Add(entity);
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

                _dbContext.UserAccessTools.Remove(exist);
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

        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int CompId)
        {
            _dbContext.Connection.Open();
            string sql = $"Select {nameof(UserAccessTools.UAToolid)} Value, {nameof(UserAccessTools.UseraccesstoolNM)} Text  from {nameof(_dbContext.UserAccessTools)} Where CompId = {CompId}";
            var data = await readDbConnection.QueryAsync<SelectListItemModel>(sql);
            _dbContext.Connection.Close();
            return data;
        }

        public async Task<IEnumerable<UserAccessTools>> GetAll()
        {
            return await _dbContext.UserAccessTools.ToListAsync();
        }

        public async Task<UserAccessTools> GetById(int id)
        {
            return await _dbContext.UserAccessTools.FirstOrDefaultAsync(a => a.UAToolid == id);
        }

        public async Task<IEnumerable<UserAccessToolsVM>> SP_Dt_ModuelMenuList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ModuleId, int? SubModuleId, int ComId)
        {
            _dbContext.Connection.Open();
            string sql = $"EXEC SP_Dt_ModuelMenuList @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@ModuleId,@SubModuleId,@ComId";
            var data = await readDbConnection.QueryAsync<UserAccessToolsVM>(sql, new { DisplayLength, DisplayStart, SortCol, SortDir, Search, ModuleId, SubModuleId, ComId });
            _dbContext.Connection.Close();
            return data;
        }

        public async Task<int> Update(UserAccessTools entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.UserAccessTools.Update(entity);
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
        public async Task<int> GetMenuSortIndex(int ModuleId, int SubModuleId)
        {
            _dbContext.Connection.Open();
            string sql = $"Select {nameof(UserAccessTools.SortIndex)} from {nameof(_dbContext.UserAccessTools)} Where ModuleID = {ModuleId} and SubModuleID = {SubModuleId} Order By {nameof(UserAccessTools.SortIndex)} desc";
            var data = await readDbConnection.QueryAsync<int>(sql);
            _dbContext.Connection.Close();
            return data.FirstOrDefault();
        }
    }
}
