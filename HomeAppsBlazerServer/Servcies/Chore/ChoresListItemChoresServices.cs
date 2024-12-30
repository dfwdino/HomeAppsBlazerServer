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

        public async Task<List<ChoreListDetailItemsModel>> GetChoreItems(int? kidid = null)
        {
            //List<ChoreListItemsModel> Chores = myDbContext.ChoreListItem.Where(mm => mm.IsDeleted == false).ToList();


            var choreDetails = from chore in myDbContext.ChoreListItem
                               join kid in myDbContext.KidsName
                                    on chore.KidsNameID equals kid.IDKidsName
                               join choreName in myDbContext.KidsChores
                                    on chore.KidsChoreID equals choreName.ChoreID
                               where chore.IsDeleted == false  &&
                                            (kidid == null || chore.KidsNameID == kidid)
                               select new ChoreListDetailItemsModel
                               {
                                   KidsName = kid.KidName,
                                   ChoreName = choreName.ChoreName,

                                   ChoreHistoryID = chore.ChoreHistoryID,
                                   KidsChoreID = chore.KidsChoreID,
                                   KidsNameID = chore.KidsNameID,
                                   StartDate = chore.StartDate,
                                   DateDone = chore.DoneDate
                               };




            return choreDetails.ToList(); //Chores;

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
