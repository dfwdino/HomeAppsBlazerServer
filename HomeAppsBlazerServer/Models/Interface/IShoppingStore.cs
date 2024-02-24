namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IShoppingStore
    {
        string? Address { get; set; }
        bool IsDeleted { get; set; }
        int ShoppingStoreID { get; set; }
        string StoreName { get; set; }
    }
}