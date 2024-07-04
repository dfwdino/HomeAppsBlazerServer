namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IPriceHistory
    {
        int PriceHistoryID { get; set; }
        decimal? Amount { get; set; }
        DateTime PriceDate { get; set; }
        int ItemID { get; set; }
        int? StoreID { get; set; }

    }

}
