using Umwelt_liteV.Data.Models.Entities;

namespace Umwelt_liteV.Data.Models.ViewModels
{
    public class CreateArticleVm
    {
        public string Title { get; set; }
        public string ShortDiscription { get; set; }
        public string Discrioption { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageTitle { get; set; }
        public int CategoryID { get; set; }
        public IList<Category> Categories { get; set; }
    }
}
