namespace HomeAppsBlazerServer.Models.Shopping.Interface
{
    public interface IPriceHistory
    {
        int PriceHistoryID { get; set; }
        decimal? Amount { get; set; }
        DateTime PriceDate { get; set; }
        int ItemShoppingItemID { get; set; }
        int? StoreID { get; set; }

    }

}
