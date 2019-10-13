using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class ResolveEntityImportHandlerArgument : PipelineArgument
    {
        public ResolveEntityImportHandlerArgument(ImportEntityArgument importEntityArgument)
        {
            ImportEntityArgument = importEntityArgument;
        }

        public ImportEntityArgument ImportEntityArgument { get; set; }
    }
}