using System.Reflection;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Net;
using System.Net.Http.Headers;
using Aban360.Api.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var root = FindRoot();
var fixturePath = Path.Combine(root, "tests", "Aban360.MergedControllers.SmokeTests", "Contracts.json");
var assemblies = new Dictionary<string, Assembly>
{
    ["Api"] = typeof(Aban360.Api.Controllers.V1.BaseController).Assembly,
    ["Bridge"] = typeof(Aban360.BrdigeApi.Controllers.V1.BaseController).Assembly
};
var descriptors = assemblies.ToDictionary(p => p.Key, p => Discover(p.Value));
if (args.Contains("--capture"))
{
    // Explicit maintenance command only. Ordinary execution never updates the baseline.
    var names = new HashSet<string>();
    using (var baseline = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "tests/Aban360.ControllerConsolidation.SmokeTests/PreMergeEndpoints.json"))))
        foreach (var item in baseline.RootElement.EnumerateArray()) names.Add(item.GetProperty("Controller").GetString()!);
    foreach (var file in Directory.GetFiles(Path.Combine(root, "tests"), "Groups.json", SearchOption.AllDirectories)
        .Where(p => !p.Contains(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar) && !p.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar)))
    {
        using var document = JsonDocument.Parse(File.ReadAllText(file));
        foreach (var group in document.RootElement.EnumerateArray()) names.Add(group.GetProperty("Controller").GetString()!);
    }
    // The first additional batch predates Groups.json; its explicit test selection is authoritative too.
    var legacySelection = File.ReadAllText(Path.Combine(root, "tests/Aban360.ControllerBatch110.RegressionTests/Program.cs"));
    var declaration = System.Text.RegularExpressions.Regex.Match(legacySelection, @"var names = new HashSet<string>\s*\{([^}]+)\}").Groups[1].Value;
    if (declaration.Length == 0) throw new Exception("Unable to read the pinned batch-110 coverage selection.");
    var legacyNames = System.Text.RegularExpressions.Regex.Matches(declaration, "\"([^\"]+)\"").Select(m => m.Groups[1].Value).ToHashSet();
    foreach (var type in assemblies["Api"].GetTypes().Where(t => legacyNames.Contains(t.Name))) names.Add(type.FullName!);
    var captured = descriptors.SelectMany(p => p.Value.Where(a => names.Contains(a.ControllerTypeInfo.FullName!)).Select(a => Describe(p.Key, a)))
        .OrderBy(c => c.Project).ThenBy(c => c.Route).ThenBy(c => string.Join(",", c.Verbs)).ToArray();
    File.WriteAllText(fixturePath, JsonSerializer.Serialize(captured, new JsonSerializerOptions { WriteIndented = true }) + Environment.NewLine);
    Console.WriteLine($"Captured {captured.Length} contracts. Review this file before committing it.");
    return;
}
var expected = JsonSerializer.Deserialize<Contract[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Contracts.json")))!;
int checks = 0, httpCases = 0;
void Check(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); checks++; }
Check(expected.Length > 0, "The contract fixture must not be empty.");
Check(expected.Select(Key).Distinct().Count() == expected.Length, "Duplicate fixture keys.");
var selected = new Dictionary<string, List<ControllerActionDescriptor>>();
foreach (var e in expected)
{
    var matches = descriptors[e.Project].Where(a => a.AttributeRouteInfo?.Template == e.Route && Verbs(a).SequenceEqual(e.Verbs)).ToArray();
    Check(matches.Length == 1, $"Missing or ambiguous endpoint: {Key(e)}");
    var current = matches.Single();
    Check(JsonSerializer.Serialize(Describe(e.Project, current)) == JsonSerializer.Serialize(e), $"HTTP contract changed: {Key(e)}");
    foreach (var verb in e.Verbs)
        Check(descriptors[e.Project].Count(a => string.Equals(a.AttributeRouteInfo?.Template, e.Route, StringComparison.OrdinalIgnoreCase) && Verbs(a).Contains(verb)) == 1,
            $"Overlapping HTTP route: {verb} {e.Route}");
    if (!selected.TryGetValue(e.Project, out var actions)) selected[e.Project] = actions = new();
    actions.Add(current);
}
foreach (var project in selected)
{
    var types = project.Value.Select(a => a.ControllerTypeInfo.AsType()).Distinct().ToArray();
    // Fail if an extra public action is accidentally exposed on one of these controllers.
    Check(descriptors[project.Key].Count(a => types.Contains(a.ControllerTypeInfo.AsType())) == project.Value.Count, "An unreviewed action was exposed: " + project.Key);
    foreach (var type in types)
    {
        var ctor = type.GetConstructors().Single();
        var dependencies = ctor.GetParameters().Select(p => DispatchProxy.Create(p.ParameterType, typeof(NoCalls))).ToArray();
        Check(ctor.Invoke(dependencies) is ControllerBase, "Constructor failed: " + type.Name);
    }
    await using var app = BuildHost(types);
    await app.StartAsync();
    using var client = app.GetTestClient();
    foreach (var contract in expected.Where(c => c.Project == project.Key))
    foreach (var verb in contract.Verbs)
    foreach (bool signedIn in new[] { false, true })
    {
        using var request = new HttpRequestMessage(new HttpMethod(verb), "/" + Materialize(contract.Route));
        request.Content = new ByteArrayContent(Array.Empty<byte>());
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(contract.ContentTypes.FirstOrDefault() ?? "application/json");
        if (signedIn) request.Headers.Add("X-Regression-User", "yes");
        using var response = await client.SendAsync(request);
        var status = !signedIn && contract.RequiresAuthorization ? HttpStatusCode.Unauthorized : HttpStatusCode.NoContent;
        Check(response.StatusCode == status, $"HTTP routing/authentication changed: {Key(contract)} {verb}, signedIn={signedIn}, got {(int)response.StatusCode}");
        if (status == HttpStatusCode.NoContent) Check(response.Headers.Contains("X-Regression-Boundary"), "A request escaped the safety boundary.");
        httpCases++;
    }
}
checks += await WorkflowChecks.Run(descriptors["Api"]);
checks += await PermissionChecks.Run(expected, descriptors["Api"]);
checks += await CrudChecks.Run(expected, descriptors["Api"]);
checks += await LookupServiceChecks.Run();
checks += await ServiceConsolidationChecks.Run();
Console.WriteLine($"PASS: {checks} checks; {expected.Length} frozen contracts; {selected.Sum(p => p.Value.Select(a => a.ControllerTypeInfo.FullName).Distinct().Count())} controllers; {httpCases} HTTP routing/authentication cases.");
Console.WriteLine("No database, SMS provider, external HTTP service or real background job was invoked.");

static string FindRoot() { var d = new DirectoryInfo(AppContext.BaseDirectory); while (d != null && !Directory.Exists(Path.Combine(d.FullName, "Aban360.Api"))) d = d.Parent; return d?.FullName ?? throw new Exception("Repository root missing."); }
static string Key(Contract c) => c.Project + ":" + string.Join(",", c.Verbs) + ":" + c.Route;
static string[] Verbs(ControllerActionDescriptor a) => a.ActionConstraints?.OfType<HttpMethodActionConstraint>().SelectMany(c => c.HttpMethods).Distinct().Order().ToArray() ?? [];
static Contract Describe(string project, ControllerActionDescriptor a)
{
    var nullable = new NullabilityInfoContext();
    var parameters = a.MethodInfo.GetParameters().Select(p => WireType(p.ParameterType) + " " + p.Name + ":" + p.IsOptional + ":" + (p.IsOptional ? p.DefaultValue?.ToString() ?? "null" : "required")
        + ":" + nullable.Create(p).ReadState + ":" + a.Parameters.Single(x => x.Name == p.Name).BindingInfo?.BindingSource?.Id).ToArray();
    var authorization = a.EndpointMetadata.OfType<IAuthorizeData>().Select(x => $"{x.Policy}|{x.Roles}|{x.AuthenticationSchemes}").Order().ToArray();
    bool anonymous = a.EndpointMetadata.OfType<IAllowAnonymous>().Any();
    var permission = a.MethodInfo.GetCustomAttribute<EndpointAuthorizationAttribute>();
    bool permissionFilter = a.FilterDescriptors.Any(f => f.Filter is TypeFilterAttribute t && t.ImplementationType == typeof(EndpointAuthorizationFilter));
    var responses = a.MethodInfo.GetCustomAttributes<ProducesResponseTypeAttribute>().Select(r => r.StatusCode + ":" + WireType(r.Type)).Order().ToArray();
    var filters = a.MethodInfo.GetCustomAttributesData().Where(x => x.AttributeType.Namespace != "System.Runtime.CompilerServices" && x.AttributeType != typeof(ActionNameAttribute)
        && x.AttributeType != typeof(RouteAttribute) && !typeof(Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute).IsAssignableFrom(x.AttributeType)
        && x.AttributeType != typeof(EndpointAuthorizationAttribute) && !(x.AttributeType == typeof(TypeFilterAttribute) && x.ConstructorArguments.FirstOrDefault().Value is Type t && t == typeof(EndpointAuthorizationFilter)))
        .Select(x => x.AttributeType.Name + ":" + string.Join("|", x.ConstructorArguments.Select(AttributeValue)) + ":" + string.Join("|", x.NamedArguments.OrderBy(n => n.MemberName).Select(n => n.MemberName + "=" + AttributeValue(n.TypedValue)))).Order().ToArray();
    var shapes = a.MethodInfo.GetParameters().Where(p => p.ParameterType != typeof(CancellationToken)).Select(p => "input:" + p.Name + ":" + Shape(p.ParameterType))
        .Concat(a.MethodInfo.GetCustomAttributes<ProducesResponseTypeAttribute>().Select(r => "output:" + r.StatusCode + ":" + Shape(r.Type))).Order().ToArray();
    var consumes = a.EndpointMetadata.OfType<ConsumesAttribute>().SelectMany(x => x.ContentTypes).Distinct().Order().ToArray();
    return new(project, a.AttributeRouteInfo!.Template!, Verbs(a), a.ActionName, parameters, WireType(a.MethodInfo.ReturnType), responses, authorization, anonymous,
        authorization.Length > 0 && !anonymous, permissionFilter ? (permission?.ControllerName ?? a.ControllerName) + "." + a.ActionName : null, filters, consumes, shapes);
}
static string WireType(Type type) => type.IsArray ? WireType(type.GetElementType()!) + "[]" : type.IsGenericType
    ? type.Name.Split('`')[0] + "<" + string.Join(",", type.GetGenericArguments().Select(WireType)) + ">" : type.Name;
static string AttributeValue(CustomAttributeTypedArgument value) => value.Value switch
{
    Type type => WireType(type),
    IEnumerable<CustomAttributeTypedArgument> items => "[" + string.Join(",", items.Select(AttributeValue)) + "]",
    null => "null",
    IFormattable formatted => formatted.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
    _ => value.Value.ToString()!
};
static string Shape(Type type, int depth = 0, HashSet<Type>? parents = null)
{
    var nullable = Nullable.GetUnderlyingType(type);
    if (nullable != null) return Shape(nullable, depth, parents) + "?";
    if (type.IsEnum) return WireType(type) + "[" + string.Join(",", Enum.GetNames(type).Select(n => n + "=" + Convert.ToInt64(Enum.Parse(type, n)))) + "]";
    if (type == typeof(string) || type == typeof(object) || type.IsPrimitive || type.ContainsGenericParameters) return WireType(type);
    if (type.IsArray) return "array<" + Shape(type.GetElementType()!, depth + 1, parents) + ">";
    if (type.IsGenericType && typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
        return WireType(type.GetGenericTypeDefinition()) + "<" + string.Join(",", type.GetGenericArguments().Select(t => Shape(t, depth + 1, parents))) + ">";
    if (type.Namespace?.StartsWith("System", StringComparison.Ordinal) == true || type.Namespace?.StartsWith("Microsoft", StringComparison.Ordinal) == true) return WireType(type);
    parents ??= new HashSet<Type>();
    if (depth > 4 || parents.Contains(type)) return WireType(type);
    var branch = new HashSet<Type>(parents) { type };
    var nullability = new NullabilityInfoContext();
    return WireType(type) + "{" + string.Join(";", type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(p => p.GetIndexParameters().Length == 0 && p.GetMethod?.IsPublic == true)
        .OrderBy(p => p.Name).Select(p =>
        {
            var annotations = p.GetCustomAttributesData().Where(a => a.AttributeType.Namespace != "System.Runtime.CompilerServices")
                .Select(a => a.AttributeType.Name + "(" + string.Join(",", a.ConstructorArguments.Select(AttributeValue)) + ")" + string.Join(",", a.NamedArguments.OrderBy(n => n.MemberName).Select(n => n.MemberName + "=" + AttributeValue(n.TypedValue)))).Order();
            var jsonName = p.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.Name is "JsonPropertyNameAttribute" or "JsonPropertyAttribute")?.ConstructorArguments.FirstOrDefault().Value?.ToString()
                ?? JsonNamingPolicy.CamelCase.ConvertName(p.Name);
            return jsonName + ":" + Shape(p.PropertyType, depth + 1, branch) + ":" + nullability.Create(p).ReadState + ":" + string.Join("|", annotations);
        })) + "}";
}
static ControllerActionDescriptor[] Discover(Assembly assembly)
{
    var services = new ServiceCollection(); services.AddLogging();
    services.AddControllers().ConfigureApplicationPartManager(m => { m.ApplicationParts.Clear(); m.ApplicationParts.Add(new AssemblyPart(assembly)); });
    using var provider = services.BuildServiceProvider();
    return provider.GetRequiredService<IActionDescriptorCollectionProvider>().ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToArray();
}
static WebApplication BuildHost(Type[] types)
{
    var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Testing", ApplicationName = typeof(NoCalls).Assembly.FullName });
    builder.Logging.ClearProviders(); builder.WebHost.UseTestServer();
    builder.Services.AddAuthentication("Regression").AddScheme<AuthenticationSchemeOptions, TestAuthentication>("Regression", _ => { });
    builder.Services.AddAuthorization();
    builder.Services.AddControllers().ConfigureApplicationPartManager(m => { m.ApplicationParts.Clear(); m.ApplicationParts.Add(new AssemblyPart(types[0].Assembly)); m.FeatureProviders.Add(new SelectedTypes(types)); });
    var app = builder.Build(); app.UseRouting(); app.UseAuthentication(); app.UseAuthorization();
    // Stop after actual routing and authorization middleware, before MVC activation/binding or business side effects.
    app.Use(async (context, next) => { if (context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>() == null) { await next(context); return; } context.Response.Headers["X-Regression-Boundary"] = "stopped"; context.Response.StatusCode = 204; });
    app.MapControllers(); return app;
}
static string Materialize(string route)
{
    var pattern = RoutePatternFactory.Parse(route);
    return string.Join("/", pattern.PathSegments.Select(segment => string.Concat(segment.Parts.Select(part => part switch
    {
        RoutePatternLiteralPart literal => literal.Content,
        RoutePatternSeparatorPart separator => separator.Content,
        RoutePatternParameterPart parameter => parameter.ParameterPolicies.Any(p => p.Content?.StartsWith("guid", StringComparison.OrdinalIgnoreCase) == true) ? "22222222-2222-2222-2222-222222222222" : parameter.ParameterPolicies.Any(p => p.Content?.StartsWith("datetime", StringComparison.OrdinalIgnoreCase) == true) ? "2026-01-02" : "17",
        _ => throw new Exception("Unsupported route part")
    }))));
}
public record Contract(string Project, string Route, string[] Verbs, string Action, string[] Parameters, string ReturnType, string[] Responses, string[] Authorization, bool Anonymous, bool RequiresAuthorization, string? Permission, string[] Filters, string[] ContentTypes, string[] Shapes);
public class NoCalls : DispatchProxy { protected override object? Invoke(MethodInfo? method, object?[]? args) => throw new InvalidOperationException("Unexpected business call: " + method?.Name); }
class SelectedTypes(Type[] types) : IApplicationFeatureProvider<ControllerFeature> { public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature) { for (int i = feature.Controllers.Count - 1; i >= 0; i--) if (!types.Contains(feature.Controllers[i].AsType())) feature.Controllers.RemoveAt(i); } }
class TestAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync() => Task.FromResult(Request.Headers.ContainsKey("X-Regression-User") ? AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.UserData, "22222222-2222-2222-2222-222222222222") }, Scheme.Name)), Scheme.Name)) : AuthenticateResult.NoResult());
}




