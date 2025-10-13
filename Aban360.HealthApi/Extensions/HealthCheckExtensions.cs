namespace Aban360.HealthApi.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using System.Reflection;

    namespace Aban360.HealthApi.Extensions
    {
        internal static class HealthCheckExtensions
        {
            internal static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
            {
                //Methods should start with "Add" and End with "HealthCheck"
                ExecuteMethods(services, configuration);

                services.AddHealthChecksUi();
                return services;
            }
            private static void ExecuteMethods(IServiceCollection services, IConfiguration configuration)
            {
                var methods = typeof(HealthCheckExtensions)
                 .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                 .Where(m => m.Name.StartsWith("Add") && m.Name.EndsWith("HealthCheck"))
                 .ToList();

                methods.ForEach(m => m.Invoke(null, new object[] { services, configuration }));
            }

            // 🔒 Private helpers for MSSQL health check (called via reflection)
            private static IServiceCollection AddSqlServerHealthCheck(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddHealthChecks()
                    .AddSqlServer(
                        connectionString: configuration.GetConnectionString("DefaultConnection"),
                        healthQuery: "SELECT 1;",
                        name: "اتصال به پایگاه داده",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "db", "sql", "sqlserver","aban360" });

                return services;
            }
            private static IServiceCollection AddReportDbHealthCheck(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddHealthChecks()
                    .AddSqlServer(
                        connectionString: configuration.GetConnectionString("ReportConnection"),
                        healthQuery: "SELECT 1;",
                        name: "اتصال به پایگاه داده گزارش",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "db", "sql", "sqlserver", "report" });

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

            internal static IEndpointRouteBuilder MapCustomHealthChecks(this IEndpointRouteBuilder app)
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
