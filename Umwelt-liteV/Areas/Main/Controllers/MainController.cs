using Microsoft.AspNetCore.Mvc;

namespace Umwelt_liteV.Areas.Main.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
