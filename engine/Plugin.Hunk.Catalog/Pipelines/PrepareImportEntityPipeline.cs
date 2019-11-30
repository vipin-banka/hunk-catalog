using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class PrepareImportEntityPipeline : CommercePipeline<ImportEntityArgument, ImportEntityArgument>, IPrepareImportEntityPipeline
    {
        public PrepareImportEntityPipeline(IPipelineConfiguration<IPrepareImportEntityPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}