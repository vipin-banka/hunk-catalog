using Plugin.Hunk.Catalog.Components;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.DisassociateFromNotLinkedParentBlock)]
    public class DisassociateFromNotLinkedParentBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly DeleteRelationshipCommand _deleteRelationshipCommand;

        public DisassociateFromNotLinkedParentBlock(DeleteRelationshipCommand deleteRelationshipCommand)
        {
            _deleteRelationshipCommand = deleteRelationshipCommand;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity();

            if (arg.ImportHandler.ParentEntityIds == null
                || !arg.ImportHandler.ParentEntityIds.Any()
                || (!(commerceEntity is Category) && !(commerceEntity is SellableItem)))
            {
                return await Task.FromResult(arg);
            }

            if (commerceEntity.HasComponent<ParentEntitiesComponent>())
            {
                var component = commerceEntity.GetComponent<ParentEntitiesComponent>();

                var allIds = arg.ImportHandler.ParentEntityIds.SelectMany(x => x.Value).Distinct().ToList();

                var relationshipNamePostfix = "To" + commerceEntity.GetType().Name;

                foreach (var componentEntityId in component.EntityIds)
                {
                    if (!allIds.Contains(componentEntityId))
                    {
                        var relationshipName = string.Empty;

                        if (componentEntityId.StartsWith(CommerceEntity
                            .IdPrefix<Sitecore.Commerce.Plugin.Catalog.Catalog>()))
                        {
                            relationshipName = typeof(Sitecore.Commerce.Plugin.Catalog.Catalog).Name +
                                               relationshipNamePostfix;
                        }
                        else if (componentEntityId.StartsWith(CommerceEntity
                            .IdPrefix<Category>()))
                        {
                            relationshipName = typeof(Category).Name +
                                               relationshipNamePostfix;
                        }

                        if (!string.IsNullOrEmpty(relationshipName))
                        {
                            await _deleteRelationshipCommand.Process(context.CommerceContext,
                                    componentEntityId,
                                    commerceEntity.Id,
                                    relationshipName)
                                .ConfigureAwait(false);
                        }
                    }
                }
            }

            return await Task.FromResult(arg);
        }
    }
}