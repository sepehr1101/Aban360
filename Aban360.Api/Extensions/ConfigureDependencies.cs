using Aban360.UserPool.Persistence.Extensions;
using Aban360.UserPool.Application.Extensions;

namespace Aban360.Api.Extensions
{
    public static class ConfigureDependencies
    {
        public static void AddDI(this IServiceCollection services)
        {
            services.AddUserPoolPersistenceInjections();
            services.AddUserPoolApplicationInjections();
        }
    }
}
