using Aban360.Api.Extensions;
using Aban360.UserPool.Persistence.Extensions;
using DeviceDetectorNET.Parser.Device;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
//DI
builder.Services.AddUserPoolExtensions();
builder.Services.AddCustomJwtBearer(configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenWithConventionalRoutes();
builder.Services.AddCaptcha();
builder.Services.AddCustomDbContext(configuration);

builder.Services.UpdateAndSeedUserPoolDb();
builder.Services.AddCustomCors();
//builder.Services.AddCustomAntiforgery();
builder.Services.AddCustomOptions(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

