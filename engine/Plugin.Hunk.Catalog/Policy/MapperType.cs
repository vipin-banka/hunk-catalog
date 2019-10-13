namespace Plugin.Hunk.Catalog.Policy
{
    public class MapperType
    {
        public MapperType()
        {
            Key = string.Empty;
            Type = string.Empty;
        }

        public string Key { get; set; }

        public string Type { get; set; }

        public string FullTypeName { get; set; }

        public string LocalizationFullTypeName { get; set; }
    }
}