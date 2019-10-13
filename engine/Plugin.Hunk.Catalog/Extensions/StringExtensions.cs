using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class StringExtensions
    {
        public static string ToLocalizationEntityId(this string commerceEntityId)
        {
            return
                $"{CommerceEntity.IdPrefix<LocalizationEntity>()}{commerceEntityId.Replace("Entity-", string.Empty)}";
        }
    }
}