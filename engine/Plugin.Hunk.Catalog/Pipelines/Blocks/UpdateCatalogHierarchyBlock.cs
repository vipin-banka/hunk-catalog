using Plugin.Hunk.Catalog.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.UpdateCatalogHierarchyBlock)]
    public class UpdateCatalogHierarchyBlock : PipelineBlock<RelationshipArgument, 
        RelationshipArgument, 
        CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public UpdateCatalogHierarchyBlock(CommerceCommander commerceCommander)
        {
            _commerceCommander = commerceCommander;
        }

        public override async Task<RelationshipArgument> Run(RelationshipArgument arg,
            CommercePipelineExecutionContext context)
        {
            if (!(new string[4]
            {
                "CatalogToCategory",
                "CatalogToSellableItem",
                "CategoryToCategory",
                "CategoryToSellableItem"
            }).Contains<string>(arg.RelationshipType, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase))
                return arg;
            CatalogItemBase source = arg.SourceEntity as CatalogItemBase;
            if (source == null)
                source = await _commerceCommander.Pipeline<IFindEntityPipeline>()
                    .Run(new FindEntityArgument(typeof(CatalogItemBase), arg.SourceName, false), context)
                    .ConfigureAwait(false) as CatalogItemBase;

            List<Tuple<string, CatalogItemBase>> tupleList = new List<Tuple<string, CatalogItemBase>>();
            if (arg.TargetName.Contains("|"))
            {
                string targetName = arg.TargetName;
                char[] chArray = new char[1] { '|' };
                foreach (string str in targetName.Split(chArray))
                    tupleList.Add(new Tuple<string, CatalogItemBase>(str, null));
            }
            else
                tupleList.Add(new Tuple<string, CatalogItemBase>(arg.TargetName, arg.TargetEntity as CatalogItemBase));

            ValueWrapper<bool> sourceChanged = new ValueWrapper<bool>(false);
            foreach (Tuple<string, CatalogItemBase> tuple in tupleList)
            {
                CatalogItemBase catalogItemBase = tuple.Item2 ?? await _commerceCommander.Pipeline<IFindEntityPipeline>()
                                                      .Run(new FindEntityArgument(typeof(CatalogItemBase), tuple.Item1, false), context).ConfigureAwait(false) as CatalogItemBase;

                if (source != null && catalogItemBase != null)
                {
                    bool changed = false;
                    var parentEntitiesComponent = catalogItemBase.GetComponent<ParentEntitiesComponent>();

                    if (arg.Mode.HasValue)
                    {
                        if (arg.Mode.Value == RelationshipMode.Create)
                        {
                            if (!parentEntitiesComponent.EntityIds.Contains(source.Id))
                            {
                                parentEntitiesComponent.EntityIds.Add(source.Id);
                                changed = true;
                            }
                        }
                        else if (arg.Mode.Value == RelationshipMode.Delete)
                        {
                            if (parentEntitiesComponent.EntityIds.Contains(source.Id))
                            {
                                parentEntitiesComponent.EntityIds.Remove(source.Id);
                                changed = true;
                            }
                        }
                    }

                    if (changed)
                    {
                        PersistEntityArgument persistEntityArgument = await _commerceCommander
                            .Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(catalogItemBase), context)
                            .ConfigureAwait(false);
                    }
                }
            }

            return arg;
        }
    }
}