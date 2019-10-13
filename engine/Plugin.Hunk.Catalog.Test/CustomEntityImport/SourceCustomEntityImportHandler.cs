using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Test.CustomEntityImport
{
    public class SourceCustomEntityImportHandler : BaseEntityImportHandler<SourceCustomEntity, CustomCommerceItem>
    {
        public SourceCustomEntityImportHandler(string sourceProduct, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceProduct, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            var commerceEntity = new CustomCommerceItem();
            commerceEntity.Id = IdWithPrefix();
            commerceEntity.Name = SourceEntity.Name;
            commerceEntity.DisplayName = SourceEntity.DisplayName;
            commerceEntity.Description = SourceEntity.Description;

            await CommerceCommander.Pipeline<IPersistEntityPipeline>()
                .Run(new PersistEntityArgument(commerceEntity), Context).ConfigureAwait(false);

            return commerceEntity;
        }

        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
        }

        protected override void MapLocalizeValues(SourceCustomEntity localizedSourceEntity, CustomCommerceItem localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
            localizedTargetEntity.Description = localizedSourceEntity.Description;
        }
    }
}