using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Auto;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies.Auto
{
    public class GasTypeService
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<GasStationService> _logger;

        public GasTypeService(MyDbContext context, ILogger<GasStationService> logger)
        {
            myDbContext = context;
            _logger = logger;
        }
        public List<GasType> GetAllAsync()
        {
            return myDbContext.GasTypes
                 .ToList();
        }

        public async Task AddAsync(GasType entry)
        {
            myDbContext.GasTypes.Add(entry);
            await myDbContext.SaveChangesAsync();
        }

        public void EditAsync(GasType entry) { 
        
        
        }
    }
}
