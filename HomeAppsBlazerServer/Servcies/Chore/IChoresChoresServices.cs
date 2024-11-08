using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public interface IChoresChoresServices
    {
        void AddChore(ChoresModel ChoresNameModel);
        void DeleteChore(ChoresModel ChoresNameModel);
        Task<ChoresModel> GetChore(int id);
        Task<List<ChoresModel>> GetChores();
        void UpdateChore(ChoresModel ChoresNameModel);
    }
}