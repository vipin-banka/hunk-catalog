using Microsoft.Extensions.DependencyInjection;
using Plugin.Hunk.Catalog.Pipelines;
using Plugin.Hunk.Catalog.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using System.Reflection;
using Sitecore.Commerce.Plugin.Pricing;

namespace Plugin.Hunk.Catalog
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IRunningPluginsPipeline>(c =>
                {
                    c.Add<Pipelines.Blocks.RegisteredPluginBlock>()
                        .After<RunningPluginsBlock>();
                })
                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure =>
                {
                    configure.Add<ConfigureServiceApiBlock>();
                })
                .AddPipeline<IImportEntityPipeline, ImportEntityPipeline>(configure =>
                {
                    configure
                        .Add<PrepImportEntityBlock>()
                        .Add<ResolveImportHandlerInstanceBlock>()
                        .Add<GetSourceEntityBlock>()
                        .Add<ValidateSourceEntityBlock>()
                        .Add<ResolveVersionedEntityBlock>()
                        .Add<ImportEntityBlock>()
                        .Add<PersistEntityBlock>();
                })
                .AddPipeline<IAssociatePriceBookPipeline, AssociatePriceBookPipeline>(configure =>
                {
                    configure
                        .Add<Pipelines.Blocks.AssociateCatalogToPriceBookBlock>();
                })
                .AddPipeline<IAssociatePromotionBookPipeline, AssociatePromotionBookPipeline>(configure =>
                {
                    configure
                        .Add<Pipelines.Blocks.AssociateCatalogToPromotionBookBlock>();
                })
                .AddPipeline<IAssociateInventorySetPipeline, AssociateInventorySetPipeline>(configure =>
                {
                    configure
                        .Add<Pipelines.Blocks.AssociateCatalogToInventorySetBlock>();
                })
                .AddPipeline<IResolveEntityImportHandlerPipeline, ResolveEntityImportHandlerPipeline>(configure =>
                {
                    configure
                        .Add<ResolveEntityImportHandlerBlock>();
                })
                .AddPipeline<ISetComponentsPipeline, SetComponentsPipeline>(configure =>
                {
                    configure
                        .Add<UpdateEntityBlock>()
                        .Add<SetEntityComponentsBlock>()
                        .Add<ImportEntityVariantsBlock>();
                })
                .AddPipeline<IResolveEntityMapperPipeline, ResolveEntityMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveEntityMapperBlock>();
                })
                .AddPipeline<IResolveComponentMapperPipeline, ResolveComponentMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveComponentMapperBlock>();
                })
                .AddPipeline<IResolveRelationshipMapperPipeline, ResolveRelationshipMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveRelationshipMapperBlock>();
                })
                .AddPipeline<IResolveEntityLocalizationMapperPipeline, ResolveEntityLocalizationMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveEntityLocalizationMapperBlock>();
                })
                .AddPipeline<IResolveComponentLocalizationMapperPipeline, ResolveComponentLocalizationMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveComponentLocalizationMapperBlock>();
                })
                .AddPipeline<IAssociateParentsPipeline, AssociateParentsPipeline>(configure =>
                {
                    configure
                        .Add<Pipelines.Blocks.AssociateCategoryToParentBlock>()
                        .Add<AssociateSellableItemToParentBlock>()
                        .Add<DisassociateFromNotLinkedParentBlock>()
                        .Add<Pipelines.Blocks.CreateRelationshipBlock>();
                })
                .AddPipeline<ICreateRelationshipPipeline, CreateRelationshipPipeline>(configure =>
                {
                    configure
                        .Add<Pipelines.Blocks.UpdateCatalogHierarchyBlock>()
                        .After<Sitecore.Commerce.Plugin.Catalog.UpdateCatalogHierarchyBlock>();
                })
                .AddPipeline<IImportLocalizeContentPipeline, ImportLocalizeContentPipeline>(configure =>
                {
                    configure
                        .Add<GetLocalizePropertiesBlock>()
                        .Add<GetLocalizationEntityBlock>()
                        .Add<SetLocalizePropertiesBlock>();
                })
                .AddPipeline<IAssociateInventoryInformationPipeline, AssociateInventoryInformationPipeline>(configure =>
                {
                    configure
                        .Add<AssociateInventoryInformationBlock>();
                })
            );

            services.RegisterAllCommands(assembly);
        }
    }
}