using Plugin.Hunk.Catalog.ImportHandlers;
using Sitecore.Commerce.Core;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Test.CustomEntityImport
{
    /// <inheritdoc />
    public class SourceCustomEntityImportHandler : CustomEntityImportHandler<SourceCustomEntity, CustomCommerceItem>
    {
        /// <inheritdoc />
        public SourceCustomEntityImportHandler(string sourceProduct, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceProduct, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
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