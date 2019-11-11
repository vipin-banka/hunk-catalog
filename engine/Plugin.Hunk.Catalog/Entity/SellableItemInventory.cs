using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Model;

namespace Plugin.Hunk.Catalog.Entity
{
    public class SellableItemInventory : IEntity
    {
        public string Id { get; set; }

        public string VariantId { get; set; }

        [InventorySetName]
        public string InventorySetName { get; set; }

        [InventoryDetail]
        public InventoryDetail InventoryDetail { get; set; }
    }
}