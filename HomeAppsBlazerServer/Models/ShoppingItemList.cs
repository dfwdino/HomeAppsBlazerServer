﻿using System.ComponentModel.DataAnnotations.Schema;
using HomeAppsBlazerServer.Models.Interface;

namespace HomeAppsBlazerServer.Models
{

    [Table("ShoppingItemList", Schema = "Shopping")]

    public class ShoppingItemList : IShoppingItemList
    {
        public int ShoppingItemListID { get; set; }
        public int ShoppingItemID { get; set; }
        public int? ShoppingStoreID { get; set; }
        public DateTime? GotItemDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public bool GotItem { get; set; }
    }
}

