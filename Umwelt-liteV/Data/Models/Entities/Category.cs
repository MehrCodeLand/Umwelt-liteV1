namespace Umwelt_liteV.Data.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int MyCategoryId { get; set; }
        public string Title { get; set; }

        public IList<Article> Articles { get; set; }
    }
}
