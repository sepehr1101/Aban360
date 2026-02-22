using Aban360.Common.Db.QueryServices;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.Common.Db.Extensions
{
    public static class ConfigureServices
    {
        public static void AddCommonDbInjections(this IServiceCollection services)
        {
            services.AddTransient<ICommonZoneService, CommonZoneService>();
            services.AddTransient<ICommonMemberQueryService, CommonMemberQueryService>();
        }
    }
}
