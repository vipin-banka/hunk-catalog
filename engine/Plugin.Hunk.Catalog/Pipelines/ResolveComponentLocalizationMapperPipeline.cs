using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    public class ResolveComponentLocalizationMapperPipeline : CommercePipeline<ResolveComponentLocalizationMapperArgument, IComponentLocalizationMapper>, IResolveComponentLocalizationMapperPipeline
    {
        public ResolveComponentLocalizationMapperPipeline(IPipelineConfiguration<IResolveComponentLocalizationMapperPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}