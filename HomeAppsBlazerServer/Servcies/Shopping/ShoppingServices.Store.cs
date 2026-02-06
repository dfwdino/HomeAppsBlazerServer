using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Models.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices
    {
        public async Task AddShoppingStoreAsyn(ShoppingStore shoppingStore)
        {
            shoppingStore.StoreName = shoppingStore.StoreName.ToTileCase();
            myDbContext.ShoppingStores.Add(shoppingStore);
            try
            {
                await myDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }




        }

        public async Task<ShoppingStore> GetShoppingStoreByIDAsync(int id)
        {
            var shoppingstore = await myDbContext.ShoppingStores.FirstOrDefaultAsync(mm => mm.ShoppingStoreID.Equals(id));
            return shoppingstore;
        }

        public async Task<List<ShoppingStore>> GetShoppingStoresAsync(string filter = "")
        {
            var storequery = myDbContext.ShoppingStores.Where(mm => mm.IsDeleted == false).AsQueryable();

            if (!filter.IsNullOrEmpty())
            {
                storequery = storequery.Where(mm => mm.StoreName.Contains(filter));
            }

            List<ShoppingStore> resutls = new List<ShoppingStore>();

            try
            {
                resutls = await storequery.ToListAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return resutls;
        }

        public async Task RemoveShoppingStore(ShoppingStore store)
        {

            if (store != null)
            {
                try
                {
                    myDbContext.ShoppingStores.Update(store);
                    await myDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var asdfa = ex.Message;
                }

            }
        }

        public async Task UpdateShoppingStore(ShoppingStore updateshoppingStore)
        {

            myDbContext.Update(updateshoppingStore);
            await myDbContext.SaveChangesAsync();
        }
    }
}
