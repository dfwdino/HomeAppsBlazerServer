using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace HomeAppsBlazerServer.Servcies
{
    public class ShoppingServices : IShoppingServices
    {
        private readonly MyDbContext myDbContext;

        public ShoppingServices(MyDbContext context)
        {
            myDbContext = context;
        }


        #region Items
        public async Task AddShoppingItemAsyn(ShoppingItem shoppingItem)
        {
            shoppingItem.ItemName = shoppingItem.ItemName.ToTileCase();

            myDbContext.ShoppingItems.Add(shoppingItem);
            await myDbContext.SaveChangesAsync();
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

        public async Task<List<ShoppingItem>> GetShoppingItemsAsync(bool showallitems = false)
        {

            var query = myDbContext.ShoppingItems.AsQueryable();

            if (!showallitems)
            {
                IList<int> ListOfAllItemsOnList = new List<int>();

                ListOfAllItemsOnList = myDbContext.ShoppingItemList.Where(mm => mm.GotItem.Equals(false)).Select(m => m.ShoppingItemID).ToList();

                query = query.Where(si => !ListOfAllItemsOnList.Contains(si.ShoppingItemID));
            }


            var result = query
                .GroupJoin(
                    myDbContext.ShoppingStores,
                    si => si.StoreID,
                    ss => ss.ShoppingStoreID,
                    (si, ss) => new { si, ss })
                .SelectMany(
                    temp => temp.ss.DefaultIfEmpty(),
                    (temp, ss) => new ShoppingItem
                    {
                        ItemName = temp.si.ItemName,
                        ElliottDontLike = temp.si.ElliottDontLike,
                        FreddyDontLike = temp.si.FreddyDontLike,
                        KidsDontLike = temp.si.KidsDontLike,
                        StoreID = ss != null ? ss.ShoppingStoreID : null,
                        ShoppingItemID = temp.si.ShoppingItemID
                    })
                .ToList();







            return result;
        }

        public async Task RemoveShoppingItem(int id)
        {
            var shoppingItem = await myDbContext.ShoppingItems.FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));

            if (shoppingItem != null)
            {
                myDbContext.ShoppingItems.Remove(shoppingItem);
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
            await myDbContext.SaveChangesAsync();


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

            List<ShoppingStore> resutls = await storequery.ToListAsync();

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
                        ItemName = shoppingItems.Where(mm => mm.ShoppingItemID == item.ShoppingItemID).Select(mm => mm.ItemName).First(),
                        storename = shoppingStores.Where(mm => mm.ShoppingStoreID == item.ShoppingStoreID).Select(mm => mm.StoreName).FirstOrDefault(),
                        Price = priceHistory.Where(mm => mm.ItemID == item.ShoppingItemID).OrderByDescending(mm => mm.ItemID).Select(mm => mm.Amount).FirstOrDefault(),
                        ShoppingItemListID = item.ShoppingItemListID
                    });
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return results;

        }

        public async Task<ShoppingItemList> GetListItem(int id)
        {
            ShoppingItemList results = new();

            try
            {

                results = myDbContext.ShoppingItemList.Where(mm => mm.ShoppingItemListID == id).FirstOrDefault();

                return results;


            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);

            }

            return results;

        }


        public async void CreateListItem(ShoppingItemList shoppingItemList)
        {

        }

        public async void UpdateListItem(ShoppingItemList shoppingItemList, int id)
        {

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
                         PriceDate = mm.PriceDate


                     })
                     .ToList();

            return itemPrice;
        }






        #endregion End Price History


    }
}
