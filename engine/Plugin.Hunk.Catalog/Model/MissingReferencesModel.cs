using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Model
{
    public class MissingReferencesModel : Sitecore.Commerce.Core.Model
    {
        public IList<string> MissingReferences { get; set; }
    }
}