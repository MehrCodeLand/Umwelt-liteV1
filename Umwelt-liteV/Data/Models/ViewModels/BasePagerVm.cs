namespace Umwelt_liteV.Data.Models.ViewModels
{
    public class BasePagerVm
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PageCount { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
