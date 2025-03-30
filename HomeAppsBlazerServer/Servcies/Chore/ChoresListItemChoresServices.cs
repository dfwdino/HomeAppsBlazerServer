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

        public List<ChoreListItemsModel> GetChoreList()
        {
            return myDbContext.ChoreListItem.ToList();
        }



        public async void UpdateChoreItem(ChoreListItemsModel ChoresNameModel)
        {
            myDbContext.ChoreListItem.Update(ChoresNameModel);
            await myDbContext.SaveChangesAsync();

        }

       

        public void DeleteChore(ChoreListItemsModel ChoresNameModel)
        {
            ChoresNameModel.IsDeleted = true;
            myDbContext.ChoreListItem.Update(ChoresNameModel);
        }

    }
}
