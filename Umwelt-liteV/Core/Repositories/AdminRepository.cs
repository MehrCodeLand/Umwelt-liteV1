using Umwelt_liteV.Core.Services;
using Umwelt_liteV.Data.Models.Entities;
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
    }
}
