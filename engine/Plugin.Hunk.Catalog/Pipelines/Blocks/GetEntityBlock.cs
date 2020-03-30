using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetEntityBlock)]
    public class GetEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly FindEntityCommand _findEntityCommand;

        public GetEntityBlock(
            FindEntityCommand findEntityCommand)
        {
            _findEntityCommand = findEntityCommand;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            string entityId = arg.ImportHandler.EntityId;

            var entityToUpdate = await _findEntityCommand.Process(context.CommerceContext, typeof(CommerceEntity), entityId)
                .ConfigureAwait(false);

            if (entityToUpdate != null)
            {
                arg.IsNew = false;
                arg.ImportHandler.SetCommerceEntity(entityToUpdate);
            }

            return arg;
        }
    }
}