namespace HomeAppsBlazerServer.Models
{
    public class ShoppingListItem
    {
        public int ShoppingItemListID { get; set; }
        public int ShoppingItemID { get; set; }
        public int? ShoppingStoreID { get; set; }
        public DateTime? GotItemDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public bool GotItem { get; set; }
        public int? NumberOfItems { get; set; }

        public string Storename { get; set; }
        public string ItemName { get; set; }

        // Navigation properties (if needed)
        // public ShoppingItem ShoppingItem { get; set; }
        // public ShoppingStore ShoppingStore { get; set; }
    }
}
