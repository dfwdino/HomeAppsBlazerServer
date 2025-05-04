using System.ComponentModel.DataAnnotations;

namespace HomeAppsBlazerServer.Models.Auto
{
    public class GasType : BaseEntity
    {
        [Key]
        public int GasTypeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Octane { get; set; }

        // Navigation property
        public virtual ICollection<MileageEntry>? MileageEntries { get; set; }
    }

}
