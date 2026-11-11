using System.Reflection;
using System.Text.Json;
using Aban360.Api.Controllers.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

// This baseline comes from the original controllers, before consolidation.
var baseline = JsonSerializer.Deserialize<List<ExpectedAction>>(
    File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "PreMergeEndpoints.json")))!;
var services = new ServiceCollection();
services.AddLogging();
services.AddControllers().AddApplicationPart(typeof(BaseController).Assembly);
using var provider = services.BuildServiceProvider();
var allActions = provider.GetRequiredService<IActionDescriptorCollectionProvider>()
    .ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToList();
var controllerNames = baseline.Select(x => x.Controller).ToHashSet();
var actions = allActions.Where(x => controllerNames.Contains(x.ControllerTypeInfo.FullName!)).ToList();
int checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
    checks++;
}
string[] Verbs(ControllerActionDescriptor action) => action.ActionConstraints!
    .OfType<HttpMethodActionConstraint>().SelectMany(x => x.HttpMethods)
    .Distinct().OrderBy(x => x).ToArray();
string TypeName(Type type) => type.IsGenericTypeDefinition
    ? type.Name.Split('`')[0] + "<" + new string(',', type.GetGenericArguments().Length - 1) + ">"
    : type.IsGenericType
    ? type.Name.Split('`')[0] + "<" + string.Join(",", type.GetGenericArguments().Select(TypeName)) + ">"
    : type == typeof(short) ? "short" : type == typeof(int) ? "int"
    : type == typeof(long) ? "long" : type == typeof(string) ? "string" : type.Name;

Check(controllerNames.Count == 70, "Expected all 70 consolidated controllers.");
Check(actions.Count == baseline.Count, "An action was lost or added during consolidation.");
foreach (var expected in baseline)
{
    var candidates = actions.Where(a => a.ControllerTypeInfo.FullName == expected.Controller
        && a.AttributeRouteInfo?.Template == expected.Route && Verbs(a).SequenceEqual(expected.Verbs)).ToList();
    Check(candidates.Count == 1, "Missing or duplicate action: " + expected.Route);
    var action = candidates.Single();
    Check(action.ActionName == expected.Action, "MVC action name changed: " + expected.Route);
    Check(action.Parameters.Select(p => TypeName(p.ParameterType) + " " + p.Name)
        .SequenceEqual(expected.Parameters), "Parameter signature changed: " + expected.Route);
    foreach (var binding in expected.ExplicitBindings)
    {
        var parameter = action.Parameters.Single(p => p.Name == binding.Key);
        Check(parameter.BindingInfo?.BindingSource?.Id == binding.Value,
            "Parameter binding changed: " + expected.Route + " / " + binding.Key);
    }
    var responses = action.MethodInfo.GetCustomAttributes<ProducesResponseTypeAttribute>()
        .Select(r => r.StatusCode + ":" + TypeName(r.Type)).OrderBy(x => x).ToArray();
    Check(responses.SequenceEqual(expected.Responses), "Response metadata changed: " + expected.Route);
    Check(action.ControllerTypeInfo.IsDefined(typeof(AuthorizeAttribute), inherit: true),
        "Authorization attribute missing: " + expected.Controller);
    foreach (var verb in expected.Verbs)
    {
        // Include all other API controllers when looking for an ambiguous endpoint.
        Check(allActions.Count(a => a.AttributeRouteInfo?.Template == expected.Route && Verbs(a).Contains(verb)) == 1,
            "Conflicting API route: " + verb + " " + expected.Route);
    }
}
foreach (var controller in actions.Select(a => a.ControllerTypeInfo.AsType()).Distinct())
{
    var constructor = controller.GetConstructors().Single();
    var dependencies = constructor.GetParameters().Select(p =>
    {
        Check(p.ParameterType.IsInterface, "Unexpected dependency: " + p.ParameterType);
        return DispatchProxy.Create(p.ParameterType, typeof(ConstructorOnlyProxy));
    }).ToArray();
    var instance = constructor.Invoke(dependencies);
    Check(instance is BaseController, "Controller construction failed: " + controller.Name);
}
Console.WriteLine($"PASS: {checks} checks; {controllerNames.Count} controllers, {baseline.Count} actions, " +
    $"{baseline.Sum(a => a.Verbs.Length)} HTTP method/route combinations match the pre-merge baseline.");
Console.WriteLine("MVC discovery, route conflicts, bindings, response metadata, authorization and constructors checked without database access.");

public class ConstructorOnlyProxy : DispatchProxy
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) =>
        throw new InvalidOperationException("Constructor unexpectedly called a dependency: " + targetMethod?.Name);
}
public record ExpectedAction(string Controller, string Route, string[] Verbs, string Action,
    string[] Parameters, Dictionary<string, string> ExplicitBindings, string[] Responses);
