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

        public string Price { get; set; } = string.Empty;


    }
}
