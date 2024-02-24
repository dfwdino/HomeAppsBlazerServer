namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IShoppingItem
    {
        bool ElliottDontLike { get; set; }
        bool FreddyDontLike { get; set; }
        bool IsDeleted { get; set; }
        bool IsGlutenFree { get; set; }
        string ItemName { get; set; }
        bool KidsDontLike { get; set; }
        int ShoppingItemID { get; set; }
    }
}