using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models.Chore
{
    [Table("ChoreList", Schema = "KidsArea")]
    public class ChoreListItemsModel
    {
        [Key]
        public int ChoreHistoryID { get; set; } // PK
        public int KidsChoreID { get; set; } // FK
        public DateTime DateDone { get; set; }
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
        public decimal Amount { get; set; }
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
}
