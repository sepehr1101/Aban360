using Aban360.Common.Extensions;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.DbSeeder.Contracts;
using Microsoft.Extensions.DependencyInjection;
using DataSeeders= Aban360.Common.Db.DbSeeder.Contracts;

namespace Aban360.LocationPool.Persistence.DbSeeder.Implementations
{
    public class DataSeedersRunner: DataSeeders.IDataSeedersRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _uow;
        public DataSeedersRunner(IServiceProvider serviceProvider, IUnitOfWork uow)
        {
            _serviceProvider = serviceProvider;
            _serviceProvider.NotNull(nameof(serviceProvider));

            _uow = uow;
            _uow.NotNull(nameof(uow));
        }
        public void RunAllDataSeeders()
        {
            var seeders = _serviceProvider.GetServices<DataSeeders.IDataSeeder>().ToList();
            foreach (var seeder in seeders.OrderBy(dataSeeder => dataSeeder.Order))
            {
                seeder.SeedData();
            }
            _uow.SaveChanges();
        }
    }
}
