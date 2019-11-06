using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Policy;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.BulkImport
{
    public abstract class BaseFileBulkImporter : BaseBulkImporter
    {
        protected BaseFileBulkImporter(IServiceProvider serviceProvider,
            CommerceContext commerceContext)
            : base(serviceProvider, commerceContext)
        {
        }

        public override async Task<bool> Import(SourceEntityDetail sourceEntityDetail)
        {
            var fileImportPolicy = CommerceContext.GetPolicy<FileImportPolicy>();

            if (!string.IsNullOrEmpty(fileImportPolicy.RootFolder))
            {
                var directoryInfo = GetDirectory(Path.GetFullPath(fileImportPolicy.RootFolder));

                if (directoryInfo != null)
                {
                    var setting = fileImportPolicy.ImportFileSettings.FirstOrDefault(x =>
                        x.Key.Equals(sourceEntityDetail.EntityType, StringComparison.OrdinalIgnoreCase));

                    if (setting != null)
                    {
                        var files = GetFiles(directoryInfo, setting.FileNamePattern);

                        foreach (var fileInfo in files)
                        {
                            await ImportFileContent(sourceEntityDetail, fileInfo);
                        }
                    }
                }
            }

            return true;
        }

        protected virtual DirectoryInfo GetDirectory(string rootFolderName)
        {
            return new DirectoryInfo(rootFolderName).GetDirectories()
                .OrderByDescending(d => d.LastWriteTimeUtc).FirstOrDefault();
        }

        protected virtual IList<FileInfo> GetFiles(DirectoryInfo directoryInfo, string fileNamePattern)
        {
            return directoryInfo.GetFiles(fileNamePattern, System.IO.SearchOption.TopDirectoryOnly);
        }

        protected abstract Task ImportFileContent(SourceEntityDetail sourceEntityDetail, FileInfo fileInfo);
    }
}