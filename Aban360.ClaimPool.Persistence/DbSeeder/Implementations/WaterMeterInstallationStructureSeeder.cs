using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class WaterMeterInstallationStructureSeeder : IDataSeeder
    {
        public int Order { get; set; } = 16;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WaterMeterInstallationStructure> _waterMeterInstallationStructure;
        public WaterMeterInstallationStructureSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _waterMeterInstallationStructure = _uow.Set<WaterMeterInstallationStructure>();
            _waterMeterInstallationStructure.NotNull(nameof(_waterMeterInstallationStructure));
        }

        public void SeedData()
        {
            if (_waterMeterInstallationStructure.Any())
            {
                return;
            }

            ICollection<WaterMeterInstallationStructure> waterMeterInstallationStructure = new List<WaterMeterInstallationStructure>()
            {
                new WaterMeterInstallationStructure(){Id=WaterMeterInstallationStructureEnum.OnGround,Title="زمینی"},
                new WaterMeterInstallationStructure(){Id=WaterMeterInstallationStructureEnum.OnWall,Title="دیواری"},
            };
            _waterMeterInstallationStructure.AddRange(waterMeterInstallationStructure);
            _uow.SaveChanges();
        }
    }
}
