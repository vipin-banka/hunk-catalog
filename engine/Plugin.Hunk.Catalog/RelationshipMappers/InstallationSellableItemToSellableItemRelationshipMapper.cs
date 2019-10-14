using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.RelationshipMappers
{
    public class InstallationSellableItemToSellableItemRelationshipMapper : SellableItemRelationshipMapper
    {
        public InstallationSellableItemToSellableItemRelationshipMapper(CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
        : base(commerceCommander, context)
        { }

        public override string Name => "InstallationSellableItemToSellableItem";
    }
}