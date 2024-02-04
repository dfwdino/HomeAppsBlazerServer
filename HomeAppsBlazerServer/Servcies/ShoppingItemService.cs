using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;

namespace HomeAppsBlazerServer.Servcies
{
    public class ShoppingItemService : IShoppingItemService
    {
        private readonly MyDbContext myDbContext;

        public Task AddShoppingItemAsyn(ShoppingItem shoppingItem)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingItem> GetShoppingItemByIDAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShoppingItem>> GetShoppingItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveShoppingItem(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShoppingItem(ShoppingItem shoppingItem, int id)
        {
            throw new NotImplementedException();
        }
    }
}