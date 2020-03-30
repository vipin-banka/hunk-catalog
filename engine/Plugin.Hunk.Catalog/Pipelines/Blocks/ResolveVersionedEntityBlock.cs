using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.EntityVersions;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ResolveVersionedEntityBlock)]
    public class ResolveVersionedEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly FindEntityCommand _findEntityCommand;
        private readonly FindEntityVersionsCommand _findEntityVersionsCommand;
        private readonly AddEntityVersionCommand _addEntityVersionCommand;

        public ResolveVersionedEntityBlock(
            FindEntityCommand findEntityCommand,
            FindEntityVersionsCommand findEntityVersionsCommand,
            AddEntityVersionCommand addEntityVersionCommand)
        {
            _findEntityCommand = findEntityCommand;
            _findEntityVersionsCommand = findEntityVersionsCommand;
            _addEntityVersionCommand = addEntityVersionCommand;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            string entityId = arg.ImportHandler.EntityId;

            var entityToUpdate = arg.ImportHandler.GetCommerceEntity() ?? await _findEntityCommand
                                     .Process(context.CommerceContext, typeof(CommerceEntity), entityId)
                                     .ConfigureAwait(false);

            bool createNewVersion = entityToUpdate != null
                                    && arg.CatalogImportPolicy.EntityVersioningScheme == EntityVersioningScheme.CreateNewVersion;

            if (entityToUpdate != null
                && arg.CatalogImportPolicy.EntityVersioningScheme == EntityVersioningScheme.UpdateLatestUnpublished)
            {
                var entities = (await _findEntityVersionsCommand.Process(context.CommerceContext, typeof(CommerceEntity), entityToUpdate.Id).ConfigureAwait(false)).ToList();

                if (entities.Any())
                {
                    var unpublishedVersion = entities.Where(x => !x.Published).Max(x => x.Version);
                    if (unpublishedVersion <= 0 || entityToUpdate.EntityVersion != unpublishedVersion)
                    {
                        createNewVersion = true;
                    }
                }
            }

            if (createNewVersion)
            {
                var newVersion = entityToUpdate.EntityVersion + 1;
                await _addEntityVersionCommand.Process(context.CommerceContext, entityToUpdate, newVersion).ConfigureAwait(false);

                entityToUpdate = await _findEntityCommand.Process(context.CommerceContext, typeof(CommerceEntity), entityId, newVersion).ConfigureAwait(false);
            }

            if (entityToUpdate != null)
            {
                arg.IsNew = false;
                arg.ImportHandler.SetCommerceEntity(entityToUpdate);
            }

            return arg;
        }
    }
}