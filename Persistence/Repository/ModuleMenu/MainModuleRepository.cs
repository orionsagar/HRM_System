using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence.Repository.HR
{
    public interface IMainModuleRepo : IGenericRepository<MainModule>
    {
        Task<IEnumerable<SelectListItemModel>> Dropdown(int CompId);
    }


    public class MainModuleRepository : IMainModuleRepo
    {
        readonly IApplicationDbContext _dbContext;
        private IApplicationReadDbConnection readDbConnection;
        private IApplicationWriteDbConnection writeDbConnection;

        public MainModuleRepository(IApplicationDbContext db, IApplicationWriteDbConnection writeDbConnection, IApplicationReadDbConnection readDbConnection)
        {
            _dbContext = db;
            this.readDbConnection = readDbConnection;
            this.writeDbConnection = writeDbConnection;
        }

        public async Task<int> Add(MainModule entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.MainModules.Add(entity);
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

                _dbContext.MainModules.Remove(exist);
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

        public async Task<IEnumerable<MainModule>> GetAll()
        {
            return await _dbContext.MainModules.ToListAsync();
        }

        public async Task<MainModule> GetById(int id)
        {
            return await _dbContext.MainModules.FirstOrDefaultAsync(a=>a.ModuleID==id);
        }

        public async Task<int> Update(MainModule entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.MainModules.Update(entity);
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
            string sql = $"Select {nameof(MainModule.ModuleID)} Value, {nameof(MainModule.ModuleName)} Text  from {nameof(_dbContext.MainModules)} Where CompId = {CompId}";
            var data = await readDbConnection.QueryAsync<SelectListItemModel>(sql);
            _dbContext.Connection.Close();
            return data;
        }

    }
}
