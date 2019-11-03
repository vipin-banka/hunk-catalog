using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Pricing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class PriceCardImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, PriceCard>
        where TSourceEntity : IEntity
    {
        protected string BookName { get; set; }

        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        protected PriceCardImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(sourceEntity, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command  = CommerceCommander.Command<AddPriceCardCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, BookName, Name, DisplayName, Description);
            return CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
        }
    }
}