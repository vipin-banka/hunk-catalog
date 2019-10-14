using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ResolveComponentMapperArgument : PipelineArgument
    {
        public ResolveComponentMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity, string componentName)
            : this(importEntityArgument, commerceEntity, null, null, componentName)
        {
        }

        public ResolveComponentMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity, Component parentComponent, object sourceComponent, string componentName)
        {
            ImportEntityArgument = importEntityArgument;
            CommerceEntity = commerceEntity;
            ComponentName = componentName;
            ParenComponent = parentComponent;
            SourceComponent = sourceComponent;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public CommerceEntity CommerceEntity { get; set; }

        public string ComponentName { get; set; }

        public Component ParenComponent { get; set; }

        public object SourceComponent { get; set; }
    }
}