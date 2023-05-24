using System.Text.RegularExpressions;
using Umwelt_liteV.Core.Services;
using Umwelt_liteV.Creator;
using Umwelt_liteV.Data.Models.Entities;
using Umwelt_liteV.Data.Models.Helper;
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
            if (message.ErrorId < 0)
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

            if (categoryVm == null)
            {
                message.ErrorId = -50;
                message.Message = "Data have problem";

                return message;
            }
            else if ((categoryVm.Title == null) || (categoryVm.Title.Length < 2))
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
            if (message.ErrorId < 0) { return message; }

            // time to save data

            var fileNameNew = CreateRandomName.CreateName();

            var article = new Article()
            {
                MyArticleId = CreateRandomId.CreateId(),
                Title = articleVm.Title.ToLower(),
                ShortDescriptions = articleVm.ShortDiscription,
                Descriptions = articleVm.Discrioption,
                ImageName = fileNameNew + articleVm.ImageFile.FileName,
                CategoryId = articleVm.CategoryID,
            };

            var result = SaveImageArticle(articleVm.ImageFile, article.ImageName);
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
            else if ((articleVm.Title.Length < 2) || (articleVm.Title == null) || (articleVm.Title.Length > 20))
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


            if ((articleVm.ShortDiscription == null) || (articleVm.ShortDiscription.Length < 15) ||
                (articleVm.ShortDiscription.Length > 50))
            {
                message.ErrorId = -140;
                message.Message = "Short description has not correct\n" +
                    " structures ";

                return message;
            }
            else if ((articleVm.Discrioption == null) || (articleVm.Discrioption.Length < 30))
            {
                message.ErrorId = -160;
                message.Message = "Descriptions has incorrect structures ";

                return message;
            }
            else if (articleVm.Discrioption == null)
            {
                message.ErrorId = -175;
                message.Message = "Fill Iamge";

                return message;
            }
            else if (articleVm.ImageFile == null || articleVm.ImageFile.Length > 2097152)
            {
                message.ErrorId = -160;
                message.Message = "thats large image";

                return message;
            }
            else if (!ValidateImageFormat(articleVm.ImageFile.ContentType))
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
            else if (articleVm.CategoryID.GetType() != typeof(int))
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

            if (fileContentType == "image/png") { return true; }
            else if (fileContentType == "image/jpg") { return true; }
            else if (fileContentType == "image/jpeg") { return true; }

            return false;
        }
        private bool IsCategoryExist(int catId)
        {
            return _db.Categories.Any(u => u.CategoryId == catId);
        }
        private bool SaveImageArticle(IFormFile articleImg, string fileNewName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/articleData/image");
            if (!Directory.Exists(path)) { return false; }

            var fileInfp = new FileInfo(articleImg.FileName);
            string fileNameWithPath = Path.Combine(path, fileNewName);
            using (var stram = new FileStream(fileNameWithPath, FileMode.Create))
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

        private void UpdateArticle(Article article)
        {
            _db.Articles.Update(article);
            Save();
        }
        private void Save()
        {
            _db.SaveChanges();
        }
        private bool DeleteArticle(Article article)
        {
            var imageName = article.ImageName;
            _db.Articles.Remove(article);
            Save();

            //delete Image
            DeleteOldImage(imageName);

            // is delete ?
            return IsDeleteArticle(article.MyArticleId);
        }
        private bool IsDeleteArticle(int myID)
        {
            return _db.Articles.Any(u => u.MyArticleId == myID);
        }
        #endregion

        #region Show Article And Edit

        // ArticleListVm --> Our Data Article
        // BaseFilterVm --> Pagging and Articles
        public BaseFilterVm<ArticleListVm> GetAllUserForAdmin(int pageIndex, string ItemSearch)
        {
            // get all articles
            var articleList = _db.Articles.OrderByDescending(u => u.Created).ToList();

            var take = 10;
            var howManyPageShow = 2;

            // set all data and info 
            // for pagging
            var pager = PagingHelper.Pager(pageIndex, articleList.Count, take, howManyPageShow);

            // interesting part
            var result = articleList.Select(x => new ArticleListVm
            {
                MyArticleId = x.MyArticleId,
                Title = x.Title,
                ImageName = x.ImageName,
                DateCreated = x.Created.ToString(),
            }).ToList();


            var outPut = PagingHelper.Pagination<ArticleListVm>(result, pager);

            var baseFilterVm = new BaseFilterVm<ArticleListVm>()
            {
                EndPage = pager.EndPage,
                Entities = outPut,
                PageCount = pager.PageCount,
                StartPage = pager.StartPage,
                PageIndex = pageIndex,
            };

            return baseFilterVm;
        }
        public ArticleEditVm FindArticleByMyArticleId(int myArticleId)
        {
            var article = _db.Articles.FirstOrDefault(u => u.MyArticleId == myArticleId);

            if (article == null)
                return null;

            var articleVm = new ArticleEditVm()
            {
                MyArticle = article.MyArticleId,
                Title = article.Title,
                ImageName = article.ImageName,
                Description = article.Descriptions,
                ShortDescription = article.ShortDescriptions,
            };

            return articleVm;

        }
        public MessageData ArticleEdit(ArticleEditVm articleEdit)
        {
            var message = new MessageData();

            message = ValidateEditArticle(articleEdit);
            if(message.ErrorId < 0 ) return message;


            // ALT-IMAGE is change or not 
            if(articleEdit.AltImage != null)
            {

                var myArticle= _db.Articles.FirstOrDefault(u => u.MyArticleId == articleEdit.MyArticle);

                // delete old image 
                DeleteOldImage(myArticle.ImageName);

                var fileNameNew = CreateRandomName.CreateName();

                myArticle.Title = articleEdit.Title;
                myArticle.ShortDescriptions = articleEdit.ShortDescription;
                myArticle.Descriptions = articleEdit.Description;
                myArticle.ImageName = fileNameNew + articleEdit.AltImage.FileName;

                SaveImageArticle(articleEdit.AltImage, myArticle.ImageName);

                // update time 
                UpdateArticle(myArticle);


                message.SuccessId = 100;
                message.Message = "Done";
                return message;
            }


            // time to update data
            var article = _db.Articles.FirstOrDefault(u => u.MyArticleId == articleEdit.MyArticle);

            article.Title = articleEdit.Title;
            article.ShortDescriptions = articleEdit.ShortDescription;
            article.Descriptions = articleEdit.Description;

            // update data
            UpdateArticle(article);

            message.SuccessId = 100;
            message.Message = "Done";
            return message;
        }
        private bool DeleteOldImage(string oldImageFileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/articleData/image");
            if (Directory.Exists(path))
            {
                File.Delete(path + $"/{oldImageFileName}");
            }

            return true;

        }
        private MessageData ValidateEditArticle(ArticleEditVm articleEdit)
        {
            var message = new MessageData();

            if (articleEdit == null)
            {
                message.ErrorId = -100;
                message.Message = "The Data Is Not Found";

                return message;
            }
            else if (articleEdit.Description == null || articleEdit.Description.Length < 15)
            {
                message.ErrorId = -200;
                message.Message = "The descriptions is to short";

                return message;
            }
            else if (articleEdit.ShortDescription == null || articleEdit.ShortDescription.Length < 10)
            {
                message.ErrorId = -140;
                message.Message = "The Short descriptions is to short";

                return message;
            }
            else if(articleEdit.Title.Length < 3 || articleEdit.Title.Length > 15)
            {
                message.ErrorId = -310;
                message.Message = "title is to short or to\n long";
                return message;
            }

            var regex = new Regex("^[a-zA-Z]");
            if (!regex.IsMatch(articleEdit.Title))
            {
                message.ErrorId = -300;
                message.Message = "Title has incorrect format";

                return message;
            }
            if(articleEdit.AltImage != null)
            {
                if(articleEdit.AltImage.Length > 2097152)
                {
                    message.ErrorId = -400;
                    message.Message = "The alt image is to large";

                    return message;
                }
            }



            message.SuccessId = 100;
            message.Message = "Done";
            return message;
        }

        public DeleteArticleVm FindArticleByMyId(int id)
        {
            var article = _db.Articles.FirstOrDefault(u => u.MyArticleId == id);
            if (article == null)
                return null;

            var deleteArticle = new DeleteArticleVm()
            {
                Title = article.Title,
                Description = article.Descriptions,
                ImageName = article.ImageName,
                MyArticleId = article.MyArticleId,
            };

            return deleteArticle;

        }

        public MessageData ArticleDelete(DeleteArticleVm deleteArticle)
        {
            var message = ValidationDeleteArticle(deleteArticle);

            if (message.ErrorId < 0)
                return message;

            // time to delete Article
            var article = _db.Articles.FirstOrDefault(u => u.MyArticleId == deleteArticle.MyArticleId);

            var result = DeleteArticle(article);
            if(result == false)
            {
                message.ErrorId = -200;
                message.Message = "We cant delete Article Now";

                return message;
            }

            message.Message = "Done";
            message.SuccessId = 100;

            return message;
        }
        private MessageData ValidationDeleteArticle(DeleteArticleVm deleteArticle)
        {
            var message = new MessageData();

            if(deleteArticle.MyArticleId == 0 || deleteArticle.MyArticleId == null)
            {
                message.ErrorId = -100;
                message.Message = "Somthings Wrong\n" +
                    "we can delete Article Now!";

                return message;
            }


            message.SuccessId = 100;
            message.Message = "done";
            return message;
        }
    }
    #endregion
}

