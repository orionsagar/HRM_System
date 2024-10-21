using Application.Tasks.Queries.QEmployee;
using Application.Tasks.Queries.QMainModule;
using Application.Tasks.Queries.QSubModule;
using Domains.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UKHRM.Helper;
using Utility;

namespace UKHRM.ViewComponents
{
    public class DashboardViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        public DashboardViewComponent(IMediator mediator, IGlobalHelper global)
        {
            _mediator = mediator;
            _global = global;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.UserId = _global.GetCompID();
            ViewBag.UserName = _global.GetUserName();
            var RoleID = _global.GetRoleID();
            var RoleType = _global.GetRoleType();
            var OrgId = _global.GetOrgId();
            var ClientId = _global.GetClientId();
            if(RoleType == Enums.RoleType.Platform_Role.ToString())
            {
                ViewBag.EmpInfo = await _mediator.Send(new GetEmployeeInfoQuery { RoleType = RoleType });
            }
            if(RoleType == Enums.RoleType.Client_Role.ToString())
            {
                ViewBag.EmpInfo = await _mediator.Send(new GetEmployeeInfoQuery { ClientId = ClientId });
            }

            if(RoleType == Enums.RoleType.Org_Role.ToString())
            {
                if (OrgId != 0)
                {
                    ViewBag.EmpInfo = await _mediator.Send(new GetEmployeeInfoQuery { OrgId = OrgId });
                }
            }
            ViewBag.OrgId = OrgId;
            ViewBag.UserRoleID = RoleID;
            ViewBag.GetMenuModule = await _mediator.Send(new GetMenuModuleQuery() { RoleID = Convert.ToInt32(RoleID) });
            ViewBag.GetSubModule = await _mediator.Send(new GetSubModuleQuery() { RoleId = Convert.ToInt32(RoleID) });
            ViewBag.ModuleSubModule = await _mediator.Send(new GetModuleSubModuleQuery() { RoleId = Convert.ToInt32(RoleID) });
            ViewBag.GetMenuPageName = await _mediator.Send(new GetAllMenuPageNameQuery());

            DashboardViewModel dashboardViewModel = new DashboardViewModel();

            switch (RoleType)
            {
                case "Platform_Role":
                    return View("_PlatformAdmin_Dashboard");
                    
                case "Org_Role":
                    return View("_Org_Dashboard");
                    
                case "Client_Role":
                    return View("_Client_Dashboard");
                case "15":
                    return View("_Client_Dashboard");
                case "16":
                    return View("_PlatformAdmin_Dashboard");
                default:
                    return View("_Org_Dashboard");                    
            }

            //return View("SideNavBar");
        }
    }

}
