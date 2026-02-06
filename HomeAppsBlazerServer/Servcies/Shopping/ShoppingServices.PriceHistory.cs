using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices
    {
        public async Task AddPriceToHistry(int itemid, decimal price, int? storeid)
        {
            myDbContext.PriceHistory.Add(new PriceHistory { ItemID = itemid, StoreID = storeid, Amount = price, PriceDate = DateAndTime.Now });

            try
            {
                myDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<decimal> GetLastestPrice(int itemid, int? storeid)
        {
            var LastPriceQuery = myDbContext.PriceHistory.Where(mm => mm.ItemID == itemid);

            if (storeid.HasValue)
            {
                LastPriceQuery = LastPriceQuery.Where(mm => mm.StoreID == storeid);
            }

            var lastPriceRecord = await LastPriceQuery.OrderByDescending(mm => mm.PriceHistoryID).FirstOrDefaultAsync();

            return lastPriceRecord?.Amount ?? 0; // Return 0 if no price record found
        }

        public async Task<List<PriceHistory>> GetPriceHisotry(int itemid)
        {


            List<PriceHistory> itemPrice = myDbContext.PriceHistory
                     .Where(mm => mm.ItemID == itemid)
                     .Include(i => i.Store)
                     .OrderByDescending(mm => mm.PriceDate)
                     .Join(myDbContext.ShoppingItems, mm => mm.ItemID, si => si.ShoppingItemID, (mm, si) => new PriceHistory
                     {
                         Amount = mm.Amount,
                         PriceHistoryID = mm.PriceHistoryID,
                         PriceDate = mm.PriceDate,
                         ItemID = mm.ItemID,
                         StoreID = mm.StoreID,
                         Store = mm.Store


                     })
                     .ToList();

            return itemPrice;
        }
    }
}
