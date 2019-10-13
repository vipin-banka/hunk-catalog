using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using CommerceEntity = Sitecore.Commerce.Core.CommerceEntity;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportEntityBlock)]
    public class ImportEntityBlock : PipelineBlock<ImportEntityArgument, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly FindEntityCommand _findEntityCommand;
        private readonly ISetComponentsPipeline _setComponentsPipeline;
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public ImportEntityBlock(
            FindEntityCommand findEntityCommand,
            ISetComponentsPipeline setComponentsPipeline,
            IPersistEntityPipeline persistEntityPipeline)
        {
            _findEntityCommand = findEntityCommand;
            _setComponentsPipeline = setComponentsPipeline;
            _persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<CommerceEntity> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            string entityId = arg.ImportHandler.EntityId;
            var entity = await _findEntityCommand.Process(context.CommerceContext, typeof(CommerceEntity), entityId)
                .ConfigureAwait(false);

            if (entity == null)
            {
                arg.IsNew = true;
                // create new entity
                entity = await arg.ImportHandler.Create().ConfigureAwait(false);
                arg.ImportHandler.SetCommerceEntity(entity);
            }
            else
            {
                arg.IsNew = false;
                arg.ImportHandler.SetCommerceEntity(entity);
                // update existing entity
                await _setComponentsPipeline.Run(entity, context)
                    .ConfigureAwait(false);

                await _persistEntityPipeline.Run(new PersistEntityArgument(entity), context)
                    .ConfigureAwait(false);
            }

            return await Task.FromResult(entity);
        }
    }
}