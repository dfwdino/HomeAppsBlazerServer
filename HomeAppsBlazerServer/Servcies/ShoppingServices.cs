using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies
{
    public class ShoppingServices : IShoppingServices
    {
        private readonly MyDbContext myDbContext;

        public ShoppingServices(MyDbContext context) {
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

        public async Task<List<ShoppingItem>> GetShoppingItemsAsync()
        {
            var resutls = await myDbContext.ShoppingItems.ToListAsync();
            return resutls;
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

            if (currentshoppingItem != null)
            {
                currentshoppingItem.ItemName = shoppingItem.ItemName;
                currentshoppingItem.IsGlutenFree = shoppingItem.IsGlutenFree;
                currentshoppingItem.FreddyDontLike = shoppingItem.FreddyDontLike;
                currentshoppingItem.KidsDontLike = shoppingItem.KidsDontLike;
                currentshoppingItem.ElliottDontLike = shoppingItem.ElliottDontLike;

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
            var resutls = await myDbContext.ShoppingStores.ToListAsync();
            return resutls;
        }

        public async Task RemoveShoppingStore(int id)
        {
            var shoppingstore = await myDbContext.ShoppingStores.FirstOrDefaultAsync(mm => mm.ShoppingStoreID.Equals(id));

            if (shoppingstore != null)
            {
                myDbContext.ShoppingStores.Remove(shoppingstore);
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
            ShoppingItemList shoppingItemList = new ShoppingItemList();

            shoppingItemList.ShoppingItemID = id;
            shoppingItemList.NeedDate = DateTime.Now;
            
            myDbContext.ShoppingItemList.Add(shoppingItemList);
            myDbContext.SaveChanges();
        }


        public async Task<List<ShoppingItemList>> GetAllNeedItemsAsync()
        {
            var resutls = await myDbContext.ShoppingItemList.ToListAsync();
            return resutls;
        }

        #endregion End Need List




    }
}
