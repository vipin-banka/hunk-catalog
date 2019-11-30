using Plugin.Hunk.Catalog.Test.SellableItemEntityImport;
using Sitecore.Commerce.Core;
using System;
using Plugin.Hunk.Catalog.BulkImport;

namespace Plugin.Hunk.Catalog.Test.BulkImport
{
    public class SellableItemImport : BaseJsonFileBulkImporter<SourceProduct>
    {
        public SellableItemImport(IServiceProvider serviceProvider, 
            CommerceContext commerceContext) 
            : base(serviceProvider, commerceContext)
        {
        }
    }
}