using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Aban260.BlobPool.Infrastructure.Extenstions;
using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Extenstions;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.BlobPool.GatewayAddHoc.Extentions;
using Aban360.BlobPool.Persistence.Contexts.Implementations;
using Aban360.BlobPool.Persistence.Extensions;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.BlobPool.Persistence.Migrations;
using Aban360.Common.Authentication;
using Aban360.Common.Literals;
using FluentMigrator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

internal static class BlobPoolRegistrationChecks
{
    internal static async Task RunAsync(Action<bool, string> check)
    {
        Assembly assembly = typeof(IOpenKmQueryService).Assembly;
        check(assembly.GetName().Name == "Aban360.BlobPool" && typeof(OpenKmOptions).Assembly == assembly &&
            typeof(BlobPoolContext).Assembly == assembly && typeof(DbInitialDesign).Assembly == assembly,
            "BlobPool layers share a single assembly");
        check(typeof(DbInitialDesign).GetCustomAttribute<MigrationAttribute>()?.Version == 14040117,
            "BlobPool migration identity retained");
        foreach (string seed in new[] { "DocumentType.sql", "OpenKmMetaData.sql" })
            check(File.Exists(Path.Combine(AppContext.BaseDirectory, "DbSeeder", "DataScript", seed)), $"Seed packaged: {seed}");

        foreach (bool bridgeOrder in new[] { false, true })
        {
            ServiceCollection services = new();
            services.AddLogging();
            services.AddDbContext<BlobPoolContext>(options =>
                options.UseSqlServer("Server=127.0.0.1;Database=Aban360;Integrated Security=true"));
            using FakeOpenKmHandler handler = new(check);
            using HttpClient client = new(handler, disposeHandler: false) { BaseAddress = new Uri("https://openkm.test/") };
            services.AddSingleton(Proxy.Create<IHttpClientFactory>((_, args) =>
            {
                check((string?)args[0] == HttpClientNames.Kaj, "OpenKM uses named Kaj client");
                return client;
            }));
            services.AddSingleton(Proxy.Create<IEsbTokenProvider>((_, _) =>
                Task.FromResult(new AuthenticationHeaderValue("Bearer", "test-token"))));
            services.AddSingleton(Options.Create(new OpenKmOptions { GeMetadataEndpoint = "metadata" }));
            if (bridgeOrder)
                services.AddBlobPoolPersistenceInjections();
            services.AddBlobPoolApplicationInjections();
            if (bridgeOrder)
                services.AddBlobPoolInfrastructureInjections();
            else
                services.AddBlobPoolPersistenceInjections();
            services.AddBlobPoolGatewayInjections();
            if (!bridgeOrder)
                services.AddBlobPoolInfrastructureInjections();

            check(services.Single(item => item.ServiceType == typeof(IOpenKmQueryService)).Lifetime == ServiceLifetime.Scoped,
                "OpenKM registered once and scoped");
            using ServiceProvider provider = services.BuildServiceProvider();
            using IServiceScope scope = provider.CreateScope();
            using IServiceScope nextScope = provider.CreateScope();
            IOpenKmQueryService openKm = scope.ServiceProvider.GetRequiredService<IOpenKmQueryService>();
            check(ReferenceEquals(openKm, scope.ServiceProvider.GetRequiredService<IOpenKmQueryService>()) &&
                !ReferenceEquals(openKm, nextScope.ServiceProvider.GetRequiredService<IOpenKmQueryService>()),
                "OpenKM instance reused only within its scope");
            check(ReferenceEquals(scope.ServiceProvider.GetRequiredService<IDocumentTypeCommandService>(),
                scope.ServiceProvider.GetRequiredService<IDocumentTypeQueryService>()), "Document type repository shares a scope");
            check((await openKm.GetMetaDataProperties("test-document")).RawMetaDatas!.Single().Value == 7,
                "OpenKM HTTP mock metadata deserialized");
            handler.StatusCode = HttpStatusCode.ServiceUnavailable;
            try
            {
                await openKm.GetMetaDataProperties("test-document");
                throw new InvalidOperationException("Expected OpenKM HTTP error.");
            }
            catch (HttpRequestException error) when (error.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                check(true, "OpenKM HTTP failure propagates");
            }
        }
    }

    private sealed class FakeOpenKmHandler(Action<bool, string> check) : HttpMessageHandler
    {
        internal HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            check(request.Method == HttpMethod.Get && request.RequestUri?.AbsolutePath == "/metadata" &&
                request.RequestUri.Query.Contains("nodeId=test-document", StringComparison.Ordinal) &&
                request.Headers.Authorization?.ToString() == "Bearer test-token", "OpenKM metadata HTTP request and authentication");
            return Task.FromResult(new HttpResponseMessage(StatusCode)
            {
                Content = new StringContent("{\"simplePropertyGroup\":[{\"name\":\"title\",\"value\":7}]}")
            });
        }
    }
}
