using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Commands
{
    public class ImportEntityCommand : CommerceCommand
    {
        public ImportEntityCommand(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public virtual async Task<CommerceEntity> Process(
            CommerceContext commerceContext,
            SourceEntityDetail sourceEntityDetail)
        {
            CommerceEntity result = null;
            using (CommandActivity.Start(commerceContext, this))
            {
                var importEntityArgument = new ImportEntityArgument(sourceEntityDetail);

                await PerformTransaction(commerceContext, async () => await Pipeline<IPrepareImportEntityPipeline>().Run(importEntityArgument, commerceContext.PipelineContextOptions));

                if (!IsError(commerceContext) && importEntityArgument.ImportHandler.IsEntityImport)
                {
                    // Create/Update entity, entity components, manage versions and workflow.
                    await PerformTransaction(commerceContext,
                        async () => result = await Pipeline<IImportEntityPipeline>()
                            .Run(importEntityArgument, commerceContext.PipelineContextOptions));
                }
                else
                {
                    result = importEntityArgument.ImportHandler.GetCommerceEntity();
                }

                if (result == null)
                {
                    await commerceContext.AddMessage(commerceContext.GetPolicy<KnownResultCodes>().Error, "EntityNotFound", null, "Entity does not exists, cannot continue with import.")
                        .ConfigureAwait(false);
                }

                if (result != null && !IsError(commerceContext))
                {
                    // Manage association of sellable item with catalog and categories.
                    await PerformTransaction(commerceContext, async () => await Pipeline<IAssociateParentsPipeline>().Run(importEntityArgument, commerceContext.PipelineContextOptions));

                    if (!IsError(commerceContext))
                    {
                        // Manage localization values for sellable item
                        var importLocalizeContentArgument =
                            new ImportLocalizeContentArgument(result, importEntityArgument);
                        await PerformTransaction(commerceContext, async () =>
                            await Pipeline<IImportLocalizeContentPipeline>().Run(
                                importLocalizeContentArgument, commerceContext.PipelineContextOptions));
                    }

                    if (!IsError(commerceContext))
                    {
                        // Manage default price book for catalog.
                        await PerformTransaction(commerceContext,
                            async () => await Pipeline<IAssociatePriceBookPipeline>().Run(importEntityArgument,
                                commerceContext.PipelineContextOptions));
                    }

                    if (!IsError(commerceContext))
                    {
                        // Manage default promotion book for catalog.
                        await PerformTransaction(commerceContext,
                            async () => await Pipeline<IAssociatePromotionBookPipeline>().Run(importEntityArgument,
                                commerceContext.PipelineContextOptions));
                    }

                    if (!IsError(commerceContext))
                    {
                        // Manage default inventory set for catalog.
                        await PerformTransaction(commerceContext,
                            async () => await Pipeline<IAssociateInventorySetPipeline>().Run(importEntityArgument,
                                commerceContext.PipelineContextOptions));
                    }

                    if (!IsError(commerceContext))
                    {
                        // Manage inventory information for variants.
                        await PerformTransaction(commerceContext,
                            async () => await Pipeline<IAssociateInventoryInformationPipeline>()
                                .Run(importEntityArgument, commerceContext.PipelineContextOptions));
                    }
                }
            }

            return await Task.FromResult(result);
        }

        private bool IsError(CommerceContext commerceContext)
        {
            return commerceContext.GetMessages().Any(m =>
                m.Code.Equals(commerceContext.GetPolicy<KnownResultCodes>().Error, StringComparison.OrdinalIgnoreCase));
        }
    }
}