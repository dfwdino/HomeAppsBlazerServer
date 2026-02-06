using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices
    {
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
    }
}
