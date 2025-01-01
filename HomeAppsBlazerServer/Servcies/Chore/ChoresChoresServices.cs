using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class ChoresChoresServices : IChoresChoresServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<IChoresChoresServices> _logger;

        public ChoresChoresServices(MyDbContext myDbContext, ILogger<IChoresChoresServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public Task<List<ChoresModel>> GetChores()
        {
            _logger.LogInformation("Getting Chores.");

            List<ChoresModel> Chores = myDbContext.KidsChores.Where(mm => mm.IsDeleted == false).ToList();

            _logger.LogInformation("Got {NumberOfChors} Chores.", Chores.Count);

            return Task.FromResult(Chores);
        }

        public async void AddChore(ChoresModel ChoresNameModel)
        {
            if (ChoresNameModel is null)
            {
                return;
            }
            else if (ChoresNameModel.ChoreName == null)
            {
                return;
            }

            ChoresNameModel.ChoreName = ChoresNameModel.ChoreName.ToTileCase();
            myDbContext.KidsChores.Add(ChoresNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<ChoresModel> GetChore(int id)
        {
            return myDbContext.KidsChores.Where(mm => mm.ChoreID == id).FirstOrDefault();
        }

        public async void UpdateChore(ChoresModel ChoresNameModel)
        {
            myDbContext.KidsChores.Update(ChoresNameModel);
            myDbContext.SaveChanges();
        }

        public async void DeleteChore(ChoresModel ChoresNameModel)
        {

            myDbContext.KidsChores.Remove(ChoresNameModel);
        }

    }
}
