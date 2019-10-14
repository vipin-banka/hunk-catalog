using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveRelationshipMapperBlock)]
    public class ResolveRelationshipMapperBlock : PipelineBlock<ResolveRelationshipMapperArgument, IRelationshipMapper, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveRelationshipMapperBlock(
            CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<IRelationshipMapper> Run(ResolveRelationshipMapperArgument arg, CommercePipelineExecutionContext context)
        {
            var mapper = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetRelationshipMapper(arg.ImportEntityArgument, arg.RelationshipName, _commerceCommander, context);

            return Task.FromResult(mapper);
        }
    }
}