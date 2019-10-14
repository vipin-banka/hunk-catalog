using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.RelationshipMappers
{
    public abstract class SellableItemRelationshipMapper : BaseRelationshipMapper<SellableItem>
    {
        protected SellableItemRelationshipMapper(CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
        : base(commerceCommander, context)
        { }
    }
}