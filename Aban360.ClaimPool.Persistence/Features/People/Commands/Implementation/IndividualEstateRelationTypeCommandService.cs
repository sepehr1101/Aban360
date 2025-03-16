using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Implementation
{
    internal sealed class IndividualEstateRelationTypeCommandService : IIndividualEstateRelationTypeCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualEstateRelationType> _individualEstateRelationTypes;
        public IndividualEstateRelationTypeCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualEstateRelationTypes = _uow.Set<IndividualEstateRelationType>();
            _individualEstateRelationTypes.NotNull(nameof(_individualEstateRelationTypes));
        }

        public async Task Add(IndividualEstateRelationType individualEstateRelationType)
        {
            await _individualEstateRelationTypes.AddAsync(individualEstateRelationType);
        }

        public async Task Remove(IndividualEstateRelationType individualEstateRelationType)
        {
            _individualEstateRelationTypes.Remove(individualEstateRelationType);
        }
    }
}
