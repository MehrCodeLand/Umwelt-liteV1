using Microsoft.EntityFrameworkCore;
using Umwelt_liteV.Data.Models.Entities;

namespace Umwelt_liteV.Data.MyContext
{
    public class MyDb : DbContext
    {
        public MyDb(DbContextOptions<MyDb> options ) : base( options )
        {

        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
