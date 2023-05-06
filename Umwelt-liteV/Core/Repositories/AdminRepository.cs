using Umwelt_liteV.Core.Services;
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

        public IList<Category> GetCategories()
        {
            IList<Category> categories = _db.Categories.ToList();
            return categories;
        }

        public MessageData ValidateCategory(CreateCategoryVm categoryVm)
        {
            var message = new MessageData();

            if(categoryVm == null)
            {
                message.ErrorId = -50;
                message.Message = "Data have problem";

                return message;
            }else if((categoryVm.Title == null ) || (categoryVm.Title.Length < 2))
            {
                message.ErrorId = -100;
                message.Message = "Title is to short";

                return message;
            }


            message.SuccessId = 100; 
            message.Message = "Done";
            return message;
        }
    }
}
