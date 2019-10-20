using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class CustomEntityImportHandler<TSourceEntity, TCommerceEntity> : BaseEntityImportHandler<TSourceEntity, TCommerceEntity>
        where TSourceEntity : IEntity
        where TCommerceEntity : CommerceEntity, new()
    {
        protected CustomEntityImportHandler(string sourceEntity, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(sourceEntity, commerceCommander, context)
        { }

        public override async Task<CommerceEntity> Create()
        {
            CommerceEntity = new TCommerceEntity();
            CommerceEntity.Id = IdWithPrefix();
            Initialize();
            if (CommerceEntity == null)
            {
            }

            await CommerceCommander.Pipeline<IPersistEntityPipeline>()
                    .Run(new PersistEntityArgument(CommerceEntity), Context).ConfigureAwait(false);

            return CommerceEntity;
        }
    }
}