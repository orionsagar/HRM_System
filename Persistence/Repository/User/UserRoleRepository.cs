using Domains.Models;
using Domains.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.HR
{
    public interface IUserRoleRepo : IGenericRepository<UserRole>
    {
        Task<int> AddUserWiseModule(int id, List<UserRolewiseModule> userRolewiseModule);
        Task<IEnumerable<UserRolewiseModule>> GetUserWiseModule(int RoleId);
        Task<IEnumerable<UserAccessList>> GetRolewiseAccess(string UAPage, int RoleID);
        Task<IEnumerable<SelectListItemModel>> Dropdown(int compId, string RoleType);
        Task<IEnumerable<UserRolewiseSubModule>> GetUserWiseSubModule(int RoleId);
        Task<int> AddUserWiseSubModule(int id, List<UserRolewiseSubModule> userRolewiseModule);
        Task<int> AddReportAccess(int roleid, List<ReportAccess> reportAccesses);
        Task<int> GetRoleIDBy_U_RoleName(string UniqueRoleName);
        
    }


    public class UserRoleRepository : IUserRoleRepo
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDbConnection;
        private IApplicationWriteDbConnection writeDbConnection;

        public UserRoleRepository(IApplicationDbContext db, IApplicationWriteDbConnection writeDbConnection, IApplicationReadDbConnection readDbConnection)
        {
            _db = db;
            this.writeDbConnection = writeDbConnection;
            _readDbConnection = readDbConnection;
        }

        public async Task<int> Add(UserRole entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.UserRoles.Add(entity);
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

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _db.UserRoles.Remove(exist);
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

        public async Task<IEnumerable<UserRole>> GetAll()
        {
            return await _db.UserRoles.ToListAsync();
        }

        public async Task<UserRole> GetById(int id)
        {
            return await _db.UserRoles.Where(a => a.RoleID == id).FirstOrDefaultAsync();
        }

        public async Task<int> Update(UserRole entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.UserRoles.Update(entity);
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

        //public async Task<int> Add(UserRole entity)
        //{
        //    //entity.DateCreated = DateTime.Now;
        //    var sql = "INSERT INTO UserRole ([RoleName],[CompID],[LandingPage]) " +
        //        " Values (@RoleName,@CompID,@LandingPage);";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql, entity);
        //        return affectedRows;
        //    }
        //}



        //public async Task<int> Delete(int id)
        //{
        //    var sql = "DELETE FROM UserRole WHERE RoleID = @RoleID;";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql, new { RoleID = id });
        //        return affectedRows;
        //    }
        //}

        //public async Task<UserRole> GetById(int id)
        //{
        //    var sql = "SELECT * FROM UserRole WHERE RoleID = @RoleID;";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<UserRole>(sql, new { RoleID = id });
        //        return result.FirstOrDefault();
        //    }
        //}

        //public async Task<IEnumerable<UserRole>> GetAll()
        //{
        //    var sql = "SELECT * FROM UserRole;";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<UserRole>(sql);
        //        return result;
        //    }
        //}

        //public async Task<int> Update(UserRole entity)
        //{
        //    // entity.DateModified = DateTime.Now;
        //    var sql = "UPDATE UserRole SET [RoleName]=@RoleName,[CompID]=@CompID,[LandingPage]=@LandingPage WHERE RoleID = @RoleID;";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = await connection.ExecuteAsync(sql, entity);
        //        return affectedRows;
        //    }
        //}



        //#region User Role Wise Module

        public async Task<int> AddUserWiseModule(int id, List<UserRolewiseModule> userRolewiseModule)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var roleModules = await _db.UserRolewiseModules.Where(a => a.RoleID == id).ToListAsync();
                if (roleModules.Any())
                {
                    _db.UserRolewiseModules.RemoveRange(roleModules);
                }
                await _db.UserRolewiseModules.AddRangeAsync(userRolewiseModule);
                var affectedRows = await _db.SaveChangesAsync();
                transaction.Commit();
                return affectedRows;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> AddUserWiseSubModule(int id, List<UserRolewiseSubModule> userRolewiseModule)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var roleModules = await _db.UserRolewiseSubModules.Where(a => a.RoleID == id).ToListAsync();
                if (roleModules.Any())
                {
                    _db.UserRolewiseSubModules.RemoveRange(roleModules);
                }
                await _db.UserRolewiseSubModules.AddRangeAsync(userRolewiseModule);
                var affectedRows = await _db.SaveChangesAsync();
                transaction.Commit();
                return affectedRows;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddReportAccess(int roleid, List<ReportAccess> reportAccesses)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                var rptAccess = await _db.ReportAccess.Where(a => a.RoleId == roleid && reportAccesses.FirstOrDefault().MenuId == a.MenuId && reportAccesses.FirstOrDefault().ReportTypeName == a.ReportTypeName).ToListAsync();
                if (rptAccess.Any())
                {
                    _db.ReportAccess.RemoveRange(rptAccess);
                }
                await _db.ReportAccess.AddRangeAsync(reportAccesses);
                var affectedRows = await _db.SaveChangesAsync();
                transaction.Commit();
                return affectedRows;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }



        public async Task<IEnumerable<UserRolewiseModule>> GetUserWiseModule(int RoleId)
        {
            //var sql = "Select urm.*, m.ModuleName, m.Description from UserRolewiseModule urm inner join MainModule m on m.ModuleID = urm.ModuleID Where urm.RoleID = @RoleID;";
            //using (var connection = new SqlConnection(AppConfig.DefaultConnection))
            //{
            //    connection.Open();
            //    var result = await connection.QueryAsync<UserRolewiseModule>(sql, new { RoleID = id });
            //    return result;
            //}

            return await _db.UserRolewiseModules.Include(a => a.MainModule).Where(a => a.RoleID == RoleId).ToListAsync();
                
        }
        public async Task<IEnumerable<UserRolewiseSubModule>> GetUserWiseSubModule(int RoleId)
        {
            return await _db.UserRolewiseSubModules.Include(a => a.MainModule).Include(b=>b.SubModule).Where(a => a.RoleID == RoleId).ToListAsync();                
        }


        public async Task<IEnumerable<UserAccessList>> GetRolewiseAccess(string UAPage, int RoleID)
        {
            //var sql = "SELECT UAVeiw, UASave, UAEdit, UAdelete, UApproved FROM  dbo.UserAccessList UAList INNER JOIN UserAccesstools UATools ON UAList.uatoolid=UATools.uatoolid  WHERE (RoleID = @RoleID) AND (UATools.UAPage = @UAPage)";
            //using (var connection = new SqlConnection(AppConfig.DefaultConnection))
            //{
            //    connection.Open();
            //    var result = await connection.QueryAsync<UserAccessList>(sql, new { RoleID = RoleID, UAPage = UAPage });
            //    return result;
            //}
            return await _db.UserAccessList.Where(a=>a.RoleID==RoleID).ToListAsync();

        }

        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int compId,string RoleType)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(UserRole.RoleID)} Value, {nameof(UserRole.RoleName)} Text  from {nameof(_db.UserRoles)} where {nameof(UserRole.CompId)}=@compId and  ({nameof(UserRole.RoleType)} = @RoleType  OR @RoleType = 'Platform_Role')";
            var data = await _readDbConnection.QueryAsync<SelectListItemModel>(sql, new { compId, RoleType });
            _db.Connection.Close();
            return data;
        }
        public async Task<int> GetRoleIDBy_U_RoleName(string UniqueRoleName)
        {
            var sql = $@"select RoleID from UserRoles where U_RoleName like @UniqueRoleName";
            var roleid = await _readDbConnection.QueryFirstOrDefaultAsync<int>(sql, new { UniqueRoleName });
            return roleid;
        }

        

        //#endregion      
    }
}
