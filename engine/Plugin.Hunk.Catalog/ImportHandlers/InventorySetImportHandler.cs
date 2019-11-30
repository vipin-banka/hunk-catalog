using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Inventory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class InventorySetImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, InventorySet>
        where TSourceEntity : IEntity
    {
        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        protected InventorySetImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(sourceEntity, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command  = CommerceCommander.Command<CreateInventorySetCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, Name, DisplayName, Description);
            return CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
        }
    }
}