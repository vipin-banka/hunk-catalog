using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Metadata;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SourceBundle : IEntity
    {
        public SourceBundle()
        {
            Tags = new List<Tag>();
            BundleItems = new List<SourceBundleItem>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Manufacturer { get; set; }

        public string TypeOfGood { get; set; }

        public IList<Tag> Tags { get; set; }

        [Parents()]
        public IList<string> Parents { get; set; }

        public string BundleType { get; set; }

        public IList<SourceBundleItem> BundleItems { get; set; }
    }
}