using AutoMapper;
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
    public interface IShortLeaveAssignRepo
    {
        Task<int> Upsert(List<ShortLeaveAssign> entity);
        Task<int> Delete(int ShortLeaveAssignId);
        //Task<int> DeleteByEmpIds(List<int> empIds);
        Task<ShortLeaveAssign> GetById(int ShortLeaveAssignId);

        Task<bool> CheckDuplicateSameDate(ShortLeaveAssign entity);
        Task<List<ShortLeaveAssignEmpListVM>> ShortLeaveReport(EmpLeaveFilter model);
        Task<List<ShortLeaveAssignEmpListVM>> EmpList(ShortLeaveAssignFilter model);
        Task<List<ShortLeaveAssignEmpListVM>> ShortLeaveAssignList(ShortLeaveAssignFilter model);
    }
    public class ShortLeaveAssignRepository : IShortLeaveAssignRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;
        private IMapper _mapper;

        public ShortLeaveAssignRepository(IApplicationReadDbConnection readDb, IApplicationDbContext db, IMapper mapper)
        {
            _readDb = readDb;
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> Add(ShortLeaveAssign entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.ShortLeaveAssign.Add(entity);
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.ShortLeaveAssignId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.InnerException.Message);
            }
        }


        public async Task<int> Upsert(List<ShortLeaveAssign> entity)
        {
            // In advance table has a trigger for update due amount after insert & delete operation

            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                foreach (var item in entity)
                {
                    if (item.ShortLeaveAssignId < 1)
                    {
                        await _db.ShortLeaveAssign.AddAsync(item);
                    }
                    else
                    {
                        var pre = await _db.ShortLeaveAssign.AsNoTracking().FirstOrDefaultAsync(a => a.ShortLeaveAssignId == item.ShortLeaveAssignId);
                        if (pre != null)
                        {
                            item.AddedDate = pre.AddedDate;
                            item.AddedBy = pre.AddedBy;
                        }
                        _db.ShortLeaveAssign.Update(item);
                    }
                }
                await _db.SaveChangesAsync();

                transaction.Commit();
                return entity.Count;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }




        //public async Task<int> Delete(int ShortLeaveAssignId)
        //{
        //    using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        //    try
        //    {
        //        var exist =await _db.Advance.Where(a => a.ShortLeaveAssignId==ShortLeaveAssignId).FirstOrDefaultAsync();

        //        if (exist == null) return 0;

        //        _db.Advance.Remove(exist);
        //        await _db.SaveChangesAsync();
        //        transaction.Commit();
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        throw new Exception(ex.Message);
        //    }
        //}



        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.ShortLeaveAssign.Remove(exist);
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

        public async Task<IEnumerable<ShortLeaveAssign>> GetAll()
        {
            return await _db.ShortLeaveAssign.ToListAsync();
        }

        public async Task<ShortLeaveAssign> GetById(int id)
        {
            return await _db.ShortLeaveAssign.AsNoTracking().Where(a => a.ShortLeaveAssignId == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(ShortLeaveAssign entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.ShortLeaveAssign.Update(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.ShortLeaveAssignId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckDuplicateSameDate(ShortLeaveAssign entity)
        {
            try
            {
                return await _db.ShortLeaveAssign
                        .AnyAsync(a => a.ShortLeaveAssignId != entity.ShortLeaveAssignId
                                        && a.EffectiveDate.Date == entity.EffectiveDate
                                        && a.EmpId == entity.EmpId 
                                        && a.CompId == entity.CompId);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public async Task<List<ShortLeaveAssignEmpListVM>> EmpList(ShortLeaveAssignFilter model)
        {
            _db.Connection.Open();
            //string sqltest = $"exec [SP_EmpShortLeaveAssignList] {model.EmpCategoryId_Filter}, {model.DesignationId_Filter}, {model.SectionId_Filter}, {model.DepartmentId_Filter}, '{model.EmpName_Filter}', '{model.CardNo_Filter}', {model.CompId}";
            string sql = $"exec [SP_EmpListForShortLeaveAssign] @CatId,@DesigId,@SectId,@DeptId,@CardNo,@ComId";
            var data = await _readDb.QueryAsync<ShortLeaveAssignEmpListVM>(sql, new
            {
                CatId = model.EmpCategoryId_Filter,
                DesigId = model.DesignationId_Filter,
                SectId = model.SectionId_Filter,
                DeptId = model.DepartmentId_Filter,
                CardNo = model.CardNo_Filter ?? "all",
                ComId = model.CompId,
            });

            _db.Connection.Close();
            return data.ToList();
        }

        public async Task<List<ShortLeaveAssignEmpListVM>> ShortLeaveReport(EmpLeaveFilter model)
        {
            _db.Connection.Open();
            //string sqltest = $"exec [SP_EmpShortLeaveAssignList] {model.EmpCategoryId_Filter}, {model.DesignationId_Filter}, {model.SectionId_Filter}, {model.DepartmentId_Filter}, '{model.EmpName_Filter}', '{model.CardNo_Filter}', {model.CompId}";
            string sql = $"exec [spRPTShortLeave] @CatId,@DesigId,@SectId,@DeptId,@CardNo,@Month,@Year,@ComId";
            var data = await _readDb.QueryAsync<ShortLeaveAssignEmpListVM>(sql, new
            {
                CatId = model.EmpCategoryId_Filter,
                DesigId = model.DesignationId_Filter,
                SectId = model.SectionId_Filter,
                DeptId = model.DepartmentId_Filter,
                CardNo = model.CardNo_Filter ?? "all",
                Month = model.Month,
                Year = model.Year,
                ComId = model.CompId,
            });

            _db.Connection.Close();
            return data.ToList();
        }



        public async Task<List<ShortLeaveAssignEmpListVM>> ShortLeaveAssignList(ShortLeaveAssignFilter model)
        {
            _db.Connection.Open();
            //string sqltest = $"exec [sp_ShortLeaveAssignSearch] '{model.FindCardNo_Filter?? "all"}', '{model.FromDate_Filter}', '{model.ToDate_Filter}', {model.CompId}";
            string sql = $"exec [sp_ShortLeaveAssignSearch] @CardNo, @EmpCategoryID, @SectionID, @DepartmentID, @DesignationID,@FloorID,@LineID,@SDate,@EDate,@CompanyID";
            var data = await _readDb.QueryAsync<ShortLeaveAssignEmpListVM>(sql, new
            {
                CardNo = model.CardNo_Filter ?? "all",
                EmpCategoryID = model.EmpCategoryId_Filter,
                SectionID = model.SectionId_Filter,
                DepartmentID = model.DepartmentId_Filter,
                DesignationID = model.DesignationId_Filter,
                FloorID = model.FloorId_Filter,
                LineID = model.LineId_Filter,
                SDate = model.FromDate_Filter,
                EDate = model.ToDate_Filter,
                CompanyID = model.CompId,
            });

            _db.Connection.Close();
            return data.ToList();
        }



    }
}
