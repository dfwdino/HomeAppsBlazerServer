
using HomeAppsBlazerServer.Models.Interface;

namespace HomeAppsBlazerServer.Models
{
    public class ShoppingItemResult : IShoppingItemResult
    {
        public int ShoppingItemListID { get; set; }
        public string ItemName { get; set; }
        public ShoppingStore ShoppingStore { get; set; }

        public int? NumberOfItems { get; set; }
        public decimal? Price { get; set;}
        public int ShoppingListID { get; set; }

        public int ItemID { get; set; } 
    }
}
