using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class LocationChoresServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<LocationChoresServices> _logger;

        public LocationChoresServices(MyDbContext myDbContext, ILogger<LocationChoresServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public async Task<List<LocationModel>> GetLocations()
        {
            List<LocationModel> Chores = myDbContext.KidsLocation.Where(mm => mm.IsDeleted == false).ToList();

            return Chores;
        }

        public async void AddLocation(LocationModel ChoresNameModel)
        {
            if (ChoresNameModel is null)
            {
                return;
            }
            else if (ChoresNameModel.PlaceName == null)
            {
                return;
            }

            //ChoresNameModel.KidName = ChoresNameModel.KidName.ToTileCase();
            myDbContext.KidsLocation.Add(ChoresNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<LocationModel> GetLocation(int id)
        {
            return myDbContext.KidsLocation.Where(mm => mm.ChoreLocationId == id).FirstOrDefault();
        }

        public async void UpdateChore(LocationModel ChoresNameModel)
        {
            myDbContext.KidsLocation.Update(ChoresNameModel);
            myDbContext.SaveChanges();
        }

        public async void DeleteLocation(LocationModel ChoresNameModel)
        {

            myDbContext.KidsLocation.Remove(ChoresNameModel);
        }

    }
}
