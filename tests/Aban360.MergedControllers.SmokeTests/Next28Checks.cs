using System.Reflection;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

static class Next28Checks
{
    public static async Task RunFocused()
    {
        var services = new ServiceCollection(); services.AddLogging();
        foreach (var name in ApplicationServiceBaseline.Next28Operations.Select(o => o.Assembly).Distinct())
        {
            var assembly = Assembly.Load(name);
            string methodName = "Add" + name.Split('.')[1] + "ApplicationInjections";
            var candidates = assembly.GetExportedTypes().SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                .Where(m => m.Name == methodName && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(IServiceCollection)).ToArray();
            var injection = candidates.Length == 1 ? candidates[0] : candidates.Single(m => m.DeclaringType?.Namespace == name + ".Extensions");
            injection.Invoke(null, new object[] { services });
        }
        using var provider = services.BuildServiceProvider();
        await Run(provider.GetRequiredService<IMapper>(), services);
    }
    public static async Task<int> Run(IMapper mapper, IServiceCollection registrations)
    {
        int checks = 0, scenarios = 0, successes = 0;
        void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
        var operations = ApplicationServiceBaseline.Next28Operations;
        foreach (var group in operations.GroupBy(o => o.Service))
        {
            var assembly = Assembly.Load(group.First().Assembly);
            var contract = assembly.GetType(group.Key, true)!;
            var type = assembly.GetTypes().Single(t => t.IsClass && !t.IsAbstract && contract.IsAssignableFrom(t));
            var registration = registrations.Where(d => d.ServiceType == contract).ToArray();
            Check(registration.Length == 1 && registration[0].ImplementationType == type && registration[0].Lifetime == ServiceLifetime.Scoped, "Next28 DI registration: " + contract.Name);
            var services = new ServiceCollection();
            foreach (var descriptor in registrations) ((ICollection<ServiceDescriptor>)services).Add(descriptor);
            var probe = new ServiceProbe(mapper, CancellationToken.None);
            foreach (var parameter in type.GetConstructors().Single().GetParameters())
                services.AddScoped(parameter.ParameterType, _ => probe.Proxy(parameter.ParameterType));
            using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
            using var first = provider.CreateScope(); using var second = provider.CreateScope();
            var resolved = first.ServiceProvider.GetRequiredService(contract);
            Check(ReferenceEquals(resolved, first.ServiceProvider.GetRequiredService(contract)) && !ReferenceEquals(resolved, second.ServiceProvider.GetRequiredService(contract)), "Next28 scoped lifetime: " + contract.Name);
            foreach (var operation in group)
            {
                var beforeType = typeof(Next28Checks).Assembly.GetType(operation.Original, true)!;
                foreach (var before in beforeType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(m => m.Name == "Handle"))
                {
                var after = type.GetMethod(operation.Method, before.GetParameters().Select(p => p.ParameterType).ToArray())!;
                Check(before.ReturnType == after.ReturnType && before.GetParameters().Select(p => p.ParameterType).SequenceEqual(after.GetParameters().Select(p => p.ParameterType)), "Next28 signature: " + operation.Method);
                // These lookup operations use isolated query dependencies. Direct SQL workflows
                // are covered by the executable-source comparison, without opening databases.
                bool lookup = operation.Feature is "AssessmentByZone" or "UserPolicy" or "OnlineUsers" or "CustomerLookup" or "VillageLookup" or "ModifyCauseLookup" or "BillLists" or "ImpactSign" or "MeterReadingExcludeCause" or "TankerTariff" or "RequestDictionaries" or "RequestCartable";
                if (!lookup) continue;
                using var cancellation = new CancellationTokenSource();
                async Task<ServiceConsolidationChecks.Outcome> Compare(string mode, int fail = 0, int cancel = 0)
                {
                    ServiceProbe State() => new(mapper, cancellation.Token) { Mode = mode, FailAt = fail, CancelAt = cancel, CriticalContext = true };
                    var old = await ServiceConsolidationChecks.Execute(beforeType, before, State());
                    var current = await ServiceConsolidationChecks.Execute(type, after, State());
                    Check(old.Result == current.Result && old.Error == current.Error && old.Calls.SequenceEqual(current.Calls) && old.Mutations == current.Mutations && old.AsyncSteps.SequenceEqual(current.AsyncSteps), "Next28 behavior: " + operation.Method + " / " + mode + " / " + fail + " / " + cancel + "\nOLD " + System.Text.Json.JsonSerializer.Serialize(old) + "\nNEW " + System.Text.Json.JsonSerializer.Serialize(current));
                    scenarios++; return old;
                }
                var success = await Compare("success");
                if (success.Error is null) successes++;
                await Compare("empty");
                for (int step = 1; step <= success.Calls.Length; step++)
                {
                    await Compare("success", fail: step);
                    await Compare("success", cancel: step);
                }
                }
            }
        }
        Check(successes >= 10, "Too few successful Next28 lookup fixtures: " + successes);
        checks += await Next28ReviewChecks.Run(mapper, registrations);
        Console.WriteLine($"Next28: {checks} runtime checks; {scenarios} before/after scenarios; {successes} successful lookup fixtures. All 28 services have scoped DI registration.");
        return checks;
    }
}
