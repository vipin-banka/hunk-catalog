using System.Collections.Generic;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Entity;
using Plugin.Hunk.Catalog.Metadata;

namespace Plugin.Hunk.Catalog.Test.CatalogEntityImport
{
    public class SourceCatalog : IEntity
    {
        public SourceCatalog()
        {
            Languages = new List<LanguageEntity<SourceCatalog>>();
        }

        [EntityId()]
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceCatalog>> Languages { get; set; }
    }
}