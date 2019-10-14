using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ResolveEntityMapperArgument : PipelineArgument
    {
        public ResolveEntityMapperArgument(ImportEntityArgument importEntityArgument, CommerceEntity commerceEntity)
        {
            ImportEntityArgument = importEntityArgument;
            CommerceEntity = commerceEntity;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public CommerceEntity CommerceEntity { get; set; }
    }
}