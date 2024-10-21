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

namespace Persistence.Repository.Leave
{
    public interface ILeaveSetupRepo : IGenericRepository<LeaveSetup>
    {
        Task<int> Upsert(List<LeaveSetup> entity);
        Task<List<LeaveSetup>> GetLeaveByEmpId(int EmpId, int fiscalYearId);
        Task<List<EmployeeVM>> EmpLeaveList(EmpLeaveFilter model);
    }
    public class LeaveSetupRepository : ILeaveSetupRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;

        public LeaveSetupRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }
        public async Task<int> Add(LeaveSetup entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeaveSetup.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.LeaveSetupId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<int> Upsert(List<LeaveSetup> entity)
        {
            var empIds = entity.Select(a => a.EmpId).ToList();
            var exist = _db.LeaveSetup.Where(a => empIds.Contains(a.EmpId)).ToList();
            
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                if (exist!=null)
                {
                    _db.LeaveSetup.RemoveRange(exist);
                }

                _db.LeaveSetup.AddRange(entity);
                
                //foreach (var item in entity)
                //{
                //    var ls = exist.FirstOrDefault(a => a.EmpId == item.EmpId && a.LeaveTypeId == item.LeaveTypeId && a.FiscalYearId == item.FiscalYearId);
                //    if (ls == null)
                //    {
                //        await _db.LeaveSetup.AddAsync(item);
                //    }
                //    else
                //    {
                //        ls.LeaveTypeId = item.LeaveTypeId;
                //        ls.LeaveDays = item.LeaveDays;
                //        ls.Remark = item.Remark;
                //        ls.UpdatedBy = item.AddedBy;
                //        ls.UpdatedDate = item.AddedDate;
                //        _db.LeaveSetup.Update(ls);
                //    }
                //}
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.Count;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.LeaveSetup.Remove(exist);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<LeaveSetup>> GetAll()
        {
            return await _db.LeaveSetup.ToListAsync();
        }

        public async Task<LeaveSetup> GetById(int id)
        {
            return await _db.LeaveSetup.Where(a => a.LeaveTypeId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(LeaveSetup entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.LeaveSetup.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.LeaveSetupId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<LeaveSetup>> GetLeaveByEmpId(int EmpId, int fiscalYearId)
        {
            var leave = await _db.LeaveSetup.Where(e => e.EmpId == EmpId && e.FiscalYearId==fiscalYearId).ToListAsync();
            return leave;
        }

        public async Task<List<EmployeeVM>> EmpLeaveList(EmpLeaveFilter model)
        {
            _db.Connection.Open();
            string sqltest = $"exec [SP_EmpLeaveList] {model.EmpCategoryId_Filter}, {model.DesignationId_Filter}, {model.SectionId_Filter}, {model.DepartmentId_Filter}, '{model.EmpName_Filter}', '{model.CardNo_Filter}', {model.CompId}";
            string sql = $"exec [SP_EmpLeaveList] @CatId,@DesigId,@SectId,@DeptId,@Name,@CardNo,@IsNoLeave,@ComId,@FiscalYearId,@OrgId";
            var data = await _readDb.QueryAsync<EmployeeVM>(sql, new
            {
                CatId = model.EmpCategoryId_Filter,
                DesigId = model.DesignationId_Filter,
                SectId = model.SectionId_Filter,
                DeptId = model.DepartmentId_Filter,
                Name = model.EmpName_Filter ?? "",
                CardNo = model.CardNo_Filter ?? "",
                IsNoLeave = model.IsNoLeave,
                ComId = model.CompId,
                FiscalYearId = 0,
                OrgId = model.OrgId
            });

            _db.Connection.Close();
            return data.ToList();
        }
    }
}
