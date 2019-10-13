using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Model
{
    public class SourceEntityDetail
    {
        public SourceEntityDetail()
        {
            Components = new List<string>();
            VariantComponents = new List<string>();
        }

        public string SerializedEntity { get; set; }

        public IList<string> Components { get; set; }

        public IList<string> VariantComponents { get; set; }

        public string EntityType { get; set; }
    }
}