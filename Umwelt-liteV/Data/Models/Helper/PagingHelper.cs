namespace Umwelt_liteV.Data.Models.Helper
{
    public class PagingHelper
    {
        public static BasePager Pager(int pageIndex , int entitiesCount , int take , int howManyPageShow)
        {
            int pageCount = (int)Math.Ceiling(entitiesCount / (double)take);
            int startPage = (pageIndex - howManyPageShow) <= 0 ? 1 : (pageIndex - howManyPageShow);
            int endPage = (pageIndex + howManyPageShow) > pageCount ? pageCount : (pageIndex + howManyPageShow);
            int skip = (pageIndex - 1 ) *take;


            var basePager = new BasePager()
            {
                EndPage = endPage,
                StartPage = startPage,
                PageCount = pageCount,
                Skip = skip,
                Take = take
            };

            return basePager;
        }

        public static IEnumerable<T> Pagination<T>( IEnumerable<T> entities, BasePager pager   )
        {
            return entities.Skip(pager.Skip).Take(pager.Take);  
        }
    }

    public class BasePager
    {
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int PageCount { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

}
