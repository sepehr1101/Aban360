using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Implementation
{
    internal sealed class IndividualTagCommandService : IIndividualTagCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualTag> _individualTag;
        public IndividualTagCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualTag = _uow.Set<IndividualTag>();
            _individualTag.NotNull(nameof(_individualTag));
        }

        public async Task Add(IndividualTag individualTag)
        {
            await _individualTag.AddAsync(individualTag);
        }

        public async Task Remove(IndividualTag individualTag)
        {
            _individualTag.Remove(individualTag);
        }
    }
}
