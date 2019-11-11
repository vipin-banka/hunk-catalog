using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.RelationshipMappers
{
    public abstract class BaseRelationshipMapper<T> : IRelationshipMapper
    {
        public CommerceCommander CommerceCommander { get; }

        public CommercePipelineExecutionContext Context { get; }

        public abstract string Name { get; }

        protected BaseRelationshipMapper(CommerceCommander commerceCommander,
            CommercePipelineExecutionContext context)
        {
            CommerceCommander = commerceCommander;
            Context = context;
        }

        public virtual async Task<IList<string>> GetEntityIds(IList<string> ids)
        {
            var entityIds = new List<string>();
            var missingReferences = new List<string>();
            if (ids != null && ids.Any())
            {
                foreach (var id in ids)
                {
                    var entityId = id.EnsurePrefix(CommerceEntity.IdPrefix<T>());
                    if (await DoesEntityExists(entityId))
                    {
                        entityIds.Add(entityId);
                    }
                    else
                    {
                        missingReferences.Add(id);
                    }
                }
            }

            if (missingReferences.Any())
            {
                Context.CommerceContext.AddModel(new MissingReferencesModel()
                {
                    Name = $"Missing-{Name}",
                    MissingReferences = missingReferences
                });
            }

            return entityIds;
        }

        protected virtual async Task<bool> DoesEntityExists(string entityId)
        {
            return await CommerceCommander.Pipeline<IDoesEntityExistPipeline>()
                .Run(new FindEntityArgument(typeof(T), entityId), Context)
                .ConfigureAwait(false);
        }
    }
}