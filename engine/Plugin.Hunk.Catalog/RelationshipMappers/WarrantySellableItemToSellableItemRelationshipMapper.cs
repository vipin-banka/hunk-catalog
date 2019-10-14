using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.RelationshipMappers
{
    public class WarrantySellableItemToSellableItemRelationshipMapper : SellableItemRelationshipMapper
    {
        public WarrantySellableItemToSellableItemRelationshipMapper(CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
        : base(commerceCommander, context)
        { }

        public override string Name => "WarrantySellableItemToSellableItem";
    }
}