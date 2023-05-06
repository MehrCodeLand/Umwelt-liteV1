using System.ComponentModel.DataAnnotations;

namespace Umwelt_liteV.Data.Models.Entities
{
    public class Link
    {
        [Key]
        public int LinkId { get; set; }
        public int MyLinkId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string LinkUrl { get; set; }

        #region Rel


        #endregion

    }
}
