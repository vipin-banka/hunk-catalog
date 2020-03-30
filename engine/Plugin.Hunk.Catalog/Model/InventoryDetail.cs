using System;

namespace Plugin.Hunk.Catalog.Model
{
    public class InventoryDetail
    {
        public int Quantity { get; set; }

        public decimal InvoiceUnitPrice { get; set; }

        public string InvoiceUnitPriceCurrency { get; set; }

        public bool Preorderable { get; set; }

        public DateTimeOffset? PreorderAvailabilityDate { get; set; }

        public int? PreorderLimit { get; set; }

        public bool Backorderable { get; set; }

        public DateTimeOffset? BackorderAvailabilityDate { get; set; }

        public int? BackorderLimit { get; set; }
    }
}