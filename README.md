![alt text](https://github.com/vipin-banka/hunk-catalog/blob/feature/initial-branch/docs/hunk.png)

# hunk-catalog for Sitecore Commerce (version 9.2)
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

## What is involved in writing import code?
Overall it involves following:
1. Create your custom POCO classes representing your source entities.
2. Create custom Mapper classes by inheriting from standard implementation comes with plugin.
3. All abstract mapper classes comes with plugin have basic implementation for most of the methods, you just need to override methods that suits your requirements. Basically you need to write simple map code that will read values from source entities and write in commerce entities, components and policies.
4. Configure your mapper class in the policies or resolve using custom pipeline blocks.
5. That’s it. Once your commerce solution is deployed and bootstraped you can try the import operation using Postman calls or you can trigger it from your other custom plugins.
