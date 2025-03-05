using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Implementations
{
    public class TariffConstantQueryService : ITariffConstantQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffConstant> _tariffConstant;
        public TariffConstantQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariffConstant = _uow.Set<TariffConstant>();
            _tariffConstant.NotNull(nameof(_tariffConstant));
        }

        public async Task<TariffConstant> Get(short id)
        {
            return await _uow.FindOrThrowAsync<TariffConstant>(id);
        }

        public async Task<ICollection<TariffConstant>> Get()
        {
            return await _tariffConstant.ToListAsync();
        }
    }
}
