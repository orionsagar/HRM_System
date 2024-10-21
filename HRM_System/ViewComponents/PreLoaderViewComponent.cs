using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UKHRM.ViewComponents
{
    public class PreLoaderViewComponent : ViewComponent
    {
        public PreLoaderViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("PreLoader");
        }
    }
}
