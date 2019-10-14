using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.RelationshipMappers
{
    public class TrainingSellableItemToSellableItemRelationshipMapper : SellableItemRelationshipMapper
    {
        public TrainingSellableItemToSellableItemRelationshipMapper(CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
        : base(commerceCommander, context)
        { }

        public override string Name => "TrainingSellableItemToSellableItem";
    }
}