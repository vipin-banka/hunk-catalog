using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Model;
using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Entity
{
    public class EntityRelationship : IEntity
    {
        public string Id { get; set; }

        [Relationships]
        public IList<RelationshipDetail> RelationshipDetails { get; set; }
    }
}