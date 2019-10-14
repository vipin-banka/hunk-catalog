using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveComponentLocalizationMapperBlock)]
    public class ResolveComponentLocalizationMapperBlock : PipelineBlock<ResolveComponentLocalizationMapperArgument, IComponentLocalizationMapper, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveComponentLocalizationMapperBlock(
            CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<IComponentLocalizationMapper> Run(ResolveComponentLocalizationMapperArgument arg, CommercePipelineExecutionContext context)
        {
            IComponentLocalizationMapper mapper;

            if (arg.SourceComponent == null)
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetEntityComponentLocalizationMapper(
                    arg.CommerceEntity,
                    arg.Component,
                    arg.LanguageEntity,
                    _commerceCommander,
                    context);
            }
            else if (arg.Component.GetType() == typeof(ItemVariationComponent))
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetItemVariantComponentLocalizationMapper(
                    arg.CommerceEntity,
                    arg.Component,
                    arg.LanguageEntity,
                    arg.SourceComponent,
                    _commerceCommander,
                    context);
            }
            else
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetComponentChildComponentLocalizationMapper(
                    arg.CommerceEntity,
                    arg.Component,
                    arg.LanguageEntity,
                    arg.SourceComponent,
                    _commerceCommander,
                    context);
            }

            return Task.FromResult(mapper);
        }
    }
}