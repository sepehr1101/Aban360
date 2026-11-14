using System.Reflection;
using Aban360.Api.Controllers.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddApplicationPart(typeof(BaseController).Assembly);
builder.Services.AddEndpointsApiExplorer();
typeof(BaseController).Assembly.GetType("Aban360.Api.Extensions.ConfigureSwagger", true)!
    .GetMethod("AddSwaggerService", BindingFlags.NonPublic | BindingFlags.Static)!
    .Invoke(null, new object[] { builder.Services });
await using var app = builder.Build();
app.MapControllers();

var descriptions = app.Services.GetRequiredService<IApiDescriptionGroupCollectionProvider>()
    .ApiDescriptionGroups.Items.SelectMany(group => group.Items).ToArray();
var conflicts = descriptions.GroupBy(description => new
    {
        description.HttpMethod,
        Path = description.RelativePath?.Split('?')[0]
    })
    .Where(group => group.Count() > 1).ToArray();
Console.WriteLine($"Discovered {descriptions.Length} API descriptions and {conflicts.Length} route conflicts.");
foreach (var conflict in conflicts)
{
    Console.WriteLine($"CONFLICT: {conflict.Key.HttpMethod} {conflict.Key.Path}");
    foreach (var action in conflict)
        Console.WriteLine("  " + action.ActionDescriptor.DisplayName);
}

var document = app.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1");
if (conflicts.Length > 0)
    throw new InvalidOperationException("Duplicate API routes were found.");
Console.WriteLine($"PASS: {descriptions.Length} API descriptions have unique method/path pairs.");
Console.WriteLine($"PASS: Swagger v1 generated with {document.Paths.Count} paths.");
