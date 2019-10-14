using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ResolveRelationshipMapperArgument : PipelineArgument
    {
        public ResolveRelationshipMapperArgument(ImportEntityArgument importEntityArgument, string relationshipName)
        {
            ImportEntityArgument = importEntityArgument;
            RelationshipName = relationshipName;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }

        public string RelationshipName { get; set; }
    }
}