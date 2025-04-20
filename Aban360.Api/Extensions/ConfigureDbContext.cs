using Aban360.BlobPool.Persistence.Contexts.Implementations;
using Aban360.CalculationPool.Persistence.Contexts.Implementations;
using Aban360.ClaimPool.Persistence.Contexts.Implementation;
using Aban360.Common.Db.Interceptors;
using Aban360.InstallationPool.Persistence.Contexts.Implementations;
using Aban360.MeterPool.Persistence.Contexts.Implementations;
using Aban360.ReportPool.Persistence.Contexts.Implementations;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using Aban360.WorkflowPool.Persistence.Contexts.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Aban360.Api.Extensions
{
    public static class ConfigureDbContext
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddUserPoolDbContext(configuration, connectionString);
            services.AddLocationPoolDbContext(configuration, connectionString);
            services.AddClaimPoolDbContext(configuration, connectionString);
            services.AddCalculationPoolDbContext(configuration, connectionString);
            services.AddMeterPoolDbContext(configuration, connectionString);
            services.AddWorkflowPoolDbContext(configuration, connectionString);
            services.AddBlobPoolContext(configuration, connectionString);
            services.AddReportPoolContext(configuration, connectionString);
            services.AddInstallationPoolContext(configuration, connectionString);
        }
        private static void AddUserPoolDbContext(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.AddDbContext<UserPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                        serverDbContextOptionsBuilder =>
                        {
                            var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                            serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }
        private static void AddLocationPoolDbContext(this IServiceCollection services, IConfiguration configuration, string connectionString)
        {
            services.AddDbContext<LocationPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                        serverDbContextOptionsBuilder =>
                        {
                            var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                            serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }

        private static void AddClaimPoolDbContext(this IServiceCollection services, IConfiguration configuration1, string connectionString)
        {
            services.AddDbContext<ClaimPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }

        private static void AddCalculationPoolDbContext(this IServiceCollection services, IConfiguration configuration1, string connectionString)
        {
            services.AddDbContext<CalculationPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }

        private static void AddMeterPoolDbContext(this IServiceCollection services, IConfiguration configuration1, string connectionString)
        {
            services.AddDbContext<MeterPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                    SqlServerDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        SqlServerDbContextOptionsBuilder.CommandTimeout(minutes);
                    });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });

        }
        private static void AddWorkflowPoolDbContext(this IServiceCollection services, IConfiguration configuration1, string connectionString)
        {
            services.AddDbContext<WorkflowPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                    SqlServerDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        SqlServerDbContextOptionsBuilder.CommandTimeout(minutes);
                    });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });

        }

        private static void AddBlobPoolContext(this IServiceCollection services, IConfiguration configuration1, string connectionString)
        {
            services.AddDbContext<BlobPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                    SqlServerDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        SqlServerDbContextOptionsBuilder.CommandTimeout(minutes);
                    });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }
        
        private static void AddReportPoolContext(this IServiceCollection services, IConfiguration configuration1, string connectionString)
        {
            services.AddDbContext<ReportPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                    SqlServerDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        SqlServerDbContextOptionsBuilder.CommandTimeout(minutes);
                    });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }
        private static void AddInstallationPoolContext(this IServiceCollection services, IConfiguration configuration1, string connectionString)
        {
            services.AddDbContext<InstallationPoolContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                    SqlServerDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                        SqlServerDbContextOptionsBuilder.CommandTimeout(minutes);
                    });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }
    }
}
