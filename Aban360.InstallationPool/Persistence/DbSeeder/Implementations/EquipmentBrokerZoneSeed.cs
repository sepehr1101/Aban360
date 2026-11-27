using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.DbSeeder.Implementations
{
    public class EquipmentBrokerZoneSeed : IDataSeeder
    {
        public int Order { get; set; } = 45;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EquipmentBrokerZone> _equipmentBrokerZones;
        public EquipmentBrokerZoneSeed(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _equipmentBrokerZones = _uow.Set<EquipmentBrokerZone>();
            _equipmentBrokerZones.NotNull(nameof(_equipmentBrokerZones));
        }

        public void SeedData()
        {
            if (_equipmentBrokerZones.Any())
            {
                return;
            }

            string sqlFilePath = GetSqlFilePath();
            _uow.ExecuteBatch(sqlFilePath);
        }
        private string GetSqlFilePath()
        {
            string basePath = AppContext.BaseDirectory;
            string relativePath = @"\DbSeeder\DataScript\EquipmentBrokerZone.sql";

            string path = string.Concat(basePath, relativePath);
            return path;
        }
    }
}
