using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public interface IKidsChorseKidsServices
    {
        void AddKid(KidsNameModel kidsNameModel);
        void DeleteKid(KidsNameModel kidsNameModel);
        Task<KidsNameModel> GetKid(int id);
        Task<List<KidsNameModel>> GetKids(string filter = "");
        void UpdateKid(KidsNameModel kidsNameModel);
    }
}