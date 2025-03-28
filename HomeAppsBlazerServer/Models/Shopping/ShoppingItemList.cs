using HomeAppsBlazerServer.Models.Shopping.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Shopping
{

    [Table("ShoppingItemList", Schema = "Shopping")]

    public class ShoppingItemList : IShoppingItemList
    {
        [Key]
        public int ShoppingItemListID { get; set; }

        public int ShoppingItemID { get; set; }

        public int? ShoppingStoreID { get; set; }
        public DateTime? GotItemDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public bool GotItem { get; set; }

        public int? NumberOfItems { get; set; }

    }




}

