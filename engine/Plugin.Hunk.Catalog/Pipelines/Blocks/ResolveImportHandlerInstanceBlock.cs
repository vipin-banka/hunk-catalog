using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveImportHandlerInstanceBlock)]
    public class ResolveImportHandlerInstanceBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public ResolveImportHandlerInstanceBlock(
            CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var importHandler = await _commerceCommander.Pipeline<IResolveEntityImportHandlerPipeline>()
                .Run(new ResolveEntityImportHandlerArgument(arg), context)
                .ConfigureAwait(false);

            if (importHandler == null)
            {
                context.Abort( await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "ImportHandlerMissing", null, $"Import handler instance for entityType={arg.SourceEntityDetail.EntityType} not resolved."), context);
            }

            arg.ImportHandler = importHandler;
            return arg;
        }
    }
}