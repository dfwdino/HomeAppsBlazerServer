using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class ChoresListItemChoresServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<ChoresListItemChoresServices> _logger;

        public ChoresListItemChoresServices(MyDbContext myDbContext, ILogger<ChoresListItemChoresServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public void AddChoreItem(ChoreListItemsModel ChoresNameModel)
        {
            if (ChoresNameModel is null)
            {
                return;
            }

            if (ChoresNameModel.DoneDate is null)
            {
                ChoresNameModel.DoneDate = DateAndTime.Now;
            }

            myDbContext.ChoreListItem.Add(ChoresNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<ChoreListItemsModel?> GetChoreItem(int id)
        {
            return await myDbContext.ChoreListItem.Where(mm => mm.ChoreHistoryID == id).FirstOrDefaultAsync();
        }

        public List<ChoreListItemsModel> GetChoreList()
        {
            return myDbContext.ChoreListItem.ToList();
        }

        public List<WeeklyKidTotalViewModel> GetWeeklyChoreReport(DateTime startDate, DateTime endDate)
        {
            // Define the date range for the week
            var weekStart = startDate.Date;
            var weekEnd = endDate.Date.AddDays(1).AddSeconds(-1); // End of the specified end date

            // Join the tables to get all chore details
            var choreDetails = (from choreList in myDbContext.ChoreListItem
                                join chore in myDbContext.KidsChores on choreList.KidsChoreID equals chore.ChoreID
                                join kid in myDbContext.KidsName on choreList.KidsNameID equals kid.IDKidsName
                                join amount in myDbContext.ChoreAmount on chore.ChoreID equals amount.ChoreID
                                where !choreList.IsDeleted && !chore.IsDeleted && !kid.IsDeleted && !amount.IsDeleted
                                && choreList.DoneDate >= weekStart && choreList.DoneDate <= weekEnd
                                select new ChoreDetailViewModel
                                {
                                    ChoreName = chore.ChoreName,
                                    KidName = kid.KidName,
                                    DoneDate = choreList.DoneDate,
                                    ChoreHistoryID = choreList.ChoreHistoryID,
                                    Amount = amount.Amount ?? 0m // Handle null amounts
                                }).ToList();

            // Group by kid and calculate totals
            var kidWeeklyTotals = choreDetails
                .GroupBy(c => c.KidName)
                .Select(g => new WeeklyKidTotalViewModel
                {
                    KidName = g.Key,
                    TotalAmount = g.Sum(c => c.Amount),
                    ChoreDetails = g.ToList()
                })
                .OrderBy(k => k.KidName)
                .ToList();

            return kidWeeklyTotals;
        }

        public async void UpdateChoreItem(ChoreListItemsModel ChoresNameModel)
        {
            myDbContext.ChoreListItem.Update(ChoresNameModel);
            await myDbContext.SaveChangesAsync();

        }



        public void DeleteChoreHistory(int id)
        {
            var KidsChore = myDbContext.ChoreListItem.Where(mm => mm.ChoreHistoryID == id).FirstOrDefault();
            KidsChore.IsDeleted = true;
            myDbContext.ChoreListItem.Update(KidsChore);
            myDbContext.SaveChanges();
        }

    }
}
