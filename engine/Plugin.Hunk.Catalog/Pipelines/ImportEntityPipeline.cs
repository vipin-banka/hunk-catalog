using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class ImportEntityPipeline : CommercePipeline<ImportEntityArgument, CommerceEntity>, IImportEntityPipeline
    {
        public ImportEntityPipeline(IPipelineConfiguration<IImportEntityPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}