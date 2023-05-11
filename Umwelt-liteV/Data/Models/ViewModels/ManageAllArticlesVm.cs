using Umwelt_liteV.Data.Models.Entities;

namespace Umwelt_liteV.Data.Models.ViewModels
{
    public class ManageAllArticlesVm
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PageCount { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
