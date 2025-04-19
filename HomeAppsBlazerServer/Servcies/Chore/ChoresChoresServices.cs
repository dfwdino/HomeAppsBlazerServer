using HomeAppsBlazerServer.Components.Extensions;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models.Chore;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class ChoresChoresServices : IChoresChoresServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<IChoresChoresServices> _logger;

        public ChoresChoresServices(MyDbContext myDbContext, ILogger<IChoresChoresServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public Task<List<ChoresModel>> GetChores()
        {
            _logger.LogInformation("Getting Chores.");

            List<ChoresModel> Chores = myDbContext.KidsChores.Where(mm => mm.IsDeleted == false).OrderBy(mm => mm.ChoreName).ToList();

            _logger.LogInformation("Got {NumberOfChors} Chores.", Chores.Count);

            return Task.FromResult(Chores);
        }

        public async void AddChore(ChoresModel ChoresNameModel)
        {
            if (ChoresNameModel is null)
            {
                return;
            }
            else if (ChoresNameModel.ChoreName == null)
            {
                return;
            }

            ChoresNameModel.ChoreName = ChoresNameModel.ChoreName.ToTileCase();
            myDbContext.KidsChores.Add(ChoresNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<ChoresModel> GetChore(int id)
        {
            return myDbContext.KidsChores.Where(mm => mm.ChoreID == id).FirstOrDefault();
        }

        public async Task<List<ChoresModel>> GetChoresFilter(string filter)
        {
            return myDbContext.KidsChores.Where(mm => mm.ChoreName.Contains(filter)).ToList();
        }

        public async void UpdateChore(ChoresModel ChoresNameModel)
        {
            myDbContext.KidsChores.Update(ChoresNameModel);
            myDbContext.SaveChanges();
        }

        public async void DeleteChore(ChoresModel ChoresNameModel)
        {

            myDbContext.KidsChores.Remove(ChoresNameModel);
        }


        #region Amount

        public async void AddChoreAmount(ChoreAmountDetailModel ChoreAmountModel)
        {

            myDbContext.ChoreAmount.Add(new ChoreAmountModel { Amount = ChoreAmountModel.Amount, ChoreID = ChoreAmountModel.ChoreID });
            myDbContext.SaveChanges();

        }


        public async Task<List<ChoreAmountDetailModel>> GetChoreAmounts()
        {

            var choreAmount = (from ca in myDbContext.ChoreAmount
                               join c in myDbContext.KidsChores on ca.ChoreID equals c.ChoreID
                               orderby c.ChoreName
                               where ca.IsDeleted == false
                               select new ChoreAmountDetailModel
                               {
                                   ID = ca.ID,
                                   ChoreID = ca.ChoreID,
                                   Amount = ca.Amount,
                                   IsDeleted = ca.IsDeleted,
                                   ChoreName = c.ChoreName
                               });

            return choreAmount.ToList();

        }

        public async Task<ChoreAmountDetailModel> GetChoreAmount(int id)
        {

            var choreAmount = (from ca in myDbContext.ChoreAmount
                               join kc in myDbContext.KidsChores on ca.ChoreID equals kc.ChoreID
                               where ca.ID == id
                               select new ChoreAmountDetailModel
                               {
                                   ID = ca.ID,
                                   ChoreID = ca.ChoreID,
                                   Amount = ca.Amount,
                                   IsDeleted = ca.IsDeleted,
                                   ChoreName = kc.ChoreName
                               });


            return choreAmount.FirstOrDefault();

        }

        public void UpdateChoreAmount(ChoreAmountDetailModel choreAmountDetailModel)
        {
            var choreAmount = myDbContext.ChoreAmount.Where(mm => mm.ID == choreAmountDetailModel.ID).FirstOrDefault();

            if (choreAmountDetailModel.IsDeleted == true)
            {
                choreAmount.IsDeleted = true;
            }
            else
            {

                ChoreAmountModel newchore = new();
                newchore.Amount = choreAmountDetailModel.Amount;
                newchore.ChoreID = choreAmount.ChoreID;

                myDbContext.ChoreAmount.Add(newchore);
            }


            myDbContext.SaveChanges();
        }


        #endregion End Amount


    }
}
