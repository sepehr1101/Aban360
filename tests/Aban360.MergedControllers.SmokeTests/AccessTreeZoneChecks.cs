using Endpoint = Aban360.UserPool.Domain.Features.AceessTree.Entites.Endpoint;
using System.Reflection;
using Aban360.Tests.ServiceBaselines;
using Aban360.UserPool.Application.Features.AccessTree.Services;
using Aban360.UserPool.Domain.Features.AceessTree.Entites;
using Aban360.LocationPool.Application.Features.MainHierarchy.Services;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using Module = Aban360.UserPool.Domain.Features.AceessTree.Entites.Module;

static class AccessTreeZoneChecks
{
    public static async Task<int> Run(IMapper mapper)
    {
        int checks = 0;
        void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); checks++; }
        object Build(Type contract, Func<Type, MethodInfo, object?[], object?> invoke)
        {
            var type = contract.Assembly.GetTypes().Single(t => t.IsClass && contract.IsAssignableFrom(t));
            return type.GetConstructors().Single().Invoke(type.GetConstructors().Single().GetParameters().Select(p =>
                p.ParameterType == typeof(IMapper) ? mapper : ScopeDependencyProxy.For(p.ParameterType, (m, a) => invoke(p.ParameterType, m, a))).ToArray());
        }
        using var cancellation = new CancellationTokenSource(); var token = cancellation.Token;
        var app = new App { Id = 7, Title = "App", IsActive = true, InMenu = true, LogicalOrder = 4, Style = "app-style" };
        var module = new Module { Id = 17, AppId = 7, App = app, Title = "Module", Description = "Details", ClientRoute = "/mixed/Path", IsActive = true };
        var sub = new SubModule { Id = 27, ModuleId = 17, Module = module, Title = "Sub", IsActive = false, InMenu = true, ClientRoute = "/sub" };
        var endpoint = new Endpoint { Id = 37, SubModuleId = 27, SubModule = sub, Title = "Read", AuthValue = "Access:Read.MixedCase", IsActive = true };
        var inactive = new Endpoint { Id = 38, SubModuleId = 27, SubModule = sub, Title = "Hidden", AuthValue = "Access:Other", IsActive = false };
        app.Modules.Add(module); module.SubModules.Add(sub); sub.Endpoints.Add(endpoint); sub.Endpoints.Add(inactive);
        foreach (var item in new[]
        {
            ("App", typeof(IAppService), (object)Task.FromResult<IEnumerable<App>>(new[] { app })),
            ("Module", typeof(IModuleService), (object)Task.FromResult<IEnumerable<Module>>(new[] { module })),
            ("SubModule", typeof(ISubModuleService), (object)Task.FromResult<IEnumerable<SubModule>>(new[] { sub }))
        })
        {
            int queries = 0;
            object? Query(Type contract, MethodInfo method, object?[] args)
            {
                Check(method.Name == "GetChildrens" && (int)args[0]! == 7, item.Item1 + " must query its requested tree ID");
                queries++; return item.Item3;
            }
            var operation = ApplicationServiceBaseline.Operations.Single(o => o.Feature == item.Item1 && o.Method == "GetChildrenAsync");
            var original = typeof(AccessTreeZoneChecks).Assembly.GetType(operation.Original, true)!;
            var beforeInstance = original.GetConstructors().Single().Invoke(original.GetConstructors().Single().GetParameters().Select(p =>
                p.ParameterType == typeof(IMapper) ? mapper : ScopeDependencyProxy.For(p.ParameterType, (m, a) => Query(p.ParameterType, m, a))).ToArray());
            var before = (Task)original.GetMethod("Handle")!.Invoke(beforeInstance, new object[] { 7, token })!;
            var current = Build(item.Item2, Query);
            var after = (Task)item.Item2.GetMethod("GetChildrenAsync")!.Invoke(current, new object[] { 7, token })!;
            await before; await after;
            var oldData = before.GetType().GetProperty("Result")!.GetValue(before);
            var newData = after.GetType().GetProperty("Result")!.GetValue(after);
            Check(ServiceConsolidationChecks.Snapshot(oldData) == ServiceConsolidationChecks.Snapshot(newData), item.Item1 + " nested tree projection changed");
            Check(queries == 2, item.Item1 + " tree reads must not issue writes or extra queries");
        }
        var appService = (IAppService)Build(typeof(IAppService), (_, m, _) => m.Name == "GetChildrens" ? Task.FromResult<IEnumerable<App>>(new[] { app }) : throw new Exception("Unexpected tree query"));
        var appDto = (await appService.GetChildrenAsync(7, token)).Single();
        Check(appDto.Id == 7 && appDto.Modules.Single().Id == 17 && appDto.Modules.Single().AppTitle == "App", "App/module membership and parent title must survive mapping");
        Check(appDto.InMenu && appDto.LogicalOrder == 4 && appDto.IsActive, "App menu, order and active state must survive mapping");
        var subService = (ISubModuleService)Build(typeof(ISubModuleService), (_, _, _) => Task.FromResult<IEnumerable<SubModule>>(new[] { sub }));
        var subDto = (await subService.GetChildrenAsync(7, token)).Single();
        Check(subDto.Endpoints.Select(e => e.Id).SequenceEqual(new[] { 37, 38 }) && !subDto.IsActive, "Existing tree membership must preserve inactive nodes and order");
        Check(subDto.Endpoints.First().AuthValue == "Access:Read.MixedCase" && subDto.Endpoints.First().SubModuleTitle == "Sub", "Endpoint authorization value and parent title must remain exact");
        var endpointService = (IEndpointService)Build(typeof(IEndpointService), (_, method, args) =>
        {
            Check(method.Name == "GetInclude" && (int)args[0]! == 37, "Endpoint single query must preserve its ID and include path");
            return Task.FromResult(endpoint);
        });
        Check((await endpointService.GetSingleAsync(37, token)).AuthValue == endpoint.AuthValue, "Single endpoint permission value must remain unchanged");
        foreach (var description in new string?[] { null, "", " ", "Details" })
        {
            module.Description = description;
            var service = (IModuleService)Build(typeof(IModuleService), (_, m, _) => m.Name == "GetInclude" ? Task.FromResult<ICollection<Module>>(new[] { module }) : throw new Exception("Unexpected module query"));
            var title = string.IsNullOrWhiteSpace(description) ? "Module" : "Module-" + description;
            Check((await service.GetAllAsync(token)).Single().Title == title && (await service.GetAllAsync(token)).Single().Title == title, "Module title formatting must preserve blank descriptions and avoid repeated suffixes");
            Check(module.Title == "Module", "Formatting must not mutate the tracked module title");
        }

        var zones = new List<Zone> { new() { Id = 100, Title = "Zone A", RegionId = 2, Region = new Region { Id = 2, Title = "Region" } }, new() { Id = 200, Title = "Zone B", RegionId = 2, Region = new Region { Id = 2, Title = "Region" } } };
        var ids = new List<int> { 100, 200, 100 };
        var gate = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        var zoneService = (IZoneService)Build(typeof(IZoneService), (_, method, args) =>
        {
            if (method.Name == "GetCount") { Check(ReferenceEquals(args[0], ids), "Zone count must preserve the original ID collection, including duplicates"); return gate.Task; }
            Check(method.Name == "Get", "Zone gateways must issue only expected queries");
            return args.Length == 0 ? Task.FromResult<ICollection<Zone>>(zones) : Task.FromResult(zones.Single(z => z.Id == (int)args[0]!));
        });
        T Gateway<T>()
        {
            var type = typeof(T).Assembly.GetTypes().Single(t => t.IsClass && typeof(T).IsAssignableFrom(t));
            return (T)Activator.CreateInstance(type, zoneService)!;
        }
        Check(await Gateway<IZoneTitleAddhoc>().Handle(100, token) == "Zone A", "Zone title gateway must use the actual consolidated service");
        var dictionaries = await Gateway<IZoneAllQueryAddhoc>().Get(token);
        Check(dictionaries.Select(z => z.Id).SequenceEqual(new[] { 100, 200 }) && dictionaries.Select(z => z.Title).SequenceEqual(new[] { "Zone A", "Zone B" }), "Zone dictionary projection must preserve IDs, titles and order");
        var pending = Gateway<IZoneCountQueryAddhoc>().GetCount(ids, token);
        Check(!pending.IsCompleted, "Zone count gateway must await the real service and persistence query");
        gate.SetResult(2);
        Check(await pending == 2, "Zone count result must pass through unchanged");
        Check(zones.Select(z => z.Title).SequenceEqual(new[] { "Zone A", "Zone B" }), "Zone reads must not mutate tracked entities");
        Console.WriteLine($"PASS: {checks} focused access-tree and Zone checks (nested mapping, exact permission values, menu flags, titles and real-service gateways).");
        return checks;
    }
}

public class ScopeDependencyProxy : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> Callback { get; set; } = null!;
    public static object For(Type contract, Func<MethodInfo, object?[], object?> callback)
    {
        var proxy = (ScopeDependencyProxy)DispatchProxy.Create(contract, typeof(ScopeDependencyProxy)); proxy.Callback = callback; return proxy;
    }
    protected override object? Invoke(MethodInfo? method, object?[]? args) => Callback(method!, args ?? []);
}
