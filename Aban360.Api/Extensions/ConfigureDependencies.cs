﻿using Aban360.UserPool.Application.Extensions;
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

namespace Aban360.Api.Extensions
{
    internal static class ConfigureDependencies
    {
        public static void AddDI(this IServiceCollection services)
        {
            services.AddUserPoolDI();
            services.AddLocationPoolDI();
            services.AddClaimPoolDI();
            services.AddReportPoolDI();
            services.AddCalculationPoolDI();
            services.AddMeterPoolDI();
            services.AddWorkflowPoolDI();
            services.AddBlobPoolDI();
            services.AddSystemPoolDI();
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
        }

        private static void AddReportPoolDI(this IServiceCollection services)
        {
            services.AddReportPoolPersistenceInjections();
        }
        private static void AddCalculationPoolDI(this IServiceCollection services)
        {
            services.AddCalculationPoolApplicationInjections();
            services.AddCalculationPoolPersistenceInjections();
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
            services.AddBlboPoolPoolApplicationInjections();
            services.AddBlobPoolPersistenceInjections();
            services.AddBlobPoolGatewayInjections();
        }
        private static void AddSystemPoolDI(this IServiceCollection services)
        {
            services.AddSystemPoolApplicationInjections();
        }
    }
}
