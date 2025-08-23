namespace Aban360.HealthApi.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

    namespace Aban360.HealthApi.Extensions
    {
        public static class HealthCheckExtensions
        {
            public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddSqlServerHealthCheck(configuration);
                services.AddHealthChecksUi();

                return services;
            }

            // 🔒 Private helper for MSSQL health check
            private static IServiceCollection AddSqlServerHealthCheck(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddHealthChecks()
                    .AddSqlServer(
                        connectionString: configuration.GetConnectionString("DefaultConnection"),
                        healthQuery: "SELECT 1;",
                        name: "sqlserver",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "db", "sql", "sqlserver" });

                return services;
            }

            // 🔒 Private helper for UI
            private static IServiceCollection AddHealthChecksUi(this IServiceCollection services)
            {
                services.AddHealthChecksUI(options =>
                {
                    options.SetEvaluationTimeInSeconds(60);
                    options.MaximumHistoryEntriesPerEndpoint(50);
                    options.AddHealthCheckEndpoint("default", "/healthz");
                }).AddInMemoryStorage();

                return services;
            }

            public static IEndpointRouteBuilder MapCustomHealthChecks(this IEndpointRouteBuilder app)
            {
                app.MapHealthChecksEndpoint();
                app.MapHealthChecksUiEndpoint();
                return app;
            }
            private static void MapHealthChecksEndpoint(this IEndpointRouteBuilder app)
            {
                app.MapHealthChecks("/healthz", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            }
            private static void MapHealthChecksUiEndpoint(this IEndpointRouteBuilder app)
            {
                app.MapHealthChecksUI(options => { options.UIPath = "/health-ui"; });
            }
        }
    }
}
