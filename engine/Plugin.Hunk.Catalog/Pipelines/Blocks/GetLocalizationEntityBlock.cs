using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetLocalizationEntityBlock)]
    public class GetLocalizationEntityBlock : PipelineBlock<ImportLocalizeContentArgument, ImportLocalizeContentArgument, CommercePipelineExecutionContext>
    {
        private readonly IFindEntityPipeline _findEntityPipeline;
        public GetLocalizationEntityBlock(IFindEntityPipeline findEntityPipeline)
        {
            _findEntityPipeline = findEntityPipeline;
        }

        public override async Task<ImportLocalizeContentArgument> Run(ImportLocalizeContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.CommerceEntity != null 
                && arg.HasLocalizationContent)
            {
                LocalizedEntityComponent localizationComponent = arg.CommerceEntity.GetComponent<LocalizedEntityComponent>();

                if (localizationComponent != null)
                {
                    if (!string.IsNullOrEmpty(localizationComponent.Entity?.EntityTarget))
                    {
                        var localizationEntityId = localizationComponent.Entity?.EntityTarget;
                        var localizationEntity = await _findEntityPipeline.Run(new FindEntityArgument(typeof(LocalizationEntity), localizationEntityId, arg.CommerceEntity.EntityVersion), context).ConfigureAwait(false);

                        arg.LocalizationEntity = localizationEntity as LocalizationEntity;
                    }
                }

            }

            return await Task.FromResult(arg);
        }
    }
}