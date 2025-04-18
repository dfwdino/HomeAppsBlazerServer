﻿using HomeAppsBlazerServer.Models.Shopping.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAppsBlazerServer.Models
{
    [Table("PriceHistory", Schema = "Shopping")]
    public class PriceHistory : IPriceHistory
    {
        [Key]
        public int PriceHistoryID { get; set; }
        public decimal? Amount { get; set; }
        public DateTime PriceDate { get; set; }
        public int ItemID { get; set; }
        public int? StoreID { get; set; }


    }

}
