using Domains.Models;
using Domains.ViewModels;
using UKHRM.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UKHRM.ViewComponents
{
    public class EmployeeFilterViewComponent : ViewComponent
    {
        public readonly CommonDropdown _common;
        public readonly IGlobalHelper _global;
        public EmployeeFilterViewComponent(CommonDropdown common, IGlobalHelper global = null)
        {
            _common = common;
            _global = global;
        }

        public async Task<IViewComponentResult> InvokeAsync(EmployeeFilterVM model)
        {
            var compId = _global.GetCompID();
            var orgid = _global.GetOrgId();
            var ClientId = _global.GetClientId();
            ViewBag.OrgId = await _common.OrganisationDropdown(orgid, ClientId);
            ViewBag.EmpCategory = await _common.EmployeeCategoryDropdown(compId,orgid,ClientId);
            ViewBag.Designation = await _common.DesignationDropdown(compId, orgid);
            ViewBag.Section = await _common.SectionDropdown(compId, orgid);
            ViewBag.Department = await _common.DepartmentDropdown(compId, orgid);           
            return View("EmployeeFilter",model);
        }
    }
}
