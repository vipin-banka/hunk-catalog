using Plugin.Hunk.Catalog.Model;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IEntityBulkImporter
    {
        Task<bool> Import(SourceEntityDetail sourceEntityDetail);
    }
}