using HomeAppsBlazerServer.Servcies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models
{
    [Table("ShoppingStores", Schema = "Shopping")]
    public class ShoppingStore
    {
        public ShoppingStore() {
         
        }

        public int ShoppingStoreID { get; set; }
        public required string StoreName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
