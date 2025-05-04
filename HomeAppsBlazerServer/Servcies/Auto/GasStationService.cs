using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Auto;
using HomeAppsBlazerServer.Servcies.Chore;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies.Auto
{
    public class GasStationService
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<IChoresChoresServices> _logger;

        public GasStationService(MyDbContext myDbContext, ILogger<IChoresChoresServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }
        public List<GasStation> GetAllAsync()
        {
            return myDbContext.GasStations
                 .Include(c => c.MileageEntries)
                 .ThenInclude(me => me.GasType)
                 .Include(c => c.MileageEntries)
                 .ThenInclude(me => me.GasStation)
                 .ToList();
        }
    }
}
