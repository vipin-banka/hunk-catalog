using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Plugin.Hunk.Catalog.Policy;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.LogEntityImportResultBlock)]
    public class LogEntityImportResultBlock : PipelineBlock<LogEntityImportResultArgument, LogEntityImportResultArgument, CommercePipelineExecutionContext>
    {
        public override async Task<LogEntityImportResultArgument> Run(LogEntityImportResultArgument arg, CommercePipelineExecutionContext context)
        {
            var model = arg.ImportEntityCommand.Models.OfType<SourceEntityModel>().FirstOrDefault();

            StringBuilder message = new StringBuilder();

            if (model != null)
            {
                var errors = GetMessages(arg.ImportEntityCommand, context.GetPolicy<KnownResultCodes>().Error);
                var warnings = GetMessages(arg.ImportEntityCommand, context.GetPolicy<KnownResultCodes>().Warning);
                var result = errors != null && errors.Any() ? "Fail"
                    : warnings != null && warnings.Any() ? "SuccessWithWarnings" : "Success";

                message.AppendLine();
                message.Append(">>");
                message.Append(model.EntityType);
                message.Append(",");
                message.Append(model.Id);
                message.Append(",");
                message.Append(result);
                LogMessages(message, "Errors", errors);
                LogMessages(message, "Warnings", warnings);
                LogMessages(message, GetMissingReferences(arg.ImportEntityCommand));
                WriteLog(message, context);
            }

            return await Task.FromResult(arg);
        }

        private IList<string> GetMessages(CommerceCommand command, string code)
        {
            return command.Messages.Where(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).Select(x => x.Text).ToList();
        }

        private IList<MissingReferencesModel> GetMissingReferences(CommerceCommand command)
        {
            return command.Models.OfType<MissingReferencesModel>().ToList();
        }

        private void LogMessages(StringBuilder stringBuilder, string title, IList<string> messages)
        {
            if (messages != null && messages.Any())
            {
                stringBuilder.AppendLine();
                stringBuilder.Append("-->");
                stringBuilder.Append(title);
                stringBuilder.AppendLine();
                stringBuilder.Append("------------------------------");
                stringBuilder.AppendLine();
                foreach (var message in messages)
                {
                    stringBuilder.Append(message);
                    stringBuilder.AppendLine();
                }
                stringBuilder.Append("------------------------------");
            }
        }

        private void LogMessages(StringBuilder stringBuilder, IList<MissingReferencesModel> missingReferencesModels)
        {
            if (missingReferencesModels != null && missingReferencesModels.Any())
            {
                foreach (var missingReferencesModel in missingReferencesModels)
                {
                    if (missingReferencesModel.MissingReferences != null
                        && missingReferencesModel.MissingReferences.Any())
                    {
                        stringBuilder.AppendLine();
                        stringBuilder.Append("-->");
                        stringBuilder.Append(missingReferencesModel.Name);
                        var list = missingReferencesModel.MissingReferences.Aggregate(new StringBuilder(),
                            (s, i) => s.Append($"{i},"), s => s.ToString()).Trim(',');
                        stringBuilder.Append("=");
                        stringBuilder.Append(list);
                    }
                }
            }
        }


        private void WriteLog(StringBuilder stringBuilder, CommercePipelineExecutionContext context)
        {
            var fileImportPolicy = context.GetPolicy<FileImportPolicy>();
            var directoryInfo = GetDirectory(Path.GetFullPath(fileImportPolicy.RootFolder));
            var logFile = directoryInfo.GetFiles("log*.txt").FirstOrDefault();

            if (logFile == null)
            {
                File.WriteAllText(Path.Combine(directoryInfo.FullName, "Log.txt"), "");
                logFile = directoryInfo.GetFiles("log*.txt").FirstOrDefault();
            }

            if (logFile != null)
            {
                File.AppendAllText(logFile.FullName, stringBuilder.ToString());
            }
        }

        protected virtual DirectoryInfo GetDirectory(string rootFolderName)
        {
            return new DirectoryInfo(rootFolderName).GetDirectories()
                .OrderByDescending(d => d.LastWriteTimeUtc).FirstOrDefault();
        }
    }
}