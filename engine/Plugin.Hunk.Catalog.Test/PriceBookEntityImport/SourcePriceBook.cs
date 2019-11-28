using Plugin.Hunk.Catalog.Abstractions;

namespace Plugin.Hunk.Catalog.Test.PriceBookEntityImport
{
    public class SourcePriceBook : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}