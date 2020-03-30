using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportEntityVariantsBlock)]
    public class ImportEntityVariantsBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ImportEntityVariantsBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg?.SourceEntity != null)
            {
                await ImportVariants(arg.ImportHandler.GetCommerceEntity(), arg, context)
                    .ConfigureAwait(false);
            }

            return arg;
        }

        private async Task ImportVariants(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument, CommercePipelineExecutionContext context)
        {
            var orphanVariants = new List<ItemVariationComponent>();
            ItemVariationsComponent itemVariationsComponent = null;
            var sourceEntityHasVariants = importEntityArgument.ImportHandler.HasVariants();
            if (!sourceEntityHasVariants
                && commerceEntity.HasComponent<ItemVariationsComponent>())
            {
                itemVariationsComponent = commerceEntity.GetComponent<ItemVariationsComponent>();
                if (itemVariationsComponent.Variations != null
                    && itemVariationsComponent.Variations.Any())
                {
                    orphanVariants = itemVariationsComponent.Variations;
                }
            }

            if (sourceEntityHasVariants)
            {
                itemVariationsComponent =
                    commerceEntity.GetComponent<ItemVariationsComponent>();

                var variants = importEntityArgument.ImportHandler.GetVariants();

                foreach (var variant in variants)
                {
                    var itemVariantMapper = await _commerceCommander.Pipeline<IResolveComponentMapperPipeline>()
                        .Run(
                            new ResolveComponentMapperArgument(importEntityArgument, commerceEntity,
                                itemVariationsComponent, variant, string.Empty), context).ConfigureAwait(false);

                    if (itemVariantMapper == null)
                    {
                        await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "ItemVariationMapperMissing", null, $"Item variation mapper instance for variantId={variant.Id} not resolved.");
                        continue;
                    }

                    var action = itemVariantMapper.GetComponentAction();
                    Component itemVariationComponent = itemVariantMapper.Execute(action);

                    if (action != ComponentAction.Remove
                        && importEntityArgument.SourceEntityDetail.VariantComponents != null
                        && importEntityArgument.SourceEntityDetail.VariantComponents.Any())
                    {
                        foreach (var variantComponentName in importEntityArgument.SourceEntityDetail.VariantComponents)
                        {
                            var itemVariantChildComponentMapper = await _commerceCommander
                                .Pipeline<IResolveComponentMapperPipeline>().Run(new
                                    ResolveComponentMapperArgument(importEntityArgument, commerceEntity,
                                        itemVariationComponent, variant, variantComponentName), context)
                                .ConfigureAwait(false);

                            if (itemVariantChildComponentMapper != null)
                            {
                                var childComponent = itemVariantChildComponentMapper.Execute();
                                childComponent.SetComponentMetadataPolicy(variantComponentName);
                            }
                            else
                            {
                                await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "ComponentChildComponentMapperMissing", null, $"Component's child component mapper instance for entityType={importEntityArgument.SourceEntityDetail.EntityType} and key={variantComponentName} not resolved.");
                            }
                        }
                    }
                }

                orphanVariants = (from n in itemVariationsComponent.Variations
                                  join o in variants on n.Id equals o.Id into p
                                  where !p.Any()
                                  select n).ToList();
            }

            if (itemVariationsComponent != null
                && orphanVariants != null
                && orphanVariants.Any())
            {
                foreach (var orphanVariant in orphanVariants)
                {
                    if (importEntityArgument.CatalogImportPolicy.DeleteOrphanVariant)
                    {
                        itemVariationsComponent.ChildComponents.Remove(orphanVariant);
                    }
                    else
                    {
                        orphanVariant.Disabled = true;
                    }
                }
            }
        }
    }
}