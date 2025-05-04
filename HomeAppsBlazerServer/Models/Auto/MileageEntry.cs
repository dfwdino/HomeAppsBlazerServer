using System.ComponentModel.DataAnnotations;

namespace HomeAppsBlazerServer.Models.Auto
{
    public class MileageEntry : BaseEntity
    {
        [Key]
        public int EntryID { get; set; }
        public int CarID { get; set; }
        public int GasTypeID { get; set; }
        public int StationID { get; set; }
        public DateTime EntryDate { get; set; }
        public decimal Odometer { get; set; }
        public decimal Gallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal TotalCost => Gallons * PricePerGallon;
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Car? Car { get; set; }
        public virtual GasType? GasType { get; set; }
        public virtual GasStation? GasStation { get; set; }
    }
}
