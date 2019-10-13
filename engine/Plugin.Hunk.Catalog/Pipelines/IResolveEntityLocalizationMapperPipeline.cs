using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.ResolveEntityLocalizationMapperPipeline)]
    public interface IResolveEntityLocalizationMapperPipeline : IPipeline<ResolveEntityLocalizationMapperArgument, IEntityLocalizationMapper, CommercePipelineExecutionContext>
    {
    }
}