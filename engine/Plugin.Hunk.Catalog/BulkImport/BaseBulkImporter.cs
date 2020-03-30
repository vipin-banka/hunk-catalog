using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Commands;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.BulkImport
{
    public abstract class BaseBulkImporter : IEntityBulkImporter
    {
        protected readonly IServiceProvider ServiceProvider;

        protected readonly CommerceContext CommerceContext;

        protected BaseBulkImporter(IServiceProvider serviceProvider,
            CommerceContext commerceContext)
        {
            ServiceProvider = serviceProvider;
            CommerceContext = commerceContext;
        }

        public abstract Task<bool> Import(SourceEntityDetail sourceEntityDetail);

        protected virtual async Task<ImportEntityCommand> ImportContent(SourceEntityDetail sourceEntityDetail)
        {
            using (var newCommerceContext = new CommerceContext(CommerceContext.Logger, CommerceContext.TelemetryClient,
                CommerceContext.LocalizableMessagePipeline)
            {
                GlobalEnvironment = CommerceContext.GlobalEnvironment,
                Environment = CommerceContext.Environment,
                Headers = CommerceContext.Headers
            })
            {
                string[] policyKeys =
                {
                    "IgnoreCaching"
                };

                newCommerceContext.AddPolicyKeys(policyKeys);

                var command = ServiceProvider.GetService<ImportEntityCommand>();
                await command.Process(newCommerceContext, sourceEntityDetail)
                    .ConfigureAwait(false);

                return command;
            }
        }

        protected virtual SourceEntityModel GetSourceEntityModel<T>(T sourceEntity, SourceEntityDetail sourceEntityDetail)
            where T : IEntity
        {
            var entity = sourceEntity as IEntity;
            if (entity == null)
            {
                return null;
            }

            return new SourceEntityModel()
            {
                Id = entity.Id,
                EntityType = sourceEntityDetail.EntityType
            };
        }

        protected virtual async Task Log(ImportEntityCommand command)
        {
            var commercePipelineExecutionContextOptions = new CommercePipelineExecutionContextOptions(
                new CommerceContext(CommerceContext.Logger, CommerceContext.TelemetryClient)
                {
                    Environment = CommerceContext.Environment
                });

            await ServiceProvider.GetService<ILogEntityImportResultPipeline>()
                .Run(new LogEntityImportResultArgument(command), commercePipelineExecutionContextOptions)
                .ConfigureAwait(false);
        }
    }
}