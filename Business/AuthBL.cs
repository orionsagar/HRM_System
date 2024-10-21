using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IAuthBL
    {
        Task<int> GET_RoleID_BY_U_ROLENAME(string UniqueRoleName = "Org_SuperAdmin");
        Task<SubModule_Menu_VM> Get_SubModule_Menu_By_ModuleId(int ModuleId, int RoleId);
    }
    public class AuthBL : IAuthBL
    {
        private IUnitOfWork _work;

        public AuthBL(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> GET_RoleID_BY_U_ROLENAME(string UniqueRoleName = "Org_SuperAdmin")
        {
            var roleid = await _work.UserRoles.GetRoleIDBy_U_RoleName(UniqueRoleName);
            return roleid;
        }

        public async Task<SubModule_Menu_VM> Get_SubModule_Menu_By_ModuleId(int ModuleId, int RoleId )
        {
            var submodule_menu = await _work.UserAccess.GetSubModule_And_Menu(ModuleId, RoleId);
            return submodule_menu;
        }
    }
}
