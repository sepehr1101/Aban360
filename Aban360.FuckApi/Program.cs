using DNTCaptcha.Core;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenWithConventionalRoutes();


builder.Services.AddDNTCaptcha(options =>
{
    options.UseMemoryCacheStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
                                            // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
                                            // options.UseCookieStorageProvider(SameSiteMode.None) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                            // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                                            // .UseDistributedSerializationProvider()

   // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
   // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
   //.UseCustomFont(Path.Combine(_env.WebRootPath, "fonts", "IRANSans(FaNum)_Bold.ttf")) // This is optional.
   .AbsoluteExpiration(minutes: 7)
   .ShowThousandsSeparators(false)
   .WithEncryptionKey("This is my secure key!")
   .InputNames(// This is optional. Change it if you don't like the default names.
       new DNTCaptchaComponent
       {
           CaptchaHiddenInputName = "PolCaptchaText",
           CaptchaHiddenTokenName = "PolCaptchaToken",
           CaptchaInputName = "PolCaptchaInputText"
       })
   .Identifier("polCaptcha")// This is optional. Change it if you don't like its default name.
   ;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapControllers();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.Run();

