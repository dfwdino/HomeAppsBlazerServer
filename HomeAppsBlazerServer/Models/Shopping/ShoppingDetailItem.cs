namespace HomeAppsBlazerServer.Models.Shopping
{
    public class ShoppingDetailItem
    {
        public int ShoppingItemID { get; set; }
        public string ItemName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool KidsDontLike { get; set; }
        public bool FreddyDontLike { get; set; }
        public bool ElliottDontLike { get; set; }
        public string StoreName { get; set; }
        public decimal? LastPrice { get; set; }  // Nullable in case no price is found

        public DateTime? GotItemDate { get; set; }

        public string ItemBrandName { get; set; }
    }

}
