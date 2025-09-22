using Aban360.UserPool.Application.Extensions;
using Aban360.UserPool.Persistence.Extensions;
using Aban360.ReportPool.Persistence.Extentions;
using Aban360.ReportPool.Application.Extensions;
using Aban360.ReportPool.GatewayAdhoc.Extensions;
using Aban360.LocationPool.Persistence.Extensions;
using Aban360.LocationPool.Application.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Extentions;
using Aban360.BlobPool.Persistence.Extensions;
using Aban360.BlobPool.Application.Extenstions;
using Aban260.BlobPool.Infrastructure.Extenstions;
using Aban360.BlobPool.GatewayAddHoc.Extentions;
namespace Aban360.BrdigeApi.Extensions
{
    internal static class ConfigureDependencies
    {
        public static void AddDI(this IServiceCollection services)
        {
            services.AddUserPoolDI();
            services.AddReportPoolDI();
            services.AddLocationPoolDI();
            services.AddBlobPoolDI();
        }

        private static void AddUserPoolDI(this IServiceCollection services)
        {
            services.AddUserPoolPersistenceInjections();
            services.AddUserPoolApplicationInjections();
        }
        private static void AddReportPoolDI(this IServiceCollection services)
        {
            services.AddReportPoolApplicationInjections();
            services.AddReportPoolPersistenceInjections();
            services.AddReportPoolGatewayInjections();
        }
        private static void AddLocationPoolDI(this IServiceCollection services)
        {
            services.AddLocationPoolPersistenceInjections();
            services.AddLocationPoolApplicationInjections();
            services.AddLocationPoolGatewayInjections();
        }
        private static void AddBlobPoolDI(this IServiceCollection services)
        {
            services.AddBlobPoolPersistenceInjections();
            services.AddBlobPoolApplicationInjections();
            services.AddBlobPoolInfrastructureInjections();
            services.AddBlobPoolGatewayInjections();
        }
    }
}
