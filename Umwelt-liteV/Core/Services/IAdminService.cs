using Umwelt_liteV.Data.Models.Entities;
using Umwelt_liteV.Data.Models.Helper;
using Umwelt_liteV.Data.Models.Structs;
using Umwelt_liteV.Data.Models.ViewModels;

namespace Umwelt_liteV.Core.Services
{
    public interface IAdminService
    {
        IList<Category> GetCategories();
        MessageData AddCategory(CreateCategoryVm createCategoryVm);
        MessageData AddArticle(CreateArticleVm articleVm);
        BaseFilterVm<ArticleListVm> GetAllUserForAdmin(int pageIndex, string itemSearch);
        ArticleEditVm FindArticleByMyArticleId(int myArticleId);
        MessageData ArticleEdit(ArticleEditVm articleEdit);
        DeleteArticleVm FindArticleByMyId(int id);
        MessageData ArticleDelete(DeleteArticleVm deleteArticle);
    }
}
