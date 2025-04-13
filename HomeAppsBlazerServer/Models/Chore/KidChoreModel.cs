using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Chore
{


    [Table("ChoreAmount", Schema = "KidsArea")]
    public record class ChoreAmountModel
    {
        [Key]
        public int ID { get; set; } // PK
        public int ChoreID { get; set; } // FK  
        public decimal? Amount { get; set; }
        public bool IsDeleted { get; set; }

    }

    public record class ChoreAmountDetailModel
    {
        [Key]
        public int ID { get; set; } // PK
        public int ChoreID { get; set; } // FK  
        public decimal? Amount { get; set; }
        public bool IsDeleted { get; set; }

        public string ChoreName { get; set; }
    }

    [Table("ChoreList", Schema = "KidsArea")]
    public record class ChoreListItemsModel
    {
        [Key]
        public int ChoreHistoryID { get; set; } // PK
        public int KidsChoreID { get; set; } // FK
        public DateTime? DoneDate { get; set; }
        public bool IsDeleted { get; set; }
        public int KidsNameID { get; set; } // FK

    }

    // KidsArea.Chores
    [Table("Chores", Schema = "KidsArea")]
    public class ChoresModel
    {
        [Key]
        public int ChoreID { get; set; } // PK
        public string ChoreName { get; set; }
        public bool IsDeleted { get; set; }
        public int? ChoreLocationID { get; set; } // FK, nullable
    }

    // KidsArea.KidsName
    [Table("KidsName", Schema = "KidsArea")]
    public class KidsNameModel
    {
        [Key]
        public int IDKidsName { get; set; } // PK
        public string KidName { get; set; }
        public bool IsDeleted { get; set; }
    }

    // KidsArea.Location
    [Table("Location", Schema = "KidsArea")]
    public class LocationModel
    {
        [Key]
        public int ChoreLocationId { get; set; } // PK
        public string PlaceName { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class ChoreDetailViewModel
    {
        public string ChoreName { get; set; }
        public string KidName { get; set; }
        public DateTime? DoneDate { get; set; }
        public decimal Amount { get; set; } = 0m;
        public int ChoreHistoryID { get; set; }
    }

    // Summary model for weekly totals per kid
    public class WeeklyKidTotalViewModel
    {
        public string KidName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SavingAmount { get; set; }
        public decimal RetirementAmount { get; set; }
        public List<ChoreDetailViewModel> ChoreDetails { get; set; }
    }

}
