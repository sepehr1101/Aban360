using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Commands.Implementations
{
    public class TariffCommandService : ITariffCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Tariff> _tariff;
        public TariffCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariff = _uow.Set<Tariff>();
            _tariff.NotNull(nameof(_tariff));
        }

        public async Task Add(Tariff tariff)
        {
            await _tariff.AddAsync(tariff);
        }

        public async Task Remove(Tariff tariff)
        {
            _tariff.Remove(tariff);
        }
    }
}
