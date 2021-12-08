# Image Max Size Handler

This component checks all images uploaded to the media section and restricts the size of the pixel image. This stops clients uploading rediculous size images into the media section. 

## Installation
1. Copy the .cs files into your project
2. In startup.cs add `.AddNotificationHandler<MediaSavedNotification, ImageMaxSizeHandler>()` in `ConfigureServices(){}`. Add it right before `.Build(). For Example:
   ```C#
    services.AddUmbraco(_env, _config)
        .AddBackOffice()
        .AddWebsite()
        .AddComposers()
        .AddNotificationHandler<MediaSavedNotification, ImageMaxSizeHandler>()
        .Build();
   ```     
3. By default it will restrict images to 1920px wide, however you can add the following configuration to appsettings.json to configure the maximum sizes. The 0 below defaults to any height.
    ```
    "DM": {
        "Imaging": {
            "MaxWidth": 1500,
            "MaxHeight": 0
        }
    }
    ```