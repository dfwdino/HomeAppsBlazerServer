using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class LocationChoresServices : ILocationChoresServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<ILocationChoresServices> _logger;

        public LocationChoresServices(MyDbContext myDbContext, ILogger<ILocationChoresServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public async Task<List<LocationModel>> GetLocationsAsync()
        {
            List<LocationModel> Chores = myDbContext.KidsLocation.Where(mm => mm.IsDeleted == false).ToList();

            return Chores;
        }

        public async void AddLocationAsync(LocationModel location)
        {
            if (location is null)
            {
                return;
            }
            else if (location.PlaceName == null)
            {
                return;
            }

            location.PlaceName = location.PlaceName.ToTileCase();
            myDbContext.KidsLocation.Add(location);

            myDbContext.SaveChanges();
        }

        public async Task<LocationModel> GetLocationAsync(int id)
        {
            return myDbContext.KidsLocation.Where(mm => mm.ChoreLocationId == id).FirstOrDefault();
        }

        public async void UpdateChoreAsync(LocationModel location)
        {
            myDbContext.KidsLocation.Update(location);

            myDbContext.SaveChanges();
        }

        public void DeleteLocationAsync(LocationModel ChoresNameModel)
        {
            ChoresNameModel.IsDeleted = true;

            myDbContext.KidsLocation.Update(ChoresNameModel);

            myDbContext.SaveChanges();
            
        }

    }
}
