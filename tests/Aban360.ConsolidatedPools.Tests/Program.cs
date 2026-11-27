using System.Reflection;
using Aban360.MeterPool.Application.Extensions;
using Aban360.MeterPool.Application.Features.Management.Services;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Implementations;
using Aban360.MeterPool.Persistence.Extentions;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using Aban360.PaymentPool.Application.Extensions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Services;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Implementations;
using Aban360.PaymentPool.Persistence.Extensions;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.SystemPool.Application.Extenstions;
using Aban360.SystemPool.Application.Features.UserGuide.Services;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Aban360.SystemPool.Persistence.DbSeeder.Implementations;
using Aban360.SystemPool.Persistence.Extensions;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;
using Aban360.CommunicationPool.Application.Extensions;
using Aban360.CommunicationPool.Application.Features.Hubs.Services;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Domain.Features.Hubs.Entities;
using Aban360.CommunicationPool.Persistence.Contexts.Implementations;
using Aban360.CommunicationPool.Persistence.Extensions;
using Aban360.CommunicationPool.Persistence.Features.Hubs.Commands.Contracts;
using Aban360.InstallationPool.Application.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Services;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Implementations;
using Aban360.InstallationPool.Persistence.Extensions;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Aban360.WorkflowPool.Application.Extensions;
using Aban360.WorkflowPool.Application.Features.Design.Services;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Domain.Features.Design.Entities;
using Aban360.WorkflowPool.Persistence.Contexts.Implementation;
using Aban360.WorkflowPool.Persistence.DbSeeder.Implementations;
using Aban360.WorkflowPool.Persistence.Extensions;
using AutoMapper;
using FluentMigrator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommunicationMigration = Aban360.CommunicationPool.Persistence.Migrations.DbInitialDesign;
using InstallationMigration = Aban360.InstallationPool.Persistence.Migrations.DbInitialDesign;
using WorkflowMigration = Aban360.WorkflowPool.Persistence.Migrations.DbInitialDesign;
using SystemMigration = Aban360.SystemPool.Persistence.Migrations._1404_07_12_DbInitialDesign;
using MeterMigration = Aban360.MeterPool.Persistence.Migrations.DbInitialDesign;
using PaymentMigration = Aban360.PaymentPool.Persistence.Migrations.DbInitialDesign;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        const string localSchemaInspection = "Server=127.0.0.1;Database=Aban360;Integrated Security=true;TrustServerCertificate=true";
        CheckModule("CommunicationPool", 14040310, typeof(CommunicationMigration), typeof(HubEvent), typeof(IHubEventService), typeof(CommunicationPoolContext));
        CheckModule("InstallationPool", 14030130, typeof(InstallationMigration), typeof(EquipmentBroker), typeof(IEquipmentBrokerService), typeof(InstallationPoolContext));
        CheckModule("WorkflowPool", 14040103, typeof(WorkflowMigration), typeof(WorkflowStatus), typeof(IWorkflowStatusService), typeof(WorkflowPoolContext));
        CheckModule("SystemPool", 14040712, typeof(SystemMigration), typeof(Faq), typeof(IFaqApplicationService), typeof(IFaqService));
        CheckModule("MeterPool", 14031218, typeof(MeterMigration), typeof(CounterState), typeof(ICounterStateService), typeof(MeterPoolContext));
        CheckModule("PaymentPool", 14040202, typeof(PaymentMigration), typeof(Bank), typeof(IBankReceiptService), typeof(PaymentPoolContext));

        ServiceCollection bridge = new();
        bridge.AddCommunicationPoolPersistenceInjections();
        Check(bridge.Any(item => item.ServiceType == typeof(IHubEventCreateService)) &&
            bridge.All(item => item.ServiceType != typeof(IHubEventService)), "Bridge must register Communication Persistence only.");

        ServiceCollection legacyWorkflow = new();
        Aban360.WorkflowPool.Application.Extentsions.ConfigureServices.AddWorkflowPoolApplicationInjections(legacyWorkflow);
        CheckScoped<IWorkflowStatusService>(legacyWorkflow);
        Check(legacyWorkflow.All(item => item.ServiceType.Namespace?.StartsWith("Aban360.WorkflowPool.Persistence.", StringComparison.Ordinal) != true),
            "The legacy Workflow extension must register Application services only.");

        ServiceCollection services = CreateServices(localSchemaInspection);
        CheckScoped<IHubEventService>(services);
        CheckScoped<IEquipmentBrokerService>(services);
        CheckScoped<IWorkflowStatusService>(services);
        CheckScoped<IFaqApplicationService>(services);
        CheckScoped<IFaqService>(services);
        CheckScoped<ICounterStateService>(services);
        CheckScoped<ICounterStateQueryService>(services);
        CheckScoped<IBankReceiptService>(services);
        CheckScoped<IBankReceiptBankQueryService>(services);
        using ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();
        using IServiceScope second = provider.CreateScope();
        IServiceProvider scoped = scope.ServiceProvider;
        Check(scoped.GetRequiredService<IHubEventService>() != second.ServiceProvider.GetRequiredService<IHubEventService>(),
            "Communication service must be scoped.");
        Check(ReferenceEquals(scoped.GetRequiredService<IEquipmentBrokerCommandService>(), scoped.GetRequiredService<IEquipmentBrokerQueryService>()),
            "Installation command and query contracts must share a scoped repository.");
        Check(scoped.GetRequiredService<IEquipmentBrokerService>() != second.ServiceProvider.GetRequiredService<IEquipmentBrokerService>(),
            "Installation service must be scoped.");
        Check(scoped.GetRequiredService<IWorkflowStatusService>() != second.ServiceProvider.GetRequiredService<IWorkflowStatusService>(),
            "Workflow service must be scoped.");
        IMapper mapper = scoped.GetRequiredService<IMapper>();
        Check(mapper.Map<NotifyTextMessageOutput>(new NotifyTextMessageInput { Message = "test" }).Message == "test",
            "Communication mapper profile was not registered.");
        Check(mapper.Map<EquipmentBroker>(new EquipmentBrokerCreateDto { Title = "test" }).Title == "test",
            "Installation mapper profile was not registered.");
        Check(mapper.Map<WorkflowStatusGetDto>(new WorkflowStatus { Title = "test" }).Title == "test",
            "Workflow mapper profile was not registered.");
        Check(!scoped.GetRequiredService<IValidator<EquipmentBrokerCreateDto>>().Validate(new EquipmentBrokerCreateDto()).IsValid,
            "Installation must still load its validator.");
        Check(scoped.GetServices<IDataSeeder>().Any(seeder => seeder is WorkflowStatusSeeder), "Workflow seed registration missing.");
        Check(scoped.GetServices<IDataSeeder>().Any(seeder => seeder is FaqSeeder), "FAQ seed registration missing.");
        Check(scoped.GetRequiredService<IFaqApplicationService>() != second.ServiceProvider.GetRequiredService<IFaqApplicationService>(),
            "FAQ service must be scoped.");
        Check(File.Exists(Path.Combine(AppContext.BaseDirectory, "DbSeeder", "DataScript", "Faq.sql")),
            "FAQ seed SQL was not copied to its runtime path.");
        Check(scoped.GetRequiredService<CommunicationPoolContext>().Model.FindEntityType(typeof(HubEvent))?.GetSchema() == "CommunicationPool",
            "Communication SQL schema changed.");
        Check(scoped.GetRequiredService<InstallationPoolContext>().Model.FindEntityType(typeof(EquipmentBroker))?.GetSchema() == "InstallationPool",
            "Installation SQL schema changed.");
        Check(scoped.GetRequiredService<WorkflowPoolContext>().Model.FindEntityType(typeof(WorkflowStatus))?.GetSchema() == "WorkflowPool",
            "Workflow SQL schema changed.");
        Check(scoped.GetRequiredService<MeterPoolContext>().Model.FindEntityType(typeof(CounterState))?.GetSchema() == "MeterPool",
            "Meter SQL schema changed.");
        Check(scoped.GetRequiredService<PaymentPoolContext>().Model.FindEntityType(typeof(Bank))?.GetSchema() == "PaymentPool",
            "Payment SQL schema changed.");
        Check(scoped.GetRequiredService<ICounterStateQueryService>() != second.ServiceProvider.GetRequiredService<ICounterStateQueryService>(),
            "Meter query service must be scoped.");
        Check(scoped.GetRequiredService<IBankReceiptBankQueryService>() != second.ServiceProvider.GetRequiredService<IBankReceiptBankQueryService>(),
            "Payment bank query service must be scoped.");
        foreach (string seed in new[] { "Meter.sql", "Bank.sql", "BankAccount.sql" })
            Check(File.Exists(Path.Combine(AppContext.BaseDirectory, "DbSeeder", "DataScript", seed)), $"Missing packaged seed asset: {seed}");
        Check(File.Exists(Path.Combine(AppContext.BaseDirectory, "DbSeeder", "DataScript", "EquipmentBrokerZone.sql")),
            "Installation seed SQL was not copied to its runtime path.");
        Console.WriteLine("PASS: six assemblies, migration identities, host DI boundaries, scoped services, validation, EF models and seed assets.");
        await TaxPoolChecks.RunAsync();

        if (args is ["--docker-sql"])
            await DockerSqlTests.RunAsync();
        else if (args.Length != 0)
            throw new ArgumentException("Expected no argument or --docker-sql.");
    }

    internal static ServiceCollection CreateServices(string connectionString)
    {
        ServiceCollection services = new();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = connectionString,
                ["ConnectionStrings:ReportConnection"] = connectionString
            }).Build());
        services.AddDbContext<CommunicationPoolContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<InstallationPoolContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<WorkflowPoolContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<MeterPoolContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<PaymentPoolContext>(options => options.UseSqlServer(connectionString));
        services.AddCommunicationPoolApplicationInjections();
        services.AddCommunicationPoolPersistenceInjections();
        services.AddInstallationPoolPersistenceInjections();
        services.AddInstallationPoolApplicationInjections();
        services.AddWorkflowPoolApplicationInjections();
        services.AddWorkflowPoolPersistenceInjections();
        services.AddSystemPoolApplicationInjections();
        services.AddSystemPoolPersistenceInjections();
        services.AddMeterPoolApplicationInjections();
        services.AddMeterPoolPersistenceInjections();
        services.AddPaymentPoolApplicationInjections();
        services.AddPaymentPoolPersistenceInjections();
        return services;
    }

    private static void CheckModule(string name, long version, Type migration, params Type[] types)
    {
        Assembly assembly = migration.Assembly;
        Check(assembly.GetName().Name == $"Aban360.{name}" && types.All(type => type.Assembly == assembly),
            $"{name} types must be in a single module assembly.");
        Check(!assembly.GetReferencedAssemblies().Any(reference => reference.Name?.StartsWith($"Aban360.{name}.", StringComparison.Ordinal) == true),
            $"{name} references a legacy assembly.");
        Check(migration.GetCustomAttribute<MigrationAttribute>()?.Version == version, $"{name} migration version changed.");
    }

    private static void CheckScoped<T>(IServiceCollection services) =>
        Check(services.Single(service => service.ServiceType == typeof(T)).Lifetime == ServiceLifetime.Scoped,
            $"{typeof(T).Name} must be registered once and scoped.");

    internal static void Check(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }
}
