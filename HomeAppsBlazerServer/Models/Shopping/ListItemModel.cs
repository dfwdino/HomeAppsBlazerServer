using HomeAppsBlazerServer.Models.Shopping;

namespace HomeAppsBlazerServer.Models
{
    public class ListItemModel
    {
        public ListItemModel()
        {
            ShoppingItemList = new ShoppingItemList();
        }

        public ShoppingItemList? ShoppingItemList { get; set; }

        public string StoreName { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public string Price { get; set; } = string.Empty;

    }
}
