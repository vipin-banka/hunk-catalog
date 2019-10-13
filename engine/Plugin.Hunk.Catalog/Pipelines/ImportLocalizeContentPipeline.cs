using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class ImportLocalizeContentPipeline : CommercePipeline<ImportLocalizeContentArgument, ImportLocalizeContentArgument>, IImportLocalizeContentPipeline
    {
        public ImportLocalizeContentPipeline(IPipelineConfiguration<IImportLocalizeContentPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}