using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    public class ImpactSignCommandService : IImpactSignCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ImpactSign> _impactSign;
        public ImpactSignCommandService(IUnitOfWork uow)
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
    }
}
