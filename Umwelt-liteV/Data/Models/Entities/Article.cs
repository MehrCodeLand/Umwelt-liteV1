using System.ComponentModel.DataAnnotations;

namespace Umwelt_liteV.Data.Models.Entities
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        public int MyArticleId { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string Descriptions { get; set; }
        public string ImageName { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;


        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
