using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Auto
{
    [Table("GasStations", Schema = "Auto")]
    public class GasStation : BaseEntity
    {
        [Key]
        public int StationID { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<MileageEntry>? MileageEntries { get; set; }
    }
}
