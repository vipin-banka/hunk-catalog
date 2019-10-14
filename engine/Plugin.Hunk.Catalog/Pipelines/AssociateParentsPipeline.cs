using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class AssociateParentsPipeline : CommercePipeline<ImportEntityArgument, ImportEntityArgument>, IAssociateParentsPipeline
    {
        public AssociateParentsPipeline(IPipelineConfiguration<IAssociateParentsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}