using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class AssociatePriceBookPipeline : CommercePipeline<ImportEntityArgument, ImportEntityArgument>, IAssociatePriceBookPipeline
    {
        public AssociatePriceBookPipeline(IPipelineConfiguration<IAssociatePriceBookPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}