using Aban360.ClaimPool.Persistence.Contexts.Implementation;
using Aban360.Common.Db.Interceptors;
using Aban360.UserPool.Persistence.Contexts.Implementation;
using Microsoft.EntityFrameworkCore;

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
    }
}
