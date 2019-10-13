using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Metadata;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SourceProductVariant : IEntity
    {
        [EntityId()]
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Breadth { get; set; }

        public string Length { get; set; }
    }
}