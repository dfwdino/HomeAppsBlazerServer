using HomeAppsBlazerServer.Models.Interface;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models
{

    [Table("ShoppingItemList", Schema = "Shopping")]

    public class ShoppingItemList : IShoppingItemList
    {
        public int ShoppingListItemID { get; set; }

        public IShoppingItem ShoppingItem { get; set; }

        public IShoppingStore ShoppingStore { get; set; }

        public int? ShoppingStoreID { get; set; }
        public DateTime? GotItemDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public bool GotItem { get; set; }

        public int? NumberOfItems { get; set; }
        public decimal? Price { get; set; }

    }
}

