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

            return message;
        }
        private MessageData ValidateCreateArticle(CreateArticleVm articleVm)
        {
            var message = new MessageData();

            return message;
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
