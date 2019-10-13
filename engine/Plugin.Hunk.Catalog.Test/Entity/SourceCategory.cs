using System.Collections.Generic;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Entity;
using Plugin.Hunk.Catalog.Metadata;

namespace Plugin.Hunk.Catalog.Test.Entity
{
    public class SourceCategory : IEntity
    {
        public SourceCategory()
        {
            Parents = new List<string>();
            Languages = new List<LanguageEntity<SourceCategory>>();
        }

        [EntityId()]
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Parents()]
        public IList<string> Parents { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceCategory>> Languages { get; set; }
    }
}