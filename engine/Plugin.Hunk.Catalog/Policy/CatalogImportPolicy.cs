using System.Collections.Generic;
using Plugin.Hunk.Catalog.Model;

namespace Plugin.Hunk.Catalog.Policy
{
    public class CatalogImportPolicy : Sitecore.Commerce.Core.Policy
    {
        public CatalogImportPolicy()
        {
            DeleteOrphanVariant = true;
            EntityVersioningScheme = EntityVersioningScheme.UpdateLatestUnpublished;
            Mappings = new Mappings();
            VariantComponents = new List<string>();
        }

        public bool DeleteOrphanVariant { get; set; }

        public Mappings Mappings { get; set; }

        public EntityVersioningScheme EntityVersioningScheme { get; set; }

        public IList<string> VariantComponents { get; set; }

        public bool IgnoreIndexUpdates { get; set; }
    }
}