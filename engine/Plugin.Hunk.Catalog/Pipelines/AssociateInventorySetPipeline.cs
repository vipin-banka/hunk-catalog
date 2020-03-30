using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class AssociateInventorySetPipeline : CommercePipeline<ImportEntityArgument, ImportEntityArgument>, IAssociateInventorySetPipeline
    {
        public AssociateInventorySetPipeline(IPipelineConfiguration<IAssociateInventorySetPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}