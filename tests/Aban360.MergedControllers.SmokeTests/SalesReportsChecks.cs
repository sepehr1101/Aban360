using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

internal static class SalesReportsChecks
{
    sealed record Input(string Group, string OldName, string OldNamespace, string Namespace, string Service, Dictionary<string, string> Names);
    sealed record ConsumerInput(string Path, string Current, Dictionary<string, JsonElement> Bindings);
    static readonly Dictionary<string, long> Amounts = new()
    {
        ["DomesticWaterAmount"] = 101001, ["DomesticSewageAmount"] = 202002,
        ["NonDomesticWaterAmount"] = 303003, ["NonDomesticSewageAmount"] = 404004,
        ["InstallationAmount"] = 505005, ["EquipmentAmount"] = 606006, ["Amount"] = 707007
    };
    public static async Task Run()
    {
        var inputs = JsonSerializer.Deserialize<Input[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "SalesReportsInputs.json")))!;
        var consumers = JsonSerializer.Deserialize<ConsumerInput[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "SalesReportsConsumers.json")))!;
        var assembly = Assembly.Load("Aban360.CalculationPool.Application"); var test = typeof(SalesReportsChecks).Assembly;
        int checks = 0, scenarios = 0, positive = 0, consumerMethods = 0;
        void Check(bool value, string message) { if (!value) throw new Exception(message); checks++; }
        var registrations = new ServiceCollection(); registrations.AddLogging();
        Aban360.CalculationPool.Application.Extensions.ConfigureServices.AddCalculationPoolApplicationInjections(registrations);
        using var mapperProvider = registrations.BuildServiceProvider(); var mapper = mapperProvider.GetRequiredService<IMapper>();
        foreach (var group in inputs.GroupBy(i => i.Namespace + ".I" + i.Service))
        {
            var contract = assembly.GetType(group.Key, true)!;
            var implementation = assembly.GetTypes().Single(t => t.IsClass && !t.IsAbstract && contract.IsAssignableFrom(t));
            var registration = registrations.Where(d => d.ServiceType == contract).ToArray();
            Check(registration.Length == 1 && registration[0].ImplementationType == implementation && registration[0].Lifetime == ServiceLifetime.Scoped, "Production registration changed: " + group.Key);
            IServiceCollection local = new ServiceCollection(); local.Add(registration[0]);
            var probe = State("success", CancellationToken.None);
            foreach (var dependency in implementation.GetConstructors().Single().GetParameters()) local.AddScoped(dependency.ParameterType, _ => probe.Proxy(dependency.ParameterType));
            using var provider = local.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
            using var scope = provider.CreateScope(); using var other = provider.CreateScope();
            Check(ReferenceEquals(scope.ServiceProvider.GetRequiredService(contract), scope.ServiceProvider.GetRequiredService(contract)) && !ReferenceEquals(scope.ServiceProvider.GetRequiredService(contract), other.ServiceProvider.GetRequiredService(contract)), "Scoped lifetime changed");
            int groupPositive = 0;
            foreach (var input in group)
            {
                var before = test.GetType("BeforeSalesReports." + input.OldNamespace + "." + input.OldName, true)!;
                foreach (var method in before.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    var after = implementation.GetMethod(input.Names[method.Name], method.GetParameters().Select(p => p.ParameterType).ToArray())!;
                    Check(after is not null && after.ReturnType == method.ReturnType, "Operation signature changed: " + input.OldName);
                    await Compare(before, method, implementation, after!, true); positive++; groupPositive++;
                }
            }
            Check(groupPositive > 0, "Group has no successful fixture: " + group.Key);
            Console.WriteLine($"PASS SalesReports group: {group.First().Group}; {groupPositive} successful operations.");
        }
        foreach (var input in consumers)
        {
            string currentName = input.Current;
            var after = typeof(Aban360.Api.Controllers.V1.BaseController).Assembly.GetType(currentName, true)!;
            var before = test.GetType("BeforeSalesReportsConsumers." + currentName, true)!;
            Check(Attributes(before.GetCustomAttributesData()) == Attributes(after.GetCustomAttributesData()), "Controller route/permission metadata changed");
            foreach (var method in before.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var current = after.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray())!;
                Check(current is not null && method.ReturnType == current.ReturnType && Attributes(method.GetCustomAttributesData()) == Attributes(current.GetCustomAttributesData()), "Action metadata changed: " + currentName + "." + method.Name);
                Check(method.GetParameters().Zip(current!.GetParameters()).All(p => p.First.Name == p.Second.Name && p.First.IsOptional == p.Second.IsOptional && Equals(p.First.DefaultValue, p.Second.DefaultValue) && Attributes(p.First.GetCustomAttributesData()) == Attributes(p.Second.GetCustomAttributesData())), "Action input/binding changed");
                await Compare(before, method, after, current, false); consumerMethods++;
            }
        }
        Check(positive == 19, "Incomplete application success coverage");
        Console.WriteLine($"SalesReports runtime: {checks} checks / {scenarios} before-after scenarios / {positive} successful operations / {consumerMethods} consumer methods; all five scoped services resolved. No live database or external calls.");

        ServiceProbe State(string mode, CancellationToken token)
        {
            var state = new ServiceProbe(mapper, token) { Mode = mode, CriticalContext = true };
            state.ArgumentFactory = p => { var value = ServiceConsolidationChecks.Sample(p.ParameterType, tag: p.Name ?? "input"); Patch(value); return value; };
            state.ResultTransform = (contract, method, args, value) =>
            {
                if (contract.Name == "IBedBesQueryService" && method.Name == "GetLatestByCustomerNumberTanker" && value is not null)
                    foreach (var pair in new Dictionary<string, long> { ["SumItems"] = mode == "negative-delivery" ? 50 : 500, ["Water"] = 200, ["Budget"] = 50, ["Tax"] = 25 }) Set(value, pair.Key, pair.Value);
                if (contract.Name == "IMeterReadingDetailQueryService" && method.Name == "GetUpdated" && value is IList rows)
                    for (int i = 0; i < rows.Count; i++) rows[i]!.GetType().GetProperty("BillId")!.SetValue(rows[i], i == 0 ? "111" : "222");
                return value;
            };
            return state;
        }
        async Task Compare(Type oldType, MethodInfo oldMethod, Type currentType, MethodInfo method, bool application)
        {
            string priorDirectory = Directory.GetCurrentDirectory();
            string? artifactRoot = oldMethod.Name == "GetStiExcluded" ? Path.Combine(Path.GetTempPath(), "Aban360-SalesReports-" + Guid.NewGuid().ToString("N")) : null;
            string? jsonDirectory = artifactRoot is null ? null : Path.Combine(artifactRoot, "AppData", "Jsons");
            if (jsonDirectory is not null) { Directory.CreateDirectory(jsonDirectory); Directory.SetCurrentDirectory(artifactRoot!); }
            try
            {
            using var cancellation = new CancellationTokenSource();
            void Equal(ServiceConsolidationChecks.Outcome a, ServiceConsolidationChecks.Outcome b, string mode)
            {
                Check(Canonical(a) == Canonical(b), $"Behavior mismatch: {oldType.Name}.{oldMethod.Name}/{mode}\nBEFORE {Canonical(a)}\nAFTER {Canonical(b)}"); scenarios++;
            }
            async Task<ServiceConsolidationChecks.Outcome> Mode(string mode, int fail = 0, int cancel = 0)
            {
                var oldState = State(mode, cancellation.Token); var state = State(mode, cancellation.Token);
                oldState.FailAt = state.FailAt = fail; oldState.CancelAt = state.CancelAt = cancel;
                var writes = new List<(MethodInfo Method, object?[] Args)>();
                state.CallObserver = (contract, call, args) => { if (application && contract.Namespace?.Contains(".Persistence.Features.Sale.Commands") == true) writes.Add((call, args)); };
                var before = await ServiceConsolidationChecks.Execute(oldType, oldMethod, oldState);
                var beforeArtifacts = CollectArtifacts();
                var current = await ServiceConsolidationChecks.Execute(currentType, method, state);
                var afterArtifacts = CollectArtifacts();
                Equal(before, current, mode + "/" + fail + "/" + cancel);
                if (jsonDirectory is not null) Check(beforeArtifacts.SequenceEqual(afterArtifacts), "Generated report JSON changed");
                if (jsonDirectory is not null && mode == "success" && fail == 0 && cancel == 0) Check(before.Error is null && beforeArtifacts.Length == 1, "STI fixture did not produce its report JSON");
                if (application && mode == "success" && fail == 0 && cancel == 0)
                {
                    foreach (var write in writes)
                    foreach (var argument in write.Args.Where(a => a is not null))
                    {
                        foreach (var amount in Amounts)
                            if (argument!.GetType().GetProperty(amount.Key) is { } property) Check(Convert.ToInt64(property.GetValue(argument)) == amount.Value, "Persisted amount changed: " + amount.Key);
                        foreach (string name in new[] { "RegisterByUserId", "RemoveByUserId" })
                            if (argument!.GetType().GetProperty(name) is { } property)
                            {
                                var actor = property.GetValue(argument);
                                if (name == "RemoveByUserId" && argument.GetType().Name != "DeleteDto") Check(actor is null, "Creation payload unexpectedly marks a removal");
                                else Check(actor is Guid id && id == Guid.Parse("22222222-2222-2222-2222-222222222222"), "Actor attribution changed");
                            }
                    }
                    Check(!state.Calls.Any(c => c.Contains("SaveChanges") || c.Contains("Commit")), "Caller-controlled commit boundary changed");
                }
                return before;
            }
            var success = await Mode("success");
            if (application) Check(success.Error is null, "Seeded success did not execute: " + oldType.Name + "." + oldMethod.Name + " / " + success.Error);
            await Mode("empty");
            if (success.Calls.Any(c => c.Contains("FluentValidation.IValidator")))
            {
                var rejected = await Mode("invalid-validation");
                Check(rejected.Error is not null && rejected.Calls.Length == 1, "Rejected validation reached a query/write");
            }
            if (application && oldType.Name == "TankerWaterDetailByCustomerNumberGetHandler")
            {
                Check(LastDataContains(success.Result, "225"), "Delivery amount calculation changed");
                var negative = await Mode("negative-delivery"); Check(LastDataContains(negative.Result, "0"), "Negative delivery amount was not clamped");
            }
            for (int step = 1; step <= success.Calls.Length; step++)
            {
                await Mode("success", fail: step); await Mode("success", cancel: step);
                if (!success.AsyncSteps.Contains(step)) continue;
                var a = State("success", cancellation.Token); var b = State("success", cancellation.Token); a.GateAt = b.GateAt = step;
                var old = ServiceConsolidationChecks.Execute(oldType, oldMethod, a); var current = ServiceConsolidationChecks.Execute(currentType, method, b);
                Check(!old.IsCompleted && !current.IsCompleted && CanonicalText(JsonSerializer.Serialize(a.Calls)) == CanonicalText(JsonSerializer.Serialize(b.Calls)), "Dependency was no longer awaited");
                a.Gate.SetResult(true); b.Gate.SetResult(true); Equal(await old, await current, "pending/" + step);
                if (jsonDirectory is not null)
                {
                    var files = CollectArtifacts(); Check(files.Length == 2 && files[0] == files[1], "Pending report JSON generation changed");
                }
            }
            string[] CollectArtifacts()
            {
                if (jsonDirectory is null) return Array.Empty<string>();
                var files = Directory.GetFiles(jsonDirectory, "*.json");
                var contents = files.Select(File.ReadAllText).Select(CanonicalText).Order().ToArray();
                foreach (var file in files) File.Delete(file);
                return contents;
            }
            }
            finally
            {
                if (artifactRoot is not null)
                {
                    Directory.SetCurrentDirectory(priorDirectory);
                    foreach (var file in Directory.GetFiles(jsonDirectory!)) File.Delete(file);
                    Directory.Delete(jsonDirectory!); Directory.Delete(Path.Combine(artifactRoot, "AppData")); Directory.Delete(artifactRoot);
                }
            }
        }
    }
    static bool LastDataContains(string result, string value)
    {
        using var doc = JsonDocument.Parse(result);
        return doc.RootElement.GetProperty("ReportData").EnumerateArray().Last().EnumerateObject().Any(p => p.Value.ValueKind == JsonValueKind.String && p.Value.GetString() == value);
    }
    static string Attributes(IEnumerable<CustomAttributeData> attributes) => string.Join("|", attributes.Where(a => a.AttributeType.Namespace != "System.Runtime.CompilerServices").Select(a => a.ToString()).Order());
    static string Canonical(ServiceConsolidationChecks.Outcome outcome) => CanonicalText(JsonSerializer.Serialize(outcome));
    static string CanonicalText(string text)
    {
        // Random report filenames differ between executions; keep fixed caller IDs intact.
        var ids = new Dictionary<string, string>();
        text = Regex.Replace(text, @"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-4[a-fA-F0-9]{3}-[89abAB][a-fA-F0-9]{3}-[a-fA-F0-9]{12}", m => { if (!ids.ContainsKey(m.Value)) ids[m.Value] = "generated-id-" + ids.Count; return ids[m.Value]; });
        return Regex.Replace(text, @"\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?(?:Z|[+-]\d\d:\d\d)?", m => DateTime.TryParse(m.Value, out var date) && Math.Abs((date.ToUniversalTime() - DateTime.UtcNow).TotalSeconds) < 120 ? "current-time" : m.Value);
    }
    static void Set(object value, string name, long amount)
    {
        if (value.GetType().GetProperty(name) is { } p) p.SetValue(value, Convert.ChangeType(amount, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType));
    }
    static void Patch(object? value)
    {
        if (value is null || value is string || value.GetType().IsValueType || value is Aban360.Common.ApplicationUser.IAppUser) return;
        foreach (var p in value.GetType().GetProperties().Where(p => p.SetMethod?.IsPublic == true && p.GetIndexParameters().Length == 0))
        {
            if (Amounts.TryGetValue(p.Name, out long amount)) Set(value, p.Name, amount);
            if (p.PropertyType == typeof(string) && p.Name.Contains("Date")) p.SetValue(value, p.Name.StartsWith("To") ? "1405/02/01" : "1405/01/01");
        }
    }
}
