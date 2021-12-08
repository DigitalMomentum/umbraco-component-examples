using DM.Core.Components.Imaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;

using System.IO;
using System.Linq;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Gif;
using Microsoft.Extensions.Configuration;

namespace DM.Core.Components.Imaging
{
    public class ImageMaxSizeHandler : INotificationHandler<MediaSavedNotification>
    {
        private readonly MediaFileManager _mediaFileSystem;
        private readonly IMediaService _mediaService;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
        private readonly ImageMaxSizeConfig _imagingConfig;
        private readonly ContentSettings _contentSettings;

        public ImageMaxSizeHandler(IMediaService mediaService, MediaFileManager mediaFileSystem, IOptions<ContentSettings> contentSettings, IContentTypeBaseServiceProvider contentTypeBaseServiceProvider, IConfiguration configuration)
        {


            _imagingConfig = ImageMaxSizeConfig.GetConfigInstance(configuration);

            _mediaFileSystem = mediaFileSystem;
            _mediaService = mediaService;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;


            _contentSettings = contentSettings.Value;

        }

        public void Handle(MediaSavedNotification notification)
        {
            string[] supportedTypes = _contentSettings.Imaging.ImageFileTypes;
            int maxWidth = _imagingConfig.MaxWidth;
            int maxHeight = _imagingConfig.MaxHeight;
            if(maxWidth == 0 && maxHeight == 0)
            {
                maxWidth = 1920;
            }
            foreach (IMedia media in notification.SavedEntities)
            {
                string UmbracoFileAlias = _contentSettings.Imaging.AutoFillImageProperties?.FirstOrDefault().Alias; //This could be hardcoded to "umbracoFile" but probably better this way 
                if (media.ContentType.Alias == "Image" && media.HasProperty(UmbracoFileAlias))
                {
                    string path = null;
                    try
                    {
                        var umbracoFile = JsonConvert.DeserializeObject<Umbraco.Cms.Core.PropertyEditors.ValueConverters.ImageCropperValue>(media.GetValue<string>(UmbracoFileAlias));

                        path = umbracoFile.Src;
                    }
                    catch
                    {
                        path = media.GetValue<string>(UmbracoFileAlias); //Do we still need this?
                    }


                    if (path != null)
                    {
                        string extension = Path.GetExtension(path).ToLower();

                        if (!string.IsNullOrEmpty(extension)) //Check to see if it has a path
                        {
                            extension = extension.Substring(1);
                            if (supportedTypes.Contains(extension))
                            {
                                // Resize the image to 1920px wide, height is driven by the
                                // aspect ratio of the image.

                                string fullPath = _mediaFileSystem.FileSystem.GetFullPath(path);

                                if (_mediaFileSystem.FileSystem.FileExists(path))
                                {
                                    MemoryStream outStream = new MemoryStream();
                                    using (var imgStream = _mediaFileSystem.FileSystem.OpenFile(path))
                                    {
                                        IImageFormat format;
                                        using (Image image = Image.Load(imgStream, out format))
                                        {
                                            //Check to see if the image is too big and then resize
                                            if ((maxWidth != 0 && image.Width > maxWidth) || (maxHeight != 0 && image.Height > maxHeight))
                                            {
                                                //Bigger than allowed, so resize
                                                image.Mutate(x => x.Resize(new Size(maxWidth, maxHeight)));
                                                image.Save(outStream, format);
                                            }
                                        }
                                    }
                                    if (outStream.Length > 0)
                                    {
                                        //We have an updated image file
                                        _mediaFileSystem.FileSystem.AddFile(path, outStream, true);
                                        _mediaService.Save(media); //This updates the correct image size
                                    }
                                }
                            }
                        }


                    }

                }
            }
        }
    }
}
