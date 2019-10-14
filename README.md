![alt text](https://github.com/vipin-banka/hunk-catalog/blob/feature/initial-branch/docs/hunk.png)

# hunk-catalog for Sitecore Commerce (for XC 9.2)
Plugin for Sitecore Commerce that does the heavy lifting of importing an entity in Sitecore Commerce and allows you to write simple and maintainable custom import implementations.

## What is this?
In most of Sitecore Commerce projects you need to write custom catalog import code based on your specific requirements and catalog information in your PIM. Without a standard architecture each team does it in different way and it becomes hard to maintain such implementations in a longer time period. The idea with this plugin is to create a standard framework for import processes that can be utilized in any commerce implementation without compromising on flexibility and extensibility of Sitecore commerce architecture and also gives you type safe way to write your own custom implementations.

This plugin will give you ability to focus on your entity designs rather than import implementation. It makes you free from writing lots of boilerplate code for import process.

It supports following:
* Creation and update of Catalog, Category and SellableItem commerce entities.
* Creation and update of any custom commerce entities.
* Adding, updating and removing child components on commerce entities.
* Managing parent child relationships between catalog, category and sellable items.
* Creation and update of item variants for sellable items.
* Adding, updating and removing child components on item variants.
* Managing various relationships e.g. related, cross sell on sellable items.
* Localized content for commerce entities.
* Localized content for commerce entities child components.
* Localized content for item variants.
* Localized content for item variant child components.
* Configure entity versioning scheme.
* Easily customizabe using Policies or custom pipeline blocks.

Note: For localization and versioning make sure to set proper policies in the respective policy set file(s). Refer PlugIn.LocalizeEntities.PolicySet-1.0.0.json file for localization and PlugIn.Versioning.PolicySet-1.0.0.json file for versioning.

## Flow Chart
![alt text](https://github.com/vipin-banka/hunk-catalog/blob/feature/initial-branch/docs/flow-chart.png)

## What is your responsility in custom import?
You need do following:
1. Create your custom POCO classes representing your source entities.
2. Create custom Mapper classes by inheriting from standard implementation comes with this plugin.
3. All abstract mapper classes comes with this plugin have basic implementation for most of the methods, you just need to override methods that suits your requirements. Basically you need to write simple map code that will read values from source entity and write in commerce entity, component or policy.
4. Configure your mapper class in the policies or resolve using custom pipeline blocks.
5. That’s it. Once your commerce solution is deployed and bootstraped you can try the import operation using Postman calls or you can trigger it from your other custom plugins.

## How to use it?
* Create a new plugin in your commerce solution. see [Plugin.Hunk.Catalog.Test](https://github.com/vipin-banka/hunk-catalog/tree/master/engine/Plugin.Hunk.Catalog.Test) for reference.
* Add a dependency on Hunk.Catalog to the new plugin.
  - From the package manager console: Install-Package Plugin.Hunk.Catalog
  - Using the Nuget package manager add a dependency on Plugin.Hunk.Catalog.

## Getting started
Let's say you want to import a sellable item. Your sellable item support all OOTB fields for Sitecore Commerce sellable item and in addition your sellable item also has two more fields for "Accessories" and "Dimensions".

You also want to import variants for your sellable item and your variants also has two more fields for "Breadth" and "Length".

### Create two c# classes representing your source sellable item and source variant. Your classes will look like below:

**Class for your source sellable item**, see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProduct.cs) for reference.
```c#
namespace Plugin.Hunk.Catalog.Test.SellableItemEntityImport
{    
    public class SourceProduct : IEntity
    {
        public SourceProduct()
        {
            Parents = new List<string>();
            Languages = new List<LanguageEntity<SourceProduct>>();
            Variants = new List<SourceProductVariant>();
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
        
        [Variants()]
        public IList<SourceProductVariant> Variants { get; set; }

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
* To be able to import variants add a property like below and decorate it with Variants attribute. Note that i have used source variant class type to define my list of variants. You can name it anything you like.
```
        [Variants()]
        public IList<SourceProductVariant> Variants { get; set; }
```

**Class for your source varint**, see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/SellableItemEntityImport/SourceProductVariant.cs) for reference.
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

### Create Sitecore Commerce component classes to store custom catalog information with sellable item and item variants.

**Component class to store custom data with sellable item**, see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/Components/SellableItemComponent.cs) for reference.
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

**Component class to store custom data with item variant**, see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Plugin.Hunk.Catalog.Test/Components/VariantComponent.cs) for reference.
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

### Create Sellable Item mapper class

Write code to read field/property values from your custom sellable item and write to Sitecore Commerce Sellable Item commerce entity.
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
* Override *Initialize* method and write code to read values from your custom sellable item class and assign to class properties
* Override *Map* method and write code to read values from your custom sellable item class and assign to commerce entitiy properties.

### Create Sellable Item Child Component mapper class

Write code to read field/property values from your custom sellable item and write to Sitecore Commerce Component.
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
* Override *Map* method and write mapping code.

### Create Item Variant mapper class

Write code to read field/property values from your custom variant and write to Sitecore Commerce Item Variant component.
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

### Create Item Variant's Child Component mapper class

Write code to read field/property values from your custom variant write to Sitecore Commerce component.
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

### Configure mapper classes.

**Create a Policy Set file and add "CatalogImportPolicy" in it.**, see sample [here](https://github.com/vipin-banka/hunk-catalog/blob/master/engine/Sitecore.Commerce.Engine/wwwroot/data/Environments/PlugIn.CatalogImport.PolicySet-1.0.0.json) for reference.
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

* Attach this policy set with commerce environment.

### How to test?
* Build and deploy commerce solution.
* Bootstrap commerce using postman.
* Import [Sample Postman Collection](https://github.com/vipin-banka/hunk-catalog/blob/master/postman/import/Catalog%20Import.postman_collection.json) in postman.
* Open **Import Sellable Item** request.
* Execute the request.
