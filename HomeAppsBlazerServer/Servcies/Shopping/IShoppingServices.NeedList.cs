using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial interface IShoppingServices
    {
        Task AddItemToList(int id, string futureDate);

        Task<bool> GotItem(int id);

        Task<List<ShoppingItemResult>> GetAllNeedItemsAsync();

        Task<ListItemModel> GetListItem(int id);

        void CreateListItem(ShoppingItemList shoppingItemList);

        void UpdateListItem(ShoppingItemList shoppingItemList, int id);
    }
}
