using Stimulsoft.Base;

namespace Aban360.Api.Extensions
{
    public static class ConfigureSti
    {
        public static void AddStiFonts(this WebApplicationBuilder builder)
        {
            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazir", "Vazir-FD-WOL.ttf"));
            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazir", "Vazir-Bold-FD-WOL.ttf"));
            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazir", "Vazir-Light-FD-WOL.ttf"));
            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazir", "Vazir-Medium-FD-WOL.ttf"));

            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazirmatn", "Vazirmatn-FD-Regular.ttf"));
            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazirmatn", "Vazirmatn-FD-Bold.ttf"));
            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazirmatn", "Vazirmatn-FD-Light.ttf"));
            StiFontCollection.AddFontFile(Path.Combine(builder.Environment.ContentRootPath, "AppData", "Fonts", "Vazirmatn", "Vazirmatn-FD-Medium.ttf"));
        }
    }
}
