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

namespace Persistence.Repository
{
    public interface ITasksRepo : IGenericRepository<Tasks>
    {
        Task<IEnumerable<TasksVM>> SP_Dt_TaskList(DataTableParamVM param);
    }
    public class TasksRepository : ITasksRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public TasksRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _readDb = readDb;
            _db = db;
        }
        public async Task<int> Add(Tasks entity)
        {
            _db.Tasks.Add(entity);
            int returnId = await _db.SaveChangesAsync();
            return returnId;
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.Tasks.Remove(exist);
                await _db.SaveChangesAsync();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<IEnumerable<Tasks>> GetAll()
        {
            _db.Connection.Open();
            string sql = $@"select * from Tasks";
            var data = await _readDb.QueryAsync<Tasks>(sql);
            _db.Connection.Close();
            return data;
        }

        public async Task<Tasks> GetById(int id)
        {
            _db.Connection.Open();
            string sql = $@"select * from Tasks where JobId = @JobId";
            var data = await _readDb.QueryFirstOrDefaultAsync<Tasks>(sql, new { JobId = id });
            _db.Connection.Close();
            return data;
        }

        public async Task<IEnumerable<TasksVM>> SP_Dt_TaskList(DataTableParamVM param)
        {
            _db.Connection.Open();
            string sql = $@"EXEC SP_Dt_TaskList @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@OrgId,@ClientId";
            var data = await _readDb.QueryAsync<TasksVM>(sql, new { param.DisplayLength, param.DisplayStart, param.SortDir, param.SortCol, param.Search, param.OrgId, param.ClientId });
            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<int> Update(Tasks entity)
        {
            try
            {
                _db.Tasks.Update(entity);
                await _db.SaveChangesAsync();
                return entity.TaskId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
        public async Task<List<SelectListItemModel>> Dropdown()
        {
            _db.Connection.Open();
            string sql = $"Select TaskId Value,TaskName Text from Tasks";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql);
            _db.Connection.Close();
            return data.ToList();
        }
    }
}
