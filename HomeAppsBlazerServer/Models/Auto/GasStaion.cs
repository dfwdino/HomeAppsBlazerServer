using System.ComponentModel.DataAnnotations;

namespace HomeAppsBlazerServer.Models.Auto
{
    public class GasStation : BaseEntity
    {
        [Key]
        public int StationID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Brand { get; set; }

        // Navigation property
        public virtual ICollection<MileageEntry>? MileageEntries { get; set; }
    }
}
