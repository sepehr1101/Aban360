using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Implementation
{
    internal sealed class IndividualDiscountTypeCommandService : IIndividualDiscountTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualDiscountType> _individualDiscountType;
        public IndividualDiscountTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualDiscountType = _uow.Set<IndividualDiscountType>();
            _individualDiscountType.NotNull(nameof(_individualDiscountType));
        }

        public async Task Add(IndividualDiscountType individualDiscountType)
        {
            await _individualDiscountType.AddAsync(individualDiscountType);
        }

        public async Task Remove(IndividualDiscountType individualDiscountType)
        {
            _individualDiscountType.Remove(individualDiscountType);
        }
    }
}
