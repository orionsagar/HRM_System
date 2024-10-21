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
    public interface ILeaveAllocation : IGenericRepository<LeaveAllocation>
    {
        Task<List<LeaveAllocationVM>> DtList(DataTableParamVM paramVM);
    }
    public class LeaveAllocationRepository : ILeaveAllocation
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationReadDbConnection _readdb;

        public LeaveAllocationRepository(IApplicationDbContext context, IApplicationReadDbConnection readdb)
        {
            _context = context;
            _readdb = readdb;
        }

        public async Task<int> Add(LeaveAllocation entity)
        {
            _context.LeaveAllocations.Add(entity);
            return await _context.SaveChangesAsync();
        }


        public async Task<int> Delete(int id)
        {
            var exist = await GetById(id);
            if (exist == null) return 0;
            _context.LeaveAllocations.Remove(exist);
            return await _context.SaveChangesAsync();
        }

            public async Task<List<LeaveAllocationVM>> DtList(DataTableParamVM paramVM)
        {
            _context.Connection.Open();
            var sql = $@"select la.*,t.Name EmpType,e.Name EmpName from LeaveAllocations la
                        left join EmployeeTypes t on la.EmpTypeId = t.EmpTypeId
                        left join Employees e on e.EmpId = la.EmpId
                        where la.OrgId = @OrgId";
            var data = await _readdb.QueryAsync<LeaveAllocationVM>(sql, new { OrgId = paramVM.OrgId });
            _context.Connection.Close();
            return data.ToList();
        }

        public async Task<IEnumerable<LeaveAllocation>> GetAll()
        {
            var data = await _context.LeaveAllocations.ToListAsync();
            return data;
        }

        public async Task<LeaveAllocation> GetById(int id)
        {
            return await _context.LeaveAllocations.Where(p => p.LeaveAllocationId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(LeaveAllocation entity)
        {
            _context.LeaveAllocations.Update(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
