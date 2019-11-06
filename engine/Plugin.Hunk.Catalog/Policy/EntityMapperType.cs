using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Policy
{
    public class EntityMapperType : MapperType
    {
        public EntityMapperType()
        {
            Components = new List<string>();
        }

        public string ImportHandlerTypeName { get; set; }

        public IList<string> Components { get; set; }

        public string BulkImporterTypeName { get; set; }
    }
}