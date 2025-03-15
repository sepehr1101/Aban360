using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Implementations
{
   internal sealed class TariffCalculationModeCommandService : ITariffCalculationModeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffCalculationMode> _tariffCalculationMode;
        public TariffCalculationModeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariffCalculationMode = _uow.Set<TariffCalculationMode>();
            _tariffCalculationMode.NotNull(nameof(_tariffCalculationMode));
        }

        public async Task Add(TariffCalculationMode tariffCalculationMode)
        {
            await _tariffCalculationMode.AddAsync(tariffCalculationMode);
        }

        public async Task Remove(TariffCalculationMode tariffCalculationMode)
        {
            _tariffCalculationMode.Remove(tariffCalculationMode);
        }
    }
}
