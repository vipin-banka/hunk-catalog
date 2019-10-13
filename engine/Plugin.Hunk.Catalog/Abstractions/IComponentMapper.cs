using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IComponentMapper
    {
        ComponentAction GetComponentAction();

        Component Map();

        Component Remove();
    }
}