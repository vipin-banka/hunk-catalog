using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Extensions;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveEntityLocalizationMapperBlock)]
    public class ResolveEntityLocalizationMapperBlock : PipelineBlock<ResolveEntityLocalizationMapperArgument, IEntityLocalizationMapper, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveEntityLocalizationMapperBlock(
            CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<IEntityLocalizationMapper> Run(ResolveEntityLocalizationMapperArgument arg, CommercePipelineExecutionContext context)
        {
            var mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetEntityLocalizationMapper(
                arg.LanguageEntity,
                arg.ImportEntityArgument,
                _commerceCommander,
                context);

            return Task.FromResult(mapper);
        }
    }
}