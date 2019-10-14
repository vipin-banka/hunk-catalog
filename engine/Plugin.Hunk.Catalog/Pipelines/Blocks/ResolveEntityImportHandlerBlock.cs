using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveEntityImportHandlerBlock)]
    public class ResolveEntityImportHandlerBlock : PipelineBlock<ResolveEntityImportHandlerArgument, IEntityImportHandler, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveEntityImportHandlerBlock(
            CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override Task<IEntityImportHandler> Run(ResolveEntityImportHandlerArgument arg, CommercePipelineExecutionContext context)
        {
            var importHandler = arg.ImportEntityArgument.CatalogImportPolicy.Mappings.GetImportHandler(arg.ImportEntityArgument, _commerceCommander, context);

            return Task.FromResult(importHandler);
        }
    }
}