using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public interface IShoppingServices
    {

        #region Store
        Task<List<ShoppingStore>> GetShoppingStoresAsync(string filter = "");
        Task<ShoppingStore> GetShoppingStoreByIDAsync(int id);
        Task AddShoppingStoreAsyn(ShoppingStore shoppingStore);
        Task RemoveShoppingStore(ShoppingStore store);
        Task UpdateShoppingStore(ShoppingStore shoppingStore);

        #endregion End Store


        #region Brands
        Task<List<ItemBrand>> GetItemBrandsAsync(string filter = "");
        Task<ItemBrand> GetItemBrandseByIDAsync(int id);
        Task AddItemBrandAsyn(ItemBrand shoppingStore);
        Task RemoveItemBrands(int id);
        Task UpdateItemBrand(ItemBrand shoppingStore, int id);

        #endregion End Brands

        #region Items
        Task<List<ShoppingDetailItem>> GetShoppingItemsAsync(bool showallitems = false, string filter = "");
        Task<List<ShoppingItem>> GetShoppingItemsFilterAsync(string filter = "");

        Task<ShoppingItem> GetShoppingItemByIDAsync(int id);
        Task<ShoppingItem> AddShoppingItemAsyn(ShoppingItem shoppingItem,CancellationToken cancellationToken);
        Task RemoveShoppingItem(int id);
        Task UpdateShoppingItem(ShoppingItem shoppingItem, int id);

        #endregion End Items


        #region Need List
        Task AddItemToList(int id, string futureDate);

        Task<bool> GotItem(int id);

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
