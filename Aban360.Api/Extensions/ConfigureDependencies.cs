﻿using Aban360.UserPool.Persistence.Extensions;
using Aban360.UserPool.Application.Extensions;
using Aban360.LocationPool.Persistence.Extensions;
using Aban360.LocationPool.Application.Extensions;
using Aban360.ClaimPool.Persistence.Extensions;
using Aban360.ClaimPool.Application.Extentions;
using Aban360.LocationPool.GatewayAdhoc.Extentions;
using Aban360.ReportPool.Persistence.Extentions;
using Aban360.CalculationPool.Persistence.Extensions;

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
            services.AddCalculationPoolPersistenceInjections();
            services.AddCalculationPoolApplicationInjections();
        }
    }
}
