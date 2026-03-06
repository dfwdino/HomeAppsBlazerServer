using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;


namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices
    {



        public async Task AddItemToList(int id, string FutureDate = "")
        {

            bool NotOnList = myDbContext.ShoppingItemList.Any(mm => mm.GotItem.Equals(false) && mm.ShoppingItemID == id);


            if (NotOnList)
            {
                await Console.Out.WriteLineAsync($"Found Item {id} in the list. Add adding again.");
                return;
            }

            ShoppingItem shoppingItem = myDbContext.ShoppingItems.Where(mm => mm.ShoppingItemID.Equals(id)).FirstOrDefault();

            if (shoppingItem == null)
            {
                await Console.Out.WriteLineAsync($"Item {id} not found. Cannot add to list.");
                return;
            }

            ShoppingItemList shoppingItemList = new ShoppingItemList();

            shoppingItemList.ShoppingItemID = id;
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

            List<ShoppingItemList> shoppingItemList = myDbContext.ShoppingItemList.Include(mm => mm.ShoppingItem)
                                                            .ThenInclude(m => m.PriceHistory)
                                                        .Include(mm => mm.ShoppingItem)
                                                            .ThenInclude(m => m.ItemBrand)
                                                        .Include(mm => mm.ShoppingStore).Where(mm => mm.GotItem == false).ToList();

            try
            {
                foreach (var item in shoppingItemList)
                {
                    results.Add(new ShoppingItemResult
                    {
                        ItemID = item.ShoppingItemID,
                        ItemName = item.ShoppingItem.ItemName, 
                        storename = item.ShoppingStore?.StoreName,
                        Price = item.ShoppingItem.PriceHistory.Where(mm => mm.ItemShoppingItemID == item.ShoppingItemID).OrderByDescending(mm => mm.PriceDate).Select(mm => mm.Amount).FirstOrDefault(),
                        NumberOfItems = item.NumberOfItems,
                        ShoppingItemListID = item.ShoppingItemListID,
                        NeedDate = item.NeedDate,
                        Notes = item.Notes,
                        BrandName = item.ShoppingItem.ItemBrand?.BrandName
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

                results.ShoppingItemList = myDbContext.ShoppingItemList.Include(m => m.ShoppingStore).Include(m => m.ShoppingItem).Where(mm => mm.ShoppingItemListID == id).FirstOrDefault();

                if (results.ShoppingItemList != null)
                {
                    var price = myDbContext.PriceHistory.Where(mm => mm.ItemShoppingItemID.Equals(results.ShoppingItemList.ShoppingItemID)).OrderByDescending(mm => mm.PriceHistoryID).FirstOrDefault()?.Amount;

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

        public async Task UpdateNeedList(ShoppingItemResult needitem)
        {

            ShoppingItemList shoppingItemList = myDbContext.ShoppingItemList.Where(mm => mm.ShoppingItemListID == needitem.ShoppingItemListID).FirstOrDefault();
            shoppingItemList.Notes = needitem.Notes;
            shoppingItemList.NeedDate = needitem.NeedDate;
            shoppingItemList.NumberOfItems = needitem.NumberOfItems;


            myDbContext.ShoppingItemList.Update(shoppingItemList);
            myDbContext.SaveChanges();

        }


        public async Task<bool> GotItem(int id)
        {
            ShoppingItemList? currentitem = await GetShoppingListItem(id);


            if (currentitem == null)
            {
                return false;
            }

            currentitem.GotItem = true;
            currentitem.GotItemDate = DateTime.Now;
            currentitem.ShoppingItem.IsDeleted = currentitem.ShoppingItem.IsOneTimeOnly ? true : false;

            await myDbContext.SaveChangesAsync();

            return true;


        }

        private async Task<ShoppingItemList?> GetShoppingListItem(int id)
        {
            return await myDbContext.ShoppingItemList.FirstOrDefaultAsync(mm => mm.ShoppingItemListID.Equals(id));


        }
    }
}
