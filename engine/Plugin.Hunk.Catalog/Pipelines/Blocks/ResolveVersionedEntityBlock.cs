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
        private readonly AddEntityVersionCommand _addEntityVersionCommand;

        public ResolveVersionedEntityBlock(
            FindEntityCommand findEntityCommand,
            AddEntityVersionCommand addEntityVersionCommand)
        {
            _findEntityCommand = findEntityCommand;
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
                VersioningEntity versioningEntity = await _findEntityCommand.Process(context.CommerceContext, typeof(VersioningEntity), VersioningEntity.GetIdBasedOnEntityId(entityToUpdate.Id), new int?()).ConfigureAwait(false) as VersioningEntity;

                if (versioningEntity?.Versions != null && versioningEntity.Versions.Any())
                {
                    var unpublishedVersion = versioningEntity.Versions.Where(x => !x.Published).Max(x => x.Version);
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