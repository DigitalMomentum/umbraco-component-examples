# Thumbnail Content List View

This content list view component can be used to show a list of thumbnails and titles in a content list view, as well as of the standard grid and table layouts.

This example is geared towards showing a product, product name and price, however it can be altered to suit any data. 

[Preview](_screenshots/listview.png)

## Trialing
To trial this component, install Umbraco with the starter kit and use the instalation instructions below to use for the products list view. 

## Intallation

1. Copy the App_Plugins folder to you umbraco installation
2. Enable the list view for your document type and click "create custom ist view"
3. Add any additional columns displayed. For this example the list view requres Photos, productName, Price (but these cn be edited to be your own property aliases)
3. Add a new layout, pick an icon, name, and add the layout path to the html file `/App_Plugins/DM.ListViews/Products/Products.html`


## Editing Properties
If you have property aliases that are different to the examples, you'll need to make a few adjustments to the html and possibly the JS file. 

Edit `app_plugins\DM.ListView\Products\Products.html` and replace `item.photos`, `item.productName`, `item.price` to match your aliases. 
*For example, if yout image media picker alias is called `featureImage`, then change `item.photos` to `item.featureImage`

If you need to change the property alias of the image, you will need to make a small update to the `app_plugins\DM.ListView\Products\Products.controller.js` file
Find `value.photos` (there are 2 to edit) and change to your alias. 

