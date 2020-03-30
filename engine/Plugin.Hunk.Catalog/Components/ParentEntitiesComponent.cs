using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Components
{
    public class ParentEntitiesComponent : Sitecore.Commerce.Core.Component
    {
        public ParentEntitiesComponent()
        {
            this.EntityIds = new List<string>();
        }

        public IList<string> EntityIds { get; set; }
    }
}