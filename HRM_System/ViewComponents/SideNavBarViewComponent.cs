using Application.Tasks.Queries.QMainModule;
using Application.Tasks.Queries.QSubModule;
using UKHRM.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UKHRM.ViewComponents
{
    public class SideNavBarViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IGlobalHelper _global;

        public SideNavBarViewComponent(IMediator mediator, IGlobalHelper global)
        {
            _mediator = mediator;
            _global = global;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.UserId = _global.GetCompID();
            ViewBag.UserName = _global.GetUserName();
            var RoleID = _global.GetRoleID();
            ViewBag.UserRoleID = RoleID;
            ViewBag.GetMenuModule = await _mediator.Send(new GetMenuModuleQuery() { RoleID = Convert.ToInt32(RoleID) });
            ViewBag.GetSubModule = await _mediator.Send(new GetSubModuleQuery() { RoleId = Convert.ToInt32(RoleID) });
            ViewBag.ModuleSubModule = await _mediator.Send(new GetModuleSubModuleQuery() { RoleId = Convert.ToInt32(RoleID) });
            ViewBag.GetMenuPageName = await _mediator.Send(new GetAllMenuPageNameQuery());

            return View("SideNavBar");
        }
    }
}
