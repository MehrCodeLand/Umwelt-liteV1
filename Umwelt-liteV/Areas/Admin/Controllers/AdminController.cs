using Microsoft.AspNetCore.Mvc;
using Umwelt_liteV.Core.Services;
using Umwelt_liteV.Data.Models.Helper;
using Umwelt_liteV.Data.Models.ViewModels;

namespace Umwelt_liteV.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class AdminController : Controller
    {
        private readonly IAdminService _admin;
        public AdminController(IAdminService admin)
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
            if (createArticle.Categories == null || createArticle.Categories.Count == 0)
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
            if (message.ErrorId < 0)
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
        public IActionResult CreateCategory(CreateCategoryVm categoryVm)
        {
            var message = _admin.AddCategory(categoryVm);
            if (message.ErrorId < 0)
            {
                TempData["error"] = message.Message.ToString();
                return RedirectToAction("Main");
            }

            TempData["success"] = message.Message.ToString();
            return RedirectToAction("Main");
        }

        public IActionResult ArticleList(int pageId = 1, string search = "")
        {
            // we need manage paggination
            var articleList = _admin.GetAllUserForAdmin(pageId, search);
            return View(articleList);
        }


        [Route("EditArticle")]
        public IActionResult EditArticle(int id)
        {
            var editArticleVm = _admin.FindArticleByMyArticleId(id);
            if(editArticleVm == null)
            {
                TempData["error"] = "We can do that right now";
                return RedirectToAction("Main");
            }

            return View(editArticleVm);
        }

        [Route("EditArticle")]
        [HttpPost]
        public IActionResult EditArticle(ArticleEditVm articleEdit)
        {
            var message = _admin.ArticleEdit(articleEdit);

            if (message.ErrorId < 0)
            {
                TempData["error"] = message.Message.ToString();

                // find article again
                var sendArticleAgain = _admin.FindArticleByMyArticleId(articleEdit.MyArticle);
                return View(sendArticleAgain);
            }

            TempData["success"] = message.Message.ToString();
            return RedirectToAction("Main");
        }

    }
}
