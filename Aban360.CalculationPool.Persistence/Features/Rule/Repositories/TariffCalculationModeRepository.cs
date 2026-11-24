using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Repositories
{
    internal sealed class TariffCalculationModeRepository : ITariffCalculationModeCommandService, ITariffCalculationModeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffCalculationMode> _tariffCalculationMode;
        public TariffCalculationModeRepository(IUnitOfWork uow)
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

        public async Task<TariffCalculationMode> Get(TariffCalculationModeEnum id)
        {
            return await _uow.FindOrThrowAsync<TariffCalculationMode>(id);
        }

        public async Task<ICollection<TariffCalculationMode>> Get()
        {
            return await _tariffCalculationMode.ToListAsync();
        }
    }
}
