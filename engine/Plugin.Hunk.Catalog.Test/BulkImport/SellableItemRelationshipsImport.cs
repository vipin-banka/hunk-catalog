using Plugin.Hunk.Catalog.Test.SellableItemEntityImport;
using Sitecore.Commerce.Core;
using System;
using Plugin.Hunk.Catalog.BulkImport;
using Plugin.Hunk.Catalog.Entity;

namespace Plugin.Hunk.Catalog.Test.BulkImport
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