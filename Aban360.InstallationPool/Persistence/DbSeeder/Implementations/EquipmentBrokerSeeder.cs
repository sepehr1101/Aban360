using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.InstallationPool.Persistence.DbSeeder.Implementations
{
    public class EquipmentBrokerSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EquipmentBroker> _equipmentBroker;
        public EquipmentBrokerSeeder(IUnitOfWork uow)
        {
            _uow=uow;
            _uow.NotNull(nameof(uow));

            _equipmentBroker = _uow.Set<EquipmentBroker>();
            _equipmentBroker.NotNull(nameof(_equipmentBroker));
        }

        public void SeedData()
        {
            if (_equipmentBroker.Any())
            {
                return;
            }
             
             ICollection<EquipmentBroker> equipmentBrokers = new List<EquipmentBroker>()
             {
                 new EquipmentBroker(){Title="پرواز برنز",Website="Todo",ApiUrl="Todo"},
                 new EquipmentBroker(){Title="نگین هوشمند بسپار",Website="Todo",ApiUrl="Todo"},
             };

            _equipmentBroker.AddRange(equipmentBrokers);
            _uow.SaveChanges();
        }
    }
}
