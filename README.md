![hunk-catalog](https://github.com/vipin-banka/hunk-catalog/blob/master/images/hunk.png)

# hunk-catalog for Sitecore Commerce
Plugin for Sitecore Commerce that allows you to write simple and maintainable custom catalog import implementations.

## What is this?
In most of Sitecore Commerce projects you need to write custom catalog import code based on your specific requirements and catalog information in your PIM. Without a standard approach each team does it in a different way. The idea with this plugin is to create a standard approach for import processes that can be utilized in any commerce implementation without compromising with flexibility and extensibility of Sitecore commerce architecture and gives you type safe way to write your own custom implementations.

In a typical catalog import implementation you create a plugin (at minimum) that reads data from external system, takes care of XC specific things (entities, components, pipelines, commands, versioning, localization etc.) and writes the source content to Sitecore XC entities.
![A typical catalog import implementation](https://github.com/vipin-banka/hunk-catalog/blob/master/images/a-general-catalog-import-implementation.png)

With **hunk-catalog** import implementation you still create a plugin that reads data from external system and writes the source content to Sitecore XC entities but **hunk-catalog** will take care of all XC specific things for you. With this implementation you are free from writing lots of boilerplate code for import process.
![hunk-catalog import implementation](https://github.com/vipin-banka/hunk-catalog/blob/master/images/hunk-catalog-import-implementation.png)

**This plugin supports following:**
* Creation and update of Catalog, Category and SellableItem commerce entities.
* Creation and update of any custom commerce entities.
* Adding, updating and removing child components on commerce entities.
* Creation and update of item variants for sellable items.
* Adding, updating and removing child components on item variants.
* Managing parent child relationships between catalog, category and sellable items.
* Managing relationships e.g. related, cross sell on sellable items.
* Store Localized content for commerce entities, components, item variants and item variant child components.

## Flow Chart - A full flow for single entity import.
![alt text](https://github.com/vipin-banka/hunk-catalog/blob/master/docs/flow-chart.png)

## What is your responsibility?
You need to do following:
* **Create a plugin**
  - you need to create a XC plugin that will reference **hunk-catalog** plugin.
* **Model source entities**
  - Create c# classes representing your source entities.
  - These will be simple POCO classes representing source information.
* **Model Commerce Entities and Components**
  - Create custom Sitecore commerce entity classes (optional, for a catalog import not necessary)
  - Create custom Sitecore commerce component classes, these will be simple POCO classes representing information that will be stored in commerce engine.
* **Write Mappers**
  - Create c# classes and write simple mapping code.
  - Write code to read information from source and write in target.
* **Configure Commerce Engine**
  - This step involves configuration of mappers into your commerce engine environment.
  - Import process will trigger your custom mappers.

That’s it. 
Once your commerce engine solution is deployed and bootstrapped you can use the import operation.

## How to use it?
* Create a new plugin in your commerce solution.
* Add a dependency on Hunk.Catalog to the new plugin.
  - From the package manager console: *Install-Package Plugin.Hunk.Catalog*
  - Using the Nuget package manager add a dependency on *Plugin.Hunk.Catalog*.

## <a name="getting-started">Getting started
This plugin covers lot of things so let's go by samples. 
There are samples for following:
1. [Import a Sellable Item](#import-sellable-item)
2. [Import a Sellable Item with Variants](#import-sellable-item-with-variants)
3. [Import a Sellable Item with Localize Content](#import-sellable-item-with-localize-content)
4. [Import a Sellable Item with Relationships](#import-sellable-item-with-relationships)
5. [Import a Catalog Item](#import-catalog-item)
6. [Import a Category Item](#import-category-item)
7. [Import a Custom Entity](#import-custom-entity)

### <a name="import-sellable-item">1. Import a sellable item
Let's say you want to import a sellable item. Your sellable item has few custom fields i.e. "Accessories" and "Dimensions" and you need to add a custom commerce component for that.

Follow below steps:

#### 1.1 Create a c# class for source sellable item.
This class represents source sellable item content.
```c#
namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{    
    public class SourceProduct : IEntity
    {
        public SourceProduct()
        {
            Parents = new List<string>();
            Tags = new List<Tag>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Manufacturer { get; set; }

        public string TypeOfGood { get; set; }

        public IList<Tag> Tags { get; set; }

        [Parents()]
        public IList<string> Parents { get; set; }
        
        public string Accessories { get; set; }

        public string Dimensions { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProduct.cs).

**notes**
* Inherit your class from "IEntity" interface and implement it, this interface only demands for one string property named as Id.
* You can design this class as you want.
* To be able to set relationship with Catalogs and Categories add a property like below and decorate it with Parents attribute. You can name it anything you like.
```c#
        [Parents()]
        public IList<string> Parents { get; set; }
```

#### 1.2 Create Commerce component class
This will be used to store custom information with sellable item in Sitecore XC.
```c#
namespace Plugin.Hunk.Catalog.Test.Components
{
    public class SellableItemComponent : Sitecore.Commerce.Core.Component
    {
        public string Accessories { get; set; }

        public string Dimensions { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/Components/SellableItemComponent.cs).

#### 1.3 Create Sellable Item mapper class
In this class write code to read source content and write to Sitecore Commerce Sellable Item entity.
```c#
namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SourceProductImportHandler : SellableItemImportHandler<SourceProduct>
    {
        public SourceProductImportHandler(string sourceProduct, 
        CommerceCommander commerceCommander, 
        CommercePipelineExecutionContext context)
            : base(sourceProduct, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            ProductId = SourceEntity.Id;
            Name = SourceEntity.Name;
            DisplayName = SourceEntity.DisplayName;
            Description = SourceEntity.Description;
            Brand = SourceEntity.Brand;
            Manufacturer = SourceEntity.Manufacturer;
            TypeOfGood = SourceEntity.TypeOfGood;
            Tags = SourceEntity.Tags.Select(t=>t.Name).ToList();
        }
        
        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
            CommerceEntity.Brand = SourceEntity.Brand;
            CommerceEntity.Manufacturer = SourceEntity.Manufacturer;
            CommerceEntity.TypeOfGood = SourceEntity.TypeOfGood;
            CommerceEntity.Tags = SourceEntity.Tags;
        }
    }
}
```
See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProductImportHandler.cs).

**notes**
* Inherit this class from *SellableItemImportHandler* class.
* Specify your custom sellable item class as type argument.
* Override *Initialize* and *Map* methods like the sample above.

#### 1.4 Create Sellable Item Component mapper class
In this class write code to read source content and write to Sitecore Commerce Component.
```c#
namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SellableItemComponentMapper : BaseEntityComponentMapper<SourceProduct, SellableItemComponent>
    {
        public SellableItemComponentMapper(SourceProduct product, 
            CommerceEntity commerceEntity, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(product, commerceEntity, commerceCommander, context)
        { }

        protected override void Map(SellableItemComponent component)
        {
            component.Accessories = SourceEntity.Accessories;
            component.Dimensions = SourceEntity.Dimensions;
        }
    }
}
```
See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SellableItemComponentMapper.cs).

**notes**
* Inherit this class from *BaseEntityComponentMapper* class.
* Specify your custom sellable item class as first type argument.
* Specify your Sitecore Commerce Component class as second type argument.
* Override *Map* method similar to sample above. 

#### 1.5 Configure commerce engine
I would recommend to creating a separate Policy Set file and add CatalogImportPolicy in it.
You need to configure your custom mapper classes in this policy as shown in below image:
![entity-mappings](https://github.com/vipin-banka/hunk-catalog/blob/master/images/Entity-mappings.png)
see complete policy details [here](#catalog-import-policy).
Once you are done with changes in policy set, you need to assign this policy set to a commerce environment.

#### 1.6 How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Sellable Item** request.
![import-sellable-item](https://github.com/vipin-banka/hunk-catalog/blob/master/images/import-sellable-item.png)
* Provide correct data in the request body.
* notes
  - you need to pass two parameters
   - "metadata": this is to tell import process what to import, it includes following
     - EntityType: The entity key defined for entity  mapper in the policy, for this sample it is SellableItem.
     - Components: Comma separated component mapper keys defined in the policy, for this sample it is SellableItemComponent.
     - VariantComponents: Comma separated variant component mapper keys defined in the policy, for this sample there is none so we can leave it blank.
   - "entity": This is serialized value for your source entity, this will change based on your source entity.
* **make sure to give some value for "Parents".** see details [here](#link-to-catalog).
* Execute the request.
* Open Business tools and check your new sellable item.

### 2. <a name="import-sellable-item-with-variants">Import a Sellable Item with Variants
This sample depends on previous sample of importing a sellable item, make sure you have looked into that first.
	
Let's say you want to import a sellable item with variants. Your variant has few custom fields as well.

Follow all the steps to import a sellable item and do the following.

#### 2.1 Create your source variant class
This class represent source variant content.
```c#
namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class SourceProductVariant : IEntity
    {
        public SourceProductVariant()
        {
            Tags = new List<Tag>();
        }

        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Disabled { get; set; }

        public IList<Tag> Tags { get; set; }

        public string Breadth { get; set; }

        public string Length { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProductVariant.cs).

**notes**
* Inherit your class from "IEntity" interface and implement it, this interface only demands for one string property named as Id.

#### 2.2 Add variants to source sellable item class
Add following property in your source sellable item class.
```c#
public class SourceProduct : IEntity
{
	// other properties
	
	[Variants()]
        public IList<SourceProductVariant> Variants { get; set; }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProduct.cs).

**notes**
* This property should be decorated with Variants attribute and the type argument in property should represent your source variant class.

#### 2.3 Create Component class
```c#
namespace Plugin.Hunk.Catalog.Test.Components
{
    public class VariantComponent : Sitecore.Commerce.Core.Component
    {
        public string Breadth { get; set; }

        public string Length { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/Components/VariantComponent.cs).

#### 2.4 Create Item Variant mapper class
In this class write code to read from source variant and write to Sitecore Commerce Item Variant component.

```c#
namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class ItemVariationComponentMapper : BaseItemVariationComponentMapper<SourceProduct, SourceProductVariant>
    {
        public ItemVariationComponentMapper(SourceProduct product, 
          SourceProductVariant productVariant, 
          CommerceEntity commerceEntity, 
          Component parentComponent, 
          CommerceCommander commerceCommander, 
          CommercePipelineExecutionContext context)
            :base(product, productVariant, commerceEntity, parentComponent, commerceCommander, context)
        { }

        protected override string ComponentId => SourceVariant.Id;

        protected override void Map(ItemVariationComponent component)
        {
            component.Id = SourceVariant.Id;
            component.DisplayName = SourceVariant.DisplayName;
            component.Description = SourceVariant.Description;
            component.Disabled = SourceVariant.Disabled;
            component.Tags = SourceVariant.Tags;
        }
    }
}
```
See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/ItemVariationComponentMapper.cs).

**notes**
* Inherit this class from *BaseItemVariationComponentMapper* class.
* Specify your custom sellable item class as first type argument.
* Specify your custom variant item class as second type argument.
* Override *Map* method and like sample above.

#### 2.5 Create Item Variant's Child Component mapper class
In this class write code to read source content and write to Sitecore Commerce component.
```c#
namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{
    public class VariantComponentMapper : BaseVariantComponentMapper<SourceProduct, SourceProductVariant, VariantComponent>
    {
        public VariantComponentMapper(SourceProduct product, 
          SourceProductVariant productVariant, 
          CommerceEntity commerceEntity, 
          Component parentComponent, 
          CommerceCommander commerceCommander, 
          CommercePipelineExecutionContext context)
            :base(product, productVariant, commerceEntity, parentComponent, commerceCommander, context)
        { }

        protected override void Map(VariantComponent component)
        {
            component.Breadth = SourceVariant.Breadth;
            component.Length = SourceVariant.Length;
        }
    }
}
```
See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/VariantComponentMapper.cs).

**notes**
* Inherit this class from *BaseVariantComponentMapper* class.
* Specify your custom sellable item class as first type argument.
* Specify your custom variant item class as second type argument.
* Specify your Sitecore Commerce Component class as third type argument.
* Override *Map* method like sample above.

#### 2.6 Configure Commerce Engine
In the previous sample we created a policy set file, you need to configure your variant custom mapper classes there as shown in below image:
![variant-mappings](https://github.com/vipin-banka/hunk-catalog/blob/master/images/variant-mappings.png)
see complete policy details [here](#catalog-import-policy).

#### 2.7 How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Sellable Item with Variants** request.
![import-sellable-items-with-variants](https://github.com/vipin-banka/hunk-catalog/blob/master/images/import-sellable-item-with-variants.png)
* Provide correct data in the request body.
* notes
  - you need to pass two parameters
   - "metadata": this is to tell import process what to import, it includes following
     - EntityType: The unique name defined for entity  mapper in the policy, for current sample it is SellableItem.
     - Components: Comma separated component mapper keys defined in the policy, for current sample it is SellableItemComponent.
     - VariantComponents: Comma separated variant component mapper keys defined in the policy, for current sample it is VariantComponent.
   - "entity": This is serialized value for your source entity, this will change based on your source entity.
* **make sure to give some value in "Parents" property.** see details [here](#link-to-catalog)
* Execute the request.
* Open Business tools and check your sellable item and variants.

### 3. <a name="import-sellable-item-with-localize-content">Import Sellable Item with Localize Content
This sample depends on previous sample of importing a sellable item. Please follow that before continuing with this.
The sample is focused on sellable item, but localize content can be imported for other entities similarly.

#### 3.1 Set Localization Policy set
In "PlugIn.LocalizeEntities.	PolicySet-1.0.0.json" file set localization details for your entity.
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/maste/engine/Sitecore.Commerce.Engine/wwwroot/data/Environments/PlugIn.LocalizeEntities.PolicySet-1.0.0.json).

#### 3.2 Add language property to source sellable item class
Add following property in your source sellable item class.
```c#
public class SourceProduct : IEntity
{
	// other properties
	
	[Languages()]
        public IList<LanguageEntity<SourceProduct>> Languages { get; set; }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProduct.cs).

**notes**
* This property should be decorated with Languages attribute and the type argument in property should represent your source product class.

#### 3.3 Override method in mapper classes.
Each mapper class supports a method to map localize content, in your mapper class override this method:
```c#
protected override void MapLocalizeValues(SourceProduct localizedSourceEntity, SellableItem localizedTargetEntity)
{
    localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
    localizedTargetEntity.Description = localizedSourceEntity.Description;
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProductImportHandler.cs).

#### 3.4 How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Sellable Item with Languages** request.
![import-with-localize-content](https://github.com/vipin-banka/hunk-catalog/blob/master/images/import-sellable-item-with-languages.png)
* Provide correct data in the request body.
* Execute the request.
* Open Business tools and verify sellable item has been added with localized content.

### <a name="import-sellable-item-with-relationships">4. Import sellable item with relationships

#### 4.1 Add relationship property to source sellable item class
Add following property in your source sellable item class.
```c#
public class SourceProduct : IEntity
{
	// other properties
	
	[Relationships()]
        public IList<RelationshipDetail> RelationshipDetails { get; set; }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProduct.cs)

**notes**
* You must add this property to your sellable item source class as is. It must be decorated with Relationships attribute and must have return type as IList&lt;RelationshipDetail&gt;.

#### 4.2 Verify commerce engine configuration
See policy details [here](#catalog-import-policy), make sure all mappers for relationship details from this plugin are configured properly.
![relationship-mappings](https://github.com/vipin-banka/hunk-catalog/blob/master/images/relationship-mappings.png)

#### 4.3 How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Sellable Item with Relationship** request.
![import-sellable-item-with-relationships](https://github.com/vipin-banka/hunk-catalog/blob/master/images/import-sellable-item-with-relationship.png)
* Provide correct data in the request body.
* Execute the request.
* Open Business tools and verify sellable item has been added with localized content.

### <a name="import-catalog-item">5. Import Catalog item

#### 5.1 Create a c# class for source catalog item.
```c#
namespace Plugin.Hunk.Catalog.Test.CatalogEntityImport
{
    public class SourceCatalog : IEntity
    {
        public SourceCatalog()
        {
            Languages = new List<LanguageEntity<SourceCatalog>>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceCatalog>> Languages { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/CatalogEntityImport/SourceCatalog.cs).

**notes**
* Inherit your class from "IEntity" interface and implement it, this interface only demands for one string property named as Id.
* You can design this class as you want.

#### 5.2 Create Catalog Item mapper class
In this class write code to read source catalog content and write to Sitecore Commerce Catalog entity.
```c#
namespace Plugin.Hunk.Catalog.Test.CatalogEntityImport
{
    public class SourceCatalogImportHandler : CatalogImportHandler<SourceCatalog>
    {
        public SourceCatalogImportHandler(string sourceCatalog, 
            CommerceCommander commerceCommander, 
            CommercePipelineExecutionContext context)
            : base(sourceCatalog, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            Name = SourceEntity.Name;
            DisplayName = SourceEntity.DisplayName;
        }

        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
        }

        protected override void MapLocalizeValues(SourceCatalog localizedSourceEntity, 
		Sitecore.Commerce.Plugin.Catalog.Catalog localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
        }
    }
}
```
See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/CatalogEntityImport/SourceCatalogImportHandler.cs).

**notes**
* Inherit this class from *CatalogImportHandler* class.
* Specify your custom catalog item class as type argument.
* Override *Initialize*, *Map* and "MapLocalizeValues" methods.

#### 5.3 Configure commerce engine
Configure your mappers in catalog import policy.

See sample configuration [here](#catalog-import-policy).

#### 5.4 How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Catalog Item** request.
![import-catalog-item](https://github.com/vipin-banka/hunk-catalog/blob/master/images/import-catalog-item.png)
* Provide correct data in the request body.
* Execute the request.
* Open Business tools and verify catalog item there.

### <a name="import-category-item">6. Import Category item

#### 6.1 Create a c# class for source category item.
```c#
namespace Plugin.Hunk.Catalog.Test.CategoryEntityImport
{
    public class SourceCategory : IEntity
    {
        public SourceCategory()
        {
            Parents = new List<string>();
            Languages = new List<LanguageEntity<SourceCategory>>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Parents()]
        public IList<string> Parents { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceCategory>> Languages { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/CategoryEntityImport/SourceCategory.cs).

**notes**
* Inherit your class from "IEntity" interface and implement it, this interface only demands for one string property named as Id.
* You can design this class as you want.
* Make sure to add Parents property to link your category with catalog or category.

#### 6.2 Create Category Item mapper class
In this class write code to read from source category item and write to Sitecore Commerce Category entity.
```c#
namespace Plugin.Hunk.Catalog.Test.CategoryEntityImport
{
    public class SourceCategoryImportHandler : CategoryImportHandler<SourceCategory>
    {
        public SourceCategoryImportHandler(string sourceCategory, 
	  CommerceCommander commerceCommander, 
	  CommercePipelineExecutionContext context)
            : base(sourceCategory, commerceCommander, context)
        {
        }

        protected override void Initialize()
        {
            Name = SourceEntity.Name;
            DisplayName = SourceEntity.DisplayName;
            Description = SourceEntity.Description;
        }

        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
        }

        protected override void MapLocalizeValues(SourceCategory localizedSourceEntity, 
		Sitecore.Commerce.Plugin.Catalog.Category localizedTargetEntity)
        {
            localizedTargetEntity.DisplayName = localizedSourceEntity.DisplayName;
            localizedTargetEntity.Description = localizedSourceEntity.Description;
        }
    }
}
```
See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/CategoryEntityImport/SourceCategoryImportHandler.cs).

**notes**
* Inherit this class from *CategoryImportHandler* class.
* Specify your custom category item class as type argument.
* Override *Initialize*, *Map* and *MapLocalizeValues* methods.

#### 6.3 Configure commerce engine
Configure your mappers in catalog import policy.

See sample configuration [here](#catalog-import-policy).

#### 6.4 How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Category Item** request.
![import-category-item](https://github.com/vipin-banka/hunk-catalog/blob/master/images/import-category-item.png)
* Provide correct data in the request body.
* Execute the request.
* Open Business tools and verify category item.

### <a name="import-custom-entity">7. Import Custom Entity
It is also possible to import content in a new custom commerce entity.
	
Follow below steps:

#### 7.1 Create a c# class for source custom item.

```c#
namespace Plugin.Hunk.Catalog.Test.CustomEntityImport
{
    public class SourceCustomEntity : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/CustomEntityImport/SourceCustomEntity.cs).

**notes**
* Inherit your class from "IEntity" interface and implement it, this interface only demands for one string property named as Id.
* You can design this class as you want.

#### 7.2 Create Custom Commerce Entity class

```
namespace Plugin.Hunk.Catalog.Test.CustomEntityImport
{
    public class CustomCommerceItem : CommerceEntity
    {
        public string Description { get; set; }
    }
}
```
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/CustomEntityImport/CustomCommerceItem.cs).

**notes**
- Inherit from CommerceEntity class from Sitecore.Commerce.Core.
- Add whatever properties you need, for simplicity I have just added Description.

#### 7.3 Create Custom Item mapper class
In this class write code to read from source entity and write to Sitecore Commerce Custom Entity.
```c#
namespace Plugin.Hunk.Catalog.Test.CustomEntityImport
{
    public class SourceCustomEntityImportHandler : BaseEntityImportHandler<SourceCustomEntity, CustomCommerceItem>
    {
        public SourceCustomEntityImportHandler(string sourceProduct, 
	  CommerceCommander commerceCommander, 
	  CommercePipelineExecutionContext context)
            : base(sourceProduct, commerceCommander, context)
        {
        }

        public override async Task<CommerceEntity> Create()
        {
            var commerceEntity = new CustomCommerceItem();
            commerceEntity.Id = IdWithPrefix();
            commerceEntity.Name = SourceEntity.Name;
            commerceEntity.DisplayName = SourceEntity.DisplayName;
            commerceEntity.Description = SourceEntity.Description;

            await CommerceCommander.Pipeline<IPersistEntityPipeline>()
                .Run(new PersistEntityArgument(commerceEntity), Context).ConfigureAwait(false);

            return commerceEntity;
        }

        public override void Map()
        {
            CommerceEntity.Name = SourceEntity.Name;
            CommerceEntity.DisplayName = SourceEntity.DisplayName;
            CommerceEntity.Description = SourceEntity.Description;
        }
    }
}
```
See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/CustomEntityImport/SourceCustomEntityImportHandler.cs).

**notes**
* Inherit this class from *BaseEntityImportHandler* class.
* Specify your source entity item class as first type argument.
* Specify your commerce entity item class as second type argument.
* Override *Create* and *Map* methods.

#### 7.4 Configure commerce engine
Configure your mappers in catalog import policy.

See sample configuration [here](#catalog-import-policy).

#### 7.5 How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Custom Entity** request.
![import-custom-entity](https://github.com/vipin-banka/hunk-catalog/blob/master/images/import-custom-item.png)
* Provide correct data in the request body.
* Execute the request.
* You can check entity in database.

### <a name="link-to-catalog">8. Link Sellable Item with Catalog & Category
To link sellable item with catalog or category or both, in your source sellable item class you should have a property like below
```c#
        [Parents()]
        public IList<string> Parents { get; set; }
```
This property must contain name of catalog or category or both. category name must be in format "{catalog-name}/{category-name}".
Examples:
- A single value like "Hunk1_Catalog" will associate sellable item with catalog.
- A single value like "Hunk1_Catalog/Hunk1_Category Name" will associate sellable item with category inside catalog.
- Multiple values like "Hunk1_Catalog" and "Hunk1_Catalog/Hunk1_Category Name" will associate sellable item with both catalog and category.
	
### <a name="catalog-import-policy">9. Catalog Import Policy Set
I recommend that for all configuration a separate Policy Set should be created, and that policy set must define CatalogImportPolicy this plugin heavily relies on.

Catalog import policy allows you to set following values:

1. **DeleteOrphanVariant** - You can set a Boolen value. True will remove any variants those exists in Sitecore Commerce but does not come with-in external system content during import process. False will keep the variants but will set them as disabled.

2. **EntityVersioningScheme** - This will be used to determine which version of entity should be used to do the updates, this is not applicable if entity does not exist, import process will always create a new entity if that does not exist. You have following options to choose from:
   - CreateNewVersion - This option will create a new version every time for your imported entity.
   - UpdateLatestUnpublished - This option will first look for your latest version and if that version is in unpublished state, it will make updates in that version only but if latest version is already published than it will create a new version.
   - UpdateLatest - This will not create a new version and will always update in the latest version no matter whether published or not.
 
3. **EntityMappings** - In this section you must configure all custom entity mappers you have created. Give a unique key to each entry, you need to use these keys as entity name in "EntityType" property for metadata in import api.
 
4. **EntityComponentMappings** - In this section you have to configure all custom component mappers you have created. Give a unique key to each entry, you need to use these keys as component names in "Components" property for metadata in import api.
 
5. **ItemVariationMappings** - In this section you have to configure one item variant mapper.
 
6. **ItemVariationComponentMappings** - In this section you must to configure all custom item variant component mappers you have created. Give a unique key to each entry, you need to use these keys as variant component names in "VariantComponents" property for metadata in import api.
 
7. **RelationshipMappings** - In this section you must configure all relationship mappers, OOTB this plugin comes with four which supports relationship of sellable item to sellable item. Give a unique key to each entry, you need to use these keys with your relationship data to pass to import process.

A complete Catalog Import Policy is below:
```json{
{
  "$type": "Plugin.Hunk.Catalog.Policy.CatalogImportPolicy, Plugin.Hunk.Catalog",
  "DeleteOrphanVariant": true,
  "EntityVersioningScheme": "UpdateLatest",
  "Mappings": {
    "EntityMappings": [
      {
     	"$type": "Plugin.Hunk.Catalog.Policy.EntityMapperType, Plugin.Hunk.Catalog",
	"Key": "Catalog",
	"ImportHandlerTypeName": "Plugin.Hunk.Catalog.Test.CatalogEntityImport.SourceCatalogImportHandler, Plugin.Hunk.Catalog.Test"
      },
      {
	"$type": "Plugin.Hunk.Catalog.Policy.EntityMapperType, Plugin.Hunk.Catalog",
	"Key": "Category",
	"ImportHandlerTypeName": "Plugin.Hunk.Catalog.Test.CategoryEntityImport.SourceCategoryImportHandler, Plugin.Hunk.Catalog.Test"
      },
      {
	"$type": "Plugin.Hunk.Catalog.Policy.EntityMapperType, Plugin.Hunk.Catalog",
	"Key": "SellableItem",
	"ImportHandlerTypeName": "Plugin.Hunk.Catalog.Test.SellableItemEntityImport.SourceProductImportHandler, Plugin.Hunk.Catalog.Test"
      },
      {
	"$type": "Plugin.Hunk.Catalog.Policy.EntityMapperType, Plugin.Hunk.Catalog",
	"Key": "SourceCustomEntity",
	"ImportHandlerTypeName": "Plugin.Hunk.Catalog.Test.CustomEntityImport.SourceCustomEntityImportHandler, 
	Plugin.Hunk.Catalog.Test"
      }
   ],
  "EntityComponentMappings": [
     {
	"$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
	"Key": "SellableItemComponent",
	"FullTypeName": "Plugin.Hunk.Catalog.Test.SellableItemEntityImport.SellableItemComponentMapper, Plugin.Hunk.Catalog.Test"
     }
   ],
  "ItemVariationMappings": [
    {
	"$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
	"FullTypeName": "Plugin.Hunk.Catalog.Test.SellableItemEntityImport.ItemVariationComponentMapper, Plugin.Hunk.Catalog.Test"
    }
   ],
  "ItemVariationComponentMappings": [
    {
	"$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
	"Key": "VariantComponent",
	"FullTypeName": "Plugin.Hunk.Catalog.Test.SellableItemEntityImport.VariantComponentMapper, Plugin.Hunk.Catalog.Test"
    }
   ],
  "RelationshipMappings": [
    {
	"$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
	"Key": "TrainingProducts",
	"FullTypeName": "Plugin.Hunk.Catalog.RelationshipMappers.TrainingSellableItemToSellableItemRelationshipMapper, Plugin.Hunk.Catalog"
    },
    {
	"$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
	"Key": "InstallationProducts",
	"FullTypeName": "Plugin.Hunk.Catalog.RelationshipMappers.InstallationSellableItemToSellableItemRelationshipMapper, Plugin.Hunk.Catalog"
    },
    {
	"$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
	"Key": "RelatedProducts",
	"FullTypeName": "Plugin.Hunk.Catalog.RelationshipMappers.RelatedSellableItemToSellableItemRelationshipMapper, Plugin.Hunk.Catalog"
    },
    {
	"$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
	"Key": "WarrantyProducts",
	"FullTypeName": "Plugin.Hunk.Catalog.RelationshipMappers.WarrantySellableItemToSellableItemRelationshipMapper, Plugin.Hunk.Catalog"
     }
   ]
  }
}
```
