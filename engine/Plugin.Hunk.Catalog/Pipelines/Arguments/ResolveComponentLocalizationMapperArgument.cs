using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ResolveComponentLocalizationMapperArgument : PipelineArgument
    {
        public ResolveComponentLocalizationMapperArgument(ImportEntityArgument importEntityArgument,
            CommerceEntity commerceEntity, Component component, ILanguageEntity languageEntity)
        : this(importEntityArgument, commerceEntity, component, languageEntity, null)
        {
        }
        public ResolveComponentLocalizationMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity, Component component, ILanguageEntity languageEntity, IEntity sourceComponent)
        {
            ImportEntityArgument = importEntityArgument;
            LanguageEntity = languageEntity;
            CommerceEntity = commerceEntity;
            Component = component;
            SourceComponent = sourceComponent;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public ILanguageEntity LanguageEntity { get; set; }

        public CommerceEntity CommerceEntity { get; set; }

        public Component Component { get; set; }

        public IEntity SourceComponent { get; set; }
    }
}