using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;

using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices
    {
        public async Task<List<MonthlyShoppingReport>> GetMonthlyShoppingReportAsync()
        {
            var items = await myDbContext.ShoppingItemList
                        .Include(x => x.ShoppingItem)
                            .ThenInclude(x => x.PriceHistory)
                        .OrderBy(x => x.GotItemDate.Value.Year)
                        .ThenBy(x => x.GotItemDate.Value.Month)
                        .Where(x => x.GotItemDate != null)
                        .ToListAsync();


            var monthlySummary = items
                
                .GroupBy(x => new
                {
                    x.GotItemDate!.Value.Year,   
                    x.GotItemDate!.Value.Month
                })
                
                .Select(g => new MonthlyShoppingReport
                {
                    Year = g.Key.Year,
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month),
                    TotalItems = g
                            .GroupBy(m => new
                            {
                                m.ShoppingItemID,
                                Day = m.GotItemDate!.Value.Date    
                            })
                            .Count(),

                    TotalAmount = g
                                .GroupBy(x => new
                                {
                                    x.ShoppingItemID,
                                    Day = x.GotItemDate!.Value.Date    
                                })
                                .Sum(itemGroup =>
                                {
                                    var latestPrice = itemGroup
                                        .SelectMany(x => x.ShoppingItem.PriceHistory)
                                        .OrderByDescending(x => x.PriceDate)
                                        .FirstOrDefault();

                                    return (latestPrice?.Amount ?? 0) * 1;
                                }),
                    Items = string.Join(",", g
                                        .GroupBy(m => new
                                        {
                                            Name = m.ShoppingItem.ItemName.Trim(),
                                            Day = m.GotItemDate!.Value.Date      
                                        })
                                        .GroupBy(d => d.Key.Name)                
                                        .Select(itemGroup => $"{itemGroup.Key} | {itemGroup.Count()}"))
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();

            return monthlySummary;
        }

    }
}
