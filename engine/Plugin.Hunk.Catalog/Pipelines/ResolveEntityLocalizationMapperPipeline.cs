using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class ResolveEntityLocalizationMapperPipeline : CommercePipeline<ResolveEntityLocalizationMapperArgument, IEntityLocalizationMapper>, IResolveEntityLocalizationMapperPipeline
    {
        public ResolveEntityLocalizationMapperPipeline(IPipelineConfiguration<IResolveEntityLocalizationMapperPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}