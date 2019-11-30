using Plugin.Hunk.Catalog.Abstractions;

namespace Plugin.Hunk.Catalog.Test.PromotionEntityImport
{
    public class SourcePromotion : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string BookName { get; set; }

        public System.DateTimeOffset ValidFrom { get; set; }

        public System.DateTimeOffset ValidTo { get; set; }

        public string Text { get; set; }

        public string CartText { get; set; }

        public bool IsExclusive { get; set; }
    }
}