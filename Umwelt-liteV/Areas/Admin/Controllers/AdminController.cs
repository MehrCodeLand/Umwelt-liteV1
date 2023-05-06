using Microsoft.AspNetCore.Mvc;

namespace Umwelt_liteV.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class AdminController : Controller
    {
        public IActionResult Main() => View();
    }
}
