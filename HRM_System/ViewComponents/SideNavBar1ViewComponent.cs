using Application.Tasks.Queries.QMainModule;
using Application.Tasks.Queries.QSubModule;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Business;
using Domains.ViewModels;
using System.Web.Mvc;

namespace UKHRM.ViewComponents
{
    public class SideNavBar1ViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;
        private readonly IAuthBL _auth;

        public SideNavBar1ViewComponent(IMediator mediator, IGlobalHelper global, IAuthBL auth)
        {
            _mediator = mediator;
            _global = global;
            _auth = auth;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.UserId = _global.GetCompID();
            ViewBag.UserName = _global.GetUserName();
            var RoleID = _global.GetRoleID();
            ViewBag.UserRoleID = RoleID;
            //ViewBag.GetMenuModule = await _mediator.Send(new GetMenuModuleQuery() { RoleID = Convert.ToInt32(RoleID) });
            //ViewBag.GetSubModule = await _mediator.Send(new GetSubModuleQuery() { RoleId = Convert.ToInt32(RoleID) });
            ViewBag.ModuleSubModule = await _mediator.Send(new GetModuleSubModuleQuery() { RoleId = Convert.ToInt32(RoleID) });
            ViewBag.GetMenuPageName = await _mediator.Send(new GetAllMenuPageNameQuery());
            return View("SideNavBar1");
        }
    }
}
