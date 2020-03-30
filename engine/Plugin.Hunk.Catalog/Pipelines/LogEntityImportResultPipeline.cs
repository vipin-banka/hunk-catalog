using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class LogEntityImportResultPipeline : CommercePipeline<LogEntityImportResultArgument, LogEntityImportResultArgument>, ILogEntityImportResultPipeline
    {
        public LogEntityImportResultPipeline(IPipelineConfiguration<ILogEntityImportResultPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}