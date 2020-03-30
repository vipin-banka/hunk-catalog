using Plugin.Hunk.Catalog.Entity;
using Sitecore.Commerce.Core;
using System;

namespace Plugin.Hunk.Catalog.BulkImport
{
    public class SellableItemInventoryImport : BaseJsonFileBulkImporter<SellableItemInventory>
    {
        public SellableItemInventoryImport(IServiceProvider serviceProvider, 
            CommerceContext commerceContext) 
            : base(serviceProvider, commerceContext)
        {
        }
    }
}