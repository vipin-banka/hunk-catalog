using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Entity;
using Plugin.Hunk.Catalog.Metadata;
using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Test.CustomEntityImport
{
    public class SourceCustomEntity : IEntity
    {
        public SourceCustomEntity()
        {
            Languages = new List<LanguageEntity<SourceCustomEntity>>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceCustomEntity>> Languages { get; set; }
    }
}