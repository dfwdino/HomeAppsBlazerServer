using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Auto
{

    [Table("Cars", Schema = "Auto")]
    public class Car : BaseEntity
    {
        [Key]
        public int CarID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Make must be between 2 and 50 characters", MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<MileageEntry>? MileageEntries { get; set; }
    }
}
