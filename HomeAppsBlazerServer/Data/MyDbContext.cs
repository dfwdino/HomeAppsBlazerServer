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



    }
}
