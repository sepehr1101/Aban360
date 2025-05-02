using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class WaterMeterInstallationMethodSeeder : IDataSeeder
    {
        public int Order { get; set; } = 16;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterInstallationMethod> _waterMeterInstallationMethod;
        public WaterMeterInstallationMethodSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterInstallationMethod = _uow.Set<WaterMeterInstallationMethod>();
            _waterMeterInstallationMethod.NotNull(nameof(_waterMeterInstallationMethod));
        }

        public void SeedData()
        {
            if (_waterMeterInstallationMethod.Any())
            {
                return;
            }

            ICollection<WaterMeterInstallationMethod> waterMeterInstallationMethod = new List<WaterMeterInstallationMethod>()
            {
                new WaterMeterInstallationMethod(){Title="زمینی"},
                new WaterMeterInstallationMethod(){Title="دیواری"},
            };
            _waterMeterInstallationMethod.AddRange(waterMeterInstallationMethod);
            _uow.SaveChanges();
        }
    }
}
