using System.Text.RegularExpressions;
using Umwelt_liteV.Core.Services;
using Umwelt_liteV.Creator;
using Umwelt_liteV.Data.Models.Entities;
using Umwelt_liteV.Data.Models.Structs;
using Umwelt_liteV.Data.Models.ViewModels;
using Umwelt_liteV.Data.MyContext;

namespace Umwelt_liteV.Core.Repositories
{
    public class AdminRepository : IAdminService
    {
        private readonly MyDb _db;
        public AdminRepository(MyDb db)
        {
            _db = db;
        }


        #region Category

        public MessageData AddCategory(CreateCategoryVm createCategoryVm)
        {
            var message = new MessageData();

            message = ValidateCategory(createCategoryVm);
            if(message.ErrorId < 0)
            {
                return message;
            }

            var category = new Category()
            {
                MyCategoryId = CreateRandomId.CreateId(),
                Title = createCategoryVm.Title.ToLower(),
            };

            _db.Categories.Add(category);
            Save();

            var result = IsAddCategory(category.MyCategoryId);
            if (!result)
            {
                message.ErrorId = -200;
                message.Message = "Somthings Wrong";

                return message;
            }


            message.SuccessId = 100;
            message.Message = "Done\nCategory wad Added.";
            return message;
        }

        public IList<Category> GetCategories()
        {
            IList<Category> categories = _db.Categories.ToList();
            return categories;
        }

        private MessageData ValidateCategory(CreateCategoryVm categoryVm)
        {
            var message = new MessageData();

            if(categoryVm == null)
            {
                message.ErrorId = -50;
                message.Message = "Data have problem";

                return message;
            }
            else if((categoryVm.Title == null ) || (categoryVm.Title.Length < 2))
            {
                message.ErrorId = -100;
                message.Message = "Title is to short";

                return message;
            }

            var result = IsWasAddedCategory(categoryVm.Title);
            if (result)
            {
                message.ErrorId = -300;
                message.Message = "this Category Was added before";

                return message;
            }

            message.SuccessId = 100; 
            message.Message = "Done";
            return message;
        }

        private bool IsWasAddedCategory(string title)
        {
            return _db.Categories.Any(c => c.Title == title);
        }

        private bool IsAddCategory(int catId)
        {
            return _db.Categories.Any(u => u.MyCategoryId == catId);
        }



        #endregion

        #region Article

        public MessageData AddArticle(CreateArticleVm articleVm)
        {
            var message = new MessageData();

            message = ValidateCreateArticle(articleVm);
            if(message.ErrorId < 0 ) { return message; }

            // time to save data

            var fileNameNew = CreateRandomName.CreateName();

            var article = new Article()
            {
                MyArticleId = CreateRandomId.CreateId(),
                Title = articleVm.Title.ToLower(),
                ShortDescriptions = articleVm.ShortDiscription,
                Descriptions = articleVm.Discrioption,
                ImageName = fileNameNew,
                CategoryId = articleVm.CategoryID,
            };

            var result = SaveImageArticle(articleVm.ImageFile , fileNameNew);
            if (!result)
            {
                message.ErrorId = -300;
                message.Message = "We Can Save Image!";

                return message;
            }

            // time to save our models
            _db.Articles.Add(article);
            Save();

            // teas  was add or not
            result = IsArticleAdded(article.MyArticleId);
            if (!result)
            {
                message.ErrorId = -510;
                message.Message = "somthings wrong,we cant\n" +
                    " add Article now";

                return message;
            }


            message.SuccessId = 450;
            message.Message = "Aericle Was Added\n" +
                "Stay Greeen";
            return message;
        }
        private MessageData ValidateCreateArticle(CreateArticleVm articleVm)
        {


            var message = new MessageData();

            if (articleVm == null)
            {
                message.ErrorId = -100;
                message.Message = "Data is not find.";
                return message;
            }
            else if ((articleVm.Title.Length < 2) ||(articleVm.Title == null) || (articleVm.Title.Length > 20))
            {
                message.ErrorId = -110;
                message.Message = "Title is not correct structures";

                return message;
            }

            var regex = new Regex("^[a-zA-Z]");
            if (!regex.IsMatch(articleVm.Title))
            {
                message.ErrorId = -130;
                message.Message = "Title has not number!";

                return message;
            }

            articleVm.Title = articleVm.Title.Replace(" ", "");


            if((articleVm.ShortDiscription == null) || (articleVm.ShortDiscription.Length < 15) || 
                (articleVm.ShortDiscription.Length > 50 ))
            {
                message.ErrorId = -140;
                message.Message = "Short description has not correct\n" +
                    " structures ";

                return message;
            } 
            else if((articleVm.Discrioption == null ) || (articleVm.Discrioption.Length < 30 ))
            {
                message.ErrorId = -160;
                message.Message = "Descriptions has incorrect structures ";

                return message;
            }
            else if(articleVm.Discrioption == null)
            {
                message.ErrorId = -175;
                message.Message = "Fill Iamge";

                return message;
            }
            else if(articleVm.ImageFile == null || articleVm.ImageFile.Length > 2097152)
            {
                message.ErrorId = -160;
                message.Message = "thats large image";

                return message;
            }
            else if(!ValidateImageFormat(articleVm.ImageFile.ContentType))
            {
                message.ErrorId = -300;
                message.Message = "incorect image format jpg or png need";
                return message;
            }
            else if (!IsCategoryExist(articleVm.CategoryID))
            {
                message.ErrorId = -210;
                message.Message = "This categor is not exist";

                return message;
            }
            else if(articleVm.CategoryID.GetType() != typeof(int))
            {
                message.ErrorId = -240;
                message.Message = "we have some problem with cat id ";

                return message;
            }

            message.SuccessId = 100;
            message.Message = "All is done";
            return message;
        }        
        private bool ValidateImageFormat(string fileContentType)
        {

            if(fileContentType == "image/png") { return true; }
            else if(fileContentType == "image/jpg") { return true; }

            return false;
        }
        private bool IsCategoryExist( int catId)
        {
            return _db.Categories.Any(u => u.CategoryId == catId);
        }
        private bool SaveImageArticle( IFormFile articleImg , string fileNewName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/articleData/image");
            if (!Directory.Exists(path)) { return false; }

            var fileInfp = new FileInfo(articleImg.FileName);
            fileNewName += fileInfp.Extension;
            string fileNameWithPath = Path.Combine(path, fileNewName);
            using(var stram = new FileStream(fileNameWithPath, FileMode.Create))
            {
                articleImg.CopyTo(stram);
            }

            return true;
        }
        private bool IsArticleAdded(int myArticleId)
        {
            return _db.Articles.Any(u => u.MyArticleId == myArticleId);
        }

        #endregion

        #region CRUD

        private void Save()
        {
            _db.SaveChanges();
        }

        #endregion
    }
}
