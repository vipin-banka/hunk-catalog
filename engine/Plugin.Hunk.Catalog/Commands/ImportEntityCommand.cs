using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
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

                // Create/Update entity, entity components, manage versions and workflow.
                await PerformTransaction(commerceContext, async () => result = await Pipeline<IImportEntityPipeline>().Run(importEntityArgument, commerceContext.PipelineContextOptions));

                if (result != null)
                {
                    // Manage association of sellable item with catalog and categories.
                    await PerformTransaction(commerceContext, async () => await Pipeline<IAssociateParentsPipeline>().Run(importEntityArgument, commerceContext.PipelineContextOptions));

                    // Manage localization values for sellable item
                    var importLocalizeContentArgument =
                        new ImportLocalizeContentArgument(result, importEntityArgument);
                    await PerformTransaction(commerceContext, async () => await Pipeline<IImportLocalizeContentPipeline>().Run(
                        importLocalizeContentArgument, commerceContext.PipelineContextOptions));

                    // Manage default price book for catalog.
                    await PerformTransaction(commerceContext, async () => await Pipeline<IAssociatePriceBookPipeline>().Run(importEntityArgument, commerceContext.PipelineContextOptions));

                    // Manage default promotion book for catalog.
                    await PerformTransaction(commerceContext, async () => await Pipeline<IAssociatePromotionBookPipeline>().Run(importEntityArgument, commerceContext.PipelineContextOptions));

                    // Manage default inventory set for catalog.
                    await PerformTransaction(commerceContext, async () => await Pipeline<IAssociateInventorySetPipeline>().Run(importEntityArgument, commerceContext.PipelineContextOptions));
                }
            }

            return await Task.FromResult(result);
        }
    }
}