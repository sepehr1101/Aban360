using System.Reflection;
using System.Text.Json;

namespace Aban360.Tests.ServiceBaselines;

// Explicit mapping between the new application boundary and frozen handler contracts.
// This lets older controller comparisons keep checking calls, inputs and ordering.
public static class ApplicationServiceBaseline
{
    public static readonly ServiceOperation[] Operations = new[] { "ServiceConsolidation15.json", "ServiceConsolidation30.json", "ServiceConsolidation20.json", "ServiceConsolidation19.json" }
        .SelectMany(file => JsonSerializer.Deserialize<ServiceOperation[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, file)))!).ToArray();
    private static readonly Dictionary<(string, string), string> Names = Operations.ToDictionary(
        o => (o.Service, o.Method), o => o.OldContract + "." + o.OldMethod);
    public static string CallName(Type contract, MethodInfo method) =>
        Names.TryGetValue((contract.FullName!, method.Name), out var name) ? name : contract.FullName + "." + method.Name;
}

public record ServiceOperation(string Feature, string Assembly, string Original, string OldContract,
    string Service, string Method, string OldMethod);
