using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;
using Microsoft.EntityFrameworkCore;

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

        public DateTime GetNextSunday(DateTime dateTime)
        {
            DateTime today = dateTime;

            // Calculate the days until next Sunday
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)dateTime.DayOfWeek + 7) % 7;

            // If today is Sunday, set the next Sunday to 7 days later
            if (daysUntilSunday == 0)
            {
                daysUntilSunday = 7;
            }

            // Get the next Sunday date
            DateTime nextSunday = dateTime.AddDays(daysUntilSunday);

            return nextSunday;

        }


        public async Task<List<ChoreListDetailItemsModel>> GetChoreItemsByKid(int? kidid = null)
        {
            var choreDetails = from chore in myDbContext.ChoreListItem
                               join kid in myDbContext.KidsName
                                    on chore.KidsNameID equals kid.IDKidsName
                               join choreName in myDbContext.KidsChores
                                    on chore.KidsChoreID equals choreName.ChoreID
                               where chore.IsDeleted == false &&
                                            (kidid == null || chore.KidsNameID == kidid)
                               //&& (chore.DoneDate == null && chore.DoneDate.Value <= GetNextSunday(DateTime.Now))

                               select new ChoreListDetailItemsModel
                               {
                                   KidsName = kid.KidName,
                                   ChoreName = choreName.ChoreName,

                                   ChoreHistoryID = chore.ChoreHistoryID,
                                   KidsChoreID = chore.KidsChoreID,
                                   KidsNameID = chore.KidsNameID,
                                   StartDate = chore.StartDate,
                                   DateDone = chore.DoneDate,
                                   Amount = chore.Amount == null ? choreName.Amount : chore.Amount
                               };

            return await choreDetails.ToListAsync();
        }

        public void AddChoreItem(ChoreListItemsModel ChoresNameModel)
        {
            if (ChoresNameModel is null)
            {
                return;
            }

            //ChoresNameModel.ChoreName = ChoresNameModel.ChoreName.ToTileCase();
            myDbContext.ChoreListItem.Add(ChoresNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<ChoreListItemsModel?> GetChoreItem(int id)
        {
            return await myDbContext.ChoreListItem.Where(mm => mm.ChoreHistoryID == id).FirstOrDefaultAsync();
        }

        public async void UpdateChoreItem(ChoreListItemsModel ChoresNameModel)
        {
            myDbContext.ChoreListItem.Update(ChoresNameModel);
            await myDbContext.SaveChangesAsync();

        }

        public async void MakeAsDone(int id)
        {
            var donechore = await myDbContext.ChoreListItem.Where(mm => mm.ChoreHistoryID == id).FirstOrDefaultAsync();

            if (donechore != null)
            {
                donechore.DoneDate = DateTime.Now;
                myDbContext.ChoreListItem.Update(donechore);
                await myDbContext.SaveChangesAsync();
            }
        }

        public void DeleteChore(ChoreListItemsModel ChoresNameModel)
        {
            ChoresNameModel.IsDeleted = true;
            myDbContext.ChoreListItem.Update(ChoresNameModel);
        }

    }
}
