using System.Text.RegularExpressions;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Contexts.Implementations;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Implementations;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Aban360.SystemPool.Application.Features.UserGuide.Services;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Persistence.Migrations;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.CommunicationPool.Application.Features.Hubs.Services;
using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using Aban360.CommunicationPool.Domain.Features.Hubs.Entities;
using Aban360.CommunicationPool.Persistence.Contexts.Implementations;
using Aban360.InstallationPool.Application.Features.Definition.Services;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.InstallationPool.Persistence.Contexts.Implementations;
using Aban360.WorkflowPool.Application.Features.Design.Services;
using Aban360.WorkflowPool.Domain.Constants;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Contexts.Implementation;
using Aban360.WorkflowPool.Persistence.DbSeeder.Implementations;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MeterUnitOfWork = Aban360.MeterPool.Persistence.Contexts.Contracts.IUnitOfWork;

internal static class DockerSqlTests
{
    internal static async Task RunAsync()
    {
        string? password = Environment.GetEnvironmentVariable("ABAN360_DOCKER_SQL_PASSWORD");
        if (string.IsNullOrEmpty(password) ||
            !int.TryParse(Environment.GetEnvironmentVariable("ABAN360_DOCKER_SQL_PORT"), out int port) ||
            port is < 1 or > 65535)
            throw new InvalidOperationException("Use Run-DockerIntegration.ps1 to provision the disposable SQL Server.");

        SqlConnectionStringBuilder connection = new()
        {
            DataSource = $"tcp:127.0.0.1,{port}",
            InitialCatalog = "master",
            UserID = "sa",
            Password = password,
            ConnectTimeout = 3,
            TrustServerCertificate = true
        };
        await using (SqlConnection master = new(connection.ConnectionString))
        {
            await WaitForSqlServerAsync(master);
            // Dapper uses [Aban360] explicitly. Fail if the database already exists.
            await using SqlCommand createDatabase = new("CREATE DATABASE [Aban360]", master);
            await createDatabase.ExecuteNonQueryAsync();
            await using SqlCommand createAtlas = new("CREATE DATABASE [Atlas]", master);
            await createAtlas.ExecuteNonQueryAsync();
        }

        connection.InitialCatalog = "Aban360";
        string database = connection.ConnectionString;
        ServiceCollection services = Program.CreateServices(database);
        services.AddFluentMigratorCore().ConfigureRunner(runner => runner
            .AddSqlServer()
            .WithGlobalConnectionString(database)
            .ScanIn(typeof(CommunicationPoolContext).Assembly, typeof(InstallationPoolContext).Assembly,
                typeof(WorkflowPoolContext).Assembly, typeof(_1404_07_12_DbInitialDesign).Assembly,
                typeof(MeterPoolContext).Assembly, typeof(PaymentPoolContext).Assembly).For.Migrations());

        await using ServiceProvider provider = services.BuildServiceProvider();
        await using AsyncServiceScope scope = provider.CreateAsyncScope();
        IServiceProvider scoped = scope.ServiceProvider;
        scoped.GetRequiredService<IMigrationRunner>().MigrateUp();

        await RunSqlScriptAsync(database, Path.Combine(AppContext.BaseDirectory, "HubEventPresence.sql"));

        Guid userId = Guid.NewGuid();
        await scoped.GetRequiredService<IHubEventService>().CreateAsync(new HubEventCreateDto("docker-room", userId), CancellationToken.None);
        CommunicationPoolContext communication = scoped.GetRequiredService<CommunicationPoolContext>();
        Program.Check(await communication.Set<HubEvent>().AnyAsync(
            row => row.UserId == userId && row.ConnectionId == "docker-room" && row.LastSeenDateTime != null),
            "Communication Dapper insert was not visible through EF Core.");

        await scoped.GetRequiredService<IEquipmentBrokerService>().CreateAsync(new EquipmentBrokerCreateDto
        {
            Title = "کارگزار آزمایشی",
            Website = "https://broker.test",
            ApiUrl = "https://broker.test/api"
        }, CancellationToken.None);
        await scoped.GetRequiredService<IUnitOfWork>().SaveChangesAsync();
        await using AsyncServiceScope readScope = provider.CreateAsyncScope();
        ICollection<EquipmentBrokerGetDto> brokers = await readScope.ServiceProvider.GetRequiredService<IEquipmentBrokerService>()
            .GetAllAsync(CancellationToken.None);
        Program.Check(brokers.Single().Title == "کارگزار آزمایشی", "Installation repository did not round-trip the broker.");

        IDataSeeder workflowSeeder = scoped.GetServices<IDataSeeder>().Single(seeder => seeder is WorkflowStatusSeeder);
        workflowSeeder.SeedData();
        workflowSeeder.SeedData();
        ICollection<WorkflowStatusGetDto> statuses = await readScope.ServiceProvider.GetRequiredService<IWorkflowStatusService>()
            .WorkflowStatusGetAllAsync(CancellationToken.None);
        Program.Check(statuses.Select(status => status.Id).OrderBy(id => id).SequenceEqual(Enum.GetValues<WorkflowStatusEnum>()),
            "Workflow seed must produce one persisted row per status.");

        IFaqApplicationService faq = scoped.GetRequiredService<IFaqApplicationService>();
        Program.Check(!(await faq.GetAllAsync(CancellationToken.None)).Any(), "FAQ migration must create an empty table.");
        await RunSqlScriptAsync(database, Path.Combine(AppContext.BaseDirectory, "DbSeeder", "DataScript", "Faq.sql"));
        IEnumerable<FaqGetAllDto> entries = await faq.GetAllAsync(CancellationToken.None);
        Program.Check(entries.Any(entry => entry.Id > 0 && !string.IsNullOrWhiteSpace(entry.Header)),
            "The FAQ service could not read entries from the packaged seed script.");

        CounterState state = new() { Title = "Test counter state", HeadquartersId = 1, HeadquartersTitle = "Test headquarters", IsEnabled = true };
        await scoped.GetRequiredService<ICounterStateCommandService>().Add(state);
        await scoped.GetRequiredService<MeterUnitOfWork>().SaveChangesAsync();
        CounterState savedState = await readScope.ServiceProvider.GetRequiredService<ICounterStateQueryService>().Get(state.Id);
        Program.Check(state.Id > 0 && savedState.Title == state.Title && savedState.IsEnabled,
            "Meter counter state did not round-trip through SQL Server.");

        connection.InitialCatalog = "Atlas";
        string atlas = connection.ConnectionString;
        await RunSqlScriptAsync(atlas, Path.Combine(AppContext.BaseDirectory, "Atlas", "001_PaymentPool_Base_Atlas.sql"));
        await RunSqlScriptAsync(atlas, Path.Combine(AppContext.BaseDirectory, "Atlas", "002_BankReceipt_Atlas.sql"));
        await using PaymentPoolContext atlasContext = new(new DbContextOptionsBuilder<PaymentPoolContext>().UseSqlServer(atlas).Options);
        Bank bank = new() { BankName = "Test bank", CentralBankCode = "999" };
        atlasContext.Banks.Add(bank);
        await atlasContext.SaveChangesAsync();
        Bank savedBank = await scoped.GetRequiredService<IBankReceiptBankQueryService>().Get(bank.Id);
        Program.Check(savedBank.BankName == bank.BankName, "Bank receipt query did not read the bank from Atlas.");

        BankReceiptImport import = new()
        {
            Id = Guid.NewGuid(), UserId = userId, BankId = bank.Id, BankName = bank.BankName,
            FileName = "test-receipt.txt", FileHash = new string('a', 64), CreatedAt = DateTime.UtcNow
        };
        await scoped.GetRequiredService<IBankReceiptImportRepository>().Create(import, CancellationToken.None);
        BankReceiptImport savedImport = await readScope.ServiceProvider.GetRequiredService<IBankReceiptImportRepository>()
            .Get(import.Id, userId, CancellationToken.None);
        Program.Check(savedImport.BankId == bank.Id && savedImport.FileName == import.FileName &&
            savedImport.Status == BankReceiptImportStatus.Uploaded, "The bank receipt import did not round-trip through Atlas.");

        Console.WriteLine("PASS: Docker SQL Server migrations, Communication presence, Installation round-trip, Workflow seeding, System FAQ, Meter counter state and Atlas bank receipts.");
    }

    private static async Task RunSqlScriptAsync(string connectionString, string path)
    {
        string script = await File.ReadAllTextAsync(path);
        await using SqlConnection sql = new(connectionString);
        await sql.OpenAsync();
        foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(batch))
                continue;
            await using SqlCommand command = new(batch, sql);
            await command.ExecuteNonQueryAsync();
        }
    }

    private static async Task WaitForSqlServerAsync(SqlConnection connection)
    {
        using CancellationTokenSource deadline = new(TimeSpan.FromMinutes(2));
        while (true)
        {
            try
            {
                await connection.OpenAsync(deadline.Token);
                return;
            }
            catch (SqlException) when (!deadline.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(2), deadline.Token);
            }
        }
    }
}
