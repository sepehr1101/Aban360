using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Api.Hubs.Implementations;
using Aban360.Tests.ServiceBaselines;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

static class ServiceConsolidationChecks
{
    public static async Task<int> Run()
    {
        int checks = 0, scenarios = 0, consumerMethods = 0, knownLegacyMappingFaults = 0;
        void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); checks++; }
        var operations = ApplicationServiceBaseline.Operations;
        Check(operations.Length == 494 && operations.Select(o => o.Feature).Distinct().Count() == 104, "Incomplete service baseline");
        var assemblies = operations.Select(o => o.Assembly).Distinct().Select(Assembly.Load).ToArray();
        var registrations = new ServiceCollection(); registrations.AddLogging();
        foreach (var assembly in assemblies)
        {
            var expectedInjection = "Add" + assembly.GetName().Name!.Split('.')[1] + "ApplicationInjections";
            var candidates = assembly.GetExportedTypes().SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(m => m.Name == expectedInjection && m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType == typeof(IServiceCollection)).ToArray();
            // WorkflowPool contains an older duplicate in a misspelled namespace; use the API's registration.
            var inject = candidates.Length == 1 ? candidates[0] : candidates.Single(m =>
                m.DeclaringType?.Namespace == assembly.GetName().Name + ".Extensions");
            inject.Invoke(null, new object[] { registrations });
        }
        using var mappingProvider = registrations.BuildServiceProvider();
        var mapper = mappingProvider.GetRequiredService<IMapper>();
        checks += await AccessTreeZoneChecks.Run(mapper);
        checks += await MainTagTransactionChecks.Run();
        checks += await MappingCorrectionChecks.Run(mapper);
        foreach (var group in operations.GroupBy(o => o.Service))
        {
            var assembly = Assembly.Load(group.First().Assembly);
            var contract = assembly.GetType(group.Key, true)!;
            var implementation = assembly.GetTypes().Single(t => t.IsClass && !t.IsAbstract && contract.IsAssignableFrom(t));
            var registered = registrations.Where(d => d.ServiceType == contract).ToArray();
            Check(registered.Length == 1 && registered[0].ImplementationType == implementation && registered[0].Lifetime == ServiceLifetime.Scoped,
                "Production DI registration changed: " + contract.Name);
            var local = new ServiceCollection();
            foreach (var registration in registrations) ((ICollection<ServiceDescriptor>)local).Add(registration);
            local.AddSingleton(mapper);
            var diState = new ServiceProbe(mapper, CancellationToken.None);
            foreach (var dependency in implementation.GetConstructors().Single().GetParameters().Where(p => p.ParameterType != typeof(IMapper)))
            {
                var type = dependency.ParameterType;
                local.AddScoped(type, _ => diState.Proxy(type));
            }
            using var provider = local.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
            using var first = provider.CreateScope(); using var second = provider.CreateScope();
            var resolved = first.ServiceProvider.GetRequiredService(contract);
            Check(ReferenceEquals(resolved, first.ServiceProvider.GetRequiredService(contract)) &&
                !ReferenceEquals(resolved, second.ServiceProvider.GetRequiredService(contract)), "Scoped lifetime changed: " + contract.Name);

            foreach (var operation in group)
            {
                if (operation.Feature == "MainTagGroup" && operation.Method is "CreateAsync" or "UpdateAsync" or "DeleteAsync") continue;
                var original = typeof(ServiceConsolidationChecks).Assembly.GetType(operation.Original, true)!;
                var before = original.GetMethod(operation.OldMethod)!;
                var after = implementation.GetMethod(operation.Method)!;
                Check(before.ReturnType == after.ReturnType && before.GetParameters().Select(p => p.ParameterType).SequenceEqual(after.GetParameters().Select(p => p.ParameterType)),
                    "Application signature changed: " + operation.Feature + "." + operation.Method);
                await Compare(original, before, implementation, after, true, operation.Feature);
            }
        }

        var consumers = new[] { "ServiceConsumers15.json", "ServiceConsumers30.json", "ServiceConsumers20.json", "ServiceConsumers19.json", "ServiceConsumersWave20.json" }.SelectMany(file =>
            JsonSerializer.Deserialize<Consumer[]>(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, file)))!).ToArray();
        Check(consumers.Length == 112, "Missing API, Bridge, gateway or hub consumers");
        foreach (var consumer in consumers)
        {
            var original = typeof(ServiceConsolidationChecks).Assembly.GetType(consumer.Original, true)!;
            var current = Assembly.Load(consumer.Assembly).GetType(consumer.Current, true)!;
            Check(Attributes(original.GetCustomAttributesData()).SequenceEqual(Attributes(current.GetCustomAttributesData())),
                "Consumer route or class metadata changed: " + current.Name);
            var methods = original.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => typeof(Task).IsAssignableFrom(m.ReturnType)).ToArray();
            var currentMethods = current.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => typeof(Task).IsAssignableFrom(m.ReturnType)).ToArray();
            Check(methods.Length == currentMethods.Length, "Consumer operation count changed: " + current.Name);
            foreach (var before in methods)
            {
                var after = current.GetMethod(before.Name, before.GetParameters().Select(p => p.ParameterType).ToArray())!;
                Check(after is not null && after.ReturnType == before.ReturnType, "Consumer signature changed: " + current.Name);
                Check(Attributes(before.GetCustomAttributesData()).SequenceEqual(Attributes(after!.GetCustomAttributesData())),
                    "Consumer HTTP verbs, route, response metadata or filters changed: " + current.Name + "." + before.Name);
                Check(before.GetParameters().Zip(after.GetParameters()).All(pair => pair.First.Name == pair.Second.Name
                    && pair.First.IsOptional == pair.Second.IsOptional && Equals(pair.First.DefaultValue, pair.Second.DefaultValue)
                    && Attributes(pair.First.GetCustomAttributesData()).SequenceEqual(Attributes(pair.Second.GetCustomAttributesData()))),
                    "Consumer binding or defaults changed: " + current.Name + "." + before.Name);
                // Actual file opening is exercised by ReportGeneration.SmokeTests.
                if (!(current.Name == "ServerReportsController" && before.Name == "Download"))
                    await Compare(original, before, current, after!, false, current.Name);
                consumerMethods++;
            }
        }
        await CheckRemovalPipelines();
        Check(consumerMethods >= 490, "Consumer behavioral coverage shrank");
        Check(knownLegacyMappingFaults == 4, "Known legacy mapping defects need explicit review if their behavior changes");
        Console.WriteLine("KNOWN LEGACY DEFECTS: 4 tag lookup mapping errors remain in both frozen handlers and current services; these are not successful scenarios.");
        Console.WriteLine($"PASS: {checks} service consolidation checks; 104 real services, 491 compared methods + 3 SQL transaction methods; {consumers.Length} consumers/{consumerMethods} operations; {scenarios} before/after scenarios.");
        return checks;

        async Task CheckRemovalPipelines()
        {
            foreach (var feature in new[] { "RequestFlat", "RequestIndividualTag", "RequestWaterMeterTag" })
            {
                var operation = operations.Single(o => o.Feature == feature && o.Method == "DeleteAsync");
                var contract = Assembly.Load(operation.Assembly).GetType(operation.Service, true)!;
                var implementation = contract.Assembly.GetTypes().Single(t => t.IsClass && contract.IsAssignableFrom(t));
                var controller = typeof(Aban360.Api.Controllers.V1.BaseController).Assembly.GetType("Aban360.Api.Controllers.V1.ClaimPool.Draft." + feature + "Controller", true)!;
                var method = controller.GetMethod("Delete")!;
                var delayed = new ServiceProbe(mapper, CancellationToken.None) { RealService = implementation, GateAt = 2 };
                var pending = Execute(controller, method, delayed);
                Check(!pending.IsCompleted && delayed.Calls.Count == 2 && !delayed.Calls.Any(c => c.Contains("SaveChangesAsync")), feature + " saved before removal completed");
                delayed.Gate.SetResult(true);
                var success = await pending;
                Check(success.Error is null && success.Calls.Length == 3 && success.Calls[^1].Contains("SaveChangesAsync"), feature + " removal/save order changed");
                var failed = await Execute(controller, method, new ServiceProbe(mapper, CancellationToken.None) { RealService = implementation, FailAt = 2 });
                Check(failed.Error?.Contains("intentional-service-failure-2") == true && failed.Calls.Length == 2, feature + " swallowed removal failure or saved afterward");
                using var cancellation = new CancellationTokenSource(); cancellation.Cancel();
                var cancelled = await Execute(controller, method, new ServiceProbe(mapper, cancellation.Token) { RealService = implementation, CancelAt = 2 });
                Check(cancelled.Error?.Contains("CanceledException") == true && cancelled.Calls.Length == 2, feature + " swallowed removal cancellation or saved afterward");
                Console.WriteLine($"PASS: {feature} controller + real service preserve removal/save ordering, errors and cancellation.");
                }
        }

        async Task Compare(Type oldType, MethodInfo oldMethod, Type newType, MethodInfo newMethod, bool realMapping, string label)
        {
            bool correctedAwait = realMapping && (label is "RequestFlat" or "RequestIndividualTag" or "RequestWaterMeterTag") && newMethod.Name == "DeleteAsync";
            bool correctedSubscription = realMapping && label == "SubscriptionType" && newMethod.Name == "UpdateAsync";
            bool knownMappingFault = realMapping && (label == "WaterMeterTag" || label == "IndividualTag")
                && (newMethod.Name == "GetSingleAsync" || newMethod.Name == "GetBySearchAsync");
            using var source = new CancellationTokenSource(); var token = source.Token;
            var oldSuccess = await Execute(oldType, oldMethod, new ServiceProbe(realMapping ? mapper : null, token));
            var newSuccess = await Execute(newType, newMethod, new ServiceProbe(realMapping ? mapper : null, token));
            Equal(oldSuccess, newSuccess, "success");
            if (knownMappingFault)
            {
                Check(oldSuccess.Error?.StartsWith("AutoMapper.AutoMapperMappingException:") == true && oldSuccess.Error.Contains(label),
                    "Known legacy mapping failure changed: " + label + "." + newMethod.Name);
                knownLegacyMappingFaults++;
            }
            else Check(correctedSubscription ? oldSuccess.Error?.StartsWith("AutoMapper.AutoMapperMappingException:") == true && newSuccess.Error is null : oldSuccess.Error is null,
                "Baseline success fixture failed: " + label + "." + newMethod.Name + " " + oldSuccess.Error);
            if (realMapping && (label is "DocumentCategory" or "DocumentType"))
            {
                if (newMethod.Name == "CreateAsync")
                    Check(oldSuccess.Calls.Last().Contains("AQIDBA=="), "Created document icon must contain uploaded bytes as base64");
                if (newMethod.Name == "UpdateAsync")
                {
                    using var mutations = JsonDocument.Parse(oldSuccess.Mutations);
                    var entity = mutations.RootElement.EnumerateArray().Single(item => item.TryGetProperty("Icon", out _));
                    Check(entity.GetProperty("Icon").GetString() == "AQIDBA==", "Document icon update must preserve uploaded bytes as base64");
                }
            }
            if (oldSuccess.Calls.Any(c => c.Contains("FluentValidation.IValidator")))
            {
                var rejected = await Mode("invalid-validation");
                Check(rejected.Error?.Contains(label is "Individual" or "Siphon" or "Flat" ? "BaseException" : "CustomValidationException") == true && rejected.Calls.Length == 1
                    && rejected.Error.Contains("first-validation-error") && rejected.Error.Contains("second-validation-error"),
                    "Invalid input reached persistence or validation error changed: " + label + "." + newMethod.Name + " " + JsonSerializer.Serialize(rejected));
            }
            await Mode("empty");
            if (realMapping && label == "ReadingBound" && newMethod.Name == "ValidateAsync")
            {
                foreach (var mode in new[] { "bound-empty", "bound-blank", "bound-length", "bound-start", "bound-gap", "bound-nonnumeric", "bound-reversed" })
                {
                    var result = await Mode(mode);
                    Check(mode == "bound-reversed" ? result.Error is null
                        : result.Error?.Contains(mode == "bound-nonnumeric" ? "FormatException:" : "ReadingException:") == true,
                        "Reading bound validation branch changed: " + mode);
                    var expectedMessage = mode switch
                    {
                        "bound-empty" or "bound-blank" => Aban360.Common.Literals.ExceptionLiterals.ReadingBlockNotFound,
                        "bound-length" => Aban360.Common.Literals.ExceptionLiterals.InvalidBlocksReadingNumberChars,
                        "bound-start" => Aban360.Common.Literals.ExceptionLiterals.InvalidBoundAndBlockStartReadingNumber,
                        "bound-gap" => string.Format(Aban360.Common.Literals.ExceptionLiterals.InvalidStartReadingNumber, "sample-Title"),
                        _ => null
                    };
                    if (expectedMessage is not null) Check(result.Error?.EndsWith(expectedMessage) == true, "Wrong reading bound rejection: " + mode);
                    using var tracked = JsonDocument.Parse(result.Mutations);
                    Check(tracked.RootElement[0].GetProperty("IsValid").GetBoolean() == (mode == "bound-reversed"),
                        "Reading bound validation flag changed: " + mode);
                }
            }
            if (realMapping && label == "RequestUser" && newMethod.Name == "GetAllAsync")
                Check((await Mode("null-query-result")).Error?.Contains("InvalidDataException") == true, "RequestUser null collection guard changed");
            if (oldMethod.GetParameters().Any(p => p.ParameterType == typeof(bool))) await Mode("false-flag");
            if (label == "TicketStatus" && newMethod.Name == "GetSingleAsync")
            {
                var missing = await Mode("missing");
                Check(missing.Error?.Contains("InvalidIdException") == true, "TicketStatus no longer rejects a missing identifier");
            }
            else if (newMethod.Name == "GetSingleAsync") await Mode("missing");
            for (int step = 1; step <= oldSuccess.Calls.Length; step++)
            {
                var a = new ServiceProbe(realMapping ? mapper : null, token) { FailAt = step };
                var b = new ServiceProbe(realMapping ? mapper : null, token) { FailAt = step };
                var before = await Execute(oldType, oldMethod, a); var after = await Execute(newType, newMethod, b);
                if (correctedAwait && step == 2)
                {
                    // Intentional review fix: the frozen handler used to ignore Remove's failed task.
                    Check(before.Error is null && after.Error?.Contains("intentional-service-failure-2") == true
                        && before.Calls.SequenceEqual(after.Calls), label + " must propagate removal failure");
                    scenarios++;
                }
                else Equal(before, after, "failure " + step);
            }
            source.Cancel();
            foreach (var step in oldSuccess.AsyncSteps.Where(step => oldSuccess.Calls[step - 1].StartsWith("IFormFile.CopyToAsync")))
            {
                var before = await Execute(oldType, oldMethod, new ServiceProbe(realMapping ? mapper : null, token) { CancelAt = step });
                var after = await Execute(newType, newMethod, new ServiceProbe(realMapping ? mapper : null, token) { CancelAt = step });
                Equal(before, after, "file-copy cancellation");
                Check(after.Error?.Contains("CanceledException") == true && after.Calls.Length == step,
                    "Cancelled file copy continued to persistence: " + label);
            }
            Equal(await Execute(oldType, oldMethod, new ServiceProbe(realMapping ? mapper : null, token) { CancelAt = 1 }),
                await Execute(newType, newMethod, new ServiceProbe(realMapping ? mapper : null, token) { CancelAt = 1 }), "cancellation");
            foreach (var step in oldSuccess.AsyncSteps)
            {
                Outcome? previous = null;
                foreach (var target in new[] { (oldType, oldMethod), (newType, newMethod) })
                {
                    var state = new ServiceProbe(realMapping ? mapper : null, CancellationToken.None) { GateAt = step };
                    var pending = Execute(target.Item1, target.Item2, state);
                    if (correctedAwait && step == 2 && target.Item1 == oldType)
                        Check(pending.IsCompleted && state.Calls.Count == step, "Frozen " + label + " missing-await baseline unexpectedly changed");
                    else Check(!pending.IsCompleted && state.Calls.Count == step, "Operation did not await dependency: " + label + "/" + step);
                    state.Gate.SetResult(true);
                    var outcome = await pending;
                    Check(knownMappingFault || correctedSubscription && target.Item1 == oldType ? outcome.Error?.StartsWith("AutoMapper.AutoMapperMappingException:") == true : outcome.Error is null,
                        "Delayed operation changed: " + label);
                    if (previous is not null) Equal(previous, outcome, "await " + step);
                    previous = outcome;
                }
            }
            async Task<Outcome> Mode(string mode)
            {
                var a = await Execute(oldType, oldMethod, new ServiceProbe(realMapping ? mapper : null, token) { Mode = mode });
                var b = await Execute(newType, newMethod, new ServiceProbe(realMapping ? mapper : null, token) { Mode = mode });
                Equal(a, b, mode); return b;
            }
            void Equal(Outcome before, Outcome after, string mode)
            {
                // Preserve the faulty frozen handler; explicitly verify the authorized correction.
                if (correctedSubscription && before.Error?.StartsWith("AutoMapper.AutoMapperMappingException:") == true)
                {
                    Check(after.Error is null && before.Result == after.Result && before.Calls.SequenceEqual(after.Calls),
                        "SubscriptionType correction must retain dependency calls and remove the reverse-map failure: " + mode);
                    scenarios++; return;
                }
                Check(before == after || JsonSerializer.Serialize(before) == JsonSerializer.Serialize(after),
                    $"Behavior changed: {label}.{newMethod.Name} ({mode})\nBEFORE {JsonSerializer.Serialize(before)}\nAFTER {JsonSerializer.Serialize(after)}");
                scenarios++;
            }
        }
    }

    static async Task<Outcome> Execute(Type type, MethodInfo method, ServiceProbe state)
    {
        object? result = null; string? error = null;
        try
        {
            var constructor = type.GetConstructors().Single();
            state.SeedConnection = method.Name == "OnDisconnectedAsync";
            var instance = constructor.Invoke(constructor.GetParameters().Select(p => state.Proxy(p.ParameterType)).ToArray());
            if (instance is ControllerBase controller)
                controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                { new Claim(ClaimTypes.UserData, "22222222-2222-2222-2222-222222222222"), new Claim(ClaimTypes.Name, "service-reviewer") }, "regression")) } };
            if (instance is Hub hub) hub.Context = new ServiceHubCaller(state.Token);
            var arguments = method.GetParameters().Select(p => p.ParameterType == typeof(CancellationToken) ? state.Token :
                p.ParameterType == typeof(bool) && state.Mode == "false-flag" ? false : Sample(p.ParameterType, 0, p.Name ?? "input")).ToArray();
            foreach (var argument in arguments.Where(a => a is not null))
            {
                var icon = argument!.GetType().GetProperty("Icon");
                if (icon?.PropertyType == typeof(IFormFile)) icon.SetValue(argument, new ServiceProbeFile(state));
            }
            var task = (Task)method.Invoke(instance, arguments)!;
            await task;
            if (method.ReturnType.IsGenericType) result = task.GetType().GetProperty("Result")!.GetValue(task);
        }
        catch (Exception ex)
        {
            while (ex is TargetInvocationException { InnerException: not null }) ex = ex.InnerException;
            error = ex.GetType().FullName + ":" + ex.Message;
        }
        return new Outcome(Snapshot(result), error, state.Calls.ToArray(), state.AsyncSteps.ToArray(), Snapshot(state.Tracked));
    }

    public static object? Sample(Type type, int depth = 0, string tag = "sample", bool empty = false)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type == typeof(string)) return "sample-" + tag;
        if (type == typeof(bool)) return true;
        if (type == typeof(IFormFile)) return new FormFile(new MemoryStream(new byte[] { 1, 2, 3, 4 }), 0, 4, "Icon", "icon.png")
            { Headers = new HeaderDictionary(), ContentType = "image/png" };
        if (type == typeof(IAppUser)) return new AppUser(new ClaimsPrincipal(new ClaimsIdentity(new[]
            { new Claim(ClaimTypes.UserData, "22222222-2222-2222-2222-222222222222"), new Claim(ClaimTypes.Name, "service-reviewer") }, "regression")));
        if (type == typeof(Guid)) return Guid.Parse("22222222-2222-2222-2222-222222222222");
        if (type == typeof(DateTime)) return new DateTime(2020, 2, 3, 4, 5, 6, DateTimeKind.Utc);
        if (type == typeof(DateTimeOffset)) return new DateTimeOffset(2020, 2, 3, 4, 5, 6, TimeSpan.Zero);
        if (type == typeof(DateOnly)) return new DateOnly(2020, 2, 3);
        if (type == typeof(TimeOnly)) return new TimeOnly(4, 5);
        if (type.IsEnum) return Enum.GetValues(type).GetValue(0);
        if (type.IsPrimitive || type == typeof(decimal)) return Convert.ChangeType(17, type, CultureInfo.InvariantCulture);
        if (type == typeof(void)) return null;
        if (type.IsArray)
        {
            var element = type.GetElementType()!;
            var array = Array.CreateInstance(element, empty || depth > 2 ? 0 : 2);
            for (int i = 0; i < array.Length; i++) array.SetValue(Sample(element, depth + 1, tag + i), i);
            return array;
        }
        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>) || type.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
        {
            var args = type.GetGenericArguments();
            var dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(args))!;
            if (!empty && depth < 2) dictionary.Add(Sample(args[0], depth + 1, "key")!, Sample(args[1], depth + 1, "value"));
            return dictionary;
        }
        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
        {
            var element = type.GetGenericArguments()[0];
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(element))!;
            if (!empty && depth < 2)
            {
                var id = element.GetProperty("Id");
                var ids = element.Name == "DiscountType" && id?.PropertyType.IsEnum == true ? Enum.GetValues(id.PropertyType) : null;
                int count = ids?.Length ?? 2;
                for (int i = 0; i < count; i++)
                {
                    var item = Sample(element, depth + 1, tag + i);
                    if (ids is not null) id!.SetValue(item, ids.GetValue(i));
                    list.Add(item);
                }
            }
            return list;
        }
        if (type.IsValueType) return Activator.CreateInstance(type);
        if (depth > 2 || type.IsInterface || type.IsAbstract) return null;
        var constructor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();
        if (constructor is null) return null;
        var value = constructor.Invoke(constructor.GetParameters().Select(p => Sample(p.ParameterType, depth + 1, p.Name ?? tag, empty)).ToArray());
        foreach (var property in type.GetProperties().Where(p => p.SetMethod?.IsPublic == true && p.GetIndexParameters().Length == 0))
            property.SetValue(value, Sample(property.PropertyType, depth + 1, property.Name, empty));
        if (type.FullName == "Aban360.LocationPool.Domain.Features.MainHierarchy.Entities.ReadingBound")
        {
            type.GetProperty("FromReadingNumber")!.SetValue(value, "0001");
            type.GetProperty("ToReadingNumber")!.SetValue(value, "0004");
        }
        if (type.FullName == "Aban360.LocationPool.Domain.Features.MainHierarchy.Entities.ReadingBlock")
        {
            type.GetProperty("FromReadingNumber")!.SetValue(value, tag.EndsWith("1") ? "0003" : "0001");
            type.GetProperty("ToReadingNumber")!.SetValue(value, tag.EndsWith("1") ? "0004" : "0002");
        }
        return value;
    }

    public static string Snapshot(object? value) => JsonSerializer.Serialize(Normalize(value));
    static object? Normalize(object? value, int depth = 0)
    {
        if (value is null || depth > 5) return null;
        if (value is CancellationToken token) return token.CanBeCanceled ? "caller-token" : "none";
        if (value is IAppUser user) return "user:" + user.UserId;
        if (value is HubConnectionRegistry registry) return registry.GetConnectionIds().Order().ToArray();
        // Each envelope creates a fresh server timestamp. Compare the rest of Meta normally.
        if (value is ApiMeta meta)
        {
            if (Math.Abs((meta.ServerDateTime - DateTime.Now).TotalSeconds) > 60)
                throw new InvalidOperationException("Response server timestamp is stale");
            return new { meta.NextAction, ServerDateTime = "current-server-time" };
        }
        var type = value.GetType();
        if (value is string || type.IsPrimitive || type.IsEnum || type.IsValueType) return value;
        if (value is IEnumerable sequence) return sequence.Cast<object?>().Select(v => Normalize(v, depth + 1)).ToArray();
        return type.GetProperties().Where(p => p.GetMethod?.IsPublic == true && p.GetIndexParameters().Length == 0)
            .OrderBy(p => p.Name).ToDictionary(p => p.Name, p =>
            {
                var field = p.GetValue(value);
                // These original operations set ValidFrom from the wall clock on both sides.
                if ((type.FullName == "Aban360.WorkflowPool.Domain.Features.Design.Entities.State"
                    || type.FullName == "Aban360.WorkflowPool.Domain.Features.Design.Entities.Workflow"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.People.Entities.IndividualTag"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.People.Entities.IndividualDiscountType"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestIndividualDiscountType"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestIndividualTag"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestWaterMeterTag"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestEstate"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestTracking"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestIndividual"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestSiphon"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.Draft.Entites.RequestWaterMeter"
                    || type.FullName == "Aban360.ClaimPool.Domain.Features.WasteWater.Entities.Siphon") && p.Name == "ValidFrom"
                    && field is DateTime timestamp && Math.Abs((timestamp - DateTime.Now).TotalSeconds) < 60)
                    return "current-record-version-time";
                if (type.FullName == "Aban360.WorkflowPool.Domain.Features.Design.Entities.Workflow" && p.Name == "Name"
                    && field is string generatedName && System.Text.RegularExpressions.Regex.IsMatch(generatedName, "^[a-f0-9]{10}$")
                    && type.GetProperty("ValidFrom")?.GetValue(value) is DateTime created && Math.Abs((created - DateTime.Now).TotalSeconds) < 60)
                    return "current-workflow-name";
                return Normalize(field, depth + 1);
            });
    }

    record Consumer(string Assembly, string Original, string Current);
    record Outcome(string Result, string? Error, string[] Calls, int[] AsyncSteps, string Mutations);

    static string[] Attributes(IEnumerable<CustomAttributeData> attributes) => attributes
        .Where(a => a.AttributeType.Namespace != "System.Runtime.CompilerServices")
        .Select(a => a.AttributeType.FullName + ":" + string.Join("|", a.ConstructorArguments)
            + ":" + string.Join("|", a.NamedArguments.OrderBy(n => n.MemberName))).Order().ToArray();
}

public sealed class ServiceProbe(IMapper? mapper, CancellationToken token)
{
    public readonly CancellationToken Token = token;
    public string Mode = "success";
    public int FailAt, CancelAt, GateAt;
    public bool SeedConnection;
    public Type? RealService;
    public readonly List<string> Calls = [];
    public readonly List<int> AsyncSteps = [];
    public readonly List<object?> Tracked = [];
    public readonly TaskCompletionSource<bool> Gate = new(TaskCreationOptions.RunContinuationsAsynchronously);
    public object Proxy(Type type)
    {
        if (type == typeof(IConfiguration)) return new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            { ["FileManagement:ExcelExpireDay"] = "7" }).Build();
        if (type == typeof(IMapper) && mapper is not null) return mapper;
        if (RealService is not null && type.IsAssignableFrom(RealService))
            return RealService.GetConstructors().Single().Invoke(RealService.GetConstructors().Single().GetParameters().Select(p => Proxy(p.ParameterType)).ToArray());
        if (type == typeof(HubConnectionRegistry))
        {
            var registry = new HubConnectionRegistry();
            if (SeedConnection) registry.Add(ServiceHubCaller.Connection);
            Tracked.Add(registry); return registry;
        }
        var proxy = (ServiceDependencyProxy)DispatchProxy.Create(type, typeof(ServiceDependencyProxy));
        proxy.Contract = type; proxy.State = this; return proxy;
    }
    public object? Invoke(Type contract, MethodInfo method, object?[] args)
    {
        Calls.Add(ApplicationServiceBaseline.CallName(contract, method) + "(" + string.Join("|", args.Select(a => a is CancellationToken c ? c == Token ? "caller" : "wrong-token" : ServiceConsolidationChecks.Snapshot(a))) + ")");
        int step = Calls.Count;
        if (method.ReturnType == typeof(Task)) { AsyncSteps.Add(step); return CompleteVoid(step); }
        if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            AsyncSteps.Add(step);
            var type = method.ReturnType.GetGenericArguments()[0];
            object? result = type == typeof(ValidationResult) ? Mode == "invalid-validation"
                ? new ValidationResult(new[] { new ValidationFailure("First", "first-validation-error"), new ValidationFailure("Second", "second-validation-error") })
                : new ValidationResult()
                : Mode == "null-query-result" && method.Name == "Get" && !type.IsValueType ? null
                : Mode == "missing" && type.IsClass && type.Namespace?.Contains(".Entit") == true ? null
                : ServiceConsolidationChecks.Sample(type, empty: Mode == "empty");
            if (contract.FullName == "Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts.IReadingBlockQueryService"
                && method.Name == "GetByBoundId" && result is IList blocks && Mode.StartsWith("bound-"))
            {
                if (Mode == "bound-empty") blocks.Clear();
                else if (blocks.Count == 2)
                {
                    if (Mode == "bound-reversed") { var first = blocks[0]; blocks[0] = blocks[1]; blocks[1] = first; }
                    else
                    {
                        var block = blocks[Mode is "bound-gap" or "bound-nonnumeric" ? 1 : 0]!;
                        block.GetType().GetProperty(Mode == "bound-length" ? "ToReadingNumber" : "FromReadingNumber")!.SetValue(block, Mode switch
                        {
                            "bound-blank" => "", "bound-length" => "001", "bound-start" => "0000", "bound-gap" => "0004", "bound-nonnumeric" => "00AA",
                            _ => throw new InvalidOperationException("Unknown bound case")
                        });
                    }
                }
            }
            Tracked.Add(result);
            return typeof(ServiceProbe).GetMethod(nameof(Complete), BindingFlags.Instance | BindingFlags.NonPublic)!.MakeGenericMethod(type).Invoke(this, new[] { (object)step, result });
        }
        ThrowIfNeeded(step);
        return method.ReturnType == typeof(void) ? null : ServiceConsolidationChecks.Sample(method.ReturnType);
    }
    async Task CompleteVoid(int step) { if (GateAt == step) await Gate.Task; ThrowIfNeeded(step); }
    public async Task CopyFileAsync(Stream target, CancellationToken cancellationToken)
    {
        Calls.Add("IFormFile.CopyToAsync(" + (cancellationToken == Token ? "caller" : cancellationToken == CancellationToken.None ? "none" : "other") + ")");
        int step = Calls.Count; AsyncSteps.Add(step);
        await CompleteVoid(step);
        await target.WriteAsync(new byte[] { 1, 2, 3, 4 }, cancellationToken);
    }
    async Task<T> Complete<T>(int step, object? result) { if (GateAt == step) await Gate.Task; ThrowIfNeeded(step); return (T)result!; }
    void ThrowIfNeeded(int step)
    {
        if (FailAt == step) throw new InvalidOperationException("intentional-service-failure-" + step);
        if (CancelAt == step) throw new OperationCanceledException("intentional-service-cancellation", Token);
    }
}

sealed class ServiceProbeFile(ServiceProbe state) : IFormFile
{
    public string ContentType => "image/png";
    public string ContentDisposition => "form-data";
    public IHeaderDictionary Headers { get; } = new HeaderDictionary();
    public long Length => 4;
    public string Name => "Icon";
    public string FileName => "icon.png";
    public Stream OpenReadStream() => new MemoryStream(new byte[] { 1, 2, 3, 4 });
    public void CopyTo(Stream target) => target.Write(new byte[] { 1, 2, 3, 4 });
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => state.CopyFileAsync(target, cancellationToken);
    public override string ToString() => "service-comparison-icon";
}

public class ServiceDependencyProxy : DispatchProxy
{
    public Type Contract = null!;
    public ServiceProbe State = null!;
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) => State.Invoke(Contract, targetMethod!, args ?? []);
}

sealed class ServiceHubCaller(CancellationToken token) : HubCallerContext
{
    public const string Connection = "service-comparison-connection";
    public override string ConnectionId => Connection;
    public override string UserIdentifier => "22222222-2222-2222-2222-222222222222";
    public override ClaimsPrincipal User { get; } = new(new ClaimsIdentity(new[]
        { new Claim(ClaimTypes.UserData, "22222222-2222-2222-2222-222222222222") }, "regression"));
    public override IDictionary<object, object?> Items { get; } = new Dictionary<object, object?>();
    public override IFeatureCollection Features { get; } = new FeatureCollection();
    public override CancellationToken ConnectionAborted => token;
    public override void Abort() { }
}
