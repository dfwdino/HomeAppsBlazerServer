using HomeAppsBlazerServer.Models;

namespace HomeAppsBlazerServer.Servcies
{
    public interface IShoppingStoreService
    {
        Task<List<ShoppingStore>> GetShoppingStoresAsync();
        Task<ShoppingStore> GetShoppingStoreByIDAsync(int id);
        Task AddShoppingStoreAsyn(ShoppingStore shoppingStore);
        Task RemoveShoppingStore(int id);
        Task UpdateShoppingStore(ShoppingStore shoppingStore,int id);
    }
}
