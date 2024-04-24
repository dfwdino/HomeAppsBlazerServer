using HomeAppsBlazerServer.Models;

namespace HomeAppsBlazerServer.Servcies
{
    public interface IShoppingServices
    {

        #region Store
        Task<List<ShoppingStore>> GetShoppingStoresAsync();
        Task<ShoppingStore> GetShoppingStoreByIDAsync(int id);
        Task AddShoppingStoreAsyn(ShoppingStore shoppingStore);
        Task RemoveShoppingStore(int id);
        Task UpdateShoppingStore(ShoppingStore shoppingStore, int id);

        #endregion End Store

        #region Items
        Task<List<ShoppingItem>> GetShoppingItemsAsync(bool showallitems = false);
        Task<ShoppingItem> GetShoppingItemByIDAsync(int id);
        Task AddShoppingItemAsyn(ShoppingItem shoppingItem);
        Task RemoveShoppingItem(int id);
        Task UpdateShoppingItem(ShoppingItem shoppingItem, int id);

        #endregion End Items


        #region Need List
        Task AddItemToList(int id);

        Task<List<ShoppingItemResult>> GetAllNeedItemsAsync();

        #endregion End Need List

        #region Price History

        Task AddPriceToHistry(int itemid, decimal price, int? storeid);
        Task<List<PriceHistory>> GetPriceHisotry(int itemid);

        #endregion End Price History

    }
}
