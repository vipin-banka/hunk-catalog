using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Model
{
    public class LocalizableComponentPropertiesValues
    {
        public LocalizableComponentPropertiesValues()
        {
            PropertyValues = new List<LocalizablePropertyValues>();
        }

        public string Path { get; set; }

        public string Id { get; set; }

        public IList<LocalizablePropertyValues> PropertyValues { get; set; }
    }
}