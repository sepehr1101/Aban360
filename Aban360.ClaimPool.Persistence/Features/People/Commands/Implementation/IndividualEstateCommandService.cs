using Aban360.ClaimPool.Domain.Features.People.Base;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Implementation
{
    internal sealed class IndividualEstateCommandService : IIndividualEstateCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualEstate> _individualEstates;
        public IndividualEstateCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualEstates = _uow.Set<IndividualEstate>();
            _individualEstates.NotNull(nameof(_individualEstates));
        }

        public async Task Add(IndividualEstate individualEstate)
        {
            await _individualEstates.AddAsync(individualEstate);
        }

        public async Task Remove(IndividualEstate individualEstate)
        {
            _individualEstates.Remove(individualEstate);
        }
    } 
}
