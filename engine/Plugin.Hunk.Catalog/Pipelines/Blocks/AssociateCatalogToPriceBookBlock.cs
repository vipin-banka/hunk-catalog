using System;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateCatalogToPriceBookBlock)]
    public class AssociateCatalogToPriceBookBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly AddPriceBookCommand _addPriceBookCommand;
        private readonly AssociateCatalogToBookCommand _associateCatalogToBookCommand;

        public AssociateCatalogToPriceBookBlock(
            IDoesEntityExistPipeline doesEntityExistPipeline,
            AddPriceBookCommand addPriceBookCommand,
            AssociateCatalogToBookCommand associateCatalogToBookCommand)
        {
            _doesEntityExistPipeline = doesEntityExistPipeline;
            _addPriceBookCommand = addPriceBookCommand;
            _associateCatalogToBookCommand = associateCatalogToBookCommand;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            var commerceEntity = arg.ImportHandler.GetCommerceEntity() as Sitecore.Commerce.Plugin.Catalog.Catalog;

            if (commerceEntity == null)
            {
                return await Task.FromResult(arg);
            }

            var priceBookName = arg.ImportHandler.GetPropertyValueFromSource<PriceBookNameAttribute, string>();

            if (string.IsNullOrEmpty(priceBookName))
            {
                return await Task.FromResult(arg);
            }

            var priceBookExists = await _doesEntityExistPipeline
                .Run(new FindEntityArgument(typeof(PriceBook), priceBookName.ToEntityId<PriceBook>()), context)
                .ConfigureAwait(false);

            if (!priceBookExists)
            {
                await _addPriceBookCommand.Process(context.CommerceContext, priceBookName).ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(commerceEntity.PriceBookName)
                || !priceBookName.Equals(commerceEntity.PriceBookName, StringComparison.OrdinalIgnoreCase))
            {
                await _associateCatalogToBookCommand
                    .Process(context.CommerceContext, priceBookName, commerceEntity.Name)
                    .ConfigureAwait(false);
            }

            return await Task.FromResult(arg);
        }
    }
}