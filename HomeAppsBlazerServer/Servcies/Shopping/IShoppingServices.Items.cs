using HomeAppsBlazerServer.Models.Shopping;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial interface IShoppingServices
    {
        Task<List<ShoppingDetailItem>> GetShoppingItemsAsync(bool showallitems = false, string filter = "");
        Task<List<ShoppingItem>> GetShoppingItemsFilterAsync(string filter = "");

        Task<ShoppingItem> GetShoppingItemByIDAsync(int id);
        Task<ShoppingItem> AddShoppingItemAsyn(ShoppingItem shoppingItem,CancellationToken cancellationToken);
        Task RemoveShoppingItem(int id);
        Task UpdateShoppingItem(ShoppingItem shoppingItem, int id);
    }
}
