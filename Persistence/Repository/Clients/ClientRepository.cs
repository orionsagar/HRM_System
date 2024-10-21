using Domains.Models;
using Domains.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using Persistence.Repository.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Clients
{
    public interface IClientRepo : IGenericRepository<Client>
    {
        Task<IEnumerable<ClientViewModel>> SP_Dt_ClientList(int DisplayLength, int Start, int SortCol, string SortDir, string Search, int ClientId, string RoleType);
        Task<List<ClientViewModel>> GetClientInfo(int ClientId);
        Task<IEnumerable<SelectListItemModel>> Dropdown();
        Task<IEnumerable<SelectListItemModel>> Dropdown(int ClientId);
    }
    public class ClientRepository : IClientRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public ClientRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _readDb = readDb;
            _db = db;
        }

        public async Task<int> Add(Client entity)
        {
            _db.Clients.Add(entity);
            int returnId = await _db.SaveChangesAsync();

            return returnId;
        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.Clients.Remove(exist);
                int returnId = await _db.SaveChangesAsync();
                transaction.Commit();
                return returnId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ClientViewModel>> GetClientInfo(int ClientId)
        {
            _db.Connection.Open();
            string sql = $"select * from Clients where ClientID = @ClientID";
            var data = await _readDb.QueryAsync<ClientViewModel>(sql, new { ClientId });
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<IEnumerable<ClientViewModel>> SP_Dt_ClientList(int DisplayLength, int Start, int SortCol, string SortDir, string Search, int ClientId, string RoleType)
        {
            _db.Connection.Open();
            string cmd = $"EXEC SP_Dt_ClientList {DisplayLength},{Start},{SortCol},'{SortDir}','{Search}',{ClientId}";
            string sql = $"EXEC SP_Dt_ClientList @DisplayLength,@Start,@SortCol,@SortDir,@Search,@ClientId,@RoleType";
            var data = await _readDb.QueryAsync<ClientViewModel>(sql, new { DisplayLength, Start, SortCol, SortDir, Search, ClientId, RoleType });
            _db.Connection.Close();
            return data;
        }

        public async Task<Client> GetById(int id)
        {
            return await _db.Clients.AsNoTracking().Where(a => a.ClientID == id).FirstOrDefaultAsync();
        }

  

        public Task<IEnumerable<Client>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(Client entity)
        {
            _db.Clients.Update(entity);
            int returnId = await _db.SaveChangesAsync();

            return returnId;
        }

        public async Task<IEnumerable<SelectListItemModel>> Dropdown()
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(Client.ClientID)} Value, {nameof(Client.Name)} Text  from {nameof(_db.Clients)}";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data;
        }
        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int ClientId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(Client.ClientID)} Value, {nameof(Client.Name)} Text  from {nameof(_db.Clients)} where {nameof(Client.ClientID)} = {ClientId}";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data;
        }

    }
}
