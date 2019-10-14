using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.ResolveComponentMapperPipeline)]
    public interface IResolveComponentMapperPipeline : IPipeline<ResolveComponentMapperArgument, IComponentMapper, CommercePipelineExecutionContext>
    {
    }
}