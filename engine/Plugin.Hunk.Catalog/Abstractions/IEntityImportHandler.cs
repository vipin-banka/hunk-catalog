using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.Abstractions
{
    public interface IEntityImportHandler
    {
        string EntityId { get; }

        IDictionary<string, IList<string>> ParentEntityIds { get; set; }

        Task<CommerceEntity> Create();

        IEntity GetSourceEntity();

        bool Validate();

        CommerceEntity GetCommerceEntity();

        void SetCommerceEntity(CommerceEntity commerceEntity);

        IList<string> GetParentList();

        bool HasVariants();

        IList<IEntity> GetVariants();

        bool HasLanguages();

        IList<ILanguageEntity> GetLanguages();

        bool HasVariants(ILanguageEntity languageEntity);

        IList<IEntity> GetVariants(ILanguageEntity languageEntity);

        bool HasRelationships();

        IList<RelationshipDetail> GetRelationships();

        TOutput GetPropertyValueFromSource<TMetadata, TOutput>() 
            where TMetadata : Attribute
            where TOutput : class;

        bool IsEntityImport { get; }

        void BeforePersistEntity();

        void AfterPersistEntity();
    }

    public interface IEntityImportHandler<TCommerceEntity>
    where TCommerceEntity : CommerceEntity
    {
        TCommerceEntity CommerceEntity { get; set; }
    }
}