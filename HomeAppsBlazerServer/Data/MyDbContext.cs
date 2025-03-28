using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Chore;
using HomeAppsBlazerServer.Models.Shopping;
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

        #region Chores

        public DbSet<KidsNameModel> KidsName { get; set; }
        public DbSet<ChoresModel> KidsChores { get; set; }
        public DbSet<LocationModel> KidsLocation { get; set; }

        public DbSet<ChoreListItemsModel> ChoreListItem { get; set; }


        #endregion


    }
}
