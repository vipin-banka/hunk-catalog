namespace Plugin.Hunk.Catalog.Policy
{
    public class CatalogImportPolicy : Sitecore.Commerce.Core.Policy
    {
        public CatalogImportPolicy()
        {
            DeleteOrphanVariant = true;
        }

        public bool DeleteOrphanVariant { get; set; }

        public Mappings Mappings { get; set; }
    }
}