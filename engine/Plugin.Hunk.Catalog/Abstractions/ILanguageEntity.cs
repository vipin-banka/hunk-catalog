namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface ILanguageEntity
    {
        string Language { get; set; }

        IEntity GetEntity();
    }

    public interface ILanguageEntity<T> : ILanguageEntity
    where T : IEntity
    {
        T Entity { get; set; }
    }
}