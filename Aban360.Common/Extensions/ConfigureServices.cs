using Microsoft.Extensions.DependencyInjection;
using Aban360.Application.Features.Security.Services.Contracts;
using Aban360.Common.Contrats;

namespace Aban360.Common.Extensions
{
    public static class ConfigureServices
    {
        public static void AddCommonInjections(this IServiceCollection services)
        {
            services.AddScoped<ISecurityOpertions, SecurityOperations>();
        }
    }
}
