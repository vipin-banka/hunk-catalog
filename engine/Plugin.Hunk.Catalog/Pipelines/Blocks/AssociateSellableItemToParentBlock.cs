using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateSellableItemToParentBlock)]
    public class AssociateSellableItemToParentBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly AssociateSellableItemToParentCommand _associateSellableItemToParent;

        public AssociateSellableItemToParentBlock(AssociateSellableItemToParentCommand associateSellableItemToParent)
        {
            _associateSellableItemToParent = associateSellableItemToParent;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity();
            string entityId = commerceEntity.Id;

            if (arg.ImportHandler.ParentEntityIds == null
                || !arg.ImportHandler.ParentEntityIds.Any()
                || !(commerceEntity is SellableItem))
            {
                return await Task.FromResult(arg);
            }

            foreach (var catalog in arg.ImportHandler.ParentEntityIds)
            {
                if (catalog.Value == null || !catalog.Value.Any())
                {
                    await _associateSellableItemToParent
                    .Process(context.CommerceContext, catalog.Key, catalog.Key, entityId).ConfigureAwait(false);
                }
                else
                {
                    foreach (var parentId in catalog.Value)
                    {
                        await _associateSellableItemToParent
                        .Process(context.CommerceContext, catalog.Key, parentId, entityId).ConfigureAwait(false);
                    }
                }
            }

            return await Task.FromResult(arg);
        }
    }
}