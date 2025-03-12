using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class TariffCalculationModeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 9;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffCalculationMode> _tariffCalculationMode;
        public TariffCalculationModeSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariffCalculationMode = _uow.Set<TariffCalculationMode>();
            _tariffCalculationMode.NotNull(nameof(_tariffCalculationMode));
        }

        public void SeedData()
        {
            if (_tariffCalculationMode.Any())
            {
                return;
            }

            var tariffCalculationMode = GetTariffCalculationMode();
            _tariffCalculationMode.AddRange(tariffCalculationMode);
            _uow.SaveChanges();
        }
        private ICollection<TariffCalculationMode> GetTariffCalculationMode()
        {
            ICollection<TariffCalculationMode> tariffCalculationMode = new List<TariffCalculationMode>()
            {
                new TariffCalculationMode(){Id=TariffCalculationModeEnum.Interval,Title="بازه ای"},
                new TariffCalculationMode(){Id=TariffCalculationModeEnum.CurrentTime,Title="آخرین مقدار"},
            };

            return tariffCalculationMode;
        }
    }
}
