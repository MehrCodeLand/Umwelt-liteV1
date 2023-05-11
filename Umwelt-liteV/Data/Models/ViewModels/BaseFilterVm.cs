namespace Umwelt_liteV.Data.Models.ViewModels
{
    public class BaseFilterVm<T>
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PageCount { get; set; }

        private IEnumerable<T> entities;
        public IEnumerable<T> Entities { get { return entities; } set { entities = value; } }
    }
}
