using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Persistence.DAL;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using UKHRM.Helper;
using Domains.Models;

namespace UKHRM.Extensions
{
    [Authorize]
    public class CustomAuthorizationFilterAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        public async void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {
                // DatabaseContext context = new DatabaseContext();
                // IDbConnection sqlConnection = new SqlConnection(AppConfig.DefaultConnection);


                string host = filterContext.HttpContext.Request.Host.Value;
                string fullPath = filterContext.HttpContext.Request.GetDisplayUrl();
                string currentPath = filterContext.HttpContext.Request.Path.Value;

                var UserId = DataEncryption.DecryptString(filterContext.HttpContext.Request.Cookies["UserID"]);
                var UserRole = DataEncryption.DecryptString(filterContext.HttpContext.Request.Cookies["Role"]);
                var UserRoleID = DataEncryption.DecryptString(filterContext.HttpContext.Request.Cookies["RoleID"]);

                var role = Convert.ToString(filterContext.HttpContext.Session.GetString("Role"));

                if (!string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(currentPath))
                {
                    //var roleValue = Convert.ToInt32(role);


                    var sql = "Select Uat.uatoolid,Uat.UAPage,Uat.Category, Ual.* from UserAccesstools Uat " +
                            " inner join UserAccessList Ual on ual.uatoolid = Uat.UAtoolID " +
                            " Where RoleID = " + UserRoleID + " and UAPage = '" + currentPath.Substring(1) + "'";
                    var result = await GetList(sql);

                    //var roleMasterDetails = (from rolemaster in context.RoleMasters
                    //                         where rolemaster.RoleId == roleValue
                    //                         select rolemaster).FirstOrDefault();

                    //if (roleMasterDetails != null && !(roleMasterDetails.RoleName.ToLower() == "admin" ||
                    //                                   roleMasterDetails.RoleName.ToLower() == "superadmin"))

                    if (result != null)
                    {
                        //filterContext.HttpContext.Session.Abandon();
                        if (result.UAVeiw != true || result.UAVeiw == null)
                        {
                            filterContext.Result = new RedirectToRouteResult
                            (
                                new RouteValueDictionary
                                    (new
                                    { controller = "Error", action = "accessdenied" }
                                ));
                        }


                    }
                }
                else
                {
                    //filterContext.HttpContext.Session.Abandon();

                    filterContext.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary
                        (new
                        { controller = "Error", action = "Error" }
                        ));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UserPageAccess> GetList(string sqlQuery, string extraQuery = "")
        {
            var sql = sqlQuery;
            using (var connection = new SqlConnection(AppConfig.DefaultConnection))
            {
                connection.Open();
                var result = await connection.QueryAsync<UserPageAccess>(sql + extraQuery);
                return result.FirstOrDefault();
            }
        }
    }
}
