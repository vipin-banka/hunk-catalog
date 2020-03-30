using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class AssociatePromotionBookPipeline : CommercePipeline<ImportEntityArgument, ImportEntityArgument>, IAssociatePromotionBookPipeline
    {
        public AssociatePromotionBookPipeline(IPipelineConfiguration<IAssociatePromotionBookPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}