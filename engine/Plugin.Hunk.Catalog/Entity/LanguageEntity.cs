using Plugin.Hunk.Catalog.Abstractions;

namespace Plugin.Hunk.Catalog.Entity
{
    public class LanguageEntity<T> : ILanguageEntity<T>
    where T : IEntity
    {
        public string Language { get; set; }

        public IEntity GetEntity()
        {
            return Entity;
        }

        public T Entity { get; set; }
    }
}