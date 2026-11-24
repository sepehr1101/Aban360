using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Aban360.InstallationPool.Persistence.Features.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.InstallationPool.Persistence.Extensions
{
    internal static class ConfigureMergedRepositories
    {
        private static readonly Type[] RepositoryTypes =
        {
            typeof(EquipmentBrokerRepository),
            typeof(EquipmentBrokerZoneRepository),
            typeof(SewageEquipmentBrokerRepository),
            typeof(SewageEquipmentBrokerZoneRepository),
        };

        internal static bool IsMergedRepository(Type type) => RepositoryTypes.Contains(type);

        internal static void AddMergedPersistenceRepositories(this IServiceCollection services)
        {
            AddRepository<EquipmentBrokerRepository, IEquipmentBrokerCommandService, IEquipmentBrokerQueryService>(services);
            AddRepository<EquipmentBrokerZoneRepository, IEquipmentBrokerZoneCommandService, IEquipmentBrokerZoneQueryService>(services);
            AddRepository<SewageEquipmentBrokerRepository, ISewageEquipmentBrokerCommandService, ISewageEquipmentBrokerQueryService>(services);
            AddRepository<SewageEquipmentBrokerZoneRepository, ISewageEquipmentBrokerZoneCommandService, ISewageEquipmentBrokerZoneQueryService>(services);
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
