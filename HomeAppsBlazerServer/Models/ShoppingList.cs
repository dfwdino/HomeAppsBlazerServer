

using HomeAppsBlazerServer.Models.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models
{

    [Table("ShoppingItemList", Schema = "Shopping")]
    public class ShoppingList : IShoppingList
    {
        [Key]
        public int ShoppingItemListID { get; set; }
        public int ShoppingItemID { get; set; }
        public int ShoppingStoreID { get; set; }
        public DateTime GotItemDate { get; set; }
        public DateTime NeedDate { get; set; }
        public bool GotItem { get; set; }
    }

}
