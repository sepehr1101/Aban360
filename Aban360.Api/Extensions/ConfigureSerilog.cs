using Aban360.Api.Extensions;
using Aban360.Common.Db.Extensions;
using Serilog;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;

namespace Aban360.Api.Extensions
{
    public static class ConfigureSerilog
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            AddSelfDebug();
            AddService(services, configuration);
            AddUi(services);
            return services;
        }
        private static void AddService(IServiceCollection services, IConfiguration configuration)
        {            
            services.AddSerilog(options =>
            {
                //we can configure serilog from configuration
                options.ReadFrom.Configuration(configuration);
            });
        }
        private static void AddSelfDebug()
        {
            Serilog.Debugging.SelfLog.Enable(msg => {
                Console.WriteLine($"Serilog Error: {msg}");
                System.Diagnostics.Debug.WriteLine($"Serilog Error: {msg}");
            });
        }
        private static void AddUi(IServiceCollection services)
        {
            var connectionString = MigrationRunner.GetConnectionInfo().Item1;
            services.AddSerilogUi(options => options
                .UseSqlServer(opts => opts
                .WithConnectionString(connectionString)
                .WithTable("Logs")));
        }

        public static void UseSerilogInterface(this IApplicationBuilder app)
        {            
            app.UseSerilogUi(opts => opts            
                .WithRoutePrefix("log")
                .WithAuthenticationType(Serilog.Ui.Web.Models.AuthenticationType.Custom)
                .EnableAuthorizationOnAppRoutes()
                .HideSerilogUiBrand()
            );
        }
    }
}
