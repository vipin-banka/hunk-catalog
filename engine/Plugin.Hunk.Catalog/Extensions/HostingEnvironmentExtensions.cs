using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static DirectoryInfo GetDefaultCustomCatalogContentDirectory(this IHostingEnvironment hostingEnvironment)
        {
            var path = Path.Combine(hostingEnvironment.WebRootPath, "data", "CustomCatalogs");
            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path);
            }

            return null;
        }
    }
}