using Umwelt_liteV.Data.Models.Entities;

namespace Umwelt_liteV.Core.Services
{
    public interface IAdminService
    {
        IList<Category> GetCategories();
    }
}
