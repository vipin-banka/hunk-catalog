using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Plugin.Hunk.Catalog.Policy;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.PrepImportEntityBlock)]
    public class PrepImportEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            context.CommerceContext.AddUniqueObject(arg);

            var catalogImportPolicy = context.CommerceContext.GetPolicy<CatalogImportPolicy>();

            if (catalogImportPolicy == null)
            {
                catalogImportPolicy = new CatalogImportPolicy();
                await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "CatalogImportPolicyMissing", null, "Catalog import policy not found in the environment, created a default catalog import policy.")
                    .ConfigureAwait(false);
            }

            arg.CatalogImportPolicy = catalogImportPolicy;

            return arg;
        }
    }
}