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
using Aban360.ReportPool.Infrastructure.Extensions;
using Aban360.CalculationPool.Application.Extensions;
using Aban360.CalculationPool.Persistence.Extensions;
using Aban360.ClaimPool.Application.Extentions;
using Aban360.ClaimPool.Persistence.Extensions;
using Aban360.OldCalcPool.Application.Extentions;
using Aban360.OldCalcPool.Persistence.Extensions;
namespace Aban360.BrdigeApi.Extensions
{
    internal static class ConfigureDependencies
    {
        public static void AddDI(this IServiceCollection services)
        {
            services.AddUserPoolDI();
            services.AddReportPoolDI();
            services.AddLocationPoolDI();
            services.AddClaimPoolDI();
            services.AddBlobPoolDI();
            services.AddCalculationPoolDI();
            services.AddOldCalcPoolDI();
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
            services.AddReportPoolInfrastructureInjections();
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
        private static void AddCalculationPoolDI(this IServiceCollection services)
        {
            services.AddCalculationPoolApplicationInjections();
            services.AddCalculationPoolPersistenceInjections();
        }
        private static void AddClaimPoolDI(this IServiceCollection services)
        {
            services.AddClaimPoolApplicationInjections();
            services.AddClaimPoolPersistenceInjections();
        }
        private static void AddOldCalcPoolDI(this IServiceCollection services)
        {
            services.AddOldCalcPoolApplicationInjections();
            services.AddOldCalcPoolPersistenceInjections();
        }
    }
}