namespace HomeAppsBlazerServer.Models
{
    public record MonthlyShoppingReport
    {
        public string Month { get; set; }
        public int Year { get; set; }

        public int TotalItems { get; set; }

        public string Items { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
