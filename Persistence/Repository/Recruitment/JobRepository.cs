using Domains.Models;
using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domains.Models.Recruitment;
using static Domains.ViewModels.RecruitmentVM;

namespace Persistence.Repository.Recruitment
{
    public interface IJobRepo : IGenericRepository<Job>
    {
        Task<List<JobVM>> SP_Dt_JobList(DataTableParamVM param);
        Task<List<SelectListItemModel>> Dropdown();
    }
    public class JobRepository : IJobRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public JobRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _db = db;
            _readDb = readDb;
        }

        public async Task<int> Add(Job entity)
        {
            try
            {
                _db.Jobs.Add(entity);
                await _db.SaveChangesAsync();
                return entity.JobId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.Jobs.Remove(exist);
                await _db.SaveChangesAsync();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<IEnumerable<Job>> GetAll()
        {
            _db.Connection.Open();
            string sql = $@"select * from Jobs";
            var data = await _readDb.QueryAsync<Job>(sql);
            _db.Connection.Close();
            return data;
        }

        public async Task<Job> GetById(int id)
        {
            _db.Connection.Open();
            string sql = $@"select * from Jobs where JobId = @JobId";
            var data = await _readDb.QueryFirstOrDefaultAsync<Job>(sql, new {JobId = id});
            _db.Connection.Close();
            return data;
        }

        public async Task<List<JobVM>> SP_Dt_JobList(DataTableParamVM param)
        {
            _db.Connection.Open();
            string sql = $@"EXEC SP_Dt_JobList @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@CompId,@OrgId,@ClientId";
            var data = await _readDb.QueryAsync<JobVM>(sql, new {param.DisplayLength,param.DisplayStart,param.SortDir,param.SortCol,param.Search, param.CompId, param.OrgId, param.ClientId});
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<int> Update(Job entity)
        {
            try
            {
                _db.Jobs.Update(entity);
                await _db.SaveChangesAsync();
                return entity.JobId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<List<SelectListItemModel>> Dropdown()
        {
            _db.Connection.Open();
            string sql = $"Select JobId Value,SOCCode Text from Jobs";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data.ToList();
        }
    }
}
