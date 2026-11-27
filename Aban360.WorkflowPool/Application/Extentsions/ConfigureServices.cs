using Microsoft.Extensions.DependencyInjection;

namespace Aban360.WorkflowPool.Application.Extentsions
{
    public static class ConfigureServices
    {
        public static void AddWorkflowPoolApplicationInjections(this IServiceCollection services)
        {
            Extensions.ConfigureServices.AddWorkflowPoolApplicationInjections(services);
        }
    }
}
