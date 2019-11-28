using Plugin.Hunk.Catalog.Abstractions;

namespace Plugin.Hunk.Catalog.Test.PromotionBookEntityImport
{
    public class SourcePromotionBook : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}