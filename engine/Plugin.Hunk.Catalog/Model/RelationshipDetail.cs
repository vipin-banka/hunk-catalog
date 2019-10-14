using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Model
{
    public class RelationshipDetail
    {
        public RelationshipDetail()
        {
            Ids = new List<string>();
        }

        public string Name { get; set; }

        public IList<string> Ids { get; set; }
    }
}