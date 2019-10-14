using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Policy
{
    public class Mappings
    {
        public Mappings()
        {
            EntityMappings = new List<EntityMapperType>();
            EntityComponentMappings = new List<MapperType>();
            ItemVariationMappings = new List<MapperType>();
            ItemVariationComponentMappings = new List<MapperType>();
        }

        public IList<EntityMapperType> EntityMappings { get; set; } 

        public IList<MapperType> EntityComponentMappings { get; set; }

        public IList<MapperType> ItemVariationMappings { get; set; }

        public IList<MapperType> ItemVariationComponentMappings { get; set; }
    }
}