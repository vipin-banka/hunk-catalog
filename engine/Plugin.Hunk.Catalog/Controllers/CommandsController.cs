using System;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plugin.Hunk.Catalog.Commands;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Controllers
{
    public class CommandsController : CommerceController
    {
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPut]
        [Route("ImportEntity()")]
        public async Task<IActionResult> ImportEntity([FromBody] ODataActionParameters value)
        {
            if (!ModelState.IsValid || value == null)
                return new BadRequestObjectResult(ModelState);

            if (!value.ContainsKey("metadata") || value["metadata"] == null)
                return new BadRequestObjectResult(value);

            if (!value.ContainsKey("entity") || value["entity"] == null)
                return new BadRequestObjectResult(value);

            var metadata = value["metadata"].ToString();
            var entity = value["entity"].ToString();
            var sourceEntity = JsonConvert.DeserializeObject<SourceEntityDetail>(metadata);
            sourceEntity.SerializedEntity = entity;

            var command = Command<ImportEntityCommand>();
            await command.Process(CurrentContext, sourceEntity);
            return new ObjectResult(command);
        }
    }
}