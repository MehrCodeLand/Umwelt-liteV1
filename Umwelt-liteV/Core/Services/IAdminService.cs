using Umwelt_liteV.Data.Models.Entities;
using Umwelt_liteV.Data.Models.Structs;
using Umwelt_liteV.Data.Models.ViewModels;

namespace Umwelt_liteV.Core.Services
{
    public interface IAdminService
    {
        IList<Category> GetCategories();
        MessageData AddCategory(CreateCategoryVm createCategoryVm);
        MessageData AddArticle(CreateArticleVm articleVm);
        public BaseFilterVm<ArticleListVm> GetAllUserForAdmin(int pageIndex);
    }
}
