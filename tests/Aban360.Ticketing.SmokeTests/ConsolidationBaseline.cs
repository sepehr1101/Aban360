using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

// Old contracts exist only in tests. Run the same workflow assertions against the
// frozen handlers and the actual consolidated services without rewriting them.
internal static class ConsolidationBaseline
{
    public static readonly Operation[] Operations = JsonSerializer.Deserialize<Operation[]>(
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "TicketOperations.json")))!;

    public static void UseConsolidatedServices(IServiceCollection services)
    {
        var assembly = typeof(ConsolidationBaseline).Assembly;
        foreach (var group in Operations.GroupBy(o => o.OldContract))
        {
            var contract = assembly.GetType(group.Key, true)!;
            var serviceType = assembly.GetType(group.First().Service, true)!;
            var methods = group.ToDictionary(o => o.OldMethod, o => o.Method);
            services.AddScoped(contract, sp =>
            {
                var proxy = DispatchProxy.Create(contract, typeof(ConsolidatedHandlerProxy));
                ((ConsolidatedHandlerProxy)proxy).Target = sp.GetRequiredService(serviceType);
                ((ConsolidatedHandlerProxy)proxy).Methods = methods;
                return proxy;
            });
        }
    }

    public record Operation(string Feature, string Original, string OldContract, string Service, string Method, string OldMethod);
}

public class ConsolidatedHandlerProxy : DispatchProxy
{
    public object Target { get; set; } = null!;
    public Dictionary<string, string> Methods { get; set; } = null!;
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        try { return Target.GetType().GetMethod(Methods[targetMethod!.Name])!.Invoke(Target, args); }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        { ExceptionDispatchInfo.Capture(ex.InnerException).Throw(); throw; }
    }
}
