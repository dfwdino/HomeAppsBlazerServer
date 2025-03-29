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
        Task<List<ChoresModel>> GetChoresFilter(string filter);


        #region Amount

        void AddChoreAmount(ChoreAmountDetailModel choreAmountModel);

        Task<ChoreAmountDetailModel> GetChoreAmount(int id);

        void UpdateChoreAmount(ChoreAmountDetailModel choreAmountDetailModel);



        Task<List<ChoreAmountDetailModel>> GetChoreAmounts();

        #endregion End Amount
    }
}