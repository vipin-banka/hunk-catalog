using Microsoft.Extensions.DependencyInjection;
using Plugin.Hunk.Catalog.Test.EntityViews;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using System.Reflection;

namespace Plugin.Hunk.Catalog.Test
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IRunningPluginsPipeline>(c =>
                {
                    c.Add<Pipelines.Blocks.RegisteredPluginBlock>()
                        .After<RunningPluginsBlock>();
                })
                .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                {
                    c.Add<SellableItemComponentViewBlock>()
                        .Add<VariantComponentViewBlock>();
                })
            );

            services.RegisterAllCommands(assembly);
        }
    }
}