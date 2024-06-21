using HomeAppsBlazerServer.Models.Interface;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models
{
    [Table("ShoppingStores", Schema = "Shopping")]
    public class ShoppingStore : IShoppingStore
    {
        public int ShoppingStoreID { get; set; }
        public required string StoreName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}
