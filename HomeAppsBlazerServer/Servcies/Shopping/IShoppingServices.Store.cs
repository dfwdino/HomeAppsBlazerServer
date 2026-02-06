using HomeAppsBlazerServer.Models.Shopping;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial interface IShoppingServices
    {
        Task<List<ShoppingStore>> GetShoppingStoresAsync(string filter = "");
        Task<ShoppingStore> GetShoppingStoreByIDAsync(int id);
        Task AddShoppingStoreAsyn(ShoppingStore shoppingStore);
        Task RemoveShoppingStore(ShoppingStore store);
        Task UpdateShoppingStore(ShoppingStore shoppingStore);
    }
}
