using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Model;
using Plugin.Hunk.Catalog.Policy;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Minions
{
    public class BulkImportMinion : Minion
    {
        private IServiceProvider _serviceProvider;

        public override void Initialize(
            IServiceProvider serviceProvider,
            MinionPolicy policy,
            CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, policy, globalContext);
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync()
        {
            this.Logger.LogDebug(this.Name + " - Bulk import minion do not auto start");
            return (Task)null;
        }

        protected override async Task<MinionRunResultsModel> Execute()
        {
            var catalogImportPolicy = MinionContext.GetPolicy<CatalogImportPolicy>();
            if (catalogImportPolicy.Mappings != null && catalogImportPolicy.Mappings.EntityMappings != null 
                    && catalogImportPolicy.Mappings.EntityMappings.Any())
            {
                foreach (var entityMapping in catalogImportPolicy.Mappings.EntityMappings)
                {
                    if (!string.IsNullOrEmpty(entityMapping.Key) 
                        && !string.IsNullOrEmpty(entityMapping.BulkImporterTypeName))
                    {
                        var sourceEntityDetail = new SourceEntityDetail()
                        {
                            EntityType = entityMapping.Key,
                            Components = entityMapping.Components ?? new List<string>(),
                            VariantComponents = catalogImportPolicy.VariantComponents ?? new List<string>()
                        };

                        var t = Type.GetType(entityMapping.BulkImporterTypeName);
                        if (t != null)
                        {
                            if (Activator.CreateInstance(t, _serviceProvider, MinionContext) is IEntityBulkImporter
                                instance)
                            {
                                await instance.Import(sourceEntityDetail).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }

            MinionContext.ClearEntities();
            MinionContext.ClearMessages();
            MinionContext.ClearModels();
            MinionContext.ClearObjects();
            return new MinionRunResultsModel();
        }
    }
}