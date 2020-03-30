using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.AssociateInventoryInformationPipeline)]
    public interface IAssociateInventoryInformationPipeline : IPipeline<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
    }
}