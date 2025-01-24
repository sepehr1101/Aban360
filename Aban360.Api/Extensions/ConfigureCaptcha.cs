using DNTCaptcha.Core;

namespace Aban360.Api.Extensions
{
    public static class ConfigureCaptcha
    {
        const int _expireMin = 5;
        public static void AddCaptcha(this IServiceCollection services)
        {
            services.AddDNTCaptcha(options =>
            {
                options.UseMemoryCacheStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
                                                        // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
                                                        // options.UseCookieStorageProvider(SameSiteMode.None) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                        // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                                                        // .UseDistributedSerializationProvider()

               // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
               // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
               //.UseCustomFont(Path.Combine(_env.WebRootPath, "fonts", "IRANSans(FaNum)_Bold.ttf")) // This is optional.
               .AbsoluteExpiration(minutes: _expireMin)
               .ShowThousandsSeparators(false)
               .WithEncryptionKey("This is my secure key!")
               .InputNames(// This is optional. Change it if you don't like the default names.
                   new DNTCaptchaComponent
                   {
                       CaptchaHiddenInputName = "CaptchaText",
                       CaptchaHiddenTokenName = "CaptchaToken",
                       CaptchaInputName = "CaptchaInputText"
                   })
               .Identifier("Aban360Captcha")// This is optional. Change it if you don't like its default name.
               ;
            });
        }
    }
}
