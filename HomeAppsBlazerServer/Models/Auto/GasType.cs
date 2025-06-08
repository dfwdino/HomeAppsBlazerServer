using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Auto
{
    [Table("GasTypes", Schema = "Auto")]
    public class GasType : BaseEntity
    {
        [Key]
        public int GasTypeID { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }

        // Navigation property
        public virtual ICollection<MileageEntry>? MileageEntries { get; set; }
    }

}
