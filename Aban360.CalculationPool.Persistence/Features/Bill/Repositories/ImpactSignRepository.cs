using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Repositories
{
    internal sealed class ImpactSignRepository : IImpactSignCommandService, IImpactSignQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ImpactSign> _impactSign;
        public ImpactSignRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));
            _impactSign = _uow.Set<ImpactSign>();
            _impactSign.NotNull(nameof(_impactSign));
        }

        public async Task Add(ImpactSign impactSign)
        {
            await _impactSign.AddAsync(impactSign);
        }

        public void Remove(ImpactSign impactSign)
        {
            _impactSign.Remove(impactSign);
        }

        public async Task<ImpactSign> Get(ImpactSignEnum id)
        {
            return await _uow.FindOrThrowAsync<ImpactSign>(id);
        }

        public async Task<ICollection<ImpactSign>> Get()
        {
            return await _impactSign.ToListAsync();
        }
    }
}
