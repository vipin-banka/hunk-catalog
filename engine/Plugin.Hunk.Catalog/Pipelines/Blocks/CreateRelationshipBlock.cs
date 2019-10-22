using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.CreateRelationshipBlock)]
    public class CreateRelationshipBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public CreateRelationshipBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.ImportHandler.HasRelationships())
            {
                var relationships = arg.ImportHandler.GetRelationships();

                var commerceEntity = arg.ImportHandler.GetCommerceEntity();

                foreach (var relationshipDetail in relationships)
                {
                    if (!string.IsNullOrEmpty(relationshipDetail.Name))
                    {
                        var relationShipMapper = await _commerceCommander.Pipeline<IResolveRelationshipMapperPipeline>()
                            .Run(new ResolveRelationshipMapperArgument(arg, relationshipDetail.Name), context).ConfigureAwait(false);

                        if (relationShipMapper == null)
                            continue;

                        if (!string.IsNullOrEmpty(relationShipMapper.Name))
                        {
                            IList<string> targetIds = new List<string>();
                            if (relationshipDetail.Ids != null && relationshipDetail.Ids.Any())
                            {
                                targetIds = await relationShipMapper.GetEntityIds(relationshipDetail.Ids)
                                    .ConfigureAwait(false);
                            }

                            var listName = $"{relationShipMapper.Name}-{commerceEntity.FriendlyId}";

                            if (commerceEntity.EntityVersion > 1)
                            {
                                listName = $"{listName}-{commerceEntity.EntityVersion}";
                            }

                            IDictionary<string, IList<string>> dictionary = await _commerceCommander.Pipeline<IGetListsEntityIdsPipeline>().Run(new GetListsEntityIdsArgument(listName), context).ConfigureAwait(false);

                            var deleteIdsFromList = new List<string>();

                            if (dictionary != null && dictionary.Values.Any())
                            {
                                var firstList = dictionary.ElementAt(0);
                                foreach (var entityId in firstList.Value)
                                {
                                    if (targetIds != null
                                        && targetIds.Contains(entityId))
                                    {
                                        targetIds.Remove(entityId);
                                    }
                                    else
                                    {
                                        deleteIdsFromList.Add(entityId);
                                    }
                                }

                                if (targetIds != null
                                    && targetIds.Any())
                                {
                                    var targetName = targetIds.JoinIds();

                                    await _commerceCommander.Command<CreateRelationshipCommand>().Process(
                                        context.CommerceContext,
                                        arg.ImportHandler.GetCommerceEntity().Id,
                                        targetName, relationShipMapper.Name);
                                }
                            }

                            if (deleteIdsFromList.Any())
                            {
                                var targetName = deleteIdsFromList.JoinIds();

                                await _commerceCommander.Command<DeleteRelationshipCommand>().Process(
                                    context.CommerceContext,
                                    arg.ImportHandler.GetCommerceEntity().Id,
                                    targetName, relationShipMapper.Name);
                            }
                        }
                    }
                }
            }

            return await Task.FromResult(arg);
        }
    }
}