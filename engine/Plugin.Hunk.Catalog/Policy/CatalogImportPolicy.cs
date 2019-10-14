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
        }

        public bool DeleteOrphanVariant { get; set; }

        public Mappings Mappings { get; set; }

        public EntityVersioningScheme EntityVersioningScheme { get; set; }
    }
}