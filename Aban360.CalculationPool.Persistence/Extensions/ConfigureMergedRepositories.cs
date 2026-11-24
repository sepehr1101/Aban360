using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Repositories;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.CalculationPool.Persistence.Extensions
{
    internal static class ConfigureMergedRepositories
    {
        private static readonly Type[] RepositoryTypes =
        {
            typeof(CompanyServiceRepository),
            typeof(CompanyServiceOfferingRepository),
            typeof(CompanyServiceTypeRepository),
            typeof(ImpactSignRepository),
            typeof(InvoiceInstallmentRepository),
            typeof(InvoiceLineItemInsertModeRepository),
            typeof(InvoiceStatusRepository),
            typeof(InvoiceTypeRepository),
            typeof(LineItemTypeRepository),
            typeof(LineItemTypeGroupRepository),
            typeof(OfferingGroupRepository),
            typeof(OfferingUnitRepository),
            typeof(WaterMeterChangeNumberHistoryRepository),
            typeof(TariffCalculationModeRepository),
            typeof(TariffRepository),
            typeof(TariffConstantRepository),
        };

        internal static bool IsMergedRepository(Type type) => RepositoryTypes.Contains(type);

        internal static void AddMergedPersistenceRepositories(this IServiceCollection services)
        {
            AddRepository<CompanyServiceRepository, ICompanyServiceCommandService, ICompanyServiceQueryService>(services);
            AddRepository<CompanyServiceOfferingRepository, ICompanyServiceOfferingCommandService, ICompanyServiceOfferingQueryService>(services);
            AddRepository<CompanyServiceTypeRepository, ICompanyServiceTypeCommandService, ICompanyServiceTypeQueryService>(services);
            AddRepository<ImpactSignRepository, IImpactSignCommandService, IImpactSignQueryService>(services);
            AddRepository<InvoiceInstallmentRepository, IInvoiceInstallmentCommandService, IInvoiceInstallmentQueryService>(services);
            AddRepository<InvoiceLineItemInsertModeRepository, IInvoiceLineItemInsertModeCommandService, IInvoiceLineItemInsertModeQueryService>(services);
            AddRepository<InvoiceStatusRepository, IInvoiceStatusCommandService, IInvoiceStatusQueryService>(services);
            AddRepository<InvoiceTypeRepository, IInvoiceTypeCommandService, IInvoiceTypeQueryService>(services);
            AddRepository<LineItemTypeRepository, ILineItemTypeCommandService, ILineItemTypeQueryService>(services);
            AddRepository<LineItemTypeGroupRepository, ILineItemTypeGroupCommandService, ILineItemTypeGroupQueryService>(services);
            AddRepository<OfferingGroupRepository, IOfferingGroupCommandService, IOfferingGroupQueryService>(services);
            AddRepository<OfferingUnitRepository, IOfferingUnitCommandService, IOfferingUnitQueryService>(services);
            AddRepository<WaterMeterChangeNumberHistoryRepository, IWaterMeterChangeNumberHistoryCommandService, IWaterMeterChangeNumberHistoryQueryService>(services);
            AddRepository<TariffCalculationModeRepository, ITariffCalculationModeCommandService, ITariffCalculationModeQueryService>(services);
            AddRepository<TariffRepository, ITariffCommandService, ITariffQueryService>(services);
            AddRepository<TariffConstantRepository, ITariffConstantCommandService, ITariffConstantQueryService>(services);
        }

        private static void AddRepository<TRepository, TCommand, TQuery>(IServiceCollection services)
            where TRepository : class, TCommand, TQuery
            where TCommand : class
            where TQuery : class
        {
            services.AddScoped<TRepository>();
            services.AddScoped<TCommand>(provider => provider.GetRequiredService<TRepository>());
            services.AddScoped<TQuery>(provider => provider.GetRequiredService<TRepository>());
        }
    }
}
