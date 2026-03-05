using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Shopping
{
    [Table("ItemBrands", Schema = "Shopping")]
    public class ItemBrand
    {
        [Key]
        [Column("ItemBrandID")] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemBrandID { get; set; }

        [Required]
        [Column("BrandName")]
        [StringLength(50)]
        public string BrandName { get; set; } = null!;

        [Column("IsDeleted")]
        public bool? IsDeleted { get; set; }
    }
}
