using System.Reflection;
using Aban360.Common.Categories.ApiResponse;
using Aban360.PaymentPool.Application.Extensions;
using Aban360.CalculationPool.Application.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PaymentServices = Aban360.PaymentPool.Application.Features;
using PaymentQueries = Aban360.PaymentPool.Persistence.Features;
using PaymentEntities = Aban360.PaymentPool.Domain.Features;
using PaymentDtos = Aban360.PaymentPool.Domain.Features;
using RuleServices = Aban360.CalculationPool.Application.Features.Rule.Services;
using RuleQueries = Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using RuleEntities = Aban360.CalculationPool.Domain.Features.Rule.Entities;
using RuleDtos = Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Controllers = Aban360.Api.Controllers.V1;

// Exercise the actual application registrations, mapping profiles and services.
// Only persistence is replaced; these checks never open a database connection.
static class LookupServiceChecks
{
    public static async Task<int> Run()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddPaymentPoolApplicationInjections();
        services.AddCalculationPoolApplicationInjections();
        int checks = 0;
        void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
            checks++;
        }

        await Verify<short, PaymentEntities.Remuneration.Entities.Bank, PaymentDtos.Remuneration.Dto.Queries.BankGetDto,
            PaymentServices.Remuneration.Services.IBankService, PaymentQueries.Remuneration.Queries.Contracts.IBankQueryService,
            Controllers.PaymentPool.Remuneration.BankController>(17);
        await Verify<short, RuleEntities.SupportedOperator, RuleDtos.SupportedOperatorGetDto,
            RuleServices.ISupportedOperatorService, RuleQueries.ISupportedOperatorQueryService,
            Controllers.CalculationPool.Rule.SupportedOperatorController>(23);
        await Verify<short, RuleEntities.SupportedField, RuleDtos.SupportedFieldGetDto,
            RuleServices.ISupportedFieldService, RuleQueries.ISupportedFieldQueryService,
            Controllers.CalculationPool.Rule.SupportedFieldController>(29);
        await Verify<Aban360.PaymentPool.Domain.Constansts.AccountTypeEnum,
            PaymentEntities.NegotiableInstrument.Entities.AccountType, PaymentDtos.NegotiableInstrument.Dto.Queries.AccountTypeGetDto,
            PaymentServices.NegotiableInstrument.Services.IAccountTypeService, PaymentQueries.NegotiableInstrument.Queries.Contracts.IAccountTypeQueryService,
            Controllers.PaymentPool.NegotiableInstrument.AccountTypeController>((Aban360.PaymentPool.Domain.Constansts.AccountTypeEnum)2);
        await Verify<Aban360.PaymentPool.Domain.Constansts.CreditorTypeEnum,
            PaymentEntities.NegotiableInstrument.Entities.CreditorType, PaymentDtos.NegotiableInstrument.Dto.Queries.CreditorTypeGetDto,
            PaymentServices.NegotiableInstrument.Services.ICreditorTypeService, PaymentQueries.NegotiableInstrument.Queries.Contracts.ICreditorTypeQueryService,
            Controllers.PaymentPool.NegotiableInstrument.CreditorTypeController>((Aban360.PaymentPool.Domain.Constansts.CreditorTypeEnum)3);
        Console.WriteLine($"PASS: {checks} lookup service checks; 5 real services, DI scopes, mapping, controllers, failures and asynchronous completion.");
        return checks;

        async Task Verify<TId, TEntity, TDto, TService, TQueries, TController>(TId id)
            where TEntity : class, new() where TDto : class, new()
            where TService : class where TQueries : class where TController : ControllerBase
        {
            var query = DispatchProxy.Create<TQueries, LookupDependency>();
            var stub = (LookupDependency)(object)query;
            // Register before building the provider so the real service gets this fake query.
            var local = new ServiceCollection();
            foreach (var descriptor in services) ((ICollection<ServiceDescriptor>)local).Add(descriptor);
            local.AddScoped<TQueries>(_ => query);
            using var provider = local.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
            using var scope = provider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<TService>();
            Check(ReferenceEquals(service, scope.ServiceProvider.GetRequiredService<TService>()), typeof(TService).Name + " scoped lifetime");
            using (var other = provider.CreateScope())
                Check(!ReferenceEquals(service, other.ServiceProvider.GetRequiredService<TService>()), typeof(TService).Name + " leaked across scopes");
            var controller = (TController)Activator.CreateInstance(typeof(TController), service)!;
            using var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            var first = Sample(1);
            var second = Sample(2);
            foreach (bool single in new[] { false, true })
            {
                int calls = 0;
                object queryTask = single ? Task.FromResult(first) : Task.FromResult<ICollection<TEntity>>(new[] { first, second });
                stub.InvokeMethod = (method, args) =>
                {
                    Check(method.Name == "Get" && args.Length == (single ? 1 : 0), "Wrong query operation: " + typeof(TService).Name);
                    if (single) Check(Equals(args[0], id), "Query identifier changed");
                    calls++;
                    return queryTask;
                };
                var data = await Read();
                Check(calls == 1, "Query executed more than once");
                if (single) Compare(first, (TDto)data!);
                else
                {
                    var items = ((ICollection<TDto>)data!).ToArray();
                    Check(items.Length == 2, "Collection cardinality changed");
                    Compare(first, items[0]); Compare(second, items[1]);
                }

                // Preserve the old mapper behavior for missing rows and empty lists.
                queryTask = single ? Task.FromResult<TEntity>(null!) : Task.FromResult<ICollection<TEntity>>(Array.Empty<TEntity>());
                data = await Read();
                Check(single ? data is null : ((ICollection<TDto>)data!).Count == 0, "Missing/empty result changed");

                var failure = new InvalidOperationException("lookup query failure");
                queryTask = single ? Task.FromException<TEntity>(failure) : Task.FromException<ICollection<TEntity>>(failure);
                calls = 0;
                try { await Read(); throw new Exception("Query failure was swallowed"); }
                catch (InvalidOperationException ex) when (ReferenceEquals(ex, failure)) { Check(calls == 1, "Failure caused a retry"); }

                using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
                queryTask = single ? Task.FromCanceled<TEntity>(cancelled.Token) : Task.FromCanceled<ICollection<TEntity>>(cancelled.Token);
                calls = 0;
                try { await Read(); throw new Exception("Query cancellation was swallowed"); }
                catch (OperationCanceledException ex) { Check(ex.CancellationToken == cancelled.Token && calls == 1, "Cancellation/token changed"); }

                var oneGate = new TaskCompletionSource<TEntity>(TaskCreationOptions.RunContinuationsAsynchronously);
                var manyGate = new TaskCompletionSource<ICollection<TEntity>>(TaskCreationOptions.RunContinuationsAsynchronously);
                queryTask = single ? oneGate.Task : manyGate.Task;
                calls = 0;
                var pending = Read();
                Check(calls == 1 && !pending.IsCompleted, "Controller/service did not await persistence");
                if (single) oneGate.SetResult(first); else manyGate.SetResult(new[] { first });
                data = await pending;
                Compare(first, single ? (TDto)data! : ((ICollection<TDto>)data!).Single());

                // Check token forwarding at the HTTP/application boundary separately.
                var fakeService = DispatchProxy.Create<TService, LookupDependency>();
                var fake = (LookupDependency)(object)fakeService;
                int serviceCalls = 0;
                var payload = new TDto();
                fake.InvokeMethod = (method, args) =>
                {
                    Check(method.Name == (single ? "GetSingleAsync" : "GetAllAsync"), "Wrong application operation");
                    Check(args.Length == (single ? 2 : 1) && Equals(args[^1], token), "Controller dropped cancellation token");
                    if (single) Check(Equals(args[0], id), "Controller changed identifier");
                    serviceCalls++;
                    return single ? Task.FromResult(payload) : Task.FromResult<ICollection<TDto>>(new[] { payload });
                };
                var fakeController = (TController)Activator.CreateInstance(typeof(TController), fakeService)!;
                data = await Read(fakeController);
                Check(serviceCalls == 1, "Application service invoked more than once");
                Check(ReferenceEquals(single ? data : ((ICollection<TDto>)data!).Single(), payload), "Controller changed service payload");

                async Task<object?> Read(TController? target = null)
                {
                    var method = typeof(TController).GetMethod(single ? "GetSingle" : "GetAll")!;
                    object?[] arguments = single ? new object?[] { id, token } : new object?[] { token };
                    var result = await (Task<IActionResult>)method.Invoke(target ?? controller, arguments)!;
                    Check(result is OkObjectResult { StatusCode: 200 }, "HTTP status changed");
                    var envelope = ((OkObjectResult)result).Value;
                    if (single)
                    {
                        Check(envelope is ApiResponseEnvelope<TDto> { Success: true, HttpStatusCode: 200 }, "Single response envelope changed");
                        return ((ApiResponseEnvelope<TDto>)envelope!).Data;
                    }
                    Check(envelope is ApiResponseEnvelope<ICollection<TDto>> { Success: true, HttpStatusCode: 200 }, "List response envelope changed");
                    return ((ApiResponseEnvelope<ICollection<TDto>>)envelope!).Data;
                }
            }

            TEntity Sample(int seed)
            {
                var entity = new TEntity();
                foreach (var property in typeof(TEntity).GetProperties().Where(p => p.CanWrite))
                {
                    if (property.Name == "Id") property.SetValue(entity, id);
                    else if (property.PropertyType == typeof(string)) property.SetValue(entity, property.Name == "Icon" && seed == 2 ? null : $"{property.Name}-{seed}");
                }
                return entity;
            }
            void Compare(TEntity entity, TDto dto)
            {
                Check(dto is not null, "Mapped DTO missing");
                foreach (var property in typeof(TDto).GetProperties())
                {
                    var source = typeof(TEntity).GetProperty(property.Name)!;
                    Check(Equals(source.GetValue(entity), property.GetValue(dto)), typeof(TService).Name + " mapping changed: " + property.Name);
                }
            }
        }
    }
}

public class LookupDependency : DispatchProxy
{
    public Func<MethodInfo, object?[], object?> InvokeMethod { get; set; } = (_, _) => throw new Exception("Unexpected lookup dependency call");
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) => InvokeMethod(targetMethod!, args ?? []);
}
