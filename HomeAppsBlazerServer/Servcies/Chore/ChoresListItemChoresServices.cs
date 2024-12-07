using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;

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

        public async Task<List<ChoreListDetailItemsModel>> GetChoreItems()
        {
            //List<ChoreListItemsModel> Chores = myDbContext.ChoreListItem.Where(mm => mm.IsDeleted == false).ToList();


            var choreDetails = from chore in myDbContext.ChoreListItem
                               join kid in myDbContext.KidsName
                                    on chore.KidsNameID equals kid.IDKidsName
                               join choreName in myDbContext.KidsChores
                                    on chore.KidsChoreID equals choreName.ChoreID
                               where chore.IsDeleted == false
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

        public async void AddChoreItem(ChoreListItemsModel ChoresNameModel)
        {
            if (ChoresNameModel is null)
            {
                return;
            }

            //ChoresNameModel.ChoreName = ChoresNameModel.ChoreName.ToTileCase();
            myDbContext.ChoreListItem.Add(ChoresNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<ChoreListItemsModel> GetChoreItem(int id)
        {
            return myDbContext.ChoreListItem.Where(mm => mm.KidsChoreID == id).FirstOrDefault();
        }

        public async void UpdateChoreItem(ChoreListItemsModel ChoresNameModel)
        {
            myDbContext.ChoreListItem.Update(ChoresNameModel);
            myDbContext.SaveChanges();
        }

        public async void DeleteChore(ChoreListItemsModel ChoresNameModel)
        {
            ChoresNameModel.IsDeleted = true;
            myDbContext.ChoreListItem.Update(ChoresNameModel);
        }

    }
}
