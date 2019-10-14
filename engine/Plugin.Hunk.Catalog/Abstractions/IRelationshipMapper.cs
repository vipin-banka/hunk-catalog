using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IRelationshipMapper
    {
        string Name { get; }

        Task<IList<string>> GetEntityIds(IList<string> ids);
    }
}