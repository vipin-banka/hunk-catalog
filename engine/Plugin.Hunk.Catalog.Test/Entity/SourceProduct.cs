using System.Collections.Generic;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Entity;
using Plugin.Hunk.Catalog.Metadata;

namespace Plugin.Hunk.Catalog.Test.Entity
{
    public class SourceProduct : IEntity
    {
        public SourceProduct()
        {
            Parents = new List<string>();
            Languages = new List<LanguageEntity<SourceProduct>>();
            Variants = new List<SourceProductVariant>();
            Tags = new List<string>();
        }

        [EntityId()]
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Manufacturer { get; set; }

        public string TypeOfGood { get; set; }

        public IList<string> Tags { get; set; }

        [Parents()]
        public IList<string> Parents { get; set; }

        public string Accessories { get; set; }

        public string Dimensions { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceProduct>> Languages { get; set; }

        [Variants()]
        public IList<SourceProductVariant> Variants { get; set; }
    }
}