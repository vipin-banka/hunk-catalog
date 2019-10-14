using System.Collections.Generic;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Model
{
    public class LocalizablePropertyValues
    {
        public LocalizablePropertyValues()
        {
            Parameters = new List<Parameter>();
        }

        public string PropertyName { get; set; }

        public IList<Parameter> Parameters { get; set; }
    }
}