//using Domains.Models;
//using Domains.ViewModels;
//using Microsoft.EntityFrameworkCore;
//using Persistence.DAL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Runtime.CompilerServices.RuntimeHelpers;
//using System.Xml.Linq;

//namespace Persistence.Repository.Clients
//{
//    public interface IClientProfile
//    {
//       Task<UserProfileVM> GetClientProfile(int UserID);
//    }

//    public class ClientProfileRepository : IClientProfile
//    {
//        private IApplicationDbContext _db;
//        private IApplicationReadDbConnection _readDb;
//        public ClientProfileRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
//        {
//            _db = db;
//            _readDb = readDb;

//        }
//        public async Task<UserProfileVM> GetClientProfile(int UserID)
//        {
//            try
//            {
//                var ClientDetailsQuery = $@"SELECT
//                                         c.[ClientID], c.[Name], c.[ClientType], c.[BusinessType],
//                                         c.[Address], c.[City], c.[State], c.[HotNumber], c.[Email1], c.[Website],
//                                         c.[Country], c.[Facebook], c.[Instagram], c.[Linkedin], c.[twitter],
//                                         c.[GooglePlus], c.[Logo], c.[CP_Name], c.[CP_Phone], c.[CP_Mobile],
//                                         c.[CP_Email], c.[Establishment_Year], c.[PostCode], c.[SRACode],
//                                         u.[FirstName], u.[LastName]
//                                         FROM
//                                           [WorkerPermitHRM_DB].[dbo].[Clients] c
//                                         LEFT JOIN
//                                           [WorkerPermitHRM_DB].[dbo].[UserInfos] u ON c.[ClientID] = u.[ClientID]-- Assuming there's a relationship between Clients and UserInfos
//                                         WHERE
//                                           c.[ClientID] = @UserID";
//                //var ClientDetailsQuery = $@"SELECT [ClientID], [Name], [ClientType], [BusinessType],
//                //                    [Address], [City], [State], [HotNumber], [Email1], [Website],
//                //                    [Country], [Facebook], [Instagram], [Linkedin], [twitter],
//                //                    [GooglePlus], [Logo], [CP_Name], [CP_Phone], [CP_Mobile],
//                //                    [CP_Email], [Establishment_Year], [PostCode], [SRACode]
//                //                    FROM [WorkerPermitHRM_DB].[dbo].[Clients] WHERE [ClientID] = @UserID";

//                var OrganisationDetailsQuery = $@"SELECT o.[OrgId], [OrgName], [ContactNo], [OrgEmail], [LandlineNumber], [LogoPath], o.[PackageId],p.[PackageName], (select Count(EmpId) from Employees e where e.OrgId = o.OrgId)NumberOfEmployees
//                                    FROM [WorkerPermitHRM_DB].[dbo].[Organisations] o 
//                                    left join Packages p on p.PackageId = o.PackageId
//                                    where o.ClientId = @UserID";


//                _db.Connection.Open();

//                var ClientDetailsGet = await _readDb.QueryFirstOrDefaultAsync<UserProfileVM>(ClientDetailsQuery, new { UserID });
//                var OrganisationDetailsGet = await _readDb.QueryAsync<MiniOrgCard_solicitor>(OrganisationDetailsQuery, new { UserID });
//                ClientDetailsGet.MiniOrgCard = OrganisationDetailsGet.ToList();
//                //var OrganisationDetailsGet = await _readDb.QueryAsync<MiniOrgCard_solicitor>(OrganisationDetailsQuery, new { UserID });
//                //var OrgPackage = OrganisationDetailsGet.FirstOrDefault()?.PackageId;

//                //foreach(var item in OrganisationDetailsGet)
//                //{
//                //    var PackageDetailsGet = await _readDb.QueryAsync<MiniOrgCard_solicitor>(PackageDetailsQuery, new { PackageId = OrgPackage });
//                //    var TotalEmployeesGet = await _readDb.QueryFirstOrDefaultAsync<MiniOrgCard_solicitor>(TotalEmployeesQuery, new { OrgId });
//                //    //var totalemployee = new MiniOrgCard_solicitor()
//                //    //{
//                //    //    totalemployee.NumberOfEmployees = TotalEmployeesGet.NumberOfEmployees;
//                //    //}
//                //}




//                _db.Connection.Close();

//                return (ClientDetailsGet);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//    }
//}


using Domains.Models;
using Domains.ViewModels;
using Microsoft.EntityFrameworkCore;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repository.Clients
{
    public interface IClientProfile
    {
        Task<UserProfileVM> GetClientProfile(int UserID, string UserRole, int RoleID);
        Task<UserProfileViewModel> GetUserProfile(int UserID);
    }

    public class ClientProfileRepository : IClientProfile
    {
        private IApplicationDbContext _db;
        private IApplicationReadDbConnection _readDb;
        public ClientProfileRepository(IApplicationDbContext db, IApplicationReadDbConnection readDb)
        {
            _db = db;
            _readDb = readDb;
        }

        public async Task<UserProfileVM> GetClientProfile(int UserID, string UserRole, int RoleID)
        {
            try
            {
                string ClientDetailsQuery = @"SELECT
                                              c.[ClientID],c.[Name],c.[Logo], c.[ClientType], c.[BusinessType],
                                              c.[Address], c.[City], c.[State], c.[HotNumber], c.[Email1], c.[Website],
                                              c.[Country], c.[Facebook], c.[Instagram], c.[Linkedin], c.[twitter],
                                              c.[GooglePlus], c.[Logo], c.[CP_Name], c.[CP_Phone], c.[CP_Mobile],
                                              c.[CP_Email], c.[Establishment_Year], c.[PostCode], c.[SRACode],
                                              u.[FirstName], u.[LastName]
                                              FROM
                                                [WorkerPermitHRM_DB].[dbo].[Clients] c
                                              LEFT JOIN
                                                [WorkerPermitHRM_DB].[dbo].[UserInfos] u ON c.[ClientID] = u.[ClientID]
                                              WHERE
                                                c.[ClientID] = @UserID";

                string OrganisationDetailsQuery = @"SELECT o.[OrgId], [OrgName], [ContactNo], [OrgEmail], [LandlineNumber], [LogoPath], o.[PackageId], p.[PackageName], 
                                                    (SELECT COUNT(EmpId) FROM Employees e WHERE e.OrgId = o.OrgId) AS NumberOfEmployees
                                                    FROM [WorkerPermitHRM_DB].[dbo].[Organisations] o 
                                                    LEFT JOIN Packages p ON p.PackageId = o.PackageId
                                                    WHERE o.ClientId = @UserID";

                _db.Connection.Open();

                var ClientDetailsGet = await _readDb.QueryFirstOrDefaultAsync<UserProfileVM>(ClientDetailsQuery, new { UserID });
                if (ClientDetailsGet == null)
                {
                    _db.Connection.Close();
                    return null;
                }

                var OrganisationDetailsGet = await _readDb.QueryAsync<MiniOrgCard_solicitor>(OrganisationDetailsQuery, new { UserID });
                ClientDetailsGet.MiniOrgCard = OrganisationDetailsGet.ToList();

                _db.Connection.Close();

                return ClientDetailsGet;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching client profile", ex);
            }
        }

        public async Task<UserProfileViewModel> GetUserProfile(int UserID)
        {
            try
            {
                string Query = @"Select uis.UserID, uis.FirstName, uis.LastName, uis.UserName, uis.[Password], uis.Email, uis.Phone, uis.UserStatus, uis.RoleID, ur.RoleName, ur.RoleType,
                                uis.OrgId, org.OrgName, org.OrgCode, org.OrgType, org.OrgEmail, org.AddressLine1, org.AddressLine2, org.AddressLine3, Org.RegNo, org.ContactNo, org.PostCode,
                                org.City, ct.[Name] CountryName, org.Website, org.LandlineNumber, org.TradingName, org.TradingPeriod, org.SectorName, org.LogoPath, org.AP_FirstName, org.AP_LastName,org.AP_PhoneNo, org.AP_Email, org.AP_NIDPath,
                                org.AP_IsCriminalHistory, org.PackageID, org.[Status] OrgStatus, c.[Name] ClientName, c.ClientType, c.[Address] ClientAddress, c.City ClientCity, c.[State] ClientState, c.Phone1 ClientPhone1, c.Phone2 ClientPhone2, c.Email1 ClientEmail1, c.Email2 ClientEmail2,
                                org.AP_DesignationName DesigationName,c.Country, c.Logo ClientLogo, c.CP_Name, c.CP_Phone, c.CP_Email, c.PostCode ClientPostCode, c.[Status] ClientStatus, c.AcHolderName, c.SortCode, c.AccountNumber from UserInfos uis 
                                left join Organisations org on org.OrgID = uis.Orgid
                                Inner join UserRoles ur on ur.RoleID = uis.RoleID
                                left join Clients c on c.ClientID = uis.ClientId
                                Left join Countrys ct on ct.CountryId = org.CountryID
                                Left join Designations ds on ds.DesigID = org.AP_DesignationID Where uis.UserID = @UserID";

                _db.Connection.Open();

                var ClientDetailsGet = await _readDb.QueryFirstOrDefaultAsync<UserProfileViewModel>(Query, new { UserID = UserID });
                if (ClientDetailsGet == null)
                {
                    _db.Connection.Close();
                    return null;
                }

                _db.Connection.Close();

                return ClientDetailsGet;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching client profile", ex);
            }
        }
    }
}
