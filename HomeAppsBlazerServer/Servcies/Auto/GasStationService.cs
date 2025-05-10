using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Auto;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies.Auto
{

    public interface IGasStationService
    {
        Task<GasStation?> GetByIdAsync(int id);
        Task<IEnumerable<GasStation>> GetAllAsync();
        Task CreateAsync(GasStation station);
        Task UpdateAsync(GasStation station);
        Task DeleteAsync(int id);
    }


    public class GasStationService : IGasStationService
    {
        private readonly MyDbContext _context;
        private readonly ILogger<CarService> _logger;


        public GasStationService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<GasStation?> GetByIdAsync(int id)
        {
            return await _context.GasStations.FindAsync(id);
        }

        public async Task<IEnumerable<GasStation>> GetAllAsync()
        {
            return await _context.GasStations
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task CreateAsync(GasStation station)
        {
            _context.GasStations.Add(station);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GasStation station)
        {
            _context.GasStations.Update(station);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var station = await _context.GasStations.FindAsync(id);
            if (station != null)
            {
                _context.GasStations.Remove(station);
                await _context.SaveChangesAsync();
            }
        }
    }
}
