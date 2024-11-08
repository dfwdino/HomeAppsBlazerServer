using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public interface ILocationChoresServices
    {
        void AddLocationAsync(LocationModel ChoresNameModel);
        void DeleteLocationAsync(LocationModel ChoresNameModel);
        Task<LocationModel> GetLocationAsync(int id);
        Task<List<LocationModel>> GetLocationsAsync();
        void UpdateChoreAsync(LocationModel ChoresNameModel);
    }
}