using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.Common.Db.DbSeeder.Implementation
{
    public abstract class AbstractDataSeedersRunner : IDataSeedersRunner
    {
        private readonly IServiceProvider _serviceProvider;
        public AbstractDataSeedersRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceProvider.NotNull(nameof(serviceProvider));
        }
        public void RunAllDataSeeders()
        {
            var seeders = _serviceProvider.GetServices<IDataSeeder>().ToList();
            foreach (var seeder in seeders.OrderBy(dataSeeder => dataSeeder.Order))
            {
                seeder.SeedData();
            }
        }
    }
}
