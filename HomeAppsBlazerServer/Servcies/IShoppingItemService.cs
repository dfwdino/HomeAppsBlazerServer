using HomeAppsBlazerServer.Models;

namespace HomeAppsBlazerServer.Servcies
{
    public interface IShoppingItemService
    {
        Task<List<ShoppingItem>> GetShoppingItemsAsync();
        Task<ShoppingItem> GetShoppingItemByIDAsync(int id);
        Task AddShoppingItemAsyn(ShoppingItem shoppingItem);
        Task RemoveShoppingItem(int id);
        Task UpdateShoppingItem(ShoppingItem shoppingItem, int id);
    }
}
