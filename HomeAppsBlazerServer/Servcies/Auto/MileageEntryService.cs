using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Auto;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies.Auto
{
    public class MileageEntryService
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<MileageEntryService> _logger;

        public MileageEntryService(MyDbContext myDbContext, ILogger<MileageEntryService> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }
        public List<MileageEntry> GetAllAsync()
        {
            return myDbContext.MileageEntries
                 .Include(c => c.GasType)
                 .Include(c => c.GasStation)
                 .ToList();
        }
    }
}
