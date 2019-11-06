using Plugin.Hunk.Catalog.Commands;
using Plugin.Hunk.Catalog.Model;
using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IEntityBulkImporter
    {
        Task<bool> Import(SourceEntityDetail sourceEntityDetail);
    }
}