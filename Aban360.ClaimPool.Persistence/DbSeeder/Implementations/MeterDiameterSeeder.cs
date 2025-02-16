using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.DbSeeder.Implementations
{
    public class MeterDiameterSeeder : IDataSeeder
    {
        public int Order { get; set; } = 22;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<MeterDiameter> _meterDiameters;
        public MeterDiameterSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _meterDiameters = _uow.Set<MeterDiameter>();
            _meterDiameters.NotNull(nameof(_meterDiameters));
        }

        public void SeedData()
        {
            if (_meterDiameters.Any())
            {
                return;
            }

            ICollection<MeterDiameter> meterDiameters = new List<MeterDiameter>()
            {
                  new MeterDiameter() { Id=1,Title="1/2 اینچ"},
                  new MeterDiameter() { Id=2,Title="3/4 اینچ"},
                  new MeterDiameter() { Id=3,Title="1 اینچ"},
                  new MeterDiameter() { Id=4,Title="1.2 اینچ"},
                  new MeterDiameter() { Id=5,Title="1.5 اینچ"},
                  new MeterDiameter() { Id=6,Title="2 اینچ"},
                  new MeterDiameter() { Id=7,Title="3 اینچ"},
                  new MeterDiameter() { Id=8,Title="4 اینچ"},
                  new MeterDiameter() { Id=9,Title="5 اینچ"},
                  new MeterDiameter() { Id=10,Title="6 اینچ"},
                  new MeterDiameter() { Id=11,Title="8 اینچ"},
                  new MeterDiameter() { Id=12,Title="10 اینچ"},
                  new MeterDiameter() { Id=13,Title="12 اینچ"},
                  new MeterDiameter() { Id=15,Title="16 اینچ"},
            };

            _meterDiameters.AddRange(meterDiameters);
            _uow.SaveChanges();
        }
    }
}
