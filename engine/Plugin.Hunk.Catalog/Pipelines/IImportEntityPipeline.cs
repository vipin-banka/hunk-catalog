using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.Hunk.Catalog.Pipelines
{
    [PipelineDisplayName(Constants.ImportEntityPipeline)]
    public interface IImportEntityPipeline: IPipeline<ImportEntityArgument, CommerceEntity, CommercePipelineExecutionContext>
    {
    }
}