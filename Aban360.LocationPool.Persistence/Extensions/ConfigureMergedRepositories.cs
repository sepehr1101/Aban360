using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.LocationPool.Persistence.Extensions
{
    internal static class ConfigureMergedRepositories
    {
        private static readonly Type[] RepositoryTypes =
        {
            typeof(CordinalDirectionRepository),
            typeof(CountryRepository),
            typeof(HeadquarterRepository),
        };

        internal static bool IsMergedRepository(Type type) => RepositoryTypes.Contains(type);

        internal static void AddMergedPersistenceRepositories(this IServiceCollection services)
        {
            AddRepository<CordinalDirectionRepository, ICordinalDirectionCommandService, ICordinalDirectionQueryService>(services);
            AddRepository<CountryRepository, ICountryCommandService, ICountryQueryService>(services);
            AddRepository<HeadquarterRepository, IHeadquarterCommandService, IHeadquarterQueryService>(services);
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
