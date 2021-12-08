using Microsoft.Extensions.Configuration;

namespace DM.Core.Components.Imaging
{
	public class ImageMaxSizeConfig
	{
		public const string AppSettingPath = "Imaging";
		public int MaxWidth { get; set; }
		public int MaxHeight { get; set; }

		public static ImageMaxSizeConfig GetConfigInstance(IConfiguration config)
		{
			
			ImageMaxSizeConfig ret = new ImageMaxSizeConfig();
			config.GetSection("DM").GetSection(AppSettingPath).Bind(ret);
			return ret;
		}
	}
}
