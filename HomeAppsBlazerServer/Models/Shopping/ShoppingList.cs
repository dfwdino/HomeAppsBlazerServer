// TODO: REVIEW - This class maps to the same table ("ShoppingItemList", Schema = "Shopping")
// as ShoppingItemList.cs but is NOT registered in MyDbContext. It may be an old/duplicate
// class that can be removed. Verify it is not used anywhere before deleting.

using HomeAppsBlazerServer.Models.Shopping.Interface;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Shopping
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
