using HomeAppsBlazerServer.Models.Shopping.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Shopping
{
    [Table("PriceHistory", Schema = "Shopping")]
    public class PriceHistory : IPriceHistory
    {
        [Key]
        public int PriceHistoryID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        public DateTime PriceDate { get; set; }

        public int ItemID { get; set; }

        [ForeignKey(nameof(Store))]
        public int? StoreID { get; set; }

        public virtual ShoppingStore? Store { get; set; }

        public virtual ShoppingItem? Item { get; set; }
    }

}
