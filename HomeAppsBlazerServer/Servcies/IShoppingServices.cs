using HomeAppsBlazerServer.Models;

namespace HomeAppsBlazerServer.Servcies
{
    public interface IShoppingServices
    {

        #region Store
        Task<List<ShoppingStore>> GetShoppingStoresAsync(string filter = "");
        Task<ShoppingStore> GetShoppingStoreByIDAsync(int id);
        Task AddShoppingStoreAsyn(ShoppingStore shoppingStore);
        Task RemoveShoppingStore(int id);
        Task UpdateShoppingStore(ShoppingStore shoppingStore, int id);

        #endregion End Store

        #region Items
        Task<List<ShoppingDetailItem>> GetShoppingItemsAsync(bool showallitems = false, string filter = "");
        Task<List<ShoppingItem>> GetShoppingItemsFilterAsync(string filter = "");

        Task<ShoppingItem> GetShoppingItemByIDAsync(int id);
        Task AddShoppingItemAsyn(ShoppingItem shoppingItem);
        Task RemoveShoppingItem(int id);
        Task UpdateShoppingItem(ShoppingItem shoppingItem, int id);

        #endregion End Items


        #region Need List
        Task AddItemToList(int id);

        Task GotItem(int id);

        Task<List<ShoppingItemResult>> GetAllNeedItemsAsync();

        Task<ListItemModel> GetListItem(int id);

        void CreateListItem(ShoppingItemList shoppingItemList);

        void UpdateListItem(ShoppingItemList shoppingItemList, int id);


        #endregion End Need List

        #region Price History

        Task AddPriceToHistry(int itemid, decimal price, int? storeid);
        Task<List<PriceHistory>> GetPriceHisotry(int itemid);


        #endregion End Price History

    }
}
