using Aban360.Common.Db.DbSeeder.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Aban360.MeterPool.Persistence.DbSeeder.Implementations
{
    public  class DataSeeduerRunner:AbstractDataSeedersRunner
    {
        public DataSeeduerRunner(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {

        }
    }
}
