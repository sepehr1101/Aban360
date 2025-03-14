using Aban360.Api.ExceptionHandlers;
using Aban360.Api.Extensions;
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

var app = builder.Build();
app.UseExceptionHandler("/error");
app.AddSwaggerApp();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "lib")),
    RequestPath = "/lib"
});
app.UseRouting();
app.UseCustomCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogInterface();

app.AddHangfireDashboard(configuration);

app.MapControllers();
app.Run();

public partial class Program { }

