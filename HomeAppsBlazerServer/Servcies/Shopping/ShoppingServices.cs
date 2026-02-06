using HomeAppsBlazerServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace HomeAppsBlazerServer.Servcies.Shopping
{
    public partial class ShoppingServices : IShoppingServices
    {
        private readonly MyDbContext myDbContext;
        private readonly ILogger<ShoppingServices> _logger;
        private readonly IMemoryCache _cache;

        private const string ShoppingItemsCacheKey = "ShoppingItemsCache";

        public ShoppingServices(MyDbContext context, ILogger<ShoppingServices> logger, IMemoryCache cache)
        {
            myDbContext = context;
            _logger = logger;
            _cache = cache;
        }

        private void ClearShoppingItemsCache()
        {
            ///TODO: Improve this to only remove relevant cache entries based on filters used.
            _cache.Remove($"{ShoppingItemsCacheKey}_false_");
            _cache.Remove($"{ShoppingItemsCacheKey}_true_");

        }
    }


    //TEST PLAY
    public static class QueryableExtensions
    {

        public static IQueryable<T> TagWithDebugInfo<T>(
                            this IQueryable<T> query,
                            [CallerMemberName] string memberName ="",
                            [CallerFilePath] string filePath = "",
                            [CallerLineNumber] int lineNumber = 0)
        {

#if !DEBUG
            return query;
#endif

            var debugInfo = $"Caller: {memberName}, File: {filePath}, Line: {lineNumber}";

            //can also add other info you wish such as userId etc.

            return query.TagWith(debugInfo); //delegate to built in TagWith


        }



    }
}
