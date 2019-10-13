using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Plugin.Hunk.Catalog.Policy;
using Sitecore.Commerce.Core;
using System;
using System.Linq;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class MappingExtensions
    {
        public static IEntityImportHandler GetImportHandler(this Mappings mappings, ImportEntityArgument importEntityArgument, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var handlerType = mappings
                .EntityMappings
                .FirstOrDefault(x => x.Key.Equals(importEntityArgument.SourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

            if (handlerType != null)
            {
                var t = Type.GetType(handlerType.ImportHandlerTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntityDetail.SerializedEntity,
                            commerceCommander, context) is
                        IEntityImportHandler handler)
                    {
                        return handler;
                    }
                }
            }

            return null;
        }

        public static IEntityMapper GetEntityMapper(this Mappings mappings, CommerceEntity targetEntity, ImportEntityArgument importEntityArgument, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = mappings
                .EntityMappings
                .FirstOrDefault(x => x.Key.Equals(importEntityArgument.SourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName ?? mapperType.ImportHandlerTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, targetEntity, commerceCommander,
                        context) is IEntityMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public static IEntityLocalizationMapper GetEntityLocalizationMapper(this Mappings mappings, ILanguageEntity languageEntity, ImportEntityArgument importEntityArgument, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = mappings
                .EntityMappings
                .FirstOrDefault(x => x.Key.Equals(importEntityArgument.SourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.ImportHandlerTypeName ?? mapperType.FullTypeName ?? mapperType.LocalizationFullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), commerceCommander, context) is
                        IEntityLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public static IComponentMapper GetEntityComponentMapper(this Mappings mappings, CommerceEntity targetEntity, ImportEntityArgument importEntityArgument, string componentMappingKey, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = mappings
                .EntityComponentMappings
                .FirstOrDefault(x => x.Key.Equals(componentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, targetEntity, commerceCommander, context) is
                        IComponentMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public static IComponentLocalizationMapper GetEntityComponentLocalizationMapper(this Mappings mappings, CommerceEntity targetEntity, Component component, ILanguageEntity languageEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var metadataPolicy = component.GetComponentMetadataPolicy();
            var mapperType = mappings
                .EntityComponentMappings
                .FirstOrDefault(x =>
                    (metadataPolicy != null && !string.IsNullOrEmpty(metadataPolicy.MapperKey) && x.Key.Equals(metadataPolicy.MapperKey, StringComparison.OrdinalIgnoreCase))
                    || x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), targetEntity, commerceCommander, context) is
                        IComponentLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public static IComponentMapper GetItemVariationComponentMapper(this Mappings mappings, CommerceEntity targetEntity, Component parentComponent, ImportEntityArgument importEntityArgument, object sourceComponent, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = mappings
                .ItemVariationMappings
                .FirstOrDefault();

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, sourceComponent, targetEntity, parentComponent,
                        commerceCommander, context) is IComponentMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public static IComponentLocalizationMapper GetItemVariantComponentLocalizationMapper(this Mappings mappings, CommerceEntity targetEntity, Component component, ILanguageEntity languageEntity, object sourceVariant, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = mappings
                .ItemVariationMappings
                .FirstOrDefault();

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), sourceVariant, targetEntity, component, commerceCommander, context) is IComponentLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public static IComponentMapper GetComponentChildComponentMapper(this Mappings mappings, CommerceEntity targetEntity, Component parentComponent, ImportEntityArgument importEntityArgument, object sourceComponent, string childComponentMappingKey, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var mapperType = mappings
                .ItemVariationComponentMappings
                .FirstOrDefault(x => x.Key.Equals(childComponentMappingKey, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, importEntityArgument.SourceEntity, sourceComponent, targetEntity,
                        parentComponent, commerceCommander, context) is IComponentMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }

        public static IComponentLocalizationMapper GetComponentChildComponentLocalizationMapper(this Mappings mappings, CommerceEntity targetEntity, Component component, ILanguageEntity languageEntity, object sourceVariant, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            var metadataPolicy = component.GetComponentMetadataPolicy();
            var mapperType = mappings
                .ItemVariationComponentMappings
                .FirstOrDefault(
                    x => (metadataPolicy != null && !string.IsNullOrEmpty(metadataPolicy.MapperKey) && x.Key.Equals(metadataPolicy.MapperKey, StringComparison.OrdinalIgnoreCase))
                        || x.Type.Equals(component.GetType().FullName, StringComparison.OrdinalIgnoreCase));

            if (mapperType != null)
            {
                var t = Type.GetType(mapperType.LocalizationFullTypeName ?? mapperType.FullTypeName);

                if (t != null)
                {
                    if (Activator.CreateInstance(t, languageEntity.GetEntity(), sourceVariant, targetEntity, component,
                        commerceCommander, context) is IComponentLocalizationMapper mapper)
                    {
                        return mapper;
                    }
                }
            }

            return null;
        }
    }
}