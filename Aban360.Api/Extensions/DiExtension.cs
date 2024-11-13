using Aban360.UserPool.Persistence.Extensions;

namespace Aban360.Api.Extensions
{
    public static class DiExtension
    {
        public static void AddUserPoolExtensions(this IServiceCollection services)
        {
            services.AddUserPoolPersistenceInjections();
        }
    }
}
