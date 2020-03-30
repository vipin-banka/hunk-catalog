using Plugin.Hunk.Catalog.Test.CatalogEntityImport;
using Sitecore.Commerce.Core;
using System;
using Plugin.Hunk.Catalog.BulkImport;

namespace Plugin.Hunk.Catalog.Test.BulkImport
{
    public class CatalogImport : BaseJsonFileBulkImporter<SourceCatalog>
    {
        public CatalogImport(IServiceProvider serviceProvider, 
            CommerceContext commerceContext) 
            : base(serviceProvider, commerceContext)
        {
        }
    }
}