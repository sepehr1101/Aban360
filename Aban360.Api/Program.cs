using Aban360.Api.Extensions;
using Aban360.UserPool.Persistence.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen.ConventionalRouting;

var builder = WebApplication.CreateBuilder(args);
var services= builder.Services;

//DI
services.AddUserPoolExtensions();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSwaggerGenWithConventionalRoutes();

services.AddCaptcha();

services.UpdateAndSeedUserPoolDb();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
