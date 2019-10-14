using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Text;
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

                foreach (var relationshipDetail in relationships)
                {
                    if (!string.IsNullOrEmpty(relationshipDetail.Name) && relationshipDetail.Ids != null &&
                        relationshipDetail.Ids.Any())
                    {
                        var relationShipMapper = await _commerceCommander.Pipeline<IResolveRelationshipMapperPipeline>()
                            .Run(new ResolveRelationshipMapperArgument(arg, relationshipDetail.Name), context).ConfigureAwait(false);

                        if (relationShipMapper == null)
                            continue;
                        
                        if (!string.IsNullOrEmpty(relationShipMapper.Name))
                        {
                            var targetIds = await relationShipMapper.GetEntityIds(relationshipDetail.Ids).ConfigureAwait(false);
                            if (targetIds.Any())
                            {
                                var targetName = targetIds.Aggregate(new StringBuilder(),
                                    (sb, id) => sb.Append(id).Append("|"), sb => sb.ToString().Trim('|'));

                                await _commerceCommander.Command<CreateRelationshipCommand>().Process(context.CommerceContext,
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