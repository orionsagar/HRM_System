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

namespace Persistence.Repository.ModuleMenu
{
    public interface IUserAccessRepo : IGenericRepository<UserAccessTools>
    {
        Task<IEnumerable<RoleWiseUserAccessToolsVM>> GetRoleWiseModuleAccess(int RoleId, int ModuleID, int? SubModuleID);
        Task<int> AddUserAccessTools(List<UserAccessList> entity, int RoleId, int ModuleId, int? SubModuleId);


        /// For Menu
        /// 
        Task<List<MainModule>> GetMenuModule(int RoleId);
        Task<List<SubModule>> GetSubModule(int RoleId);
        Task<List<UserAccessTools>> GetMenuPageName(int RoleId, int ModuleId);
        Task<List<UserAccessTools>> GetAllMenuPageName();
        Task<int> GetSortIndexByModuleId(int ModuleId);
        Task<HomeMenuVM> GetModuleSubModule(int RoleId);
        Task<List<UserAccessTools>> GetMenuByCategory(string CategoryName);
        Task<IEnumerable<SelectListItemModel>> MenuDropdownByCategory(string CatName);
        Task<SubModule_Menu_VM> GetSubModule_And_Menu(int ModuleId, int RoleId, int ComId = 1);
    }


    public class UserAccessRepository : IUserAccessRepo
    {

        private IApplicationDbContext _dbContext;
        private IApplicationReadDbConnection readDbConnection;
        private IApplicationWriteDbConnection writeDbConnection;

        public UserAccessRepository(IApplicationDbContext db, IApplicationWriteDbConnection writeDbConnection, IApplicationReadDbConnection readDbConnection)
        {
            _dbContext = db;

            this.writeDbConnection = writeDbConnection;
            this.readDbConnection = readDbConnection;
        }

        public async Task<int> AddUserAccessTools(List<UserAccessList> entity, int RoleId, int ModuleId, int? SubModuleId)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                //_dbContext.Database.UseTransaction(transaction as DbTransaction);
                var querystr = "";
                if (SubModuleId > 0) {
                    querystr = " and SubModuleID = @SubModuleId ";
                }
                else
                {                    
                    querystr = " and SubModuleID is null ";
                }
                var delSql = "Delete from  [UserAccessList] where RoleID=@RoleID and UAToolID in (select uatoolid from UserAccesstools where ModuleID=@ModuleID "+ querystr + ")";

                var sql = @$"INSERT INTO [dbo].[UserAccessList] ([Accesstools] ,[UAVeiw] ,[UASave] ,[UAEdit] ,[UAdelete] ,[UApproved] ,[RoleID] ,[UAtoolID] ,[UDisburse] ,[AddedBy] ,[UpdatedBy] ,[AddedDate] ,[UpdatedDate],[SubModuleID],[ModuleID]) VALUES (@Accesstools, @UAVeiw, @UASave, @UAEdit, @UAdelete, @UApproved, @RoleID, @UAtoolID, @UDisburse, @AddedBy, @UpdatedBy, @AddedDate, @UpdatedDate,@SubModuleID,@ModuleID)";


                await writeDbConnection.ExecuteAsync(delSql, new { RoleID = RoleId, ModuleID = ModuleId, SubModuleId }, transaction);

                var affectedRows = await writeDbConnection.ExecuteAsync(sql, entity,transaction);
                
                transaction.Commit();
                return affectedRows;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<List<UserAccessTools>> GetAllMenuPageName()
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                //_dbContext.Database.UseTransaction(transaction as DbTransaction);
                var sql = $"Select UserAccesstools.*, [UserAccessList].RoleID from UserAccesstools inner join [UserAccessList]  on UserAccesstools.uatoolid=[UserAccessList].UAtoolID where[UAVeiw] = 1 And ismenu = 1;";
                var affectedRows = await readDbConnection.QueryAsync<UserAccessTools>(sql,transaction);
                transaction.Commit();
                return affectedRows.ToList();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<List<MainModule>> GetMenuModule(int RoleId)
        {
            _dbContext.Connection.Open();
            try
            {
                //var sql = @$"Select * from ( 
	               //             select distinct  mm.ModuleID, mm.ModuleName, a.TotalPageCnt, mm.SortIndex, urm.Description 
	               //             from UserRolewiseModules  urm
		              //              inner join MainModules mm on mm.ModuleID = urm.ModuleID  
		              //              left outer join  
		              //              (	select count(0) TotalPageCnt, uat.ModuleID from[UserAccessList] ua
			             //               inner join UserAccesstools uat on uat.uatoolid =ua.UAtoolID      
			             //               where([UAVeiw] = 1 And uat.RoleID = @RoleID) group by uat.ModuleID  
		              //              ) A  on A.ModuleID = mm.ModuleID  
		              //              inner join UserAccesstools on UserAccesstools.ModuleID = A.ModuleID  
		              //              where (urm.RoleID = @RoleID) 
                //            ) A order by SortIndex asc";
                var sql = @$"select m.* from UserRolewiseModules urm
                                inner join MainModules m on m.ModuleID = urm.ModuleID
                                where RoleID = @RoleID
                                order by m.SortIndex";

                var affectedRows = await readDbConnection.QueryAsync<MainModule>(sql, new { RoleID = RoleId });
                return affectedRows.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
         public async Task<List<SubModule>> GetSubModule(int RoleId)
        {
            _dbContext.Connection.Open();
            try
            {
                var sql = @$"select sm.* from UserRolewiseSubModules ursm
                                inner join SubModule sm on sm.SubModuleID = ursm.SubModuleID
                                where RoleID = @RoleID
                                order by sm.SortIndex";

                var affectedRows = await readDbConnection.QueryAsync<SubModule>(sql, new { RoleID = RoleId });
                return affectedRows.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<HomeMenuVM> GetModuleSubModule(int RoleId)
        {
            _dbContext.Connection.Open();
            try
            {
                var sql = @$"select m.* from UserRolewiseModules urm
                                inner join MainModules m on m.ModuleID = urm.ModuleID
                                where RoleID = @RoleID
                                order by m.SortIndex";

                var cmd = @$"select sm.* from UserRolewiseSubModules ursm
                                inner join SubModule sm on sm.SubModuleID = ursm.SubModuleID
                                where RoleID = @RoleID
                                order by sm.SortIndex";

                var module = await readDbConnection.QueryAsync<MainModule>(sql, new { RoleID = RoleId });
                var submodule = await readDbConnection.QueryAsync<SubModule>(cmd, new { RoleID = RoleId });
                HomeMenuVM menuVM = new HomeMenuVM
                {
                    MainModule = module.ToList(),
                    SubModule = submodule.ToList()
                };
                return menuVM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }
         public async Task<SubModule_Menu_VM> GetSubModule_And_Menu(int ModuleId, int RoleId, int ComId = 1)
        {
            _dbContext.Connection.Open();
            try
            {
                var sql = @$"select distinct ut.UAToolid,ut.UseraccesstoolNM,ut.UAPage,ut.SubModuleID from UserAccessTools ut 
                            left join UserAccessList ul on ut.SubModuleId = ul.SubModuleId 
                            where ut.ModuleId = @ModuleId and ul.RoleID = @RoleId";

                var cmd = @$"select distinct sm.* from UserRolewiseSubModules ursm
                                inner join SubModule sm on sm.SubModuleID = ursm.SubModuleID
                                where sm.ModuleId = @ModuleId
                                order by sm.SortIndex";

                var module = await readDbConnection.QueryAsync<UserAccessToolsVM>(sql, new { RoleID = RoleId , ModuleId = ModuleId});
                var submodule = await readDbConnection.QueryAsync<SubModuleVM>(cmd, new { ModuleId = ModuleId });
                SubModule_Menu_VM menuVM = new SubModule_Menu_VM
                {
                    Menu = module.ToList(),
                    SubModule = submodule.ToList()
                };
                return menuVM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<List<UserAccessTools>> GetMenuPageName(int RoleId, int ModuleId)
        {
            _dbContext.Connection.Open();
            using var transaction = _dbContext.Connection.BeginTransaction();
            try
            {
                //_dbContext.Database.UseTransaction(transaction as DbTransaction);
                var sql = "Select UserAccesstools.*, [UserAccessList].RoleID from UserAccesstools inner join [UserAccessList]  on UserAccesstools.uatoolid=[UserAccessList].UAtoolID " +
                      " where[UAVeiw] = 1 And ismenu = 1 And roleid = @RoleID and ModuleID = @ModuleID";
               
                var affectedRows = await readDbConnection.QueryAsync<UserAccessTools>(sql, new { RoleID = RoleId, ModuleID = ModuleId },transaction);
                transaction.Commit();
                return affectedRows.ToList();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<List<UserAccessTools>> GetMenuByCategory(string CategoryName)
        {
           var menu = await _dbContext.UserAccessTools.Where(c => c.Category == CategoryName).ToListAsync();
            return menu;
        }

        public async Task<IEnumerable<RoleWiseUserAccessToolsVM>> GetRoleWiseModuleAccess(int RoleId, int ModuleID, int? SubModuleID)
        {
            _dbContext.Connection.Open();
            try
            {
                var sql = "";
                if (SubModuleID == 0 || SubModuleID == null)
                {
                    sql = @$"Select uat.uatoolid, uat.UseraccesstoolNM, uat.Category, uat.UAPage, ual.ulistID, ual.Accesstools, ual.UAVeiw, ual.UASave, ual.UAEdit, ual.UAdelete, ual.UApproved, ual.RoleID 
                        from UserAccesstools uat 
                        left outer join
                            (
                                select * from UserAccessList where RoleID = @RoleID
                            ) ual ON ual.UAtoolID = uat.uatoolid 
                        where uat.ModuleID = @ModuleID and  uat.SubModuleID is null 
                        order by  UseraccesstoolNM;";
                    var affectedRows = await readDbConnection.QueryAsync<RoleWiseUserAccessToolsVM>(sql, new { RoleID = RoleId, ModuleID = ModuleID});
                    return affectedRows;
                }
                else
                {
                    sql = @$"Select uat.uatoolid, uat.UseraccesstoolNM, uat.Category, uat.UAPage, ual.ulistID, ual.Accesstools, ual.UAVeiw, ual.UASave, ual.UAEdit, ual.UAdelete, ual.UApproved, ual.RoleID 
                        from UserAccesstools uat 
                        left outer join
                            (  
                                select * from UserAccessList where RoleID = @RoleID
                            ) ual ON ual.UAtoolID = uat.uatoolid 
                        where uat.ModuleID = @ModuleID and uat.SubModuleID = @SubModuleID 
                        order by  UseraccesstoolNM;";
                    var affectedRows = await readDbConnection.QueryAsync<RoleWiseUserAccessToolsVM>(sql, new { RoleID = RoleId, ModuleID = ModuleID, SubModuleID });
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
            
        }

        public async Task<int> Add(UserAccessTools entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.UserAccessTools.Add(entity);
                int returnId = await _dbContext.SaveChangesAsync();

                transaction.Commit();
                return returnId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }

            //_dbContext.Connection.Open();
            //using var transaction = _dbContext.Connection.BeginTransaction();
            //try
            //{
            //    _dbContext.Database.UseTransaction(transaction as DbTransaction);
            //    var sql = @$"INSERT INTO UserAccessTools ([{nameof(UserAccessTools.ModuleName)}],[{nameof(UserAccessTools.Description)}],[{nameof(UserAccessTools.Package)}],[{nameof(UserAccessTools.SortIndex)}]) 
            //            Values (@{nameof(UserAccessTools.ModuleName)}, @{nameof(UserAccessTools.Description)}, @{nameof(UserAccessTools.Package)},@{nameof(UserAccessTools.SortIndex)});
            //            SELECT CAST(SCOPE_IDENTITY() as int)";

            //    var affectedRows = await writeDbConnection.QuerySingleAsync<int>(sql, entity, transaction);
            //    transaction.Commit();
            //    return affectedRows;
            //}
            //catch (Exception)
            //{
            //    transaction.Rollback();
            //    throw;
            //}
            //finally
            //{
            //    _dbContext.Connection.Close();
            //}

        }

        public async Task<int> Delete(int id)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var exist = await GetById(id);
                if (exist == null) return 0;

                _dbContext.UserAccessTools.Remove(exist);
                int returnId = await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return returnId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UserAccessTools>> GetAll()
        {
            return await _dbContext.UserAccessTools.ToListAsync();
        }

        public async Task<UserAccessTools> GetById(int id)
        {
            return await _dbContext.UserAccessTools.FirstOrDefaultAsync(a => a.UAToolid == id);
        }
        public async Task<int> GetSortIndexByModuleId(int ModuleId)
        {
            _dbContext.Connection.Open();
            try
            {
                var sql = @$"select top 1 {nameof(UserAccessTools.SortIndex)} from {nameof(_dbContext.UserAccessTools)} where {nameof(UserAccessTools.ModuleID)} = @ModuleID Order By {nameof(UserAccessTools.SortIndex)} desc";
                var affectedRows = await readDbConnection.QueryAsync<int>(sql, new { ModuleID = ModuleId });
                return affectedRows.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbContext.Connection.Close();
            }
        }

        public async Task<int> Update(UserAccessTools entity)
        {
            using IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.UserAccessTools.Update(entity);
                int returnId = await _dbContext.SaveChangesAsync();
                transaction.Commit();
                return returnId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<SelectListItemModel>> MenuDropdownByCategory(string CatName)
        {
            _dbContext.Connection.Open();
            string sql = $"Select {nameof(UserAccessTools.UAToolid)} Value, {nameof(UserAccessTools.UseraccesstoolNM)} Text  from {nameof(_dbContext.UserAccessTools)} Where Category = '{CatName}'";
            var data = await readDbConnection.QueryAsync<SelectListItemModel>(sql);
            _dbContext.Connection.Close();
            return data;
        }
    }
}
