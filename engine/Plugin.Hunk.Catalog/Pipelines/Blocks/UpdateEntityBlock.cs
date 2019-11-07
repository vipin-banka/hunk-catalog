using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.UpdateEntityBlock)]
    public class UpdateEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public UpdateEntityBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg?.SourceEntity != null)
            {
                await SetCommerceEntityDetails(arg.ImportHandler.GetCommerceEntity(), arg, context).ConfigureAwait(false);
            }

            return arg;
        }

        private async Task SetCommerceEntityDetails(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            if (importEntityArgument.ImportHandler is IEntityMapper mapper)
            {
                mapper.Map();
            }
            else
            {
                mapper = await _commerceCommander.Pipeline<IResolveEntityMapperPipeline>()
                    .Run(new ResolveEntityMapperArgument(importEntityArgument, commerceEntity), context)
                    .ConfigureAwait(false);

                if (mapper == null)
                {
                    await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "EntityMapperMissing", null, $"Entity mapper instance for entityType={importEntityArgument.SourceEntityDetail.EntityType} not resolved.");
                }
                else
                {
                    mapper.Map();
                }
            }
        }
    }
}