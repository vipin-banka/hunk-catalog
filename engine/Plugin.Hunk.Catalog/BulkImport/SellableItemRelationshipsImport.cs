using Plugin.Hunk.Catalog.Entity;
using Sitecore.Commerce.Core;
using System;

namespace Plugin.Hunk.Catalog.BulkImport
{
    public class SellableItemRelationshipsImport : BaseJsonFileBulkImporter<EntityRelationship>
    {
        public SellableItemRelationshipsImport(IServiceProvider serviceProvider, 
            CommerceContext commerceContext) 
            : base(serviceProvider, commerceContext)
        {
        }
    }
}