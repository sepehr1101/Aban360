using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
   internal sealed class OfferingCommandService : IOfferingCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Offering> _offering;
        public OfferingCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offering = _uow.Set<Offering>();
            _offering.NotNull(nameof(Offering));
        }

        public async Task Add(Offering offering)
        {
            await _offering.AddAsync(offering);
        }

        public async Task Remove(Offering offering)
        {
            _offering.Remove(offering);
        }
    }
}
