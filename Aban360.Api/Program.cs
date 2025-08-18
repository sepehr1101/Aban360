using Aban260.BlobPool.Infrastructure.Features.DmsServices;
using Aban360.Api.ExceptionHandlers;
using Aban360.Api.Extensions;
using Aban360.Api.Hubs.Implementations;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
//DI
builder.Services.AddDI();
builder.Services.AddAuth(configuration);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.AddCaptcha();
builder.Services.AddCustomDbContext(configuration);
builder.Services.AddMigragionsAndSeeds();
builder.AddHangfire();

builder.Services.AddMvc();
builder.Services.AddRazorPages();


builder.Services.AddCustomCors();
builder.Services.AddCustomOptions(configuration);

//Exceptions
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//serilog
builder.Services.AddSerilog(configuration);
builder.Services.AddSignalR();    //todo: replace into configure signalR

//todo 
builder.Services.AddTransient<TokenInterceptor>();
builder.Services.AddHttpClient("token")//write by z-e
    .AddHttpMessageHandler<TokenInterceptor>();





var app = builder.Build();
//app.UsePathBase("/aban360");
app.UseExceptionHandler("/error");
app.AddSwaggerApp();

// Configure the HTTP request pipeline.




app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "lib")),
    RequestPath = "/lib"
});

app.UseRouting();
app.UseCustomCors();
app.UseSerilogInterface();
app.UseAuthentication();
app.UseAuthorization();


app.AddHangfireDashboard(configuration);
configuration.AddCronjobs();
app.MapHub<NotifyHub>("/v1/notify-hub");        //todo: replace into configure signalR

app.MapControllers();
app.Run();

public partial class Program { }

