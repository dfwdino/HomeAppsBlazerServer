using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Models.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices
    {
        public async Task AddItemBrandAsyn(ItemBrand itemBrand)
        {
            itemBrand.BrandName = itemBrand.BrandName.ToTileCase();
            itemBrand.IsDeleted = false;
            myDbContext.ItemBrands.Add(itemBrand);
            try
            {
                await myDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }




        }

        public async Task<ItemBrand> GetItemBrandseByIDAsync(int id)
        {
            var itemBrand = await myDbContext.ItemBrands.FirstOrDefaultAsync(mm => mm.ItemBrandsId.Equals(id));
            return itemBrand;
        }

        public async Task<List<ItemBrand>> GetItemBrandsAsync(string filter = "")
        {
            var storequery = myDbContext.ItemBrands.Where(mm => mm.IsDeleted == false).OrderBy(m => m.BrandName).AsQueryable();

            if (!filter.IsNullOrEmpty())
            {
                storequery = storequery.Where(mm => mm.BrandName.Contains(filter));
            }

            List<ItemBrand> resutls = new List<ItemBrand>();

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

        public async Task RemoveItemBrands(int id)
        {
            var itembrand = await myDbContext.ItemBrands.FirstOrDefaultAsync(mm => mm.ItemBrandsId.Equals(id));

            if (itembrand != null)
            {
                //myDbContext.ShoppingStores.Remove(shoppingstore);
                itembrand.IsDeleted = true;

                try
                {
                    await myDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var asdfa = ex.Message;
                }

            }
        }

        public async Task UpdateItemBrand(ItemBrand updateshoppingStore, int id)
        {
            var currentbrand = await myDbContext.ItemBrands.FirstOrDefaultAsync(mm => mm.ItemBrandsId.Equals(id));

            if (currentbrand != null)
            {
                currentbrand.BrandName = updateshoppingStore.BrandName;
                await myDbContext.SaveChangesAsync();

            }
        }
    }
}
