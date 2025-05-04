using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Auto;
using Microsoft.EntityFrameworkCore;

namespace HomeAppsBlazerServer.Servcies.Auto
{
    public class CarService
    {
        private readonly MyDbContext _myDbContext;
        private readonly ILogger<CarService> _logger;

        public CarService(MyDbContext myDbContext, ILogger<CarService> logger)
        {
            _myDbContext = myDbContext;
            _logger = logger;
        }

        public async Task<List<Car>> GetAllAsync() // Change return type to Task<List<Car>>
        {
            _logger.LogInformation("Getting Cars.");

            return await _myDbContext.Cars
                 //.Include(c => c.MileageEntries)
                 //.ThenInclude(me => me.GasType)
                 //.Include(c => c.MileageEntries)
                 //.ThenInclude(me => me.GasStation)
                 .ToListAsync();
        }


        public async Task<Car> GetByIdAsync(int id) // Change return type to Task<List<Car>>
        {
            _logger.LogInformation("Getting Car with ID: {CarId}", id);
            var car = await _myDbContext.Cars
                .FirstOrDefaultAsync(c => c.CarID == id);

            if (car == null)
            {
                _logger.LogWarning("Car with ID: {CarId} not found.", id);
            }
            return car;

        }

        public Task UpdateAsync(Car updatedcar)
        {
            _logger.LogInformation("Updating Car with ID: {CarId}", updatedcar.CarID);

            _myDbContext.Cars.Update(updatedcar);

            return _myDbContext.SaveChangesAsync();
        }

        public Task CreateAsync(Car updatedcar)
        {
            _logger.LogInformation("Updating Car with ID: {CarId}", updatedcar.CarID);

            _myDbContext.Cars.Add(updatedcar);

            return _myDbContext.SaveChangesAsync();
        }


        public async Task<Car> DeleteAsync(int id) // Change return type to Task<List<Car>>
        {
            _logger.LogInformation("Deleting Car with ID: {CarId}", id);
            var car = await _myDbContext.Cars
                .FirstOrDefaultAsync(c => c.CarID == id);
            if (car == null)
            {
                _logger.LogWarning("Car with ID: {CarId} not found.", id);
                return null;
            }
            car.IsDeleted = true; // Mark as deleted
            _myDbContext.Cars.Update(car);
            await _myDbContext.SaveChangesAsync();
            return car;
        }

    }
}
