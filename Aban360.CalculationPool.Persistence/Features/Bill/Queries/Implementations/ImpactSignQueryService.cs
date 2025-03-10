using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Implementations
{
    public class ImpactSignQueryService : IImpactSignQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ImpactSign> _impactSign;
        public ImpactSignQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _impactSign = _uow.Set<ImpactSign>();
            _impactSign.NotNull(nameof(_impactSign));
        }

        public async Task<ImpactSign> Get(short id)
        {
            return await _uow.FindOrThrowAsync<ImpactSign>(id);
        }

        public async Task<ICollection<ImpactSign>> Get()
        {
            return await _impactSign.ToListAsync();
        }
    }
}
