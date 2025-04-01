using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class KidsChorseKidsServices : IKidsChorseKidsServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<IKidsChorseKidsServices> _logger;

        public KidsChorseKidsServices(MyDbContext myDbContext, ILogger<IKidsChorseKidsServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public async Task<List<KidsNameModel>> GetKids(string filter = "")
        {
            IQueryable<KidsNameModel> Kids = myDbContext.KidsName.Where(mm => mm.IsDeleted == false).AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                Kids = Kids.Where(mm => mm.KidName.Contains(filter));
            }

            return Kids.ToList();
        }

        public async void AddKid(KidsNameModel kidsNameModel)
        {
            if (kidsNameModel is null)
            {
                return;
            }
            else if (kidsNameModel.KidName == null)
            {
                return;
            }

            kidsNameModel.KidName = kidsNameModel.KidName.ToTileCase();
            myDbContext.KidsName.Add(kidsNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<KidsNameModel> GetKid(int id)
        {
            return myDbContext.KidsName.Where(mm => mm.IDKidsName == id).FirstOrDefault();
        }

        public async void UpdateKid(KidsNameModel kidsNameModel)
        {
            myDbContext.KidsName.Update(kidsNameModel);
            myDbContext.SaveChanges();
        }

        public async void DeleteKid(KidsNameModel kidsNameModel)
        {

            myDbContext.KidsName.Remove(kidsNameModel);
        }

    }
}
