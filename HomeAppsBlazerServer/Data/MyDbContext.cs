using HomeAppsBlazerServer.Models.Auto;
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

        public DbSet<ItemBrand> ItemBrands { get; set; }

#region Cars

        public DbSet<Car> Cars { get; set; }
        public DbSet<GasType> GasTypes { get; set; }
        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<MileageEntry> MileageEntries { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

     

        }

    }



}
