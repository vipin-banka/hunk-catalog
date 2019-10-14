using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class CatalogImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, Sitecore.Commerce.Plugin.Catalog.Catalog>
        where TSourceEntity : IEntity
    {
        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected CatalogImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(sourceEntity, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command  = CommerceCommander.Command<CreateCatalogCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, Name, DisplayName);
            return CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
        }
    }
}