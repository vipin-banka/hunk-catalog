using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Policy
{
    public class Mappings
    {
        public IList<EntityMapperType> EntityMappings { get; set; }

        public IList<MapperType> EntityComponentMappings { get; set; }

        public IList<MapperType> ItemVariationMappings { get; set; }

        public IList<MapperType> ItemVariationComponentMappings { get; set; }
    }
}