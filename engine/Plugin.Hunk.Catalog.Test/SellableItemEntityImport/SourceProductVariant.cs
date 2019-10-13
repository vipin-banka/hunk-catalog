using System.Collections.Generic;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Metadata;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SourceProductVariant : IEntity
    {
        public SourceProductVariant()
        {
            Tags = new List<Tag>();
        }

        [EntityId()]
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Disabled { get; set; }

        public IList<Tag> Tags { get; set; }

        public string Breadth { get; set; }

        public string Length { get; set; }
    }
}