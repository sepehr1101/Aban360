using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

static class CompactRuntimeChecks
{
    sealed record Input(string Group, string Module, string OldName, string OldNamespace, string Namespace, string Service, Dictionary<string, string> Names);
    sealed record ConsumerInput(string Path, string Assembly, string Current, string Original);
    public static async Task Run()
    {
        var inputs = JsonSerializer.Deserialize<Input[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "CompactInputs.json")))!;
        var consumers = JsonSerializer.Deserialize<ConsumerInput[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "CompactConsumers.json")))!;
        int checks = 0, scenarios = 0, positive = 0, consumerMethods = 0;
        void Check(bool condition, string message) { if (!condition) throw new Exception(message); checks++; }
        var registrations = new ServiceCollection(); registrations.AddLogging();
        foreach (var module in inputs.Select(i => i.Module).Distinct())
        {
            var assembly = Assembly.Load("Aban360." + module + ".Application");
            var candidates = assembly.GetExportedTypes().SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(m => m.Name == "Add" + module + "ApplicationInjections" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(IServiceCollection)).ToArray();
            candidates.Single().Invoke(null, new object[] { registrations });
        }
        using var mapperProvider = registrations.BuildServiceProvider(); var mapper = mapperProvider.GetRequiredService<IMapper>();
        string temporary = Path.Combine(Path.GetTempPath(), "Aban360-compact-" + Guid.NewGuid().ToString("N"));
        string versionDirectory = Path.Combine(temporary, "1.0.0"), apkFile = Path.Combine(versionDirectory, "assessment.apk");
        Directory.CreateDirectory(versionDirectory); File.WriteAllBytes(apkFile, new byte[] { 1, 2, 3, 4 });
        try
        {
            foreach (var group in inputs.GroupBy(i => i.Namespace + ".I" + i.Service))
            {
                var assembly = Assembly.Load("Aban360." + group.First().Module + ".Application");
                var contract = assembly.GetType(group.Key, true)!;
                var implementation = assembly.GetTypes().Single(t => t.IsClass && !t.IsAbstract && contract.IsAssignableFrom(t));
                var registration = registrations.Where(d => d.ServiceType == contract).ToArray();
                Check(registration.Length == 1 && registration[0].ImplementationType == implementation && registration[0].Lifetime == ServiceLifetime.Scoped, "Incorrect production registration: " + contract);
                var local = new ServiceCollection(); foreach (var descriptor in registrations) ((ICollection<ServiceDescriptor>)local).Add(descriptor);
                var scopeProbe = State(group.First().Group, "success", CancellationToken.None);
                foreach (var dependency in implementation.GetConstructors().Single().GetParameters()) local.AddScoped(dependency.ParameterType, _ => scopeProbe.Proxy(dependency.ParameterType));
                using var provider = local.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
                using var first = provider.CreateScope(); using var second = provider.CreateScope();
                var instance = first.ServiceProvider.GetRequiredService(contract);
                Check(ReferenceEquals(instance, first.ServiceProvider.GetRequiredService(contract)) && !ReferenceEquals(instance, second.ServiceProvider.GetRequiredService(contract)), "Scoped lifetime changed: " + contract);
                int groupPositive = 0;
                foreach (var input in group)
                {
                    var before = typeof(CompactRuntimeChecks).Assembly.GetType("BeforeCompact." + input.OldNamespace + "." + input.OldName, true)!;
                    foreach (var oldMethod in before.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        var method = implementation.GetMethod(input.Names[oldMethod.Name], oldMethod.GetParameters().Select(p => p.ParameterType).ToArray())!;
                        Check(method is not null && method.ReturnType == oldMethod.ReturnType, "Signature changed: " + input.OldName + "." + oldMethod.Name);
                        var success = await Compare(before, oldMethod, implementation, method!, input.Group, true);
                        if (success.Error is null) { groupPositive++; positive++; }
                    }
                }
                Check(groupPositive > 0, "No successful fixture for " + group.First().Group);
                Console.WriteLine($"PASS compact group: {group.First().Group}; {groupPositive} successful operation fixtures.");
            }
            var invoiceInput = inputs.Single(i => i.OldName == "InvoiceInserterHandler");
            var invoiceType = Assembly.Load("Aban360.CalculationPool.Application").GetType(invoiceInput.Namespace + "." + invoiceInput.Service, true)!;
            var invoiceProbe = State("Invoice", "success", CancellationToken.None);
            var writes = new List<(Type Contract, object? Value)>();
            invoiceProbe.CallObserver = (contract, method, args) => { if (method.Name == "Add") writes.Add((contract, args.FirstOrDefault())); };
            var invoiceResult = await ServiceConsolidationChecks.Execute(invoiceType, invoiceType.GetMethod(invoiceInput.Names["Handle"])!, invoiceProbe);
            Check(invoiceResult.Error is null && writes.Count == 4, "Invoice staging did not complete all four existing writes");
            var invoice = (Aban360.CalculationPool.Domain.Features.Bill.Entities.Invoice)writes[0].Value!;
            var items = ((IEnumerable<Aban360.CalculationPool.Domain.Features.Bill.Entities.InvoiceLineItem>)writes[1].Value!).ToArray();
            var installment = (Aban360.CalculationPool.Domain.Features.Bill.Entities.InvoiceInstallment)writes[2].Value!;
            var meterHistory = (Aban360.CalculationPool.Domain.Features.Bill.Entities.WaterMeterChangeNumberHistory)writes[3].Value!;
            Check(invoice.Amount == 376 && items.Select(i => i.Amount).SequenceEqual(new long[] { 125, 251 }), "Existing invoice amount conversion/grouping changed");
            Check(items.Length == 2 && items.All(i => ReferenceEquals(i.Invoice, invoice)) && ReferenceEquals(installment.Invoice, invoice) && ReferenceEquals(meterHistory.Invoice, invoice), "Invoice graph links were broken");
            Check(installment.Amount == 376 && meterHistory.Consumption == 25 && meterHistory.ConstumptionAverage == 12.5f, "Installment or meter-history values changed");
            Check(!invoiceProbe.Calls.Any(c => c.Contains("SaveChanges") || c.Contains("Commit")), "Invoice service moved the existing caller's commit boundary");
            foreach (var input in consumers)
            {
                var before = typeof(CompactRuntimeChecks).Assembly.GetType(input.Original, true)!;
                var after = Assembly.Load(input.Assembly).GetType(input.Current, true)!;
                Check(Attributes(before.GetCustomAttributesData()) == Attributes(after.GetCustomAttributesData()), "Consumer class metadata changed: " + input.Current);
                foreach (var method in before.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    var current = after.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray())!;
                    Check(current is not null && method.ReturnType == current.ReturnType && Attributes(method.GetCustomAttributesData()) == Attributes(current.GetCustomAttributesData()), "Consumer action metadata/signature changed: " + input.Current + "." + method.Name);
                    Check(method.GetParameters().Zip(current!.GetParameters()).All(p => p.First.Name == p.Second.Name && p.First.IsOptional == p.Second.IsOptional && Equals(p.First.DefaultValue, p.Second.DefaultValue) && Attributes(p.First.GetCustomAttributesData()) == Attributes(p.Second.GetCustomAttributesData())), "Consumer parameter binding changed: " + input.Current);
                    await Compare(before, method, after, current!, "consumer", false); consumerMethods++;
                }
            }
            Console.WriteLine($"Compact runtime: {checks} checks passed; {scenarios} before/after scenarios; {positive} successful application fixtures; {consumerMethods} consumer methods. All 14 scoped services resolved. No live database or external calls.");
        }
        finally
        {
            File.Delete(apkFile); Directory.Delete(versionDirectory); Directory.Delete(temporary);
        }

        ServiceProbe State(string group, string mode, CancellationToken token)
        {
            var state = new ServiceProbe(mapper, token) { Mode = mode, CriticalContext = true };
            state.ArgumentFactory = p => Argument(p, group, mode);
            state.ResultTransform = (contract, method, args, value) => Result(contract, method, value, mode);
            state.InstanceSetup = instance =>
            {
                if (group != "AssessmentApk") return;
                foreach (var field in instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Where(f => f.FieldType == typeof(string) && f.Name.Contains("baseFolderPath", StringComparison.OrdinalIgnoreCase))) field.SetValue(instance, temporary);
            };
            state.ResultSnapshotTransform = value =>
            {
                if (value is ValueTuple<FileStream, string> file)
                {
                    using (file.Item1) { using var bytes = new MemoryStream(); file.Item1.CopyTo(bytes); return new { Name = file.Item2, Bytes = Convert.ToBase64String(bytes.ToArray()) }; }
                }
                return value;
            };
            return state;
        }
        async Task<ServiceConsolidationChecks.Outcome> Compare(Type oldType, MethodInfo oldMethod, Type newType, MethodInfo method, string group, bool application)
        {
            using var cancellation = new CancellationTokenSource();
            void Equal(ServiceConsolidationChecks.Outcome a, ServiceConsolidationChecks.Outcome b, string mode)
            {
                Check(a.Result == b.Result && a.Error == b.Error && a.Calls.SequenceEqual(b.Calls) && a.AsyncSteps.SequenceEqual(b.AsyncSteps) && a.Mutations == b.Mutations,
                    $"Compact behavior mismatch: {group}.{oldMethod.Name} / {mode}\nOLD {JsonSerializer.Serialize(a)}\nNEW {JsonSerializer.Serialize(b)}"); scenarios++;
            }
            async Task<ServiceConsolidationChecks.Outcome> Mode(string mode, int fail = 0, int cancel = 0)
            {
                var a = State(group, mode, cancellation.Token); var b = State(group, mode, cancellation.Token);
                a.FailAt = b.FailAt = fail; a.CancelAt = b.CancelAt = cancel;
                var old = await ServiceConsolidationChecks.Execute(oldType, oldMethod, a);
                var current = await ServiceConsolidationChecks.Execute(newType, method, b);
                Equal(old, current, mode + "/" + fail + "/" + cancel); return old;
            }
            var success = await Mode("success");
            if (application) Check(success.Error is null, "Successful fixture did not reach the business operation: " + group + "." + oldMethod.Name + " " + success.Error);
            await Mode("empty");
            if (success.Calls.Any(c => c.Contains("FluentValidation.IValidator")))
            {
                var rejected = await Mode("invalid-validation");
                Check(rejected.Error is not null && rejected.Calls.Length == 1, "Rejected validation reached persistence: " + group);
            }
            for (int step = 1; step <= success.Calls.Length; step++)
            {
                await Mode("success", fail: step); await Mode("success", cancel: step);
                if (!success.AsyncSteps.Contains(step)) continue;
                var a = State(group, "success", cancellation.Token); var b = State(group, "success", cancellation.Token); a.GateAt = b.GateAt = step;
                var old = ServiceConsolidationChecks.Execute(oldType, oldMethod, a); var current = ServiceConsolidationChecks.Execute(newType, method, b);
                Check(old.IsCompleted == current.IsCompleted && a.Calls.SequenceEqual(b.Calls), "Await boundary changed: " + group + "." + oldMethod.Name + "/" + step);
                a.Gate.SetResult(true); b.Gate.SetResult(true); Equal(await old, await current, "pending/" + step);
            }
            return success;
        }
    }
    static string Attributes(IEnumerable<CustomAttributeData> attributes) => string.Join("|", attributes.Where(a => a.AttributeType.Namespace != "System.Runtime.CompilerServices").Select(a => a.ToString()).Order());
    static object? Argument(ParameterInfo parameter, string group, string mode)
    {
        if (parameter.ParameterType == typeof(string) && parameter.Name == "userVersion") return "1.0.0";
        if (parameter.ParameterType.IsEnum && Enum.GetNames(parameter.ParameterType).Contains("Summary")) return Enum.Parse(parameter.ParameterType, "Summary");
        if (parameter.ParameterType == typeof(IntervalCalculationResultWrapper)) return new IntervalCalculationResultWrapper
        {
            Amount = 376.75, Consumption = 25, ConsumptionAverage = 12.5, Duration = 30,
            IntervalCalculationResults = new List<IntervalCalculationResult3> { new() { FromDate = "1405/01/01", ToDate = "1405/02/01", CalculationInfo = new List<IntervalCalculationResult2>
            {
                new() { OfferingTitle = "water", Amount = 125.75, Consumption = 10, Duration = 30, Formula = "water-formula", LineItemTypeTitle = "water-item" },
                new() { OfferingTitle = "sewage", Amount = 251, Consumption = 15, Duration = 30, Formula = "sewage-formula", LineItemTypeTitle = "sewage-item" }
            } } }
        };
        var value = ServiceConsolidationChecks.Sample(parameter.ParameterType, 0, parameter.Name ?? "input");
        Patch(value, 0); return value;
    }
    static void Patch(object? value, int depth)
    {
        if (value is null || depth > 3 || value is string || value.GetType().IsValueType) return;
        if (value is IEnumerable sequence) { foreach (var item in sequence) Patch(item, depth + 1); return; }
        foreach (var property in value.GetType().GetProperties().Where(p => p.SetMethod?.IsPublic == true && p.GetIndexParameters().Length == 0))
        {
            if (property.PropertyType == typeof(string))
            {
                if (property.Name.Contains("Date")) property.SetValue(value, property.Name.StartsWith("To") || property.Name.StartsWith("Current") ? "1405/01/02" : "1405/01/01");
                if (property.Name is "FromTime" or "ToTime") property.SetValue(value, property.Name == "FromTime" ? "00:00" : "23:59");
                if (property.Name == "StatusCodeClass") property.SetValue(value, "4XX");
                if (property.Name == "Input" && value.GetType().Name == "SearchInput") property.SetValue(value, "17");
            }
            else if (property.Name == "Page" && property.PropertyType == typeof(int)) property.SetValue(value, 1);
            else if (property.Name == "PageSize" && property.PropertyType == typeof(int)) property.SetValue(value, 20);
            else Patch(property.GetValue(value), depth + 1);
        }
    }
    static object? Result(Type contract, MethodInfo method, object? value, string mode)
    {
        if (method.Name == "GetIncludeAll" && contract.Name == "IZoneQueryService")
        {
            if (mode == "empty") return new List<Zone>();
            var direction = new CordinalDirection { Id = 1, Title = "direction" };
            var province = new Province { Id = 2, Title = "province", CordinalDirectionId = 1, CordinalDirection = direction };
            var headquarters = new Headquarters { Id = 3, Title = "headquarters", ProvinceId = 2, Province = province };
            var region = new Region { Id = 4, Title = "region", HeadquartersId = 3, Headquarters = headquarters };
            return new List<Zone> { new() { Id = 17, Title = "zone", RegionId = 4, Region = region } };
        }
        if (mode != "empty" && value is IList list)
        {
            if (contract.Name == "ICommonMemberQueryService" && method.Name == "GetFromClient") while (list.Count > 1) list.RemoveAt(list.Count - 1);
            if (contract.Name == "IOfferingQueryService" && method.Name == "Get")
                for (int i = 0; i < list.Count; i++) list[i]!.GetType().GetProperty("Title")?.SetValue(list[i], i == 0 ? "water" : "sewage");
        }
        if (contract.Name == "IMeterFlowQueryService" && method.Name == "GetLatestFlowInfo2") value?.GetType().GetProperty("RemovedDateTime")?.SetValue(value, null);
        return value;
    }
}
