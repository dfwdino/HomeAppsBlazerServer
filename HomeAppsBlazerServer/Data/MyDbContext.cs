using HomeAppsBlazerServer.Models;
using Microsoft.EntityFrameworkCore;



namespace HomeAppsBlazerServer.Data
{

    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<ShoppingStore> ShoppingStores { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        public DbSet<ShoppingItemList> ShoppingItemList { get; set; }

        public DbSet<PriceHistory> PriceHistory { get; set; }




    }
}
