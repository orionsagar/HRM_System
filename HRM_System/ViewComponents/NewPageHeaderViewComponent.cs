using Domains.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UKHRM.ViewComponents
{
    public class NewPageHeaderViewComponent : ViewComponent
    {
        public NewPageHeaderViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(PageHeaderProps pageHeader)
        {
            ViewData["PageHeaderData"] = pageHeader;
            ViewBag.PageHeaderBagData = pageHeader;
            return View("NewPageHeader");
        }
    }
}
