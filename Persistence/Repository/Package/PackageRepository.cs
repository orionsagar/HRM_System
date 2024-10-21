using Domains.Models;
using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Package
{
    public interface IPackage : IGenericRepository<Packages>
    {
        Task<IEnumerable<OrganisationVM>> SP_Dt_PackageList(int DisplayLength, int Start, int SortCol, string SortDir, string Search);
        Task<List<SelectListItemModel>> Dropdown();
    }
    public class PackageRepository : IPackage
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public PackageRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _db = db;
            _readDb = readDb;
        }

        public Task<int> Add(Packages entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SelectListItemModel>> Dropdown()
        {
            _db.Connection.Open();
            string sql = $"Select PackageId Value,PackageName Text from Packages";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<IEnumerable<Packages>> GetAll()
        {
            _db.Connection.Open();
            string sql = $"Select * from Packages";
            var data = await _readDb.QueryAsync<Packages>(sql);
            _db.Connection.Close();
            return data.ToList();
        }

        public Task<Packages> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrganisationVM>> SP_Dt_PackageList(int DisplayLength, int Start, int SortCol, string SortDir, string Search)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Packages entity)
        {
            throw new NotImplementedException();
        }
    }
}
