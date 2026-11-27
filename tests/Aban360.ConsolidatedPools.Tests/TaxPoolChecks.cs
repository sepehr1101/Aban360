using System.Net;
using System.Reflection;
using System.Text.Json;
using Aban360.TaxPool.Application.Extensions;
using Aban360.TaxPool.Application.Features.MaaherSTP.Services;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;
using Aban360.TaxPool.Infrastructure.Extensions;
using Aban360.TaxPool.Infrastructure.Features.MaaherTSP.Contracts;
using Aban360.TaxPool.Persistence.Extensions;
using Aban360.TaxPool.Persistence.Features.MaaherTSP.Contracts;
using Aban360.TaxPool.Persistence.Migrations;
using FluentMigrator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class TaxPoolChecks
{
    internal static async Task RunAsync()
    {
        Assembly module = typeof(DbInitialDesign).Assembly;
        Program.Check(module.GetName().Name == "Aban360.TaxPool" &&
            typeof(MaaherOptions).Assembly == module && typeof(IMaaherInvoiceQueriesService).Assembly == module &&
            typeof(IMaaherService).Assembly == module && typeof(IMaaherErrorsQueryService).Assembly == module,
            "TaxPool layers must be in the same module assembly.");
        Program.Check(typeof(DbInitialDesign).GetCustomAttribute<MigrationAttribute>()?.Version == 1404080701,
            "TaxPool migration identity changed.");
        foreach (string seed in new[] { "MaaherErrors.sql", "MaaherInvoiceStatus.sql" })
            Program.Check(File.Exists(Path.Combine(AppContext.BaseDirectory, "DbSeeder", "DataScript", seed)),
                $"Missing TaxPool seed: {seed}");

        ServiceCollection services = new();
        using RecordingHandler httpHandler = new();
        using HttpClient client = new(httpHandler, disposeHandler: false);
        services.AddSingleton(client);
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        services.Configure<MaaherOptions>(options =>
        {
            options.MaaherBaseUrl = "https://tax.test";
            options.SentInvoiceUrl = "/invoice/{0}/{1}/{2}";
            options.Tins = "test-tins";
            options.FiscalId = "test-fiscal";
            options.AuthenticationId = "test-auth";
            options.Token = "test-token";
        });
        services.AddTaxPoolApplicationInjections();
        services.AddTaxPoolInfrastructureInjections();
        services.AddTaxPoolPersistenceInjections();

        foreach (Type contract in new[] { typeof(IMaaherInvoiceQueriesService), typeof(IMaaherService), typeof(IMaaherErrorsQueryService) })
            Program.Check(services.Single(item => item.ServiceType == contract).Lifetime == ServiceLifetime.Scoped,
                $"{contract.Name} must be registered exactly once with scoped lifetime.");

        using ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();
        Program.Check(scope.ServiceProvider.GetRequiredService<IMaaherInvoiceQueriesService>().GetType().Assembly == module,
            "Tax invoice query service did not resolve from the merged assembly.");
        IMaaherService transport = scope.ServiceProvider.GetRequiredService<IMaaherService>();
        Program.Check((await transport.SendInvoice(new[] { new MaaherRequestWrapper_New { Uid = "test-invoice" } })).Count == 1,
            "Tax transport did not deserialize the fake response.");
        Program.Check(httpHandler.Received, "No request was captured by the fake transport.");
        Console.WriteLine("PASS: TaxPool assembly, migration, seed assets, scoped DI and fake invoice HTTP request.");
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public bool Received { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Program.Check(request.Method == HttpMethod.Post &&
                request.RequestUri?.AbsoluteUri == "https://tax.test/invoice/test-tins/test-fiscal/test-auth" &&
                request.Headers.Authorization?.ToString() == "Bearer test-token" &&
                request.Content?.Headers.ContentType?.MediaType == "application/json", "Tax invoice HTTP contract changed.");
            using JsonDocument body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync(cancellationToken));
            Program.Check(body.RootElement[0].GetProperty("Uid").GetString() == "test-invoice", "Invoice body changed.");
            Received = true;
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("[{}]") };
        }
    }
}
