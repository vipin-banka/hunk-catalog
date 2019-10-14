namespace Plugin.Hunk.Catalog.Model
{
    public enum EntityVersioningScheme
    {
        CreateNewVersion = 0,
        UpdateLatestUnpublished = 1,
        UpdateLatest = 2
    }
}