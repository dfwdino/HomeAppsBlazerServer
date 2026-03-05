using HomeAppsBlazerServer.Models;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial interface IShoppingServices
    {
        Task<List<MonthlyShoppingReport>> GetMonthlyShoppingReportAsync();

    }
}

