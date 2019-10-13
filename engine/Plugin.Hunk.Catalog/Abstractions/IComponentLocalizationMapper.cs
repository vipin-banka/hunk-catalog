using Plugin.Hunk.Catalog.Model;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IComponentLocalizationMapper
    {
        LocalizableComponentPropertiesValues Map(ILanguageEntity languageEntity, LocalizableComponentPropertiesValues localizableComponentPropertiesValues);
    }
}