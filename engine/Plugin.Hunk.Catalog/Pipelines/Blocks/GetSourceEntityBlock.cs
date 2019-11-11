using Plugin.Hunk.Catalog.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Model;

namespace Plugin.Hunk.Catalog.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetSourceEntityBlock)]
    public class GetSourceEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;

        public GetSourceEntityBlock(
            IDoesEntityExistPipeline doesEntityExistPipeline)
        {
            _doesEntityExistPipeline = doesEntityExistPipeline;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            arg.SourceEntity = arg.ImportHandler.GetSourceEntity();
            if (arg.SourceEntity == null)
            {
                context.Abort(await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "SourceEntityMissing", null, $"Source entity missing for entityType={arg.SourceEntityDetail.EntityType}."), context);
            }

            var parentList = arg.ImportHandler.GetParentList();

            if (parentList != null && parentList.Any())
            {
                arg.ImportHandler.ParentEntityIds = await GetParentEntities(parentList, context)
                    .ConfigureAwait(false);
            }

            return arg;
        }

        private async Task<IDictionary<string, IList<string>>> GetParentEntities(IList<string> parents, CommercePipelineExecutionContext context)
        {
            var result = new Dictionary<string, IList<string>>();
            var missingReferences = new List<string>();
            var separators = new List<string> { "/" }.ToArray();
            foreach (var parentItem in parents)
            {
                var names = parentItem.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                var catalogId = await GetCatalogId(names[0], context);
                if (!string.IsNullOrEmpty(catalogId))
                {
                    if (!result.ContainsKey(catalogId))
                    {
                        result.Add(catalogId, new List<string>());
                    }

                    if (names.Length <= 1)
                    {
                        if (!result[catalogId].Contains(catalogId))
                        {
                            result[catalogId].Add(catalogId);
                        }

                        continue;
                    }

                    var categoryId = await GetCategoryId(catalogId, names[names.Length - 1], context);
                    if (!string.IsNullOrEmpty(categoryId))
                    {
                        if (!result[catalogId].Contains(categoryId))
                        {
                            result[catalogId].Add(categoryId);
                        }
                    }
                    else
                    {
                        missingReferences.Add(parentItem);
                    }
                }
                else
                {
                    missingReferences.Add(parentItem);
                }
            }

            if (missingReferences.Any())
            {
                context.CommerceContext.AddModel(new MissingReferencesModel()
                    { Name = "Missing-Parents", MissingReferences = missingReferences });
            }

            return result;
        }

        private async Task<string> GetCatalogId(string catalogName, CommercePipelineExecutionContext context)
        {
            var catalogId = catalogName.ToEntityId<Sitecore.Commerce.Plugin.Catalog.Catalog>();
            var result = await context.ValidateEntityId(_doesEntityExistPipeline, typeof(Sitecore.Commerce.Plugin.Catalog.Catalog),
                catalogId);
            return await Task.FromResult(result);
        }

        private async Task<string> GetCategoryId(string catalogId, string categoryName, CommercePipelineExecutionContext context)
        {
            var categoryId = categoryName.ToCategoryId(catalogId.SimplifyEntityName());
            var result = await context.ValidateEntityId(_doesEntityExistPipeline, typeof(Category), categoryId);
            return await Task.FromResult(result);
        }
    }
}