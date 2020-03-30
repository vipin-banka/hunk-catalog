using Plugin.Hunk.Catalog.Abstractions;

namespace Plugin.Hunk.Catalog.Test.PriceCardEntityImport
{
    public class SourcePriceCard : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string BookName { get; set; }
    }
}