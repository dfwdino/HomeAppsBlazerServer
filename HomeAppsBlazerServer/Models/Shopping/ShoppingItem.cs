using HomeAppsBlazerServer.Models.Shopping.Interface;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Shopping
{
    [Table("ShoppingItems", Schema = "Shopping")]
    public class ShoppingItem : IShoppingItem
    {
        public int ShoppingItemID { get; set; }
        public string ItemName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool KidsDontLike { get; set; }
        public bool FreddyDontLike { get; set; }
        public bool ElliottDontLike { get; set; }

        public bool IsOneTimeOnly { get; set; }

        public int? StoreID { get; set; }
        //public int? ItemBrandsID { get; set; }

        public ItemBrand? ItemBrand { get; set; }

        public ShoppingStore? Store { get; set; }

       
    }
}
