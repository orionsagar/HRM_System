using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UKHRM.ViewComponents
{
    public class CustomFooterViewComponent : ViewComponent
    {
        public CustomFooterViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("CustomFooter");
        }
    }
}
