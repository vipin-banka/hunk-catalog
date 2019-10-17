![hunk-catalog](https://github.com/vipin-banka/hunk-catalog/blob/master/images/hunk.png)

# hunk-catalog for Sitecore Commerce (for XC 9.2)
Plugin for Sitecore Commerce that allows you to write simple and maintainable custom catalog import implementations.

## What is this?
In most of Sitecore Commerce projects you need to write custom catalog import code based on your specific requirements and catalog information in your PIM. Without a standard approach each team does it in a different way. The idea with this plugin is to create a standard approach for import processes that can be utilized in any commerce implementation without compromising with flexibility and extensibility of Sitecore commerce architecture and also gives you type safe way to write your own custom implementations.

In a typical catalog import implementation you create a plugin (at minimum) that reads data from external system, takes care of XC specific things (enities, components, pipelines, commands, versioning, localization etc.) and writes the source content to Sitecore XC entities.
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

## What is your responsility?
You need do following:
* **Create a plugin**
  - you need to create a XC plugin that will reference **hunk-catalog** plugin.
* **Model source entities**
  - Create c# classes representing your source entities.
  - These will be simple POCO classes representing source information.
* **Model Commerce Entities and Components**
  - Create custom Sitecore commerce entity classes (optional, for a catalog import not neccessary)
  - Create custom Sitecore commerce component classes, these will be simple POCO classes representing information that will be stored in commerce engine.
* **Write Mappers**
  - Create c# classes and write simple mapping code.
  - Write code to read inforamtion from source and write in target.
* **Configure Commerce Engine**
  - This step involves configuration of mappers into your commerce engine environment files.
  - Import process will trigger your custom mappers.

That’s it. 
Once your commerce engine solution is deployed and bootstraped you can use the import operation.
See sample Postman API calls and sample test console application for usage.

## How to use it?
* Create a new plugin in your commerce solution.
* Add a dependency on Hunk.Catalog to the new plugin.
  - From the package manager console: *Install-Package Plugin.Hunk.Catalog*
  - Using the Nuget package manager add a dependency on *Plugin.Hunk.Catalog*.

## Getting started

### Import a sellable item
Let's say you want to import a sellable item. Your sellable item have few custom fields i.e. "Accessories" and "Dimensions" and you need to add a custom component for that.

#### Create a c# classes for source sellable item.
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProduct.cs) for reference.
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
**notes**
* Inherit your class from "IEntity" interface and implement it, this interface only demands for one string property named as Id.
* You can design this class as your want.
* To be able to set relationship with Catalogs and Categories add a property like below and decorate it with Parents attribute. You can name it anything you like.
```
        [Parents()]
        public IList<string> Parents { get; set; }
```

#### Create Commerce component classes
This will be used to store custom information with sellable item in Sitecore XC.
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/Components/SellableItemComponent.cs) for reference.
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

#### Create Sellable Item mapper class
Write code to read field/property values from source sellable item and write to Sitecore Commerce Sellable Item.

See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProductImportHandler.cs) for reference.
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
**notes**
* Inherit this class from *SellableItemImportHandler* class.
* Specify your custom sellable item class as type argument.
* Override *Initialize* method and write code to read values from your custom sellable item class and assign to base class properties.
* Override *Map* method and write code to read values from your custom sellable item and assign to commerce entitiy.

#### Create Sellable Item Component mapper class
Write code to read field/property values from source sellable item and write to Sitecore Commerce Component.

See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SellableItemComponentMapper.cs) for reference.

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
**notes**
* Inherit this class from *BaseEntityComponentMapper* class.
* Specify your custom sellable item class as first type argument.
* Specify your Sitecore Commerce Component class (that will store additional sellable item data) as second type argument.
* Override *Map* method and write code to read values from your custom sellable item and assign to commerce component. 

#### Configure commerce engine

**Create a Policy Set file and add "CatalogImportPolicy" in it.**

see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Sitecore.Commerce.Engine/wwwroot/data/Environments/PlugIn.CatalogImport.PolicySet-1.0.0.json) for reference.
```json
{
  "$type": "Sitecore.Commerce.Core.PolicySet, Sitecore.Commerce.Core",
  "Id": "Entity-PolicySet-CatalogImportPolicySet",
  "Version": 1,
  "IsPersisted": false,
  "Name": "CatalogImportPolicySet",
  "Policies": {
    "$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Policy, Sitecore.Commerce.Core]], mscorlib",
    "$values": [
      {
        "$type": "Plugin.Hunk.Catalog.Policy.CatalogImportPolicy, Plugin.Hunk.Catalog",
        "DeleteOrphanVariant": true,
        "EntityVersioningScheme": "UpdateLatest",
        "Mappings": {
          "EntityMappings": [
            {
              "$type": "Plugin.Hunk.Catalog.Policy.EntityMapperType, Plugin.Hunk.Catalog",
              "Key": "SellableItem",
              "ImportHandlerTypeName": "Plugin.Hunk.Catalog.Test.SellableItemEntityImport.SourceProductImportHandler, Plugin.Hunk.Catalog.Test"
            }
          ],
          "EntityComponentMappings": [
            {
              "$type": "Plugin.Hunk.Catalog.Policy.MapperType, Plugin.Hunk.Catalog",
              "Key": "SellableItemComponent",
              "FullTypeName": "Plugin.Hunk.Catalog.Test.SellableItemEntityImport.SellableItemComponentMapper, Plugin.Hunk.Catalog.Test"
            }
          ],
          "ItemVariationMappings": [],
          "ItemVariationComponentMappings": [],
          "RelationshipMappings": [] 
        }
      }
    ]
  }
}
```
**notes**
* In EntityMappings section add all entity mapper types. Each entry must have a unique key.
* In EntityComponentMappings section add all entity component mapper types. Each entry must have a unique key.
* Attach this policy set with commerce environment.

#### How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Sellable Item** request.
* Provide correct data in the request body.
```json
{
	"metadata":{
		"Components":["SellableItemComponent"],
		"VariantComponents":["VariantComponent"],
		"EntityType":"SellableItem"
	},
	"entity": {
		"Id": "Sellable Item3",
		"Name": "Sellable Item3",
		"DisplayName": "Sellable Item3 Display Name",
		"Description": "Sellable Item3 Description",
		"Brand": "sample brand3",
		"Manufacturer": "sample manufacturer3",
		"TypeOfGood": "sample type of good3",
		"Tags":[{"Name":"refrigerator"}, {"Name":"tag1", "Excluded": true}],
		"Parents": ["Hunk1_Catalog/Hunk1_Category Name"],
		"Accessories": "Accessories value3",
		"Dimensions": "Dimensions value3"
	}
}
```
* Execute the request.
* Open Business tools and verify sellable item.

### Import a sellable item with variants
Let's say you want to import a sellable item with varaints. Your varaint have few custom fields as well.
Follow all the steps to import a sellable item and also do the following.

#### Create your source variant class
see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProductVariant.cs) for reference.
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
**notes**
* Inherit your class from "IEntity" interface and implement it, this interface only demands for one string property named as Id.

#### Create Component class

see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/Components/VariantComponent.cs) for reference.
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
#### Create Item Variant mapper class

Write code to read field/property values from your source variant and write to Sitecore Commerce Item Variant component.

See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/ItemVariationComponentMapper.cs) for reference.

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
**notes**
* Inherit this class from *BaseItemVariationComponentMapper* class.
* Specify your custom sellable item class as first type argument.
* Specify your custom variant item class as second type argument.
* Override *Map* method and write mapping code.

#### Create Item Variant's Child Component mapper class

Write code to read field/property values from custom variant write to Sitecore Commerce component.

See sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/VariantComponentMapper.cs) for reference.

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
**notes**
* Inherit this class from *BaseVariantComponentMapper* class.
* Specify your custom sellable item class as first type argument.
* Specify your custom variant item class as second type argument.
* Specify your Sitecore Commerce Component class (that will store additional variant data) as third type argument.
* Override *Map* method and write mapping code.

**Create a Policy Set file and add "CatalogImportPolicy" in it.**

see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Sitecore.Commerce.Engine/wwwroot/data/Environments/PlugIn.CatalogImport.PolicySet-1.0.0.json) for reference.
```json
{
  "$type": "Sitecore.Commerce.Core.PolicySet, Sitecore.Commerce.Core",
  "Id": "Entity-PolicySet-CatalogImportPolicySet",
  "Version": 1,
  "IsPersisted": false,
  "Name": "CatalogImportPolicySet",
  "Policies": {
    "$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Policy, Sitecore.Commerce.Core]], mscorlib",
    "$values": [
      {
        "$type": "Sitecore.Commerce.Core.AutoApprovePolicy, Sitecore.Commerce.Core"
      },
      {
        "$type": "Plugin.Hunk.Catalog.Policy.CatalogImportPolicy, Plugin.Hunk.Catalog",
        "DeleteOrphanVariant": true,
        "EntityVersioningScheme": "UpdateLatest",
        "Mappings": {
          "EntityMappings": [
            {
              "$type": "Plugin.Hunk.Catalog.Policy.EntityMapperType, Plugin.Hunk.Catalog",
              "Key": "SellableItem",
              "ImportHandlerTypeName": "Plugin.Hunk.Catalog.Test.SellableItemEntityImport.SourceProductImportHandler, Plugin.Hunk.Catalog.Test"
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
          "RelationshipMappings": [] 
        }
      }
    ]
  }
}
```
**notes**
* In EntityMappings section add all entity mapper types. Each entry must have a unique key.
* In EntityComponentMappings section add all entity component mapper types. Each entry must have a unique key.
* Attach this policy set with commerce environment.

#### How to test?
* Build and deploy commerce solution.
* Bootstrap commerce engine using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Execute GetToken API from your commerce Authentication collection.
* Go to **Catalog Import** collection and open **Import Sellable Item** request.
* Provide correct data in the request body.
```json
{
	"metadata":{
		"Components":["SellableItemComponent"],
		"VariantComponents":["VariantComponent"],
		"EntityType":"SellableItem"
	},
	"entity": {
		"Id": "Sellable Item3",
		"Name": "Sellable Item3",
		"DisplayName": "Sellable Item3 Display Name",
		"Description": "Sellable Item3 Description",
		"Brand": "sample brand3",
		"Manufacturer": "sample manufacturer3",
		"TypeOfGood": "sample type of good3",
		"Tags":[{"Name":"tag1"}],
		"Parents": ["Hunk1_Catalog/Hunk1_Category Name"],
		"Accessories": "Accessories value3",
		"Dimensions": "Dimensions value3",
		"Variants": [
			{
				"Id": "Variant1",
				"DisplayName": "Variant1 Display Name",
				"Description": "Variant1 Description",
				"Disabled": "true",
				"Tags": [{"Name": "stainless"}],
				"Breadth": "10m",
				"Length": "100m"
			},
			{
				"Id": "Variant2",
				"DisplayName": "Variant2 Display Name",
				"Description": "Variant2 Description",
				"Disabled": "false",
				"Tags": [{"Name":"22cuft"}, {"Name":"v2", "Excluded": true}],
				"Breadth": "20m",
				"Length": "200m"
			}
		]
	}
}
```
* Execute the request.
* Open Business tools and verify sellable item.
