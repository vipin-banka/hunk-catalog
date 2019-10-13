using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ResolveEntityLocalizationMapperArgument : PipelineArgument
    {
        public ResolveEntityLocalizationMapperArgument(ImportEntityArgument importEntityArgument, ILanguageEntity languageEntity)
        {
            ImportEntityArgument = importEntityArgument;
            LanguageEntity = languageEntity;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public ILanguageEntity LanguageEntity { get; set; }
    }
}