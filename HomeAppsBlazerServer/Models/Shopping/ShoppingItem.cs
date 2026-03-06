using HomeAppsBlazerServer.Models.Shopping.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Shopping
{
    [Table("ShoppingItems", Schema = "Shopping")]
    public class ShoppingItem : IShoppingItem
    {
        [Key]
        public int ShoppingItemID { get; set; }

        [Required]
        [StringLength(200)]
        public string ItemName { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool KidsDontLike { get; set; }
        public bool FreddyDontLike { get; set; }
        public bool ElliottDontLike { get; set; }

        [ForeignKey(nameof(ItemBrand))]
        public int? ItemBrandID { get; set; }

        public bool IsOneTimeOnly { get; set; }

        [ForeignKey(nameof(Store))]
        public int? StoreID { get; set; }

        public virtual ItemBrand? ItemBrand { get; set; }

        public virtual ShoppingStore? Store { get; set; }

        public virtual ICollection<PriceHistory> PriceHistory { get; set; } = new List<PriceHistory>();
    }
}
