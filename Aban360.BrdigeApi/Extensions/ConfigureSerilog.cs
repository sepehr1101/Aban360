using Aban360.BrdigeApi.Extensions;
using Aban360.Common.Db.Extensions;
using Serilog;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;

namespace Aban360.BrdigeApi.Extensions
{
    public static class ConfigureSerilog
    {
        public static void AddSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            AddService(services, configuration);
            AddUi(services);
        }
        private static void AddService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSerilog(options =>
            {
                //we can configure serilog from configuration
                options.ReadFrom.Configuration(configuration);
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
            app.UseSerilogUi(opts =>
              opts.WithRoutePrefix("log")
            );
        }
    }
}
