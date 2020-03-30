using System;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateCatalogToPromotionBookBlock)]
    public class AssociateCatalogToPromotionBookBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly AddPromotionBookCommand _addPromotionBookCommand;
        private readonly AssociateCatalogToBookCommand _associateCatalogToBookCommand;

        public AssociateCatalogToPromotionBookBlock(
            IDoesEntityExistPipeline doesEntityExistPipeline,
            AddPromotionBookCommand addPromotionBookCommand,
            AssociateCatalogToBookCommand associateCatalogToBookCommand)
        {
            _doesEntityExistPipeline = doesEntityExistPipeline;
            _addPromotionBookCommand = addPromotionBookCommand;
            _associateCatalogToBookCommand = associateCatalogToBookCommand;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity() as Sitecore.Commerce.Plugin.Catalog.Catalog;

            if (commerceEntity == null)
            {
                return await Task.FromResult(arg);
            }

            var promotionBookName = arg.ImportHandler.GetPropertyValueFromSource<PromotionBookNameAttribute, string>();

            if (string.IsNullOrEmpty(promotionBookName))
            {
                return await Task.FromResult(arg);
            }

            var promotionBookExists = await _doesEntityExistPipeline
                .Run(new FindEntityArgument(typeof(PromotionBook), promotionBookName.ToEntityId<PromotionBook>()), context)
                .ConfigureAwait(false);

            if (!promotionBookExists)
            {
                await _addPromotionBookCommand.Process(context.CommerceContext, promotionBookName)
                    .ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(commerceEntity.PromotionBookName)
                || !promotionBookName.Equals(commerceEntity.PromotionBookName, StringComparison.OrdinalIgnoreCase))
            {
                await _associateCatalogToBookCommand
                    .Process(context.CommerceContext, promotionBookName, commerceEntity.Name)
                    .ConfigureAwait(false);
            }

            return await Task.FromResult(arg);
        }
    }
}