using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Extensions;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveComponentMapperBlock)]
    public class ResolveComponentMapperBlock : PipelineBlock<ResolveComponentMapperArgument, IComponentMapper, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveComponentMapperBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<IComponentMapper> Run(ResolveComponentMapperArgument arg, CommercePipelineExecutionContext context)
        {
            IComponentMapper mapper;
            if (arg.ParenComponent == null)
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetEntityComponentMapper(
                    arg.CommerceEntity,
                    arg.ImportEntityArgument,
                    arg.ComponentName,
                    _commerceCommander,
                    context);
            }
            else if (arg.ParenComponent.GetType() == typeof(ItemVariationsComponent))
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetItemVariationComponentMapper(
                    arg.CommerceEntity,
                    arg.ParenComponent,
                    arg.ImportEntityArgument,
                    arg.SourceComponent,
                    _commerceCommander,
                    context);
            }
            else
            {
                mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetComponentChildComponentMapper(
                    arg.CommerceEntity,
                    arg.ParenComponent,
                    arg.ImportEntityArgument,
                    arg.SourceComponent,
                    arg.ComponentName,
                    _commerceCommander,
                    context);
            }

            return Task.FromResult(mapper);
        }
    }
}