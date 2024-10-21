using Domains.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UKHRM.ViewComponents
{
    public class PageHeaderViewComponent : ViewComponent
    {
        public PageHeaderViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(PageHeaderProps pageHeader)
        {
            ViewData["PageHeaderData"] = pageHeader;
            ViewBag.PageHeaderBagData = pageHeader;
            return View("PageHeader");
        }
    }
}
