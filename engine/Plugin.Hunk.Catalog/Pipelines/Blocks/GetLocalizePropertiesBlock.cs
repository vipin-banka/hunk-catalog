using Plugin.Hunk.Catalog.Abstractions;
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
    [PipelineDisplayName(Constants.GetLocalizePropertiesBlock)]
    public class GetLocalizePropertiesBlock : PipelineBlock<ImportLocalizeContentArgument, ImportLocalizeContentArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public GetLocalizePropertiesBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportLocalizeContentArgument> Run(ImportLocalizeContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.ImportEntityArgument.ImportHandler.HasLanguages())
            {
                IList<LocalizablePropertyValues> entityLocalizableProperties = null;

                IList<LocalizableComponentPropertiesValues> componentsPropertiesList = new List<LocalizableComponentPropertiesValues>();

                foreach (var language in arg.ImportEntityArgument.ImportHandler.GetLanguages())
                {
                    entityLocalizableProperties = await PerformEntityLocalization(arg, context, language, entityLocalizableProperties).ConfigureAwait(false);

                    if (arg.CommerceEntity.Components == null || !arg.CommerceEntity.Components.Any())
                        continue;

                    await PerformEntityComponentsLocalization(arg, context, language, componentsPropertiesList).ConfigureAwait(false);

                    if (!arg.ImportEntityArgument.ImportHandler.HasVariants(language) )
                    {
                        continue;
                    }

                    await PerformEntityVariantsLocalization(arg, context, language, componentsPropertiesList).ConfigureAwait(false);
                }

                arg.Properties = entityLocalizableProperties;
                arg.ComponentsProperties = componentsPropertiesList;
            }

            return await Task.FromResult(arg);
        }

        private async Task PerformEntityVariantsLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language,
            IList<LocalizableComponentPropertiesValues> componentsPropertiesList)
        {
            foreach (var variant in arg.ImportEntityArgument.ImportHandler.GetVariants(language))
            {
                var itemVariationComponent = arg.CommerceEntity.GetVariation(variant.Id);

                if (itemVariationComponent == null)
                    continue;

                var itemVariantComponentLocalizationMapper = await _commerceCommander
                    .Pipeline<IResolveComponentLocalizationMapperPipeline>()
                    .Run(new ResolveComponentLocalizationMapperArgument(arg.ImportEntityArgument, arg.CommerceEntity,
                            itemVariationComponent, language, variant), context).ConfigureAwait(false);

                if (itemVariantComponentLocalizationMapper != null)
                {

                    itemVariantComponentLocalizationMapper.Execute(componentsPropertiesList, itemVariationComponent,
                        language);

                    await PerformEntityVariantLocalization(arg, context, language, componentsPropertiesList,
                        itemVariationComponent, variant).ConfigureAwait(false);
                }
                else
                {
                    await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "ItemVariantComponentLocalizationMapperMissing", null, "Item variant component localization mapper instance for not resolved.");
                }
            }
        }

        private async Task PerformEntityVariantLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language, IList<LocalizableComponentPropertiesValues> componentsPropertiesList,
            ItemVariationComponent itemVariationComponent, IEntity variant)
        {
            if (itemVariationComponent.ChildComponents != null
                && itemVariationComponent.ChildComponents.Any())
            {
                foreach (var component in itemVariationComponent.ChildComponents)
                {
                    var mapper = await _commerceCommander.Pipeline<IResolveComponentLocalizationMapperPipeline>()
                        .Run(new
                            ResolveComponentLocalizationMapperArgument(arg.ImportEntityArgument, arg.CommerceEntity,
                                component, language, variant), context).ConfigureAwait(false);

                    if (mapper != null)
                    {
                        mapper.Execute(componentsPropertiesList, component, language);
                    }
                    else
                    {
                        await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "ComponentChildComponentLocalizationMapperMissing", null, $"Component's child component localization mapper missing for componentType={component.GetType().FullName} (key={component.GetComponentMetadataPolicy().MapperKey}) not resolved.");
                    }
                }
            }
        }

        private async Task PerformEntityComponentsLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language, IList<LocalizableComponentPropertiesValues> componentsPropertiesList)
        {
            foreach (var commerceEntityComponent in arg.CommerceEntity.Components)
            {
                var entityComponentLocalizationMapper = await _commerceCommander
                    .Pipeline<IResolveComponentLocalizationMapperPipeline>()
                    .Run(
                        new ResolveComponentLocalizationMapperArgument(arg.ImportEntityArgument,
                            arg.CommerceEntity, commerceEntityComponent, language), context)
                    .ConfigureAwait(false);

                if (entityComponentLocalizationMapper != null)
                {
                    entityComponentLocalizationMapper.Execute(componentsPropertiesList, commerceEntityComponent,
                        language);
                }
                else
                {
                    await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "EntityComponentLocalizationMapperMissing", null, $"Entity component localization mapper instance for componentType={commerceEntityComponent.GetType().FullName} (key={commerceEntityComponent.GetComponentMetadataPolicy().MapperKey}) not resolved.");
                }
            }
        }

        private async Task<IList<LocalizablePropertyValues>> PerformEntityLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language, IList<LocalizablePropertyValues> entityLocalizableProperties)
        {
            if (arg.ImportEntityArgument.ImportHandler is IEntityLocalizationMapper mapper)
            {
                entityLocalizableProperties = mapper.Map(language, entityLocalizableProperties);
            }
            else
            {
                var entityLocalizationMapper = await _commerceCommander
                    .Pipeline<IResolveEntityLocalizationMapperPipeline>()
                    .Run(new ResolveEntityLocalizationMapperArgument(arg.ImportEntityArgument, language),
                        context).ConfigureAwait(false);

                if (entityLocalizationMapper != null)
                {
                    entityLocalizableProperties = entityLocalizationMapper.Map(language, entityLocalizableProperties);
                }
                else
                {
                    await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Warning, "EntityLocalizationMapperMissing", null, $"Entity Localization Mapper instance for entityType={arg.ImportEntityArgument.SourceEntityDetail.EntityType} not resolved.");
                }
            }

            return entityLocalizableProperties;
        }
    }
}