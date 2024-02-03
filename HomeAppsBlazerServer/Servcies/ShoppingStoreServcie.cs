using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies
{
    public class ShoppingStoreServcie : IShoppingStoreService
    {
        private readonly MyDbContext myDbContext;

        public ShoppingStoreServcie(MyDbContext context)
        {
            myDbContext = context;
        }

        public async Task AddShoppingStoreAsyn(ShoppingStore shoppingStore)
        {
           myDbContext.ShoppingStores.Add(shoppingStore);
           await myDbContext.SaveChangesAsync();
          

        }

        public async Task<ShoppingStore> GetShoppingStoreByIDAsync(int id)
        {
           var shoppingstore = await myDbContext.ShoppingStores.FirstOrDefaultAsync(mm => mm.ShoppingStoreID.Equals(id));
            return shoppingstore;
        }

        public async Task<List<ShoppingStore>> GetShoppingStoresAsync()
        {
            var resutls = await myDbContext.ShoppingStores.ToListAsync();
            return resutls;
        }

        public async Task RemoveShoppingStore(int id)
        {
            var shoppingstore = await myDbContext.ShoppingStores.FirstOrDefaultAsync(mm => mm.ShoppingStoreID.Equals(id));

            if(shoppingstore != null)
            {
               myDbContext.ShoppingStores.Remove(shoppingstore);
                try
                {
                    await myDbContext.SaveChangesAsync();
                }
                catch (Exception ex) {
                    var asdfa = ex.Message;
                }   
                
            }
        }

        public async Task UpdateShoppingStore(ShoppingStore updateshoppingStore, int id)
        {
            var currentshoppingstore = await myDbContext.ShoppingStores.FirstOrDefaultAsync(mm => mm.ShoppingStoreID.Equals(id));

            if (currentshoppingstore != null)
            {
                currentshoppingstore.StoreName = updateshoppingStore.StoreName;
                currentshoppingstore.Address = updateshoppingStore.Address;
                await myDbContext.SaveChangesAsync();

            }
        }
    }
}
