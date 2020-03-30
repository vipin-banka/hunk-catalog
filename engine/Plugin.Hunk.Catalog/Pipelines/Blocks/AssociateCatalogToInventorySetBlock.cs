using System;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Inventory;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateCatalogToInventorySetBlock)]
    public class AssociateCatalogToInventorySetBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly CreateInventorySetCommand _createInventorySetCommand;
        private readonly AssociateCatalogToInventorySetCommand _associateCatalogToInventorySetCommand;

        public AssociateCatalogToInventorySetBlock(
            IDoesEntityExistPipeline doesEntityExistPipeline,
            CreateInventorySetCommand createInventorySetCommand,
            AssociateCatalogToInventorySetCommand associateCatalogToInventorySetCommand)
        {
            _doesEntityExistPipeline = doesEntityExistPipeline;
            _createInventorySetCommand = createInventorySetCommand;
            _associateCatalogToInventorySetCommand = associateCatalogToInventorySetCommand;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity() as Sitecore.Commerce.Plugin.Catalog.Catalog;

            if (commerceEntity == null)
            {
                return await Task.FromResult(arg);
            }

            var inventorySetName = arg.ImportHandler.GetPropertyValueFromSource<InventorySetNameAttribute, string>();

            if (string.IsNullOrEmpty(inventorySetName))
            {
                return await Task.FromResult(arg);
            }

            var inventorySetExists = await _doesEntityExistPipeline
                .Run(new FindEntityArgument(typeof(InventorySet), inventorySetName.ToEntityId<InventorySet>()), context)
                .ConfigureAwait(false);

            if (!inventorySetExists)
            {
                await _createInventorySetCommand.Process(context.CommerceContext, inventorySetName, inventorySetName, string.Empty)
                    .ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(commerceEntity.DefaultInventorySetName)
                || !inventorySetName.Equals(commerceEntity.DefaultInventorySetName, StringComparison.OrdinalIgnoreCase))
            {
                await _associateCatalogToInventorySetCommand
                    .Process(context.CommerceContext, inventorySetName, commerceEntity.Name)
                    .ConfigureAwait(false);
            }

            return await Task.FromResult(arg);
        }
    }
}