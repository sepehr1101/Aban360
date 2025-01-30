using Aban360.Common.Db.Extensions;
namespace Aban360.Api.Extensions
{
    internal static class ConfigureMigrationsAndSeeds
    {
        internal static void AddMigragionsAndSeeds(this IServiceCollection services)
        {            
            services.UpdateAndSeedDb();
        }
    }
}
