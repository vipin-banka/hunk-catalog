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
            if (!this.Policy.WakeupInterval.HasValue)
                this.Logger.LogDebug(this.Name + " - Bulk import minion do not auto start.");
            else
            {
                return base.StartAsync();
            }

            return null;
        }

        protected override async Task<MinionRunResultsModel> Execute()
        {
            var catalogImportPolicy = MinionContext.GetPolicy<CatalogImportPolicy>();
            if (catalogImportPolicy.Mappings?.EntityMappings != null 
                && catalogImportPolicy.Mappings.EntityMappings.Any())
            {
                if (catalogImportPolicy.IgnoreIndexUpdates)
                {
                    string[] policyKeys = {
                        "IgnoreIndexDeletedSitecoreItem",
                        "IgnoreIndexUpdatedSitecoreItem",
                        "IgnoreAddEntityToIndexList"
                    };

                    MinionContext.AddPolicyKeys(policyKeys);
                }

                bool success = true;

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
                                var result = await instance.Import(sourceEntityDetail).ConfigureAwait(false);
                                success = result && !success;
                            }
                        }
                    }
                }
            }

            return new MinionRunResultsModel();
        }
    }
}