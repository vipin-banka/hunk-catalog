using Newtonsoft.Json;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.BulkImport
{
    public abstract class BaseJsonFileBulkImporter<T> : BaseFileBulkImporter
    where T : IEntity
    {
        protected BaseJsonFileBulkImporter(IServiceProvider serviceProvider,
            CommerceContext commerceContext)
            : base(serviceProvider, commerceContext)
        {
        }

        protected override async Task ImportFileContent(SourceEntityDetail sourceEntityDetail, FileInfo fileInfo)
        {
            var content = File.ReadAllText(fileInfo.FullName);

            if (!string.IsNullOrEmpty(content))
            {
                var sourceEntities = JsonConvert.DeserializeObject<IList<T>>(content);
                if (sourceEntities != null && sourceEntities.Any())
                {
                    foreach (var sourceEntity in sourceEntities)
                    {
                        sourceEntityDetail.SerializedEntity = JsonConvert.SerializeObject(sourceEntity);
                        var command = await ImportContent(sourceEntityDetail)
                                .ConfigureAwait(false);

                        var sourceEntityModel = GetSourceEntityModel(sourceEntity, sourceEntityDetail);
                        if (sourceEntityModel != null)
                        {
                            command.Models.Add(sourceEntityModel);
                        }

                        var task = this.Log(command);
                        Task.WaitAll(task);
                    }
                }
            }
        }
    }
}