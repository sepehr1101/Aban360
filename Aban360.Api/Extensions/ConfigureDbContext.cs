using Aban360.UserPool.Persistence.Contexts.Implementation;
using Aban360.UserPool.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Aban360.Api.Extensions
{
    public static class ConfigureDbContext
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Aban360Context>((sp, options) =>
            {
                options.UseSqlServer(connectionString,
                        serverDbContextOptionsBuilder =>
                        {
                            var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                            serverDbContextOptionsBuilder.CommandTimeout(minutes);
                            //serverDbContextOptionsBuilder.EnableRetryOnFailure();
                        });
                options.AddInterceptors(new PersianYeKeCommandInterceptor());
                options.AddInterceptors(new RowLevelAuthenticitySaveChangeInterceptor());
            });
        }
    }
}
