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

namespace Persistence.Repository.HR
{
    public interface IUserInfoRepo : IGenericRepository<UserInfo>
    {
        Task<bool> CheckUsernameExists(string username, string password);
        Task<UserInfo> GetUserByUsername(string username, string password);
        Task<IEnumerable<SelectListItemModel>> Dropdown(int compId, int OrgId);
        Task<int> AddWithoutTransaction(UserInfo entity);
        Task<int> UpdateWithoutTrans(UserInfo entity);
        Task<string> GetUserStatus(int RoleId, int OrgId);
        Task<UserInfo> GetByOrgId_And_RoleId(int OrgId, int RoleId);
        Task<UserInfo> GetUserDataByEmail(string Email);
        Task<UserInfo> GetUserBy_OrgId_RoleId_ClientId(int OrgId, int RoleId = 0, int ClientId = 0);
        Task<List<UserInfoViewModel>> SP_Dt_UserList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, int OrgId, int ClientId, string RoleType);
        Task<string> CheckEmail(string Email);
    }

    public class UserInfoRepository : IUserInfoRepo
    {
        IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;
        public UserInfoRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _db = db;
            _readDb = readDb;
        }
        public async Task<List<UserInfoViewModel>> SP_Dt_UserList(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId, int OrgId, int ClientId, string RoleType)
        {
            _db.Connection.Open();
            string sql = $"EXEC SP_Dt_UserList @DisplayLength,@DisplayStart,@SortCol,@SortDir,@Search,@ComId,@OrgId,@ClientId, @RoleType";
            var data = await _readDb.QueryAsync<UserInfoViewModel>(sql, new { DisplayLength, DisplayStart, SortCol, SortDir, Search, ComId, OrgId, ClientId, RoleType });
            _db.Connection.Close();
            return data.ToList();
        }
        public async Task<int> Add(UserInfo entity)
        {
            using IDbContextTransaction transaction = _db.Database.BeginTransaction();
            try
            {
                _db.UserInfos.Add(entity);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return entity.UserID;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddWithoutTransaction(UserInfo entity)
        {
            try
            {
                _db.UserInfos.Add(entity);
                await _db.SaveChangesAsync();
                return entity.UserID;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CheckUsernameExists(string username, string password)
        {
           return await _db.UserInfos.AnyAsync(a => a.UserName == username && a.Password == password);
        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transection = _db.Database.BeginTransaction();
            try
            {
                var exits = await GetById(id);
                if (exits == null) return 0;
                _db.UserInfos.Remove(exits);
                await _db.SaveChangesAsync();
                transection.Commit();
                return exits.UserID;
            }
            catch (Exception ex)
            {
                transection.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UserInfo>> GetAll()
        {
            return await _db.UserInfos.Include(u => u.UserRole).ToListAsync();
        }

        public async Task<UserInfo> GetById(int id)
        {
            return await _db.UserInfos.Include(u=>u.UserRole).Where(u => u.UserID == id).FirstOrDefaultAsync();
        }
        public async Task<UserInfo> GetByOrgId_And_RoleId(int OrgId, int RoleId)
        {
            return await _db.UserInfos.FirstOrDefaultAsync(u => u.RoleID == RoleId && u.OrgId == OrgId);
        }
        public async Task<UserInfo> GetUserBy_OrgId_RoleId_ClientId(int OrgId, int RoleId = 0, int ClientId = 0)
        {
            return await _db.UserInfos.FirstOrDefaultAsync(u => u.OrgId == OrgId && (u.RoleID == RoleId || RoleId == 0) && (u.ClientId == ClientId || ClientId == 0));
        }

        public async Task<UserInfo> GetUserByUsername(string username, string password)
        {
            return await _db.UserInfos.FirstOrDefaultAsync(a => a.UserName == username && a.Password == password);
        }

        public async Task<int> Update(UserInfo entity)
        {
            using IDbContextTransaction transection = _db.Database.BeginTransaction();
            try
            {
                _db.UserInfos.Update(entity);
                await _db.SaveChangesAsync();
                transection.Commit();
                return entity.UserID;
            }
            catch (Exception ex)
            {
                transection.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> UpdateWithoutTrans(UserInfo entity)
        {            
            try
            {
                _db.UserInfos.Update(entity);
                await _db.SaveChangesAsync();
                return entity.UserID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SelectListItemModel>> Dropdown(int compId, int OrgId)
        {
            _db.Connection.Open();
            string sql = $"Select {nameof(UserInfo.UserID)} Value, {nameof(UserInfo.UserName)} Text  from {nameof(_db.UserInfos)} where CompId = @compId and OrgId = @OrgId";
            var data = await _readDb.QueryAsync<SelectListItemModel>(sql, new { compId, OrgId });
            _db.Connection.Close();
            return data;
        }

        public async Task<string> GetUserStatus(int RoleId, int OrgId)
        {
            var user = await _db.UserInfos.FirstOrDefaultAsync(u => u.RoleID == RoleId && u.OrgId == OrgId);
            var status = user.UserStatus;
            return status;
        }

        public async Task<string> CheckEmail(string Email)
        {
            var email = await _db.UserInfos.Where(e => e.Email == Email).FirstOrDefaultAsync();
            var message = "";
            if(email ==  null)
            {
                message = "This Email Not Registert";
            }
            return message;
        }

        public async Task<UserInfo> GetUserDataByEmail(string Email)
        {
            var userdata = await _db.UserInfos.Where(e => e.Email == Email).FirstOrDefaultAsync();
            return userdata;
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

        //public async Task<int> AddUserWiseModule(int Id, List<UserRolewiseModule> userRolewiseModule)
        //{
        //    var deletesql = "delete from UserRolewiseModule where RoleID=@RoleID";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var deleteRows = await connection.ExecuteAsync(deletesql, new { RoleID = Id });
        //        //return deleteRows;
        //    }

        //    //entity.DateCreated = DateTime.Now;
        //    var sql = "insert into UserRolewiseModule(RoleID,ModuleID)values(@RoleID,@ModuleID)";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var affectedRows = 0;
        //        for (var i = 0; i < userRolewiseModule.Count(); i++)
        //        {
        //            affectedRows = await connection.ExecuteAsync(sql, userRolewiseModule[i]);
        //        }

        //        return affectedRows;
        //    }

        //}
        //public async Task<IEnumerable<UserRolewiseModule>> GetUserWiseModule(int id)
        //{
        //    var sql = "Select urm.*, m.ModuleName, m.Description from UserRolewiseModule urm inner join MainModule m on m.ModuleID = urm.ModuleID Where urm.RoleID = @RoleID;";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<UserRolewiseModule>(sql, new { RoleID = id });
        //        return result;
        //    }
        //}

        //public async Task<IEnumerable<UserAccessList>> GetRolewiseAccess(string UAPage, int RoleID)
        //{
        //    var sql = "SELECT UAVeiw, UASave, UAEdit, UAdelete, UApproved FROM  dbo.UserAccessList UAList INNER JOIN UserAccesstools UATools ON UAList.uatoolid=UATools.uatoolid  WHERE (RoleID = @RoleID) AND (UATools.UAPage = @UAPage)";
        //    using (var connection = new SqlConnection(AppConfig.DefaultConnection))
        //    {
        //        connection.Open();
        //        var result = await connection.QueryAsync<UserAccessList>(sql, new { RoleID = RoleID, UAPage = UAPage });
        //        return result;
        //    }
        //}
        //#endregion
    }
}
