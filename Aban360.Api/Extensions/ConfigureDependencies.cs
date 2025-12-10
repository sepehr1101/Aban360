using Aban360.UserPool.Application.Extensions;
using Aban360.UserPool.Persistence.Extensions;
using Aban360.LocationPool.Application.Extensions;
using Aban360.LocationPool.Persistence.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Extentions;
using Aban360.ClaimPool.Application.Extentions;
using Aban360.ClaimPool.Persistence.Extensions;
using Aban360.ReportPool.Persistence.Extentions;
using Aban360.CalculationPool.Application.Extensions;
using Aban360.CalculationPool.Persistence.Extensions;
using Aban360.MeterPool.Application.Extensions;
using Aban360.MeterPool.Persistence.Extentions;
using Aban360.WorkflowPool.Persistence.Extensions;
using Aban360.WorkflowPool.Application.Extensions;
using Aban360.BlobPool.Persistence.Extensions;
using Aban360.BlobPool.Application.Extenstions;
using Aban360.BlobPool.GatewayAddHoc.Extentions;
using Aban360.SystemPool.Application.Extenstions;
using Aban360.ReportPool.Application.Extensions;
using Aban360.ReportPool.GatewayAdhoc.Extensions;
using Aban360.InstallationPool.Application.Extensions;
using Aban360.InstallationPool.Persistence.Extensions;
using Aban360.PaymentPool.Application.Extensions;
using Aban360.PaymentPool.Persistence.Extensions;
using Aban360.CalculationPool.GatewayAdhoc.Extensions;
using Aban360.ClaimPool.GatewayAdhoc.Extensions;
using Aban360.CommunicationPool.Application.Extensions;
using Aban360.CommunicationPool.Persistence.Extensions;
using Aban360.OldCalcPool.Persistence.Extensions;
using Aban360.OldCalcPool.Application.Extentions;
using Aban360.SystemPool.Persistence.Extensions;
using Aban260.BlobPool.Infrastructure.Extenstions;
using Aban360.ReportPool.Infrastructure.Extensions;
using Aban360.Common.Db.QueryServices;
using Aban360.TaxPool.Application.Extensions;
using Aban360.TaxPool.Infrastructure.Extensions;
using Aban360.TaxPool.Persistence.Extensions;

namespace Aban360.Api.Extensions
{
    internal static class ConfigureDependencies
    {
        public static void AddDI(this IServiceCollection services)
        {
            services.AddApiDI();
            services.AddUserPoolDI();
            services.AddLocationPoolDI();
            services.AddClaimPoolDI();
            services.AddCalculationPoolDI();
            services.AddMeterPoolDI();
            services.AddWorkflowPoolDI();
            services.AddBlobPoolDI();
            services.AddSystemPoolDI();
            services.AddReportPoolDI();
            services.AddInstallationPoolDI();
            services.AddPaymentPoolDI();
            services.AddCommunicationPoolDI();
            services.AddOldCalcPoolDI();
            services.AddCommonDbDI();
            services.AddTaxPoolDI();
        }

        private static void AddApiDI(this IServiceCollection services)
        {
            services.AddApiInjections();
        }
        private static void AddUserPoolDI(this IServiceCollection services)
        {
            services.AddUserPoolPersistenceInjections();
            services.AddUserPoolApplicationInjections();
        }
        private static void AddLocationPoolDI(this IServiceCollection services)
        {
            services.AddLocationPoolPersistenceInjections();
            services.AddLocationPoolApplicationInjections();
            services.AddLocationPoolGatewayInjections();
        }

        private static void AddClaimPoolDI(this IServiceCollection services)
        {
            services.AddClaimPoolApplicationInjections();
            services.AddClaimPoolPersistenceInjections();
            services.AddClaimPoolGatewayInjections();
        }

        private static void AddReportPoolDI(this IServiceCollection services)
        {
            services.AddReportPoolApplicationInjections();
            services.AddReportPoolPersistenceInjections();
            services.AddReportPoolGatewayInjections();
            services.AddReportPoolInfrastructureInjections();
        }
        private static void AddCalculationPoolDI(this IServiceCollection services)
        {
            services.AddCalculationPoolApplicationInjections();
            services.AddCalculationPoolPersistenceInjections();
            services.AddCalculationPoolGatewayInjections();
        }
        private static void AddMeterPoolDI(this IServiceCollection services)
        {
            services.AddMeterPoolApplicationInjections();
            services.AddMeterPoolPersistenceInjections();
        }
        private static void AddWorkflowPoolDI(this IServiceCollection services)
        {
            services.AddWorkflowPoolApplicationInjections();
            services.AddWorkflowPoolPersistenceInjections();
        }
        private static void AddBlobPoolDI(this IServiceCollection services)
        {
            services.AddBlobPoolApplicationInjections();
            services.AddBlobPoolPersistenceInjections();
            services.AddBlobPoolGatewayInjections();
            services.AddBlobPoolInfrastructureInjections();
        }
        private static void AddSystemPoolDI(this IServiceCollection services)
        {
            services.AddSystemPoolApplicationInjections();
            services.AddSystemPoolPersistenceInjections(); 
        }

        private static void AddInstallationPoolDI(this IServiceCollection services)
        {
            services.AddInstallationPoolApplicationInjections();
            services.AddInstallationPoolPersistenceInjections();
        }
        private static void AddPaymentPoolDI(this IServiceCollection services)
        {
            services.AddPaymentPoolApplicationInjections();
            services.AddPaymentPoolPersistenceInjections();
        }

        private static void AddCommunicationPoolDI(this IServiceCollection services)
        {
            services.AddCommunicationPoolApplicationInjections();
            services.AddCommunicationPoolPersistenceInjections();
        }
        
        private static void AddOldCalcPoolDI(this IServiceCollection services)
        {
            services.AddOldCalcPoolApplicationInjections();
            services.AddOldCalcPoolPersistenceInjections();
        }

        private static void AddCommonDbDI(this IServiceCollection services)
        {
            services.AddTransient<ICommonZoneService, CommonZoneService>();
        }

        private static void AddTaxPoolDI(this IServiceCollection services)
        {
            services.AddTaxPoolApplicationInjections();
            services.AddTaxPoolInfrastructureInjections();
            services.AddTaxPoolPersistenceInjections();
        }
    }
}
