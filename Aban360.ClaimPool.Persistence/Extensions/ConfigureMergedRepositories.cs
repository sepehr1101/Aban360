using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Repositories;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Repositories;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Repositories;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Repositories;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Repositories;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.ClaimPool.Persistence.Extensions
{
    internal static class ConfigureMergedRepositories
    {
        private static readonly Type[] RepositoryTypes =
        {
            typeof(RequestEstateRepository),
            typeof(RequestFlatRepository),
            typeof(RequestIndividualRepository),
            typeof(RequestIndividualDiscountTypeRepository),
            typeof(RequestIndividualEstateRepository),
            typeof(RequestIndividualTagRepository),
            typeof(RequestSiphonRepository),
            typeof(RequestTrackingRepository),
            typeof(RequestUserRepository),
            typeof(RequestWaterMeterRepository),
            typeof(RequestWaterMeterSiphonRepository),
            typeof(RequestWaterMeterTagRepository),
            typeof(CapacityCalculationIndexRepository),
            typeof(ConstructionTypeRepository),
            typeof(EstateBoundTypeRepository),
            typeof(EstateRepository),
            typeof(EstateWaterResourceRepository),
            typeof(FlatRepository),
            typeof(GuildRepository),
            typeof(HandoverRepository),
            typeof(OfficialHolidayRepository),
            typeof(ProfessionRepository),
            typeof(UsageLevel1Repository),
            typeof(UsageLevel2Repository),
            typeof(UsageLevel3Repository),
            typeof(UsageLevel4Repository),
            typeof(UserLeaveRepository),
            typeof(UserWorkdayRepository),
            typeof(WaterResourceRepository),
            typeof(MeterDiameterRepository),
            typeof(MeterMaterialRepository),
            typeof(MeterProducerRepository),
            typeof(MeterTypeRepository),
            typeof(MeterUseTypeRepository),
            typeof(SubscriptionTypeRepository),
            typeof(UseStateRepository),
            typeof(WaterMeterRepository),
            typeof(WaterMeterInstallationStructureRepository),
            typeof(WaterMeterTagRepository),
            typeof(WaterMeterTagDefinitionRepository),
            typeof(IndividualRepository),
            typeof(IndividualDiscountTypeRepository),
            typeof(IndividualEstateRepository),
            typeof(IndividualEstateRelationTypeRepository),
            typeof(IndividualTagRepository),
            typeof(IndividualTagDefinitionRepository),
            typeof(IndividualTypeRepository),
            typeof(GatewayRepository),
            typeof(SiphonRepository),
            typeof(SiphonDiameterRepository),
            typeof(SiphonMaterialRepository),
        };

        internal static bool IsMergedRepository(Type type) => RepositoryTypes.Contains(type);

        internal static void AddMergedPersistenceRepositories(this IServiceCollection services)
        {
            AddRepository<RequestEstateRepository, IRequestEstateCommandService, IRequestEstateQueryService>(services);
            AddRepository<RequestFlatRepository, IRequestFlatCommandService, IRequestFlatQueryService>(services);
            AddRepository<RequestIndividualRepository, IRequestIndividualCommandService, IRequestIndividualQueryService>(services);
            AddRepository<RequestIndividualDiscountTypeRepository, IRequestIndividualDiscountTypeCommandService, IRequestIndividualDiscountTypeQueryService>(services);
            AddRepository<RequestIndividualEstateRepository, IRequestIndividualEstateCommandService, IRequestIndividualEstateQueryService>(services);
            AddRepository<RequestIndividualTagRepository, IRequestIndividualTagCommandService, IRequestIndividualTagQueryService>(services);
            AddRepository<RequestSiphonRepository, IRequestSiphonCommandService, IRequestSiphonQueryService>(services);
            AddRepository<RequestTrackingRepository, IRequestTrackingCommandService, IRequestTrackingQueryService>(services);
            AddRepository<RequestUserRepository, IRequestUserCommandService, IRequestUserQueryService>(services);
            AddRepository<RequestWaterMeterRepository, IRequestWaterMeterCommandService, IRequestWaterMeterQueryService>(services);
            AddRepository<RequestWaterMeterSiphonRepository, IRequestWaterMeterSiphonCommandService, IRequestWaterMeterSiphonQueryService>(services);
            AddRepository<RequestWaterMeterTagRepository, IRequestWaterMeterTagCommandService, IRequestWaterMeterTagQueryService>(services);
            AddRepository<CapacityCalculationIndexRepository, ICapacityCalculationIndexCommandService, ICapacityCalculationIndexQueryService>(services);
            AddRepository<ConstructionTypeRepository, IConstructionTypeCommandService, IConstructionTypeQueryService>(services);
            AddRepository<EstateBoundTypeRepository, IEstateBoundTypeCommandService, IEstateBoundTypeQueryService>(services);
            AddRepository<EstateRepository, IEstateCommandService, IEstateQueryService>(services);
            AddRepository<EstateWaterResourceRepository, IEstateWaterResourceCommandService, IEstateWaterResourceQueryService>(services);
            AddRepository<FlatRepository, IFlatCommandService, IFlatQueryService>(services);
            AddRepository<GuildRepository, IGuildCommandService, IGuildQueryService>(services);
            AddRepository<HandoverRepository, IHandoverCommandService, IHandoverQueryService>(services);
            AddRepository<OfficialHolidayRepository, IOfficialHolidayCommandService, IOfficialHolidayQueryService>(services);
            AddRepository<ProfessionRepository, IProfessionCommandService, IProfessionQueryService>(services);
            AddRepository<UsageLevel1Repository, IUsageLevel1CommandService, IUsageLevel1QueryService>(services);
            AddRepository<UsageLevel2Repository, IUsageLevel2CommandService, IUsageLevel2QueryService>(services);
            AddRepository<UsageLevel3Repository, IUsageLevel3CommandService, IUsageLevel3QueryService>(services);
            AddRepository<UsageLevel4Repository, IUsageLevel4CommandService, IUsageLevel4QueryService>(services);
            AddRepository<UserLeaveRepository, IUserLeaveCommandService, IUserLeaveQueryService>(services);
            AddRepository<UserWorkdayRepository, IUserWorkdayCommandService, IUserWorkdayQueryService>(services);
            AddRepository<WaterResourceRepository, IWaterResourceCommandService, IWaterResourceQueryService>(services);
            AddRepository<MeterDiameterRepository, IMeterDiameterCommandService, IMeterDiameterQueryService>(services);
            AddRepository<MeterMaterialRepository, IMeterMaterialCommandService, IMeterMaterialQueryService>(services);
            AddRepository<MeterProducerRepository, IMeterProducerCommandService, IMeterProducerQueryService>(services);
            AddRepository<MeterTypeRepository, IMeterTypeCommandService, IMeterTypeQueryService>(services);
            AddRepository<MeterUseTypeRepository, IMeterUseTypeCommandService, IMeterUseTypeQueryService>(services);
            AddRepository<SubscriptionTypeRepository, ISubscriptionTypeCommandService, ISubscriptionTypeQueryService>(services);
            AddRepository<UseStateRepository, IUseStateCommandService, IUseStateQueryService>(services);
            AddRepository<WaterMeterRepository, IWaterMeterCommandService, IWaterMeterQueryService>(services);
            AddRepository<WaterMeterInstallationStructureRepository, IWaterMeterInstallationMethodCommandService, IWaterMeterInstallationMethodQueryService>(services);
            AddRepository<WaterMeterTagRepository, IWaterMeterTagCommandService, IWaterMeterTagQueryService>(services);
            AddRepository<WaterMeterTagDefinitionRepository, IWaterMeterTagDefinitionCommandService, IWaterMeterTagDefinitionQueryService>(services);
            AddRepository<IndividualRepository, IIndividualCommandService, IIndividualQueryService>(services);
            AddRepository<IndividualDiscountTypeRepository, IIndividualDiscountTypeCommandService, IIndividualDiscountTypeQueryService>(services);
            AddRepository<IndividualEstateRepository, IIndividualEstateCommandService, IIndividualEstateQueryService>(services);
            AddRepository<IndividualEstateRelationTypeRepository, IIndividualEstateRelationTypeCommandService, IIndividualEstateRelationTypeQueryService>(services);
            AddRepository<IndividualTagRepository, IIndividualTagCommandService, IIndividualTagQueryService>(services);
            AddRepository<IndividualTagDefinitionRepository, IIndividualTagDefinitionCommandService, IIndividualTagDefinitionQueryService>(services);
            AddRepository<IndividualTypeRepository, IIndividualTypeCommandService, IIndividualTypeQueryService>(services);
            AddRepository<GatewayRepository, IGatewayCommandService, IGatewayQueryService>(services);
            AddRepository<SiphonRepository, ISiphonCommandService, ISiphonQueryService>(services);
            AddRepository<SiphonDiameterRepository, ISiphonDiameterCommandService, ISiphonDiameterQueryService>(services);
            AddRepository<SiphonMaterialRepository, ISiphonMaterialCommandService, ISiphonMaterialQueryService>(services);
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
