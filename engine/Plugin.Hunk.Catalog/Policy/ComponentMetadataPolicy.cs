namespace Plugin.Hunk.Catalog.Policy
{
    public class ComponentMetadataPolicy : Sitecore.Commerce.Core.Policy
    {
        public ComponentMetadataPolicy()
        {
            MapperKey = string.Empty;
        }

        public string MapperKey { get; set; }
    }
}