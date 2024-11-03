using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class ChoresChoresServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<ChoresChoresServices> _logger;

        public ChoresChoresServices(MyDbContext myDbContext, ILogger<ChoresChoresServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public async Task<List<ChoresModel>> GetChores()
        {
            List<ChoresModel> Chores = myDbContext.KidsChores.Where(mm => mm.IsDeleted == false).ToList();

            return Chores;
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

            //ChoresNameModel.KidName = ChoresNameModel.KidName.ToTileCase();
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
