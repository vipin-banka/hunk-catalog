using Plugin.Hunk.Catalog.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class SellableItemImportHandler<TSourceEntity> : BaseEntityImportHandler<TSourceEntity, SellableItem>
        where TSourceEntity : IEntity
    {
        protected string ProductId { get; set; }

        protected string Name { get; set; }

        protected string DisplayName { get; set; }

        protected string Description { get; set; }

        protected string Brand { get; set; }

        protected string Manufacturer { get; set; }

        protected string TypeOfGood { get; set; }

        protected IList<string> Tags { get; set; }

        protected SellableItemImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(sourceEntity, commerceCommander, context)
        {
            Tags = new List<string>();
        }

        public override async Task<CommerceEntity> Create()
        {
            Initialize();
            var command = CommerceCommander.Command<CreateSellableItemCommand>();
            CommerceEntity = await command.Process(Context.CommerceContext, ProductId, Name, DisplayName, Description, Brand, Manufacturer, TypeOfGood, Tags.ToArray());
            return CommerceEntity;
        }

        public override bool HasVariants()
        {
            return HasVariants(SourceEntity);
        }

        public override IList<IEntity> GetVariants()
        {
            return GetVariants(SourceEntity);
        }

        public override bool HasVariants(ILanguageEntity languageEntity)
        {
            var languages = GetLanguages();
            if (languages != null
                && languages.Any())
            {
                var matchedLanguage = languages.FirstOrDefault(x =>
                    x.Language.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));
                if (matchedLanguage != null)
                {
                    var variants = GetVariants(languageEntity.GetEntity());
                    return variants != null && variants.GetEnumerator().MoveNext();
                }
            }

            return false;
        }

        public override IList<IEntity> GetVariants(ILanguageEntity languageEntity)
        {
            var languages = GetLanguages();
            if (languages != null
                && languages.Any())
            {
                var matchedLanguage = languages.FirstOrDefault(x =>
                    x.Language.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));
                if (matchedLanguage != null)
                {
                    return GetVariants(languageEntity.GetEntity());
                }
            }

            return new List<IEntity>();
        }
    }
}