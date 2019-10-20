using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class ListExtensions
    {
        public static string JoinIds(this IList<string> ids)
        {
            return ids.Aggregate(new StringBuilder(),
                (sb, id) => sb.Append(id).Append("|"), sb => sb.ToString().Trim('|'));
        }
    }
}