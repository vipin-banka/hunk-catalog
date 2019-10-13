using System;
using System.Linq;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class CommerceEntityExtensions
    {
        public static ItemVariationComponent GetVariation(
            this CommerceEntity instance,
            string variationId)
        {
            if (instance == null || !instance.HasComponent<ItemVariationsComponent>() || string.IsNullOrEmpty(variationId))
                return null;
            return instance.GetComponent<ItemVariationsComponent>().ChildComponents.OfType<ItemVariationComponent>().FirstOrDefault(x => x.Id.Equals(variationId, StringComparison.OrdinalIgnoreCase));
        }
    }
}