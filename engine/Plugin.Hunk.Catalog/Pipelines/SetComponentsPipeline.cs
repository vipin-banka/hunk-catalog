using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class SetComponentsPipeline : CommercePipeline<CommerceEntity, CommerceEntity>, ISetComponentsPipeline
    {
        public SetComponentsPipeline(IPipelineConfiguration<ISetComponentsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}