using Microsoft.AspNetCore.Mvc;

namespace Umwelt_liteV.Areas.Main.Controllers
{
    [Area(nameof(Main))]
    public class MainController : Controller
    {
        public IActionResult Home() => View();


    }
}
