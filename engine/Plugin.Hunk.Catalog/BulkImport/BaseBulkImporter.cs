using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Commands;
using Plugin.Hunk.Catalog.Model;
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
            CommerceContext.ClearEntities();
            CommerceContext.ClearMessages();
            CommerceContext.ClearModels();
            CommerceContext.ClearObjects();
            
            var command = ServiceProvider.GetService<ImportEntityCommand>();
            await command.Process(CommerceContext, sourceEntityDetail)
                .ConfigureAwait(false);
            return command;
        }
    }
}