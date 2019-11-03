using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Hunk.Catalog.Abstractions;
using Plugin.Hunk.Catalog.Extensions;
using Plugin.Hunk.Catalog.Metadata;
using Plugin.Hunk.Catalog.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Hunk.Catalog.ImportHandlers
{
    public abstract class BaseEntityImportHandler<TSourceEntity, TCommerceEntity> : IEntityImportHandler, IEntityImportHandler<TCommerceEntity>, IEntityMapper, IEntityLocalizationMapper
    where TSourceEntity : IEntity
    where TCommerceEntity : CommerceEntity
    {
        public TSourceEntity SourceEntity { get; }

        public TCommerceEntity CommerceEntity { get; set; }

        public CommerceCommander CommerceCommander { get; }

        public CommercePipelineExecutionContext Context { get; }

        public IDictionary<string, IList<string>> ParentEntityIds { get; set; }

        protected BaseEntityImportHandler(string sourceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        {
            SourceEntity = JsonConvert.DeserializeObject<TSourceEntity>(sourceEntity);
            CommerceCommander = commerceCommander;
            Context = context;
        }

        public string EntityId => IdWithPrefix();

        public IEntity GetSourceEntity()
        {
            return SourceEntity;
        }

        public virtual bool Validate()
        {
            return true;
        }

        public CommerceEntity GetCommerceEntity()
        {
            return CommerceEntity;
        }

        public void SetCommerceEntity(CommerceEntity commerceEntity)
        {
            CommerceEntity = commerceEntity as TCommerceEntity;
        }

        protected virtual string Id => SourceEntity.Id;

        protected string IdWithPrefix()
        {
            var prefix = Sitecore.Commerce.Core.CommerceEntity.IdPrefix<TCommerceEntity>();
            var id = Id;
            if (!id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return $"{prefix}{id}";
            }

            return id;
        }

        protected virtual void Initialize()
        {
        }

        public abstract Task<CommerceEntity> Create();

        public virtual IList<string> GetParentList()
        {
            return typeof(TSourceEntity).GetPropertyValueWithAttribute<ParentsAttribute, IList<string>>(SourceEntity);
        }

        public virtual bool HasLanguages()
        {
            var languages = typeof(TSourceEntity).GetPropertyValueWithAttribute<LanguagesAttribute, IEnumerable>(SourceEntity);
            return languages != null && languages.GetEnumerator().MoveNext();
        }

        public virtual IList<ILanguageEntity> GetLanguages()
        {
            var languages = typeof(TSourceEntity).GetPropertyValueWithAttribute<LanguagesAttribute, IEnumerable>(SourceEntity);
            if (languages != null)
            {
                return languages.Cast<ILanguageEntity>().ToList();
            }

            return new List<ILanguageEntity>();
        }

        public virtual bool HasVariants()
        {
            return false;
        }

        protected bool HasVariants(object instance)
        {
            var variants = typeof(TSourceEntity).GetPropertyValueWithAttribute<VariantsAttribute, IEnumerable>(instance);
            return variants != null && variants.GetEnumerator().MoveNext();
        }

        public virtual IList<IEntity> GetVariants()
        {
            return new List<IEntity>();
        }

        protected IList<IEntity> GetVariants(object instance)
        {
            var variants = typeof(TSourceEntity).GetPropertyValueWithAttribute<VariantsAttribute, IEnumerable>(instance);

            if (variants != null)
            {
                return variants.Cast<IEntity>().ToList();
            }

            return new List<IEntity>();
        }

        public virtual bool HasVariants(ILanguageEntity languageEntity)
        {
            return false;
        }

        public virtual IList<IEntity> GetVariants(ILanguageEntity languageEntity)
        {
            return new List<IEntity>();
        }

        public virtual bool HasRelationships()
        {
            var relationships = typeof(TSourceEntity).GetPropertyValueWithAttribute<RelationshipsAttribute, IEnumerable>(SourceEntity);
            return relationships != null && relationships.GetEnumerator().MoveNext();
        }

        public virtual IList<RelationshipDetail> GetRelationships()
        {
            var relationships = typeof(TSourceEntity).GetPropertyValueWithAttribute<RelationshipsAttribute, IEnumerable>(SourceEntity);
            if (relationships != null)
            {
                return relationships.Cast<RelationshipDetail>().ToList();
            }

            return new List<RelationshipDetail>();
        }

        public virtual void Map()
        {
        }

        public virtual IList<LocalizablePropertyValues> Map(ILanguageEntity languageEntity, IList<LocalizablePropertyValues> entityLocalizableProperties)
        {
            Type t = typeof(TCommerceEntity);

            var commerceEntity = Activator.CreateInstance(t) as TCommerceEntity;

            ILanguageEntity<TSourceEntity> l = languageEntity as ILanguageEntity<TSourceEntity>;
            if (l == null)
            {
                Context.Abort(Context.CommerceContext.AddMessage(Context.GetPolicy<KnownResultCodes>().Error, "LanguageEntityMissing", null, "Language entity cannot be null").Result, Context);
            }
            else
            {
                MapLocalizeValues(l.Entity, commerceEntity);

                if (entityLocalizableProperties == null)
                {
                    entityLocalizableProperties = LocalizablePropertyListManager.GetEntityProperties(t, Context);
                    entityLocalizableProperties = entityLocalizableProperties?.Clone();
                }

                if (entityLocalizableProperties == null || !entityLocalizableProperties.Any())
                {
                    return new List<LocalizablePropertyValues>();
                }

                var properties = TypePropertyListManager.GetProperties(t);
                foreach (var localizablePropertyValues in entityLocalizableProperties)
                {
                    if (!string.IsNullOrEmpty(localizablePropertyValues.PropertyName))
                    {
                        var propertyInfo = properties.FirstOrDefault(x =>
                            x.Name.Equals(localizablePropertyValues.PropertyName, StringComparison.OrdinalIgnoreCase));
                        if (propertyInfo != null)
                        {
                            var propertyValue = propertyInfo.GetValue(commerceEntity);
                            var parameter = localizablePropertyValues.Parameters.FirstOrDefault(x =>
                                x.Key.Equals(languageEntity.Language, StringComparison.OrdinalIgnoreCase));

                            if (parameter == null)
                            {
                                parameter = new Parameter {Key = languageEntity.Language, Value = null};
                                localizablePropertyValues.Parameters.Add(parameter);
                            }

                            parameter.Value = propertyValue;
                        }
                    }
                }
            }

            return entityLocalizableProperties;
        }

        protected virtual void MapLocalizeValues(TSourceEntity localizedSourceEntity, TCommerceEntity localizedTargetEntity)
        {
        }

        public TOutput GetPropertyValueFromSource<TMetadata, TOutput>() 
            where TMetadata : Attribute
            where TOutput : class
        {
            return typeof(TSourceEntity).GetPropertyValueWithAttribute<TMetadata, TOutput>(SourceEntity);
        }
    }
}