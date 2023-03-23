using System;

namespace Store.Models
{
    public class OrderFilter
    {
        public string? Number { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public int? ProviderId { get; set; }
        public string? ItemName { get; set; }
        public string? Unit { get; set; }
        public string? ProviderName { get; set; }
    }
}