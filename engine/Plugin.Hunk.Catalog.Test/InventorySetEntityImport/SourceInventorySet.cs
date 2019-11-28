using Plugin.Hunk.Catalog.Abstractions;

namespace Plugin.Hunk.Catalog.Test.InventorySetEntityImport
{
    public class SourceInventorySet : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}