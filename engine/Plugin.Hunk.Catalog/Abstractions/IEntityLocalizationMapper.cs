using System.Collections.Generic;
using Plugin.Hunk.Catalog.Model;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IEntityLocalizationMapper
    {
        IList<LocalizablePropertyValues> Map(ILanguageEntity languageEntity, IList<LocalizablePropertyValues> entityLocalizableProperties);
    }
}