using HomeAppsBlazerServer.Models.Shopping.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Shopping
{
    [Table("ShoppingStores", Schema = "Shopping")]
    public class ShoppingStore : IShoppingStore
    {

        public int ShoppingStoreID { get; set; }

        [Required(ErrorMessage = "Store Name is required")]
        public required string StoreName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}
