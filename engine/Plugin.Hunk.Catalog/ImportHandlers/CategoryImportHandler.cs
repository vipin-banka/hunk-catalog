using System.Linq;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class CategoryImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, Category>
        where TSourceEntity : IEntity
    {
        protected string CatalogId { get; set; }

        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        protected CategoryImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceEntity, commerceCommander, context)
        {
        }

        protected override string Id
        {
            get
            {
                var firstParent = ParentEntityIds.FirstOrDefault();
                string catalogId = firstParent.Key.SimplifyEntityName();
                return SourceEntity.Id.ToCategoryId(catalogId);
            }
        }

        public override async Task<CommerceEntity> Create()
        {
            if (ParentEntityIds == null || !ParentEntityIds.Any())
            {
                Context.Abort(await Context.CommerceContext.AddMessage(Context.GetPolicy<KnownResultCodes>().Error, "CategoryCatalogNotDefined", null, "Catalog must be defined to create a new category."), Context);
                return CommerceEntity;
            }

            var firstParent = ParentEntityIds.FirstOrDefault();
            CatalogId = firstParent.Key;

            Initialize();
            var command = CommerceCommander.Command<CreateCategoryCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, CatalogId, Name, DisplayName, Description);
            return CommerceEntity;
        }
    }
}