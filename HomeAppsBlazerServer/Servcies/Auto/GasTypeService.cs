using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Auto;
using HomeAppsBlazerServer.Servcies.Chore;

namespace HomeAppsBlazerServer.Servcies.Auto
{
    public class GasTypeService
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<IChoresChoresServices> _logger;

        public GasTypeService(MyDbContext context, ILogger<IChoresChoresServices> logger)
        {
            myDbContext = context;
            _logger = logger;
        }
        public List<GasType> GetAllAsync()
        {
            return myDbContext.GasTypes
                 //.Include(c => c.MileageEntries)
                 //.ThenInclude(me => me.GasType)
                 //.Include(c => c.MileageEntries)
                 //.ThenInclude(me => me.GasStation)
                 .ToList();
        }
    }
}
