using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.SetLocalizePropertiesBlock)]
    public class SetLocalizePropertiesBlock : PipelineBlock<ImportLocalizeContentArgument, ImportLocalizeContentArgument, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public SetLocalizePropertiesBlock(
            IPersistEntityPipeline persistEntityPipeline)
        {
            _persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<ImportLocalizeContentArgument> Run(ImportLocalizeContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (!arg.HasLocalizationContent || arg.LocalizationEntity == null)
                return arg;

            if (arg.Properties != null && arg.Properties.Any())
            {
                foreach (var property in arg.Properties)
                {
                    arg.LocalizationEntity.AddOrUpdatePropertyValue(property.PropertyName, property.Parameters.ToList());
                }
            }

            if (arg.ComponentsProperties != null && arg.ComponentsProperties.Any())
            {
                foreach (var componentProperty in arg.ComponentsProperties.Where(x=>!string.IsNullOrEmpty(x.Id)))
                {
                    foreach (var property in componentProperty.PropertyValues)
                    {
                        arg.LocalizationEntity.AddOrUpdateComponentValue(componentProperty.Path, componentProperty.Id, property.PropertyName, property.Parameters.ToList());
                    }
                }
            }

            await _persistEntityPipeline.Run(new PersistEntityArgument(arg.LocalizationEntity), context).ConfigureAwait(false);
            
            return await Task.FromResult(arg);
        }
    }
}