using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.ResolveRelationshipMapperPipeline)]
    public interface IResolveRelationshipMapperPipeline : IPipeline<ResolveRelationshipMapperArgument, IRelationshipMapper, CommercePipelineExecutionContext>
    {
    }
}