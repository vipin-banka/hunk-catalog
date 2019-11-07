using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetEntityComponentsBlock)]
    public class SetEntityComponentsBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public SetEntityComponentsBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg?.SourceEntity != null)
            {
                await SetCommerceEntityComponents(arg.ImportHandler.GetCommerceEntity(), arg, context).ConfigureAwait(false);
            }

            return arg;
        }

        private async Task SetCommerceEntityComponents(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument,  CommercePipelineExecutionContext context)
        {
            if (importEntityArgument.SourceEntityDetail.Components != null && importEntityArgument.SourceEntityDetail.Components.Any())
            {
                foreach (var componentName in importEntityArgument.SourceEntityDetail.Components)
                {
                    var mapper = await _commerceCommander.Pipeline<IResolveComponentMapperPipeline>()
                        .Run(new ResolveComponentMapperArgument(importEntityArgument, commerceEntity, componentName), context)
                        .ConfigureAwait(false);

                    if (mapper == null)
                    {
                        await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "EntityComponentMapperMissing", null, $"Entity component mapper instance for entityType={importEntityArgument.SourceEntityDetail.EntityType} and component={componentName} not resolved.");
                    }
                    else
                    {
                        var component =  mapper.Execute();
                        component.SetComponentMetadataPolicy(componentName);
                    }
                }
            }
        }
    }
}