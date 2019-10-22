using System;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateCategoryToParentBlock)]
    public class AssociateCategoryToParentBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly AssociateCategoryToParentCommand _associateCategoryToParent;

        public AssociateCategoryToParentBlock(AssociateCategoryToParentCommand associateCategoryToParent)
        {
            _associateCategoryToParent = associateCategoryToParent;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity();
            string entityId = commerceEntity.Id;

            if (arg.ImportHandler.ParentEntityIds == null
                || !arg.ImportHandler.ParentEntityIds.Any()
                || !(commerceEntity is Category))
            {
                return await Task.FromResult(arg);
            }

            foreach (var catalog in arg.ImportHandler.ParentEntityIds)
            {
                if (catalog.Value != null && catalog.Value.Any())
                {
                    foreach (var parentId in catalog.Value)
                    {
                        await _associateCategoryToParent
                            .Process(context.CommerceContext, catalog.Key, parentId, entityId)
                            .ConfigureAwait(false);
                    }
                }
            }

            return await Task.FromResult(arg);
        }
    }
}