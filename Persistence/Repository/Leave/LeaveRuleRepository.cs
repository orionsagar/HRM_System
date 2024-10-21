using Domains.Models;
using Domains.ViewModels;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Leave
{
    public interface ILeaveRuleRepo : IGenericRepository<LeaveRule>
    {
        Task<List<LeaveRuleVM>> Dt_List(DataTableParamVM Param);
        Task<List<LeaveRuleVM>> GetAll(int OrgId);

    }
    public class LeaveRuleRepository : ILeaveRuleRepo
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationReadDbConnection _readdb;

        public LeaveRuleRepository(IApplicationDbContext context, IApplicationReadDbConnection readdb)
        {
            _context = context;
            _readdb = readdb;
        }
        public async Task<int> Add(LeaveRule entity)
        {
            _context.LeaveRules.Add(entity);
            return await _context.SaveChangesAsync();
        }


        public async Task<int> Delete(int id)
        {
            var exist = await GetById(id);
            if (exist == null) return 0;
            _context.LeaveRules.Remove(exist);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveRuleVM>> Dt_List(DataTableParamVM Param)
        {
            try
            {
                var sql = $@"select r.*,t.Name EmpType,lt.LTypeName LeaveType from LeaveRules r 
                    left join EmployeeTypes t on r.EmpTypeId = t.EmpTypeId
                    left join LeaveType lt on lt.LeaveTypeId = r.LeaveTypeId";
                _context.Connection.Open();
               var data = await _readdb.QueryAsync<LeaveRuleVM>(sql, new { OrgId = Param.OrgId });
                _context.Connection.Close();
                return data.ToList();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public async Task<List<LeaveRuleVM>> GetAll(int OrgId)
        {
            _context.Connection.Open();
            string sql = @$"select lr.*,lt.LTypeName LeaveType,et.Name EmpType from LeaveRules lr 
                        left join LeaveType lt on lt.LeaveTypeId = lr.LeaveTypeId
                        left join EmployeeTypes et on et.EmpTypeId = lr.EmpTypeId
                        where lr.OrgId = @OrgId";
            var data = await _readdb.QueryAsync<LeaveRuleVM>(sql, new {OrgId });
            _context.Connection.Close();
            return data.ToList();
        }

        public Task<IEnumerable<LeaveRule>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveRule> GetById(int id)
        {
            return await _context.LeaveRules.Where(p => p.LeaveRuleId== id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(LeaveRule entity)
        {
            _context.LeaveRules.Update(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
