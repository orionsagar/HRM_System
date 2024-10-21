using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UKHRM.Controllers
{
    public class ProfileViewController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
