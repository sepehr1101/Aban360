using Microsoft.Extensions.DependencyInjection;
using Pol.Common.Extensions;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.DbSeeder.Contracts;

namespace Aban360.UserPool.Persistence.DbSeeder.Implementations
{
    public class DataSeedersRunner : IDataSeedersRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _uow;
        public DataSeedersRunner(IServiceProvider serviceProvider, IUnitOfWork uow)
        {
            _serviceProvider=serviceProvider;
            _serviceProvider.NotNull(nameof(serviceProvider));

            _uow=uow;
            _uow.NotNull(nameof(uow));
        }
        public void RunAllDataSeeders()
        {
            var seeders = _serviceProvider.GetServices<IDataSeeder>().ToList();            
            foreach (var seeder in seeders.OrderBy(dataSeeder => dataSeeder.Order))
            {               
                seeder.SeedData();
            }
            _uow.SaveChanges();
        }
    }
}
