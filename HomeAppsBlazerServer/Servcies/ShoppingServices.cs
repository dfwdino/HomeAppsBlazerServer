using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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


        public async Task AddItList(int id)
        {
            var NeedItem = await myDbContext.ShoppingItems.FirstOrDefaultAsync(mm => mm.ShoppingItemID.Equals(id));

            ShoppingItemList NewListItem = new ShoppingItemList();

            NewListItem.ShoppingItemID = NeedItem.ShoppingItemID;

            ///Todo
            ///Need to link store to item

            myDbContext.ShoppingItemList.Add(NewListItem);


        }

        public async Task<List<ShoppingItem>> GetShoppingItemsAsync(bool showallitems = false)
        {
            IList<string> ListOfAllItemsOnList = new List<string>();

            if (showallitems.Equals(false))
            {
                ListOfAllItemsOnList = myDbContext.ShoppingItemList.Where(mm => mm.GotItem.Equals(false)).Select(m => m.ShoppingItemID.ToString()).Distinct().ToList();
            }

           
            var results = await myDbContext.ShoppingItems
                        .Where(i => !ListOfAllItemsOnList.Any(e => i.ShoppingItemID.ToString().Contains(e)))
                        .GroupJoin(myDbContext.PriceHistory, // The table to join with
                            item => item.ShoppingItemID, // Key from the first table
                            price => price.ItemID, // Key from the second table
                            (item, prices) => new { Item = item, Prices = prices }) // Result selector
                        .SelectMany(
                            x => x.Prices.OrderByDescending(m => m.PriceDate).Take(1).DefaultIfEmpty(), // This ensures that you get items even if they don't have a price
                            (x, price) => new ShoppingItem
                            {
                                ItemName = x.Item.ItemName,
                                ShoppingItemID = x.Item.ShoppingItemID,
                                Price = price.Amount // Use the null-conditional operator here
                            })
                        .ToListAsync();

           
      
            return results;
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

            var storeid = await myDbContext.ShoppingItemList.Where(m => m.ShoppingItemID==id).OrderByDescending(m => m.GotItemDate).Select(m => m.ShoppingStoreID).LastOrDefaultAsync();

            if (currentshoppingItem != null)
            {

                //if(currentshoppingItem.Price != shoppingItem.Price)
                //{
                //   myDbContext.PriceHistory.Add(new PriceHistory { Amount = shoppingItem.Price, ItemID = currentshoppingItem.ShoppingItemID, PriceDate = DateTime.Now, StoreID = storeid });
                //}


                currentshoppingItem.ItemName = shoppingItem.ItemName;
                currentshoppingItem.IsGlutenFree = shoppingItem.IsGlutenFree;
                currentshoppingItem.FreddyDontLike = shoppingItem.FreddyDontLike;
                currentshoppingItem.KidsDontLike = shoppingItem.KidsDontLike;
                currentshoppingItem.ElliottDontLike = shoppingItem.ElliottDontLike;
                //currentshoppingItem.Price = shoppingItem.Price;


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

        public async Task<List<ShoppingStore>> GetShoppingStoresAsync()
        {
            var resutls = await myDbContext.ShoppingStores.Where(mm => mm.IsDeleted == false).ToListAsync();
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

            if (myDbContext.ShoppingItemList.FirstOrDefault(mm => mm.ShoppingItemID.Equals(id)) != null)
            {
                return;
            }

            ShoppingItemList shoppingItemList = new ShoppingItemList();

            shoppingItemList.ShoppingItemID = id;
            shoppingItemList.NeedDate = DateTime.Now;

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

            try
            {
                //mm = ShoppingItemList in lambda
                results = await myDbContext.ShoppingItemList
               .Where(mm => (mm.GotItem == false || mm.GotItem == null)
                      && (mm.NeedDate == null || mm.NeedDate <= DateTime.Today))
               .Join(myDbContext.ShoppingItems, mm => mm.ShoppingItemID, si => si.ShoppingItemID, (mm, si) => new { mm, si })
               .GroupJoin(myDbContext.ShoppingStores,
                          mmSi => mmSi.mm.ShoppingStoreID,
                          ss => ss.ShoppingStoreID,
                          (mmSi, storeGroup) => new { mmSi, storeGroup })
               .SelectMany(
                   x => x.storeGroup.DefaultIfEmpty(),
                   (x, ss) => new ShoppingItemResult
                   {
                       ShoppingItemListID = x.mmSi.mm.ShoppingItemListID,
                       ItemName = x.mmSi.si.ItemName,
                       ShoppingStore = ss, // Assuming ShoppingStore is of type ShoppingStore
                       NumberOfItems = x.mmSi.mm.NumberOfItems,
                       Price = x.mmSi.mm.Price
                   }
               )
               .ToListAsync();


            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return results;
        }

        public async Task GotItem(int id)
        {

        }

        #endregion End Need List

        #region Price History

        public async Task AddPriceToHistry(int itemid, decimal price, int? storeid)
        {
            myDbContext.PriceHistory.Add(new PriceHistory { ItemID = itemid, StoreID = storeid, Amount = price, PriceDate = DateAndTime.Now });
        }

        public async Task<List<PriceHistory>> GetPriceHisotry(int itemid)
        {


            List<PriceHistory> itemPrice = myDbContext.PriceHistory.Where(mm => mm.ItemID ==  itemid).OrderByDescending(mm => mm.PriceDate).ToList();
                                   ///.Join(myDbContext.ShoppingItems, mm => mm.ItemID, si => si.ShoppingItemID, (mm, si) => new { mm, si })
                                   //.Join(myDbContext.ShoppingStores, mm => mm.mm.StoreID, ss => ss.ShoppingStoreID, (mm, ss) => new { mm, ss })
                                   
            return itemPrice;
        }






        #endregion End Price History


    }
}
