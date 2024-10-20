using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using Microsoft.Identity.Client;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class KidsChorseServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<ShoppingServices> _logger;

        public KidsChorseServices(MyDbContext myDbContext, ILogger<ShoppingServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public async Task<List<KidsNameModel>> GetKids()
        {
            List<KidsNameModel> Kids = myDbContext.KidsName.Where(mm => mm.IsDeleted == false).ToList();

            return Kids;
        }
    }
}
