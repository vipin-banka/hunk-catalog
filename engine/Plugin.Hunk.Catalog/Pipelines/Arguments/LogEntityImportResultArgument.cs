using Plugin.Hunk.Catalog.Commands;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Pipelines.Arguments
{
    public class LogEntityImportResultArgument : PipelineArgument
    {
        public LogEntityImportResultArgument(ImportEntityCommand importEntityCommand)
        {
            ImportEntityCommand = importEntityCommand;
        }

        public ImportEntityCommand ImportEntityCommand { get; set; }
    }
}