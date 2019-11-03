using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.PersistEntityBlock)]
    public class PersistEntityBlock : PipelineBlock<ImportEntityArgument, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public PersistEntityBlock(
            IPersistEntityPipeline persistEntityPipeline)
        {
            _persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<CommerceEntity> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity();

            if (commerceEntity == null)
            {
                return await Task.FromResult((CommerceEntity) null);
            }

            await _persistEntityPipeline.Run(new PersistEntityArgument(commerceEntity), context)
                    .ConfigureAwait(false);

            return await Task.FromResult(commerceEntity);
        }
    }
}