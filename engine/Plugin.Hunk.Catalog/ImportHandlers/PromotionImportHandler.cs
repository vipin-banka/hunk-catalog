using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Pricing;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sitecore.Commerce.Plugin.Promotions;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class PromotionImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, Promotion>
        where TSourceEntity : IEntity
    {
        protected string BookName { get; set; }

        protected string Name { get; set; }

        protected System.DateTimeOffset ValidFrom { get; set; }

        protected System.DateTimeOffset ValidTo { get; set; }

        protected string Text { get; set; }

        protected string CartText { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        protected bool IsExclusive { get; set; }

        protected PromotionImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(sourceEntity, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command  = CommerceCommander.Command<AddPromotionCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, BookName, Name, ValidFrom, ValidTo, Text, CartText, DisplayName, Description, IsExclusive);
            return CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
        }
    }
}