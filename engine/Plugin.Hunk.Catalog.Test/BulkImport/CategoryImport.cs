using Plugin.Hunk.Catalog.Test.CategoryEntityImport;
using Sitecore.Commerce.Core;
using System;
using Plugin.Hunk.Catalog.BulkImport;

namespace Plugin.Hunk.Catalog.Test.BulkImport
{
    public class CategoryImport : BaseJsonFileBulkImporter<SourceCategory>
    {
        public CategoryImport(IServiceProvider serviceProvider, 
            CommerceContext commerceContext) 
            : base(serviceProvider, commerceContext)
        {
        }
    }
}