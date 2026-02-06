using HomeAppsBlazerServer.Models;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial interface IShoppingServices
    {
        Task AddPriceToHistry(int itemid, decimal price, int? storeid);
        Task<List<PriceHistory>> GetPriceHisotry(int itemid);
    }
}
