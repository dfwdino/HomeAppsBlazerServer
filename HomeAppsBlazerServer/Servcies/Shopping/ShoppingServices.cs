using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using HomeAppsBlazerServer.Models.Shopping.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public class ShoppingServices : IShoppingServices
    {
        private readonly MyDbContext myDbContext;
        private readonly ILogger<ShoppingServices> _logger;
        private readonly IMemoryCache _cache;

        private const string ShoppingItemsCacheKey = "ShoppingItemsCache";

        public ShoppingServices(MyDbContext context, ILogger<ShoppingServices> logger, IMemoryCache cache)
        {
            myDbContext = context;
            _logger = logger;
            _cache = cache;
        }

        #region Items
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

        private void ClearShoppingItemsCache()
        {  
            ///TODO: Improve this to only remove relevant cache entries based on filters used.
            _cache.Remove($"{ShoppingItemsCacheKey}_false_");
            _cache.Remove($"{ShoppingItemsCacheKey}_true_");
           
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
        #endregion End Items


        #region Shopping Store
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

        #endregion End Shopping Store


        #region Shopping Brands
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
            var storequery = myDbContext.ItemBrands.Where(mm => mm.IsDeleted == false).AsQueryable();

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

        #endregion End Shopping Store



        #region Need List

        public async Task AddItemToList(int id,string FutureDate = "")
        {

            bool NotOnList = myDbContext.ShoppingItemList.Any(mm => mm.GotItem.Equals(false) && mm.ShoppingItemID == id);


            if (NotOnList)
            {
                await Console.Out.WriteLineAsync($"Found Item {id} in the list. Add adding again.");
                return;
            }

            ShoppingItem shoppingItem = myDbContext.ShoppingItems.Where(mm => mm.ShoppingItemID.Equals(id)).FirstOrDefault();

            ShoppingItemList shoppingItemList = new ShoppingItemList();

            //I think this will error out b/c the object is not coreated yet. 
            shoppingItemList.ShoppingItemID = id;
            //shoppingItemList.NeedDate = DateTime.Now;
            shoppingItemList.ShoppingStoreID = shoppingItem.StoreID;

            shoppingItemList.NeedDate = String.IsNullOrEmpty(FutureDate) ? null : DateTime.Parse(FutureDate);


            myDbContext.ShoppingItemList.Add(shoppingItemList);

            try
            {
                myDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync(ex.Message);
            }

        }


        public async Task<List<ShoppingItemResult>> GetAllNeedItemsAsync()
        {
            List<ShoppingItemResult> results = new();

            List<ShoppingItem> shoppingItems = myDbContext.ShoppingItems.Where(mm => mm.IsDeleted == false).ToList();
            List<ShoppingStore> shoppingStores = myDbContext.ShoppingStores.Where(mm => mm.IsDeleted == false).ToList();
            List<ShoppingItemList> shoppingItemList = myDbContext.ShoppingItemList.Where(mm => mm.GotItem == false).ToList();
            List<PriceHistory> priceHistory = myDbContext.PriceHistory.ToList();
            List<ItemBrand> itemBrands = myDbContext.ItemBrands.Where(mm => mm.IsDeleted == false).ToList();

            try
            {
                foreach (var item in shoppingItemList)
                {
                    results.Add(new ShoppingItemResult
                    {
                        ItemID = item.ShoppingItemID,
                        ItemName = shoppingItems.Where(mm => mm.ShoppingItemID == item.ShoppingItemID).Select(mm => mm.ItemName).FirstOrDefault(),
                        storename = shoppingStores.Where(mm => mm.ShoppingStoreID == item.ShoppingStoreID).FirstOrDefault()?.StoreName,
                        Price = priceHistory.Where(mm => mm.ItemID == item.ShoppingItemID).OrderByDescending(mm => mm.PriceDate).Select(mm => mm.Amount).FirstOrDefault(),
                        NumberOfItems = item.NumberOfItems,
                        ShoppingItemListID = item.ShoppingItemListID,
                        NeedDate = item.NeedDate,
                        Notes = item.Notes,
                        BrandName = itemBrands.Where(mm => mm.ItemBrandsId == shoppingItems.Where(mm => mm.ShoppingItemID == item.ShoppingItemID).FirstOrDefault()?.ItemBrand?.ItemBrandsId).FirstOrDefault()?.BrandName
                    });
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return results.OrderBy(mm => mm.ItemName).ToList();

        }

        public async Task<ListItemModel> GetListItem(int id)
        {
            ListItemModel results = new();

            try
            {

                results.ShoppingItemList = myDbContext.ShoppingItemList.Where(mm => mm.ShoppingItemListID == id).FirstOrDefault();

                if (results.ShoppingItemList != null)
                {
                    results.StoreName = myDbContext.ShoppingStores.Where(mm => mm.ShoppingStoreID.Equals(results.ShoppingItemList.ShoppingStoreID)).FirstOrDefault()?.StoreName;
                    results.ItemName = myDbContext.ShoppingItems.Where(mm => mm.ShoppingItemID.Equals(results.ShoppingItemList.ShoppingItemID)).FirstOrDefault().ItemName;
                    results.BrandName = myDbContext.ItemBrands.Where(mm => mm.ItemBrandsId.Equals(myDbContext.ShoppingItems.Where(ss => ss.ShoppingItemID.Equals(results.ShoppingItemList.ShoppingItemID)).FirstOrDefault().ItemBrand.ItemBrandsId)).FirstOrDefault()?.BrandName;
                    var price = myDbContext.PriceHistory.Where(mm => mm.ItemID.Equals(results.ShoppingItemList.ShoppingItemID)).OrderByDescending(mm => mm.PriceHistoryID).FirstOrDefault()?.Amount;

                    if (string.IsNullOrEmpty(price.ToString()) && price > 0)
                    {
                        results.Price = price.ToString();
                    }

                }


            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);

            }

            return results;

        }


        public async void CreateListItem(ShoppingItemList shoppingItemList)
        {
            //Need to fix the BrandID;
            myDbContext.ShoppingItemList.Add(shoppingItemList);
            myDbContext.SaveChanges();
        }

        public async void UpdateListItem(ShoppingItemList shoppingItemList, int id)
        {
            myDbContext.ShoppingItemList.Update(shoppingItemList);
            myDbContext.SaveChanges();
        }


        public async Task<bool> GotItem(int id)
        {

            ShoppingItemList currentitem = myDbContext.ShoppingItemList.FirstOrDefault(mm => mm.ShoppingItemListID.Equals(id));

            ///TODO: Move this to a Funtion call.
            ShoppingItem shoppingItem = myDbContext.ShoppingItems.AsNoTracking().FirstOrDefault(mm => mm.ShoppingItemID.Equals(currentitem.ShoppingItemID));

            if (shoppingItem.IsOneTimeOnly)
            {
                RemoveShoppingItem(shoppingItem.ShoppingItemID);

            }

            currentitem.GotItem = true;
            currentitem.GotItemDate = DateTime.Now;

            await myDbContext.SaveChangesAsync();

            return true;


        }

        #endregion End Need List

        #region Price History

        public async Task AddPriceToHistry(int itemid, decimal price, int? storeid)
        {
            myDbContext.PriceHistory.Add(new PriceHistory { ItemID = itemid, StoreID = storeid, Amount = price, PriceDate = DateAndTime.Now });

            try
            {
                myDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<decimal> GetLastestPrice(int itemid, int? storeid)
        {
            var LastPriceQuery = myDbContext.PriceHistory.Where(mm => mm.ItemID == itemid);

            if (storeid.HasValue)
            {
                LastPriceQuery = LastPriceQuery.Where(mm => mm.StoreID == storeid);
            }

            var lastPriceRecord = await LastPriceQuery.OrderByDescending(mm => mm.PriceHistoryID).FirstOrDefaultAsync();

            return lastPriceRecord?.Amount ?? 0; // Return 0 if no price record found
        }

        public async Task<List<PriceHistory>> GetPriceHisotry(int itemid)
        {


            List<PriceHistory> itemPrice = myDbContext.PriceHistory
                     .Where(mm => mm.ItemID == itemid)
                     .Include(i => i.Store)
                     .OrderByDescending(mm => mm.PriceDate)
                     .Join(myDbContext.ShoppingItems, mm => mm.ItemID, si => si.ShoppingItemID, (mm, si) => new PriceHistory
                     {
                         Amount = mm.Amount,
                         PriceHistoryID = mm.PriceHistoryID,
                         PriceDate = mm.PriceDate,
                         ItemID = mm.ItemID,
                         StoreID = mm.StoreID,
                         Store = mm.Store


                     })
                     .ToList();

            return itemPrice;
        }






        #endregion End Price History


    }


    //TEST PLAY
    public static class QueryableExtensions
    {

        public static IQueryable<T> TagWithDebugInfo<T>(
                            this IQueryable<T> query,
                            [CallerMemberName] string memberName ="",
                            [CallerFilePath] string filePath = "",
                            [CallerLineNumber] int lineNumber = 0)
        {

#if !DEBUG
            return query;
#endif

            var debugInfo = $"Caller: {memberName}, File: {filePath}, Line: {lineNumber}";

            //can also add other info you wish such as userId etc.

            return query.TagWith(debugInfo); //delegate to built in TagWith

          
        }



    }
}

