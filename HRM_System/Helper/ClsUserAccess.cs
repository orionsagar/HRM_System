using Dapper;
using Domains.Models;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace UKHRM.Helpers
{
    public class ClsUserAccess
    {
        public static async Task<string> PageGetRolewiseAccess(string UAPage, int RoleID, string ForLink)
        {
            var rtnStr = "HideLink";
            IEnumerable<UserAccessList> userAccessList = new List<UserAccessList>();
            try
            {
                using (var connection = new SqlConnection(AppConfig.DefaultConnection))
                {
                    connection.Open();

                    //insert into voucher table --start
                    var sql = "SELECT UAVeiw, UASave, UAEdit, UAdelete, UApproved FROM  dbo.UserAccessList UAList INNER JOIN UserAccesstools UATools ON UAList.uatoolid=UATools.uatoolid  WHERE (UAList.RoleID = @RoleID) AND (UATools.UAPage = @UAPage)";
                    userAccessList = await connection.QueryAsync<UserAccessList>(sql, new { RoleID = RoleID, UAPage = UAPage });

                    if (userAccessList.Any())
                    {
                        /// For View
                        /// 
                        if (ForLink == "View")
                        {
                            if (userAccessList.FirstOrDefault().UAVeiw == true)
                            {
                                rtnStr = "ShowLink";
                            }
                            else
                            {
                                rtnStr = "HideLink";
                            }

                            return rtnStr;
                        }


                        /// For Edit
                        /// 
                        if (ForLink == "Edit")
                        {
                            if (userAccessList.FirstOrDefault().UAEdit == true)
                            {
                                rtnStr = "ShowLink";
                            }
                            else
                            {
                                rtnStr = "HideLink";
                            }

                            return rtnStr;
                        }

                        /// For Save
                        /// 
                        if (ForLink == "Save")
                        {
                            if (userAccessList.FirstOrDefault().UASave == true)
                            {
                                rtnStr = "ShowLink";
                            }
                            else
                            {
                                rtnStr = "HideLink";
                            }
                            return rtnStr;
                        }


                        /// For Delete
                        /// 
                        if (ForLink == "Delete")
                        {
                            if (userAccessList.FirstOrDefault().UAdelete == true)
                            {
                                rtnStr = "ShowLink";
                            }
                            else
                            {
                                rtnStr = "HideLink";
                            }
                            return rtnStr;
                        }


                        /// For Approved
                        /// 
                        if (ForLink == "Approved")
                        {
                            if (userAccessList.FirstOrDefault().UApproved == true)
                            {
                                rtnStr = "ShowLink";
                            }
                            else
                            {
                                rtnStr = "HideLink";
                            }
                            return rtnStr;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                rtnStr = ex.Message.ToString();
                return rtnStr;
            }

            return rtnStr;
        }
    }
}
