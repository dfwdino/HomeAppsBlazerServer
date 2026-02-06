using HomeAppsBlazerServer.Models.Shopping;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial interface IShoppingServices
    {
        Task<List<ItemBrand>> GetItemBrandsAsync(string filter = "");
        Task<ItemBrand> GetItemBrandseByIDAsync(int id);
        Task AddItemBrandAsyn(ItemBrand shoppingStore);
        Task RemoveItemBrands(int id);
        Task UpdateItemBrand(ItemBrand shoppingStore, int id);
    }
}
