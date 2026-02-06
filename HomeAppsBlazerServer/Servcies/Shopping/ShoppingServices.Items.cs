using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices
    {
        public async Task<ShoppingItem?> AddShoppingItemAsyn(ShoppingItem shoppingItem, CancellationToken cancellationToken)
        {


            bool FoundItem = await myDbContext.ShoppingItems.AnyAsync(x => x.ItemName == shoppingItem.ItemName
                                                            && x.ItemBrand.ItemBrandsId == shoppingItem.ItemBrandID, cancellationToken);

            if (FoundItem) { return null; }

            shoppingItem.ItemName = shoppingItem.ItemName.ToTileCase();


            myDbContext.ShoppingItems.Add(shoppingItem);

            try
            {
                await myDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding shopping item: {ItemName}", shoppingItem.ItemName);
                throw; // Re-throw the exception after logging
            }

            ShoppingItemList shoppingListItem = new();

            shoppingListItem.ShoppingItemID = shoppingItem.ShoppingItemID;

            if (shoppingItem.StoreID.HasValue)
            {
                shoppingListItem.ShoppingStoreID = shoppingItem.StoreID;
            }

            await myDbContext.ShoppingItemList.AddAsync(shoppingListItem, cancellationToken);

            myDbContext.SaveChanges();

            // Clear the cache so next fetch gets fresh data
            ClearShoppingItemsCache();

            return shoppingItem;
        }

        public async Task<ShoppingItem> GetShoppingItemByIDAsync(int id)
        {
            var shoppingitem = await myDbContext.ShoppingItems
                 .Include(i => i.ItemBrand)
                 .Include(i => i.Store)
                 .TagWithDebugInfo()
                 .FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));



            return shoppingitem;
        }

        public async Task AddToList(int id)
        {
            ShoppingItem NeedItem = await myDbContext.ShoppingItems.FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));
            ShoppingStore shoppingStore = null;

            if (NeedItem != null && NeedItem.StoreID != null)
            {
                shoppingStore = await myDbContext.ShoppingStores.FirstOrDefaultAsync(mm => mm.ShoppingStoreID.Equals(NeedItem.StoreID));
            }

            ShoppingItemList NewListItem = new ShoppingItemList();

            NewListItem.ShoppingItemID = NeedItem.ShoppingItemID;
            NewListItem.ShoppingStoreID = shoppingStore.ShoppingStoreID;

            myDbContext.ShoppingItemList.Add(NewListItem);
        }

        public async Task<List<ShoppingItem>> GetShoppingItemsFilterAsync(string filter = "")
        {

            IQueryable<ShoppingItem> queryShoppingItem = myDbContext.ShoppingItems
                         .Include(mm => mm.ItemBrand)
                         .Include(mm => mm.Store)
                         .Where(mm => mm.IsDeleted == false)
                         .Where(mm1 => !myDbContext.ShoppingItemList
                             .Where(mm => mm.GotItem == false)
                             .Select(mm => mm.ShoppingItemID)
                             .Contains(mm1.ShoppingItemID))
                         .Select(mm => new ShoppingItem
                         {
                             ShoppingItemID = mm.ShoppingItemID,
                             ItemName = (mm.ItemBrand != null ? string.Concat(mm.ItemBrand.BrandName, " - ") : "") + mm.ItemName,
                             IsGlutenFree = mm.IsGlutenFree,
                             KidsDontLike = mm.KidsDontLike,
                             FreddyDontLike = mm.FreddyDontLike,
                             ElliottDontLike = mm.ElliottDontLike,
                             StoreID = mm.StoreID,
                             //ItemBrandsID = mm.ItemBrandsID,
                             ItemBrand = mm.ItemBrand,
                             Store = mm.Store
                         })
                         .AsQueryable();

            if (filter.Length > 0)
            {
                queryShoppingItem = queryShoppingItem.Where(mm => mm.ItemName.Contains(filter));
            }

            return queryShoppingItem.ToList();
        }

        public async Task<List<ShoppingDetailItem>> GetShoppingItemsAsync(bool showallitems = false, string filter = "")
        {

            string cacheKey = $"{ShoppingItemsCacheKey}_{showallitems}";

            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out List<ShoppingDetailItem> cachedItems))
            {
                return cachedItems;
            }


            IQueryable<ShoppingItem> query = myDbContext.ShoppingItems.AsQueryable();

            if (!showallitems)
            {
                IList<int> ItemsNotGotten = new List<int>();

                ItemsNotGotten = myDbContext.ShoppingItemList.Where(mm => mm.GotItem.Equals(false)).Select(m => m.ShoppingItemID).ToList();

                query = query.Where(si => !ItemsNotGotten.Contains(si.ShoppingItemID));
            }


            List<ShoppingDetailItem> result = query
                        .Where(mm => mm.IsDeleted == false)
                        .GroupJoin(myDbContext.ShoppingStores,
                            si => si.StoreID,
                            ss => ss.ShoppingStoreID,
                            (si, ss) => new { si, ss = ss.DefaultIfEmpty() })
                        .SelectMany(
                            x => x.ss.DefaultIfEmpty(),
                            (x, store) => new { x.si, store })
                        .Select(item => new ShoppingDetailItem
                        {
                            ShoppingItemID = item.si.ShoppingItemID,
                            ItemName = item.si.ItemName,
                            IsDeleted = item.si.IsDeleted,
                            IsGlutenFree = item.si.IsGlutenFree,
                            KidsDontLike = item.si.KidsDontLike,
                            FreddyDontLike = item.si.FreddyDontLike,
                            ElliottDontLike = item.si.ElliottDontLike,
                            ItemBrandName = myDbContext.ItemBrands
                                .Where(ib => ib.ItemBrandsId == item.si.ItemBrand.ItemBrandsId)
                                .Select(ib => ib.BrandName)
                                .FirstOrDefault(),
                            StoreName = item.store != null ? item.store.StoreName : null,
                            LastPrice = myDbContext.PriceHistory
                                .Where(ph => ph.ItemID == item.si.ShoppingItemID)
                                .OrderByDescending(ph => ph.PriceDate)
                                .Select(ph => ph.Amount)
                                .FirstOrDefault(),
                            GotItemDate = myDbContext.ShoppingItemList
                                .Where(sil => sil.ShoppingItemID == item.si.ShoppingItemID)
                                .OrderByDescending(sil => sil.GotItemDate)
                                .Select(sil => sil.GotItemDate)
                                .FirstOrDefault()
                        })
                        .OrderByDescending(mm => mm.GotItemDate)
                        .ThenBy(mm => mm.ItemName)
                        .ToList();



            // Store in cache for 15 minutes
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task RemoveShoppingItem(int id)
        {
            var shoppingItem = await myDbContext.ShoppingItems.FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));

            if (shoppingItem != null)
            {
                shoppingItem.IsDeleted = true;

                //myDbContext.ShoppingItems.Remove(shoppingItem);
                try
                {
                    await myDbContext.SaveChangesAsync();
                    ClearShoppingItemsCache();
                }
                catch (Exception ex)
                {
                    var errormessage = ex.Message;
                }

            }


        }

        public async Task UpdateShoppingItem(ShoppingItem shoppingItem, int id)
        {
            var currentshoppingItem = await myDbContext.ShoppingItems.FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));

            if (currentshoppingItem != null)
            {
                currentshoppingItem.ItemName = shoppingItem.ItemName;
                currentshoppingItem.IsGlutenFree = shoppingItem.IsGlutenFree;
                currentshoppingItem.FreddyDontLike = shoppingItem.FreddyDontLike;
                currentshoppingItem.KidsDontLike = shoppingItem.KidsDontLike;
                currentshoppingItem.ElliottDontLike = shoppingItem.ElliottDontLike;
                currentshoppingItem.StoreID = shoppingItem.StoreID;

                await myDbContext.SaveChangesAsync();

            }
        }
    }
}
