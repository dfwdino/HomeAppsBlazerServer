using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Auto;

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
            var asdf = myDbContext.MileageEntries
                 //.Include(c => c.GasType)
                 //.Include(c => c.GasStation)
                 .ToList();

            return null;
        }

        public async Task AddAsync(MileageEntry entry)
        {
            myDbContext.MileageEntries.Add(entry);
            await myDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(MileageEntry entry)
        {
            myDbContext.MileageEntries.Update(entry);
            await myDbContext.SaveChangesAsync();
        }

        public List<MileageEntry> GetLast30DaysForCar(int carId)
        {
            var fromDate = DateTime.Now.AddDays(-30);
            return myDbContext.MileageEntries
                //.Include(c => c.GasType)
                //.Include(c => c.GasStation)
                .Where(e => e.CarID == carId && e.EntryDate >= fromDate)
                .ToList();
        }
    }
}
