using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Promotions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class PromotionBookImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, PromotionBook>
        where TSourceEntity : IEntity
    {
        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        protected string ParentBook { get; set; }

        protected PromotionBookImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(sourceEntity, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command  = CommerceCommander.Command<AddPromotionBookCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, Name, DisplayName, Description, ParentBook);
            return CommerceEntity;
        }

        public override IList<string> GetParentList()
        {
            return new List<string>();
        }
    }
}