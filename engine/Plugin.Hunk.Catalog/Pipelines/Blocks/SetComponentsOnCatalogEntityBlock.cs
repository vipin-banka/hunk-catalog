using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetComponentsOnCatalogEntityBlock)]
    public class SetComponentsOnCatalogEntityBlock : PipelineBlock<CatalogContentArgument, CatalogContentArgument, CommercePipelineExecutionContext>
    {
        private readonly ISetComponentsPipeline _setComponentsPipeline;
        public SetComponentsOnCatalogEntityBlock(ISetComponentsPipeline setComponentsPipeline)
        {
            _setComponentsPipeline = setComponentsPipeline;
        }

        public override async Task<CatalogContentArgument> Run(CatalogContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg?.Catalog != null)
            {
                await _setComponentsPipeline.Run(arg.Catalog, context).ConfigureAwait(false);
            }

            if (arg?.Categories != null && arg.Categories.Any())
            {
                foreach (var category in arg.Categories)
                {
                    await _setComponentsPipeline.Run(category, context).ConfigureAwait(false);
                }
            }

            if (arg?.SellableItems != null && arg.SellableItems.Any())
            {
                foreach(var sellableItem in arg.SellableItems)
                {
                    await _setComponentsPipeline.Run(sellableItem, context).ConfigureAwait(false);
                }
            }

            return await Task.FromResult(arg);
        }
    }
}