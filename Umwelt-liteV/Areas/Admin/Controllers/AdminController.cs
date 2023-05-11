using Microsoft.AspNetCore.Mvc;
using Umwelt_liteV.Core.Services;
using Umwelt_liteV.Data.Models.ViewModels;

namespace Umwelt_liteV.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class AdminController : Controller
    {
        private readonly IAdminService _admin;
        public AdminController( IAdminService admin )
        {
            _admin = admin;
        }
        public IActionResult Main() => View();



        [HttpGet]
        [Route("CreateArticle")]
        public IActionResult CreateArticle()
        {
            var createArticle = new CreateArticleVm();

            createArticle.Categories = _admin.GetCategories();
            if(createArticle.Categories == null || createArticle.Categories.Count == 0)
            {
                return RedirectToAction("Main");
            }

            return View(createArticle);
        }

        [HttpPost]
        [Route("CreateArticle")]
        public IActionResult CreateArticle(CreateArticleVm createArticle)
        {

            var message = _admin.AddArticle(createArticle);
            if(message.ErrorId < 0)
            {
                TempData["error"] = message.Message.ToString();
                return RedirectToAction("Main");
            }

            TempData["success"] = message.Message.ToString();
            return RedirectToAction("Main");
        }


        [HttpGet]
        [Route("CreateCategory")]
        public IActionResult CreateCategory() => View();
        [HttpPost]
        [Route("CreateCategory")]
        public IActionResult CreateCategory(CreateCategoryVm categoryVm )
        {
            var message = _admin.AddCategory(categoryVm);
            if(message.ErrorId < 0)
            {
                TempData["error"] = message.Message.ToString();
                return RedirectToAction("Main");
            }

            TempData["success"] = message.Message.ToString();
            return RedirectToAction("Main");
        }


    }
}
