using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Policy;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ImportEntityArgument : PipelineArgument
    {
        public ImportEntityArgument(SourceEntityDetail sourceEntityDetail)
        {
            SourceEntityDetail = sourceEntityDetail;
        }

        public SourceEntityDetail SourceEntityDetail { get; set; }

        public object SourceEntity { get; set; }

        public bool IsNew { get; set; }

        public IEntityImportHandler ImportHandler { get; set; }

        public CatalogImportPolicy CatalogImportPolicy { get; set; }
    }
}