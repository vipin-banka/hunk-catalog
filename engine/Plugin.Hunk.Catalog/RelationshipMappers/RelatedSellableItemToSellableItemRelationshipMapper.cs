using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.RelationshipMappers
{
    public class RelatedSellableItemToSellableItemRelationshipMapper : SellableItemRelationshipMapper
    {
        public RelatedSellableItemToSellableItemRelationshipMapper(CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
        : base(commerceCommander, context)
        { }

        public override string Name => "RelatedSellableItemToSellableItem";
    }
}