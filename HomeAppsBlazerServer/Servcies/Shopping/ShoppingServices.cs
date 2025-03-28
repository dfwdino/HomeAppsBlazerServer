﻿using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public class ShoppingServices : IShoppingServices
    {
        private readonly MyDbContext myDbContext;
        private readonly ILogger<ShoppingServices> _logger;

        public ShoppingServices(MyDbContext context, ILogger<ShoppingServices> logger)
        {
            myDbContext = context;
            _logger = logger;
        }

        #region Items
        public async Task<ShoppingItem> AddShoppingItemAsyn(ShoppingItem shoppingItem)
        {


            bool FoundItem = myDbContext.ShoppingItems.Any(x => x.ItemName == shoppingItem.ItemName);

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

            myDbContext.ShoppingItemList.Add(shoppingListItem);

            myDbContext.SaveChanges();

            return shoppingItem;
        }

        public async Task<ShoppingItem> GetShoppingItemByIDAsync(int id)
        {
            var shoppingitem = await myDbContext.ShoppingItems.FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));
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

            var query = myDbContext.ShoppingItems
                        .Where(mm1 => !myDbContext.ShoppingItemList
                            .Where(mm => mm.GotItem == false)
                            .Select(mm => mm.ShoppingItemID)
                            .Contains(mm1.ShoppingItemID))
                        .AsQueryable();

            if (filter.Length > 0)
            {
                query = query.Where(mm => mm.ItemName.Contains(filter));
            }

            // If filter is empty, return all items
            return query.ToList();
        }

        public async Task<List<ShoppingDetailItem>> GetShoppingItemsAsync(bool showallitems = false, string filter = "")
        {

            var query = myDbContext.ShoppingItems.AsQueryable();

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
                }
                catch (Exception ex)
                {
                    var asdfa = ex.Message;
                }

            }
        }

        public async Task UpdateShoppingItem(ShoppingItem shoppingItem, int id)
        {
            var currentshoppingItem = await myDbContext.ShoppingItems.FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));

            //var asdfasd = myDbContext.ShoppingItems.Where(m => m.ShoppingItemID == id).Select(m => m.StoreID).ToList();

            //int? storeid = await myDbContext.ShoppingItems.Where(m => m.ShoppingItemID == id).Select(m => m.StoreID).FirstOrDefaultAsync();

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

        public async Task RemoveShoppingStore(int id)
        {
            var shoppingstore = await myDbContext.ShoppingStores.FirstOrDefaultAsync(mm => mm.ShoppingStoreID.Equals(id));

            if (shoppingstore != null)
            {
                //myDbContext.ShoppingStores.Remove(shoppingstore);
                shoppingstore.IsDeleted = true;

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

        #endregion End Shopping Store


        #region Need List

        public async Task AddItemToList(int id)
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
            shoppingItemList.NeedDate = DateTime.Now;
            shoppingItemList.ShoppingStoreID = shoppingItem.StoreID;


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
                        ShoppingItemListID = item.ShoppingItemListID
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
            myDbContext.ShoppingItemList.Add(shoppingItemList);
            myDbContext.SaveChanges();
        }

        public async void UpdateListItem(ShoppingItemList shoppingItemList, int id)
        {
            myDbContext.ShoppingItemList.Update(shoppingItemList);
            myDbContext.SaveChanges();
        }


        public async Task GotItem(int id)
        {

            ShoppingItemList currentitem = myDbContext.ShoppingItemList.FirstOrDefault(mm => mm.ShoppingItemListID.Equals(id));

            currentitem.GotItem = true;
            currentitem.GotItemDate = DateTime.Now;

            await myDbContext.SaveChangesAsync();


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
                     .OrderByDescending(mm => mm.PriceDate)
                     .Join(myDbContext.ShoppingItems, mm => mm.ItemID, si => si.ShoppingItemID, (mm, si) => new PriceHistory
                     {
                         Amount = mm.Amount,
                         //ItemName = si.ItemName,
                         PriceHistoryID = mm.PriceHistoryID,
                         PriceDate = mm.PriceDate,
                         ItemID = mm.ItemID


                     })
                     .ToList();

            return itemPrice;
        }






        #endregion End Price History


    }
}
