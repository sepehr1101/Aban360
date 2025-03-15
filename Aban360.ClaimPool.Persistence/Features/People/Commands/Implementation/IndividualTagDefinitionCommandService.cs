using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Implementation
{
    internal sealed class IndividualTagDefinitionCommandService : IIndividualTagDefinitionCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<IndividualTagDefinition> _individualTagDefinitions;
        public IndividualTagDefinitionCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _individualTagDefinitions = _uow.Set<IndividualTagDefinition>();
            _individualTagDefinitions.NotNull(nameof(_individualTagDefinitions));
        }

        public async Task Add(IndividualTagDefinition individualTagDefinition)
        {
            await _individualTagDefinitions.AddAsync(individualTagDefinition);
        }

        public async Task Remove(IndividualTagDefinition individualTagDefinition)
        {
            _individualTagDefinitions.Remove(individualTagDefinition);
        }
    }
}
