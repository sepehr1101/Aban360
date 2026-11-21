using System.Reflection;
using System.Text.Json;
using Aban360.CalculationPool.Application.Features.MeterReading.Services;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Tracking.Services;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Tests.ServiceBaselines;
using Aban360.UserPool.Application.Features.Auth.Services;
using AutoMapper;
using Hangfire;
using Hangfire.Common;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;

static class Next28ReviewChecks
{
    public static async Task<int> Run(IMapper mapper, IServiceCollection registrations)
    {
        int checks = 0;
        void Check(bool condition, string message) { if (!condition) throw new Exception(message); checks++; }
        var op = ApplicationServiceBaseline.Next28Operations.Single(o => o.Feature == "GeneralRequestInformation");
        var before = typeof(Next28ReviewChecks).Assembly.GetType(op.Original, true)!;
        var contract = typeof(IGeneralRequestInformationService);
        var implementation = contract.Assembly.GetTypes().Single(t => t.IsClass && !t.IsAbstract && contract.IsAssignableFrom(t));
        var method = implementation.GetMethod(op.Method)!;
        var original = await ServiceConsolidationChecks.Execute(before, before.GetMethod("Handle")!, new(mapper, CancellationToken.None));
        Check(original.Error?.StartsWith("System.NullReferenceException:") == true && original.Calls.Length == 2, "Missing legacy IMoshtrakService assignment was not reproduced");
        var success = new ServiceProbe(mapper, CancellationToken.None);
        bool selectedServicesChecked = false;
        success.CallObserver = (type, called, arguments) =>
        {
            if (type == typeof(IMoshtrakService) && called.Name == "GetMoshtrakCompanyServiceDto")
            {
                var input = (MoshtrakServiceDto)arguments[0]!;
                Check(input.s0 == 17 && input.s48 == 17 && (int)arguments[1]! == 17, "Requested services or service group were not preserved");
                selectedServicesChecked = true;
            }
        };
        var corrected = await ServiceConsolidationChecks.Execute(implementation, method, success);
        Check(corrected.Error is null && selectedServicesChecked && corrected.Calls.Length == 3, "GeneralInformation did not load its company services: " + corrected.Error);
        using (var json = JsonDocument.Parse(corrected.Result))
        {
            var data = json.RootElement;
            Check(data.GetProperty("TrackNumber").GetInt32() == 17 && data.GetProperty("BillId").GetString() == "sample-BillId" && data.GetProperty("FirstName").GetString() == "sample-FirstName", "GeneralInformation lost tracking/subscriber fields");
            Check(data.GetProperty("CompanyServices").GetArrayLength() == 2 && data.GetProperty("CompanyServices")[0].GetProperty("Id").GetInt32() == 17, "GeneralInformation lost selected company services");
        }
        foreach (bool cancel in new[] { false, true })
        {
            var state = new ServiceProbe(mapper, CancellationToken.None) { FailAt = cancel ? 0 : 3, CancelAt = cancel ? 3 : 0 };
            var outcome = await ServiceConsolidationChecks.Execute(implementation, method, state);
            Check(outcome.Error?.Contains(cancel ? "OperationCanceledException" : "intentional-service-failure-3") == true && outcome.Calls.Length == 3, "GeneralInformation swallowed service failure/cancellation");
        }
        var gated = new ServiceProbe(mapper, CancellationToken.None) { GateAt = 3 };
        var pending = ServiceConsolidationChecks.Execute(implementation, method, gated);
        Check(!pending.IsCompleted && gated.Calls.Count == 3, "GeneralInformation returned before company services completed");
        gated.Gate.SetResult(true);
        Check((await pending).Error is null, "GeneralInformation failed after company services completed");

        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        foreach (var scenario in new[]
        {
            (Assembly: typeof(IMeterReadingSmsService).Assembly, Name: "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations.MeterReadingDetailSendSmsHandler", Method: "CreateJob", Args: new object[] { 17, userId }, Service: typeof(IMeterReadingSmsService)),
            (Assembly: typeof(IMeterReadingSmsService).Assembly, Name: "Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations.MeterReadingDetailSendSmsHandler", Method: "SendSms", Args: new object[] { 17, userId }, Service: typeof(IMeterReadingSmsService)),
            (Assembly: typeof(IUserRoleMembershipService).Assembly, Name: "Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts.IUpdateUsersInRoleHandler", Method: "Handle", Args: new object[] { 17, CancellationToken.None }, Service: typeof(IUserRoleMembershipService)),
            (Assembly: typeof(IUserRoleMembershipService).Assembly, Name: "Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Implementations.UpdateUsersInRoleHandler", Method: "Handle", Args: new object[] { 17, CancellationToken.None }, Service: typeof(IUserRoleMembershipService))
        })
        {
            var legacy = scenario.Assembly.GetType(scenario.Name, true)!;
            var job = new Job(legacy, legacy.GetMethod(scenario.Method)!, scenario.Args);
            var restored = InvocationData.SerializeJob(job).DeserializeJob();
            Check(restored.Type == legacy && restored.Method.Name == scenario.Method && restored.Args.Count == scenario.Args.Length, "Persisted Hangfire job cannot resolve its original type/method");
            if (scenario.Method is "CreateJob" or "SendSms")
                Check(restored.Method.GetCustomAttribute<AutomaticRetryAttribute>()?.Attempts == 2, "Persisted SMS job retry policy changed");
            var services = new ServiceCollection();
            foreach (var descriptor in registrations) ((ICollection<ServiceDescriptor>)services).Add(descriptor);
            var state = new ServiceProbe(mapper, CancellationToken.None) { GateAt = 1 };
            services.AddScoped(scenario.Service, _ => state.Proxy(scenario.Service));
            using var provider = services.BuildServiceProvider(); using var scope = provider.CreateScope();
            var target = ActivatorUtilities.GetServiceOrCreateInstance(scope.ServiceProvider, restored.Type);
            var task = (Task)restored.Method.Invoke(target, restored.Args.ToArray())!;
            Check(!task.IsCompleted && state.Calls.Count == 1 && state.Calls[0].Contains("17"), "Persisted job adapter did not await/forward its operation");
            state.Gate.SetResult(true); await task;
            foreach (bool cancel in new[] { false, true })
            {
                state.GateAt = 0; int next = state.Calls.Count + 1;
                state.FailAt = cancel ? 0 : next; state.CancelAt = cancel ? next : 0;
                Exception? error = null;
                try { await (Task)restored.Method.Invoke(target, restored.Args.ToArray())!; }
                catch (Exception caught) { error = caught; }
                Check(cancel ? error is OperationCanceledException : error is InvalidOperationException, "Persisted job adapter swallowed failure/cancellation");
            }
        }
        Console.WriteLine($"Next28 review: {checks} checks passed; reproduced missing dependency, validated corrected output and restored four legacy Hangfire job signatures.");
        return checks;
    }
}
