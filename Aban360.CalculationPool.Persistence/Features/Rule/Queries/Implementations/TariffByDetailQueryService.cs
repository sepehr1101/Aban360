using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Implementations
{
    internal sealed class TariffByDetailQueryService : ITariffByDetailQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffByDetail> _tariffByDetail;
        public TariffByDetailQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariffByDetail = _uow.Set<TariffByDetail>();
            _tariffByDetail.NotNull(nameof(_tariffByDetail));
        }

        public async Task<TariffByDetail> Get(int id)
        {
            return await _uow.FindOrThrowAsync<TariffByDetail>(id);
        }

        public async Task<ICollection<TariffByDetail>> Get()
        {
            return await _tariffByDetail.ToListAsync();
        }
    }
}
