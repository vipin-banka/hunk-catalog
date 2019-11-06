using System.Collections.Generic;

namespace Plugin.Hunk.Catalog.Policy
{
    public class FileImportPolicy : Sitecore.Commerce.Core.Policy
    {
        public FileImportPolicy()
        {
            ImportFileSettings = new List<ImportFileSetting>();
        }

        public string RootFolder { get; set; }

        public IList<ImportFileSetting> ImportFileSettings { get; set; }
    }

    public class ImportFileSetting
    {
        public string Key { get; set; }

        public string FileNamePattern { get; set; }
    }
}