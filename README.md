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

Create two c# classes representing your source sellable item and source variant. Your classes will look like below:

**Class for your source sellable item**
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

**Class for your source varint**
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
