using System.Linq;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.EntityVersions;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportEntityBlock)]
    public class ImportEntityBlock : PipelineBlock<ImportEntityArgument, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly ISetComponentsPipeline _setComponentsPipeline;
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public ImportEntityBlock(
            ISetComponentsPipeline setComponentsPipeline,
            IPersistEntityPipeline persistEntityPipeline)
        {
            _setComponentsPipeline = setComponentsPipeline;
            _persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<CommerceEntity> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var entityToUpdate = arg.ImportHandler.GetCommerceEntity();

            if (entityToUpdate == null)
            {
                arg.IsNew = true;
                // create new entity
                entityToUpdate = await arg.ImportHandler.Create().ConfigureAwait(false);
                arg.ImportHandler.SetCommerceEntity(entityToUpdate);
            }
            else
            {
                arg.IsNew = false;
                arg.ImportHandler.SetCommerceEntity(entityToUpdate);
                // update existing entity
                await _setComponentsPipeline.Run(entityToUpdate, context)
                    .ConfigureAwait(false);

                await _persistEntityPipeline.Run(new PersistEntityArgument(entityToUpdate), context)
                    .ConfigureAwait(false);
            }

            return await Task.FromResult(entityToUpdate);
        }
    }
}