using System.Reflection;
using System.Text.Json;
using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
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
    : type == typeof(long) ? "long" : type == typeof(string) ? "string"
    : type == typeof(bool) ? "bool" : type.Name;

Check(controllerNames.Count == 90, "Expected all 90 consolidated controllers.");
Check(actions.Count >= baseline.Count, "A baseline action was lost during consolidation."); // Additional actions are checked by the later consolidation batches.
foreach (var expected in baseline)
{
    var candidates = actions.Where(a => a.ControllerTypeInfo.FullName == expected.Controller
        && a.AttributeRouteInfo?.Template == expected.Route && Verbs(a).SequenceEqual(expected.Verbs)).ToList();
    Check(candidates.Count == 1, "Missing or duplicate action: " + expected.Route);
    var action = candidates.Single();
    Check(action.ActionName == expected.Action, "MVC action name changed: " + expected.Route);
    Check(action.Parameters.Select(p => TypeName(p.ParameterType) + " " + p.Name)
        .SequenceEqual(expected.Parameters), "Parameter signature changed: " + expected.Route);
    if (expected.ReturnType != null)
        Check(TypeName(action.MethodInfo.ReturnType) == expected.ReturnType,
            "Return type changed: " + expected.Route);
    if (expected.OptionalParameters != null)
    {
        foreach (var parameter in action.MethodInfo.GetParameters())
            Check(parameter.IsOptional == expected.OptionalParameters[parameter.Name!],
                "Optional parameter changed: " + expected.Route + " / " + parameter.Name);
    }
    foreach (var binding in expected.ExplicitBindings)
    {
        var parameter = action.Parameters.Single(p => p.Name == binding.Key);
        Check(parameter.BindingInfo?.BindingSource?.Id == binding.Value,
            "Parameter binding changed: " + expected.Route + " / " + binding.Key);
    }
    var responses = action.MethodInfo.GetCustomAttributes<ProducesResponseTypeAttribute>()
        .Select(r => r.StatusCode + ":" + TypeName(r.Type)).OrderBy(x => x).ToArray();
    Check(responses.SequenceEqual(expected.Responses), "Response metadata changed: " + expected.Route);
    bool anonymous = action.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
        || action.ControllerTypeInfo.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
    bool authorized = !anonymous && (action.ControllerTypeInfo.IsDefined(typeof(AuthorizeAttribute), inherit: true)
        || action.MethodInfo.IsDefined(typeof(AuthorizeAttribute), inherit: true));
    Check(authorized == expected.RequiresAuthorization, "Authorization changed: " + expected.Route);
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
    Check(instance is ControllerBase, "Controller construction failed: " + controller.Name);
    foreach (var expected in baseline.Where(e => e.Controller == controller.FullName && e.RawResponse))
    {
        // The original Role params-create controller returned its DTO directly, without the shared envelope.
        var action = actions.Single(a => a.ControllerTypeInfo.AsType() == controller
            && a.AttributeRouteInfo?.Template == expected.Route);
        Check(!action.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute), inherit: true),
            "Anonymous action must preserve the existing JWT token-validation flow: " + expected.Route);
        int handlerIndex = Array.FindIndex(constructor.GetParameters(),
            p => p.ParameterType.Name == "IRoleQueryParamsOfCreateHandler");
        var proxy = (ConstructorOnlyProxy)dependencies[handlerIndex];
        var resultType = constructor.GetParameters()[handlerIndex].ParameterType.GetMethod("Handle")!
            .ReturnType.GetGenericArguments().Single();
        var payloadConstructor = resultType.GetConstructors().OrderBy(c => c.GetParameters().Length).First();
        var payload = payloadConstructor.Invoke(payloadConstructor.GetParameters()
            .Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray());
        proxy.AllowedMethod = "Handle";
        proxy.Result = payload;
        var response = await (Task<IActionResult>)action.MethodInfo.Invoke(instance,
            new object[] { CancellationToken.None })!;
        Check(response is OkObjectResult ok && ReferenceEquals(ok.Value, payload) && ok.StatusCode == 200,
            "Raw response was wrapped or altered: " + expected.Route);
        Check(proxy.Calls == 1, "Handler invocation changed: " + expected.Route);

        var createHandler = (ConstructorOnlyProxy)dependencies[Array.FindIndex(constructor.GetParameters(),
            p => p.ParameterType.Name == "IRoleCreateHandler")];
        createHandler.AllowedMethod = "Handle";
        var unitOfWork = (ConstructorOnlyProxy)dependencies[Array.FindIndex(constructor.GetParameters(),
            p => p.ParameterType.Name == "IUnitOfWork")];
        unitOfWork.AllowedMethod = "SaveChangesAsync";
        unitOfWork.Result = 0;
        var createDto = new RoleCreateDto { Name = "review", Title = "review" };
        var createAction = actions.Single(a => a.ControllerTypeInfo.AsType() == controller
            && a.AttributeRouteInfo?.Template == "v1/role/create");
        var createResponse = await (Task<IActionResult>)createAction.MethodInfo.Invoke(instance,
            new object[] { createDto, CancellationToken.None })!;
        Check(createResponse is OkObjectResult created
            && created.Value is ApiResponseEnvelope<RoleCreateDto> envelope
            && ReferenceEquals(envelope.Data, createDto) && envelope.Success
            && envelope.HttpStatusCode == 200 && envelope.SuccessMessage == MessageResources.SuccessfulProccess,
            "Role's normal response envelope changed.");
        Check(createHandler.Calls == 1 && unitOfWork.Calls == 1, "Role create/save behavior changed.");
    }
}
string exportName = "controller-consolidation-smoke.csv";
string exportPath = Path.Combine(AppContext.BaseDirectory, "AppData", "Excels", exportName);
try
{
    Aban360.Api.Tools.ControllerInspector.ExportControllerActions(exportName);
    var exportedRoleRoutes = File.ReadLines(exportPath).Skip(1)
        .Select(line => line.Split(','))
        .Where(columns => columns[1].Trim() == "Role")
        .Select(columns => columns[^1].Trim()).OrderBy(x => x).ToArray();
    var expectedRoleRoutes = baseline.Where(e => e.Controller.EndsWith(".RoleController") && e.RequiresAuthorization)
        .Select(e => e.Route).OrderBy(x => x).ToArray();
    Check(exportedRoleRoutes.SequenceEqual(expectedRoleRoutes), "Role actions disappeared from controller export.");
}
finally
{
    if (File.Exists(exportPath)) File.Delete(exportPath);
}
Console.WriteLine($"PASS: {checks} checks; {controllerNames.Count} controllers, {baseline.Count} actions, " +
    $"{baseline.Sum(a => a.Verbs.Length)} HTTP method/route combinations match the pre-merge baseline.");
Console.WriteLine("MVC discovery, route conflicts, bindings, response metadata, authorization and constructors checked without database access.");

public class ConstructorOnlyProxy : DispatchProxy
{
    public string? AllowedMethod { get; set; }
    public object? Result { get; set; }
    public int Calls { get; private set; }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (AllowedMethod == null || targetMethod?.Name != AllowedMethod)
            throw new InvalidOperationException("Unexpected dependency call: " + targetMethod?.Name);
        Calls++;
        if (targetMethod.ReturnType == typeof(Task)) return Task.CompletedTask;
        return typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(targetMethod.ReturnType.GetGenericArguments().Single())
            .Invoke(null, new[] { Result });
    }
}
public record ExpectedAction(string Controller, string Route, string[] Verbs, string Action,
    string[] Parameters, Dictionary<string, string> ExplicitBindings, string[] Responses,
    string? ReturnType = null, Dictionary<string, bool>? OptionalParameters = null,
    bool RequiresAuthorization = true, bool RawResponse = false);
