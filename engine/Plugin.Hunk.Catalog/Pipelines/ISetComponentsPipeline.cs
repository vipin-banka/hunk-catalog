using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.SetComponentsPipeline)]
    public interface ISetComponentsPipeline : IPipeline<CommerceEntity, CommerceEntity, CommercePipelineExecutionContext>
    {
    }
}