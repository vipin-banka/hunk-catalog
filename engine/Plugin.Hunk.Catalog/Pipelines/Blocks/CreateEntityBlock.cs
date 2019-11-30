using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.CreateEntityBlock)]
    public class CreateEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var entity = arg.ImportHandler.GetCommerceEntity();

            if (entity == null)
            {
                arg.IsNew = true;
                // create new entity
                entity = await arg.ImportHandler.Create().ConfigureAwait(false);
                arg.ImportHandler.SetCommerceEntity(entity);
            }

            return await Task.FromResult(arg);
        }
    }
}