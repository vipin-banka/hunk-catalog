using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class BundleImportHandler<TSourceEntity> : SellableItemImportHandler<TSourceEntity>
        where TSourceEntity : IEntity
    {
        protected string BundleType { get; set; }

        protected IList<BundleItem> BundleItems { get; set; }

        protected BundleImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceEntity, commerceCommander, context)
        {
            BundleItems = new List<BundleItem>();
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command = CommerceCommander.Command<CreateBundleCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, BundleType, ProductId, Name, DisplayName, Description, Brand, Manufacturer, TypeOfGood, Tags.ToArray(), BundleItems);
            return CommerceEntity;
        }
    }
}