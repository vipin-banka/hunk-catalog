using System.Collections.Generic;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Entity;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SourceProduct : IEntity
    {
        public SourceProduct()
        {
            Parents = new List<string>();
            Languages = new List<LanguageEntity<SourceProduct>>();
            Variants = new List<SourceProductVariant>();
            Tags = new List<Tag>();
            RelationshipDetails = new List<RelationshipDetail>();
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

        [Relationships()]
        public IList<RelationshipDetail> RelationshipDetails { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceProduct>> Languages { get; set; }

        [Variants()]
        public IList<SourceProductVariant> Variants { get; set; }

        public string Accessories { get; set; }

        public string Dimensions { get; set; }

        [InventorySetName]
        public string InventorySetName { get; set; }
    }
}