using Sitecore.Commerce.Core;
using System.Collections.Generic;
using System.Linq;
using Plugin.Hunk.Catalog.Model;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ImportLocalizeContentArgument : PipelineArgument
    {
        public ImportLocalizeContentArgument(CommerceEntity commerceEntity, ImportEntityArgument importEntityArgument)
        {
            CommerceEntity = commerceEntity;
            ImportEntityArgument = importEntityArgument;
        }

        public CommerceEntity CommerceEntity { get; set; }

        public LocalizationEntity LocalizationEntity { get; set; }

        public IList<LocalizablePropertyValues> Properties { get; set; }

        public IList<LocalizableComponentPropertiesValues> ComponentsProperties { get; set; }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public bool HasLocalizationContent => (Properties != null && Properties.Any()) 
        || (ComponentsProperties != null && ComponentsProperties.Any());
    }
}