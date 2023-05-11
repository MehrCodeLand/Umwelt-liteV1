using Umwelt_liteV.Data.Models.ViewModels;

namespace Umwelt_liteV.Data.Models.Helper
{
    public class PagingHelper
    {
        public static BasePagerVm Pager(int pageIndex , int entitiesCount , int take , int howManyPageShow)
        {
            int pageCount = (int)Math.Ceiling(entitiesCount / (double)take);
            int startPage = (pageIndex - howManyPageShow) <= 0 ? 1 : (pageIndex - howManyPageShow);
            int endPage = (pageIndex + howManyPageShow) >= pageCount ? pageCount : (pageIndex + howManyPageShow);
            int skip = ( pageIndex - 1 )* take;


            var basePage = new BasePagerVm()
            {
                StartPage = startPage,
                EndPage = endPage,
                PageCount = pageCount,
                Skip = skip,
                Take = take,
            };

            return basePage;
        }


        public static IEnumerable<T> Pagination<T>(IEnumerable<T> entities , BasePagerVm pager)
        {
            return entities.Skip(pager.Skip).Take(pager.Take);
        }
    }



}
