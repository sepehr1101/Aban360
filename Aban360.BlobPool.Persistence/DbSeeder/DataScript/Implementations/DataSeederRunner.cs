using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Db.DbSeeder.Implementation;

namespace Aban360.BlobPool.Persistence.DbSeeder.DataScript.Implementations
{
    public class DataSeederRunner : AbstractDataSeedersRunner
    {
        public DataSeederRunner(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {

        }
    }
}
