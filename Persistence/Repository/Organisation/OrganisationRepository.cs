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

namespace Persistence.Repository.Organisation
{
    public interface IOrganisation : IGenericRepository<HrmOrganisation>
    {
        Task<IEnumerable<OrganisationVM>> SP_Dt_OrganisationList(int DisplayLength, int Start, int SortCol, string SortDir, string Search,int ClientId, int OrgId, string RoleType);
        Task<List<OrganisationVM>> GetOrganisationInfo(int OrgId);
        Task<IEnumerable<SelectListItemModel>> Dropdown();
        Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId);
        Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId = 0, int ClientId = 0);
        Task<CardVM> vw_Count_Org_And_Client();
    }
    public class OrganisationRepository : IOrganisation
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public OrganisationRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _readDb = readDb;
            _db = db;
        }

        public async Task<int> Add(HrmOrganisation entity)
        {
            _db.Organisations.Add(entity);
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

                _db.Organisations.Remove(exist);
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

        public async Task<IEnumerable<SelectListItemModel>> Dropdown()
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(HrmOrganisation.OrgId)} Value, {nameof(HrmOrganisation.OrgName)} Text  from {nameof(_db.Organisations)}";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data;
        }
        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(HrmOrganisation.OrgId)} Value, {nameof(HrmOrganisation.OrgName)} Text  from {nameof(_db.Organisations)} where {nameof(HrmOrganisation.OrgId)} = {OrgId}";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data;
        }
        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int OrgId = 0,int ClientId = 0)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(HrmOrganisation.OrgId)} Value, {nameof(HrmOrganisation.OrgName)} Text  from {nameof(_db.Organisations)} where ({nameof(HrmOrganisation.OrgId)} = {OrgId} OR {OrgId} = 0) and ClientId = {ClientId}";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data;
        }

        public Task<IEnumerable<HrmOrganisation>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<HrmOrganisation> GetById(int id)
        {
            _db.Connection.Open();
            string sql = @$"select o.*, [dbo].[GetPackageNameById](o.PackageId)PackageName, [dbo].[NumberOfEmployee](o.OrgId) NumberOfEmp, c.[Name] ClientName, ct.[Name] CountryName
from Organisations o left join clients c on c.ClientID = o.ClientID left join Designations d on d.Desigid = o.AP_DesignationID left join countrys ct on ct.countryid = o.countryid where o.OrgId = @OrgId";
            var data = await _readDb.QueryFirstOrDefaultAsync<HrmOrganisation>(sql, new { OrgId = id });
            _db.Connection.Close();
            return data;
        }

        public async Task<List<OrganisationVM>> GetOrganisationInfo(int OrgId)
        {
            _db.Connection.Open();
            string sql = $"select * from Organisations where OrgId = @OrgId";
            var data = await _readDb.QueryAsync<OrganisationVM>(sql, new { OrgId });
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<IEnumerable<OrganisationVM>> SP_Dt_OrganisationList(int DisplayLength, int Start, int SortCol, string SortDir, string Search, int ClientId, int OrgId, string RoleType)
        {
            _db.Connection.Open();
            string cmd = $"EXEC SP_Dt_OrganisationList {DisplayLength},{Start},{SortCol},'{SortDir}','{Search}',{ClientId}";
            string sql = $"EXEC SP_Dt_OrganisationList @DisplayLength,@Start,@SortCol,@SortDir,@Search,@ClientId,@OrgId, @RoleType";
            var data = await _readDb.QueryAsync<OrganisationVM>(sql, new { DisplayLength, Start, SortCol, SortDir, Search, ClientId, OrgId, RoleType });
            _db.Connection.Close();
            return data;
        }

        public async Task<int> Update(HrmOrganisation entity)
        {
            _db.Organisations.Update(entity);
            int returnId = await _db.SaveChangesAsync();
            return returnId;
        }

        public Task<CardVM> vw_Count_Org_And_Client()
        {
            throw new NotImplementedException();
        }
    }
}
